namespace Dungeon12.Drawing.SceneObjects.Dialogs
{
    using Dungeon;
    using Dungeon.Control;
    using Dungeon.Control.Pointer;
    using Dungeon.Drawing;
    using Dungeon12.Drawing.SceneObjects.UI;
    using Dungeon12.Entities.Alive.Enums;
    using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
    using Dungeon.SceneObjects.Base; using Dungeon12.SceneObjects.Base;
    using Dungeon.View.Interfaces;
    using Dungeon12.Drawing.SceneObjects.Dialogs.Origin;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Dungeon12.SceneObjects.Base;

    public class OriginDialogue : EmptyHandleSceneControl
    {
        private Action<ISceneObjectControl> destroy;
        private Action<ISceneObjectControl> add;

        public OriginDialogue(Action<ISceneObjectControl> add,Action<ISceneObjectControl> destroy)
        {
            this.destroy = destroy;
            this.add = add;
            this.AddChild(new HorizontalWindow("Dungeon12.Resources.Images.ui.horizontal(26x17).png"));

            this.Width = 26;
            this.Height = 17;
            this.AddTextCenter(new DrawText("Происхождение (Предтечи)", new DrawColor(ConsoleColor.White)) { Size = 50 }.Triforce(), true, false)
                .Top = .5;

            AddOrigins();
            AddDescription();
            
            scrollbar = new Scrollbar(13.5, Up, Down)
            {
                Left = 9.7,
                Top = 2.5
            };
            //scrollbar.Down.Active = true;
            this.AddChild(scrollbar);

            var next = new MetallButtonControl("Начать");
            next.Left = this.Width / 2 - next.Width / 2;
            next.Top = 16;

            next.OnClick = () =>
            {
                OnSelect(this.originSelected);
            };

            this.AddChild(next);
        }

        public Action<Origins> OnSelect;

        private int index = 0;

        private Origins originSelected;
        private OriginDescription originDescription;
        private void SetOrigin(Origins origin)
        {
            originSelected = origin;
            if (originDescription != null)
            {
                this.RemoveChild(originDescription);
            }

            originDescription = new OriginDescription(origin)
            {
                Left = 11,
                Top = 2.5
            };

            this.AddChild(originDescription);
        }

        private void AddDescription() => SetOrigin(this.originSelectors.First().origin);

        private List<OriginSelector> originSelectors = new List<OriginSelector>();

        private double AddOrigins()
        {
            foreach (var originSelector in originSelectors)
            {
                originSelector.Destroy?.Invoke();
                this.RemoveChild(originSelector);
            }
            originSelectors.Clear();

            double top = 2.5;
            
            foreach (var origin in typeof(Origins).All<Origins>().Skip(index).Take(4))
            {
                var selector = new OriginSelector(origin, SetOrigin)
                {
                    Top = top,
                    Left = 1.5
                };
                selector.Destroy = () => { destroy(selector); };
                originSelectors.Add(selector);
                AddChild(selector);

                add?.Invoke(selector);

                top += 3.5;
            }

            return top;
        }

        private readonly Scrollbar scrollbar;

        private bool Up()
        {
            var minus = index > 0;
            if (minus)
            {
                index--;
                AddOrigins();
            }

            scrollbar.Up.Active = !(index == 0);
            scrollbar.Down.Active = true;

            return true;
        }

        private bool Down()
        {
            var plus = index < 2;
            if (plus)
            {
                index++;
                AddOrigins();
            }
            scrollbar.Down.Active = !(index == 2);
            scrollbar.Up.Active = true;

            return true;
        }
        
        protected override ControlEventType[] Handles => new ControlEventType[]
        {
                ControlEventType.MouseWheel
        };

        public override void MouseWheel(MouseWheelEnum mouseWheelEnum)
        {
            if (mouseWheelEnum == MouseWheelEnum.Up)
            {
                Up();
            }
            else
            {
                Down();
            }
        }
    }
}