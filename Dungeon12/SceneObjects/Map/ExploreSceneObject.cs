using Dungeon;
using Dungeon.Control;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities.Map;
using System;
using System.Linq;

namespace Dungeon12.SceneObjects.Map
{
    public class ExploreSceneObject : SceneControl<Location>
    {
        public override bool AbsolutePosition => true;

        public class ExploreTitleSceneObject : DarkRectangle
        {
            private TextObject text;
            private ImageObject titleline;

            public ExploreTitleSceneObject(string titletext)
            {
                var title = titletext.AsDrawText().Gabriela().InSize(20);

                Opacity = .9;
                Width = Global.DrawClient.MeasureText(title).X + 15;
                Height = Global.DrawClient.MeasureText(title).Y + 10;

                titleline = this.AddChild(new ImageObject("Backgrounds/line339.png".AsmImg())
                {
                    Width = Width-20,
                    Height = 10,
                    Left = 10
                });
                titleline.Top = 40;

                text = this.AddTextCenter(title, vertical: false);
            }

            public bool IsBlink { get; set; }

            public TimeSpan Time { get; set; }
            private bool down = false;

            public void StartBlink()
            {
                IsBlink = true;
                ResetDefault();
            }

            public void StopBlink()
            {
                IsBlink = false;
                ResetDefault();
            }

            private void ResetDefault()
            {
                Opacity = .9;
                text.Opacity = 1;
                titleline.Opacity = 1;
                down = false;
                Time = default;
            }

            public override void Update(GameTimeLoop gameTime)
            {
                if (!IsBlink)
                    return;

                if (Time == default(TimeSpan) || Time < default(TimeSpan))
                {
                    Time = TimeSpan.FromMilliseconds(900);
                    down = !down;
                }

                Time -= gameTime.ElapsedGameTime;

                if (down)
                {
                    this.Opacity -= opacityMultiplier;
                    text.Opacity -= opacityMultiplier;
                    titleline.Opacity -= opacityMultiplier;

                }
                else
                {
                    this.Opacity += opacityMultiplier;
                    text.Opacity += opacityMultiplier;
                    titleline.Opacity += opacityMultiplier;
                }
            }

            public double opacityMultiplier = 0.008;
        }

        public ExploreTitleSceneObject ExploreTitle { get; set; }

        public ExploreSceneObject(Location location) : base(location, true)
        {
            Width = Global.Resolution.Width;
            Height = Global.Resolution.Height;

            this.AddChild(new DarkRectangle()
            {
                Opacity = 0.8,
                Width = Width,
                Height = Height,
            });

            ExploreTitle = this.AddChildCenter(new ExploreTitleSceneObject(location.Polygon.Name));
            ExploreTitle.Top = 50;

            this.AddChildCenter(new CellsSceneObject(this,location));
        }

        public override bool AllKeysHandle => true;

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            //if (key == Key.D)
            //    cells.Left += 1;
            //if (key == Key.A)
            //    cells.Left -= 1;
            //if (key == Key.S)
            //    cells.Top += 1;
            //if (key == Key.W)
            //    cells.Top -= 1;

            base.KeyDown(key, modifier, hold);
        }

        private IDrawText tooltiptext;
        public IDrawText TooltipText
        {
            get
            {
                if (tooltiptext == null)
                {
                    var name = Component?.Polygon?.Name ?? " ";
                    if (name == null)
                        name = " ";
                    tooltiptext = name.AsDrawText().Gabriela().InSize(12);
                }

                return tooltiptext;
            }
        }

        public override void Focus()
        {
            if (Component.IsOpen)
            {
                //Selection.Visible = true;
                //Hints.StepClick();
            }
        }

        public override void Unfocus()
        {
            if (Component.IsOpen)
            {
                //Hints.StepFocus();
                //Selection.Visible = false;
            }
        }
    }
}
