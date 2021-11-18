using Dungeon;
using Dungeon.Control;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using System;

namespace Dungeon12.SceneObjects.Map
{
    public class HintScenarioSceneObject : EmptySceneControl
    {
        private readonly ArrowImage Arrow;
        private readonly PlateObject Plate;

        public HintScenarioSceneObject()
        {
            this.Width = Global.Resolution.Width;
            this.Height = Global.Resolution.Height;

            Arrow = new ArrowImage()
            {
                Scale = .2,
                Left = 1157,
                Top = 266
            };

            this.AddChild(Plate = new PlateObject("Обучение",@"Для того что бы выбрать 
клетку, наведите курсор 
и нажмите на неё.")
            {
                Left = 1282,
                Top = 95
            });

            this.AddChild(Arrow);
        }

        public override bool AllKeysHandle => true;

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key == Key.D)
                Plate.Left += 1;
            if (key == Key.A)
                Plate.Left -= 1;
            if (key == Key.S)
                Plate.Top += 1;
            if (key == Key.W)
                Plate.Top -= 1;

            base.KeyDown(key, modifier, hold);
        }

        private class ArrowImage : ImageObject
        {
            public ArrowImage() : base("Backgrounds/arrow.png".AsmImg())
            {

            }
            public TimeSpan Time { get; set; }
            private bool down = false;

            public override void Update(GameTimeLoop gameTime)
            {
                if (Time == default(TimeSpan) || Time < default(TimeSpan))
                {
                    Time = TimeSpan.FromMilliseconds(800);
                    down = !down;
                }

                Time -= gameTime.ElapsedGameTime;

                if (down)
                {
                    this.Opacity -= opacityMultiplier;

                }
                else
                {
                    this.Opacity += opacityMultiplier;
                }
            }
            public double opacityMultiplier = 0.0025;
        }

        private class PlateObject : ImageObject
        {
            public TextControl Title { get; private set; }
            public TextControl Text { get; private set; }

            public PlateObject(string title, string text) : base("Backgrounds/plate250200.png".AsmImg())
            {
                this.Width = 250;
                this.Height = 200;

                Title = this.AddTextCenter(title.AsDrawText().Gabriela().InSize(16), vertical: false);
                Title.Top += 15;

                Text = this.AddTextCenter(text.AsDrawText().Gabriela().InSize(12), vertical: false);
                Text.Width = 200;
                Text.Left = 25;
                Text.Top += 50;
            }

            public TimeSpan Time { get; set; }
            private bool down = false;

            public override void Update(GameTimeLoop gameTime)
            {
                if (Time == default(TimeSpan) || Time < default(TimeSpan))
                {
                    Time = TimeSpan.FromMilliseconds(800);
                    down = !down;
                }

                Time -= gameTime.ElapsedGameTime;

                if (down)
                {
                    this.Opacity -= opacityMultiplier;
                    Text.Opacity -= opacityMultiplier;
                    Title.Opacity -= opacityMultiplier;

                }
                else
                {
                    this.Opacity += opacityMultiplier;
                    Text.Opacity += opacityMultiplier;
                    Title.Opacity += opacityMultiplier;
                }
            }

            public double opacityMultiplier = 0.0025;
        }
    }
}