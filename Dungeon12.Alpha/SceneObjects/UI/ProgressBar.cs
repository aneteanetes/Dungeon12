using Dungeon;
using Dungeon.Drawing;
using Dungeon.GameObjects;
using Dungeon12.SceneObjects;
using Dungeon.SceneObjects;
using System;

namespace Dungeon12.SceneObjects.UI
{
    public class ProgressBar : Dungeon12.SceneObjects.SceneControl<ProgressBarModel>
    {
        public override bool CacheAvailable => false;

        private TextControl titleControl;

        private ProgressBarDiscover progressBarDiscover;

        public ProgressBar(ProgressBarModel component) : base(component, true)
        {
            this.Image = "ui/journal/descoverback.png".AsmImg();
            this.Width = 7;
            this.Height = .75;

            if (component.Name != default)
            {
                var name = this.AddTextCenter(Component.Name.AsDrawText().InSize(14).Montserrat(), false, false);
                name.Top -= 2 + component.BarOffsetDown;
            }

            titleControl = this.AddTextCenter(Component.ProgressText?.Invoke(), false, false);
            titleControl.Top -= 1 + component.BarOffsetDown;

            progressBarDiscover = new ProgressBarDiscover(component);
            this.AddChild(progressBarDiscover);
        }

        public override double Scale
        {
            get => base.Scale;
            set
            {
                base.Scale = value;
                titleControl.Scale = value;
                progressBarDiscover.Scale = value;
            }
        }

        public override void Update() => titleControl.SetText(Component.ProgressText?.Invoke());

        private class ProgressBarDiscover : SceneObject<ProgressBarModel>
        {
            public override bool CacheAvailable => false;

            public ProgressBarDiscover(ProgressBarModel component):base(component,true)
            {
                this.Image = "ui/journal/descoverprogress.png".AsmImg();
                this.Left = .5;
                this.Height = .75;
                this.ScaleAndResize = true;
            }

            public override double Scale
            {
                get => base.Scale;
                set
                {
                    this.Left = .5 * value;
                    base.Scale = value;
                }
            }

            public override double Width
            {
                get
                {
                    var progress = Component.Progress.Invoke();
                    var multipler = ((progress.now / progress.max * 100) / 100);
                    var value = 6 * multipler;
                    if (value == 6)
                        Component.OnMax?.Invoke();

                    return value;
                }
            }
        }
    }

    public class ProgressBarModel : GameComponent
    {
        public string Name { get; set; }

        public Func<DrawText> ProgressText { get; set; }

        public Func<(double now,double max)> Progress { get; set; }

        public double BarOffsetDown { get; set; }

        public Action OnMax { get; set; }
    }
}
