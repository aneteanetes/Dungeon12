using Dungeon;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using System;
using System.Collections.Generic;

namespace Dungeon12.SceneObjects.Map
{
    public enum HintStates
    {
        Default,
        Click
    }

    public class HintScenarioSceneObject : EmptySceneControl
    {
        private readonly ArrowImage Arrow;
        private readonly PlateObject PlateFocus;
        private readonly PlateObject PlateClick;

        List<PlateObject> Plates = new List<PlateObject>();

        public bool IsEnabled { get; set; } = true;

        public HintScenarioSceneObject()
        {
            this.Width = Global.Resolution.Width;
            this.Height = Global.Resolution.Height;

            Arrow = new ArrowImage();

            Plates.Add(this.AddChild(PlateFocus = new PlateObject("Перемещение",@"Для перемещения 
наведите курсор 
на клетку:")
            {
                Left = 1282,
                Top = 165
            }));

            Plates.Add(this.AddChild(PlateClick = new PlateObject("Активация", @"Для перемещения и
активации клетки
нажмите на неё:")
            {
                Left = 741,
                Top = 184,
                Visible=false
            }));

            this.AddChild(Arrow);

            StepFocus();
        }

        private HintStates state = HintStates.Default;

        public void ChangeState(HintStates states)
        {
            state = states;
        }

        public void StepActivate()
        {
            ChangeState(HintStates.Click);
            Arrow.Visible = true;
            Plates.ForEach(x => x.Visible = false);
        }

        public void StepFocus()
        {
            if (state == HintStates.Default)
            {
                Arrow.Scale = .2;
                Arrow.Left = 1157;
                Arrow.Top = 266;
                Arrow.Flip = Dungeon.View.Enums.FlipStrategy.None;
                Arrow.Visible = true;

                PlateFocus.Visible = true;
                Plates.ForEach(x => { if (x != PlateFocus) { x.Visible = false; } });
            }
        }

        public void StepClick()
        {
            Arrow.Scale = .2;
            Arrow.Left = 949;
            Arrow.Top = 285;
            Arrow.Flip = Dungeon.View.Enums.FlipStrategy.Horizontally;
            Arrow.Visible = true;

            PlateClick.Visible = true; 
            Plates.ForEach(x => { if (x != PlateClick) { x.Visible = false; } });
        }

        public override bool AllKeysHandle => true;

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key == Key.D)
                Arrow.Left += 1;
            if (key == Key.A)
                Arrow.Left -= 1;
            if (key == Key.S)
                Arrow.Top += 1;
            if (key == Key.W)
                Arrow.Top -= 1;

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
            public double opacityMultiplier = 0.004;
        }

        private class PlateObject : ImageObject
        {
            public TextControl Title { get; private set; }

            public TextControl Description { get; private set; }

            public ImageObject Line { get; private set; }

            public PlateObject(string title, string text) : base("Backgrounds/plate250125.png".AsmImg())
            {
                this.Width = 250;
                this.Height = 125;

                Title = this.AddTextCenter(title.AsDrawText().Gabriela().InSize(16), vertical: false);
                Title.Top += 7;

                Description = this.AddTextCenter(text.AsDrawText().Gabriela().InSize(12), vertical: false);
                Description.Width = 200;
                //Description.Left = 25;
                Description.Top +=46;

                this.AddChild(Line = new ImageObject("Backgrounds/line230.png".AsmImg())
                {
                    Left = 20,
                    Top = 39
                });
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
                    //Description.Opacity -= opacityMultiplier;
                    //Title.Opacity -= opacityMultiplier;

                }
                else
                {
                    this.Opacity += opacityMultiplier;
                    //Description.Opacity += opacityMultiplier;
                    //Title.Opacity += opacityMultiplier;
                }
            }

            public double opacityMultiplier = 0.004;
        }
    }
}