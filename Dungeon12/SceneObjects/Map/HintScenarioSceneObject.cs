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
        Click,
        LocationOpenedAndUsed,
        NewHexAppear
    }

    public class HintScenarioSceneObject : EmptySceneControl
    {
        private readonly ArrowImage Arrow;
        private readonly PlateObject PlateFocus;
        private readonly PlateObject PlateActivation;
        private readonly PlateObject PlateClick;
        private readonly PlateObject PlateTextInput;
        private readonly PlateObject PlateHexAppear;
        private readonly PlateObject PlateOrigin;

        List<PlateObject> Plates = new List<PlateObject>();

        public bool IsEnabled { get; set; } = true;

        public HintScenarioSceneObject()
        {
            this.Width = Global.Resolution.Width;
            this.Height = Global.Resolution.Height;

            Arrow = new ArrowImage();

            Plates.Add(this.AddChild(PlateFocus = new PlateObject("Перемещение", @"Для перемещения 
наведите курсор 
на плитку:")
            {
                Left = 1282,
                Top = 165
            }));

            Plates.Add(this.AddChild(PlateClick = new PlateObject("Активация", @"Для перемещения и
активации плитки
нажмите на неё:")
            {
                Left = 741,
                Top = 184,
                Visible = false
            }));


            Plates.Add(this.AddChild(PlateActivation = new PlateObject("Локация",
                @"Каждая локация содержит в себе несколько плиток, выберите интересующую вас и нажмите для активации.",
                true, 200)
            {
                Left = 1050,
                Top = 18,
                Visible = false
            }));


            Plates.Add(this.AddChild(PlateTextInput = new PlateObject("Ввод текста",
                @"Для ввода текста нажмите на текстовое поле. Пока вы не нажмёте Enter или Esc другие элементы интерфейса не будут активны. После того как закончите ввод текста нажмите на Enter или Esc. 
Как только будете готовы - подпишите свидетельство.",
                true, 300)
            {
                Left = 137,
                Top = 69,
                Visible = false
            }));

            Plates.Add(this.AddChild(PlateHexAppear = new PlateObject("Появление плиток",
                @"Некоторые плитки после использования могут открывать доступ до следующих. После подписания свидетельства вы должны указать своё происхождение",
                true, 200)
            {
                Left = 1146,
                Top = 147,
                Visible = false
            }));


            Plates.Add(this.AddChild(PlateOrigin = new PlateObject("Выбор региона",
                @"Для выбора региона наведите на него курсор и нажмите левую кнопку мыши. Не забудьте изучить бонусы каждого региона.",
                true, 200)
            {
                Left = 111,
                Top = 451,
                Visible = false
            }));

            this.AddChild(Arrow);

            StepFocus();
        }

        private HintStates state = HintStates.Default;

        public void ChangeState(HintStates states)
        {
            state = states;
        }

        public void StepOriginSelect()
        {
            Arrow.Reset();
            Arrow.Visible = false;

            SetActivePlate(PlateOrigin);
        }

        public void StepNewHex()
        {
            if (state == HintStates.Click)
            {
                ChangeState(HintStates.LocationOpenedAndUsed);
            }

            Arrow.Reset();
            Arrow.Visible = true;
            Arrow.Left = 1008;
            Arrow.Top = 254;
            Arrow.Flip = Dungeon.View.Enums.FlipStrategy.None;

            SetActivePlate(PlateHexAppear);
        }

        public void StepTextInput()
        {
            Arrow.Reset();
            Arrow.Visible = true;
            Arrow.Left = 357;
            Arrow.Top = 176;
            Arrow.Flip = Dungeon.View.Enums.FlipStrategy.Horizontally;

            SetActivePlate(PlateTextInput);
        }

        private void SetActivePlate(PlateObject activePlate)
        {
            activePlate.Visible = true;
            Plates.ForEach(x => { if (x != activePlate) { x.Visible = false; } });
        }

        public void StepActivate()
        {
            if (state == HintStates.Click || state == HintStates.Default)
            {
                ChangeState(HintStates.Click);
                Arrow.Invert();
                Arrow.Visible = true;
                Arrow.Left = 908;
                Arrow.Top = 83;
                Arrow.Flip = Dungeon.View.Enums.FlipStrategy.None;

                SetActivePlate(PlateActivation);
            }
            else if (state == HintStates.LocationOpenedAndUsed)
            {
                StepNewHex();
            }
        }

        public void StepFocus()
        {
            if (state == HintStates.Default)
            {
                Arrow.Reset();
                Arrow.Scale = .2;
                Arrow.Left = 1157;
                Arrow.Top = 266;
                Arrow.Flip = Dungeon.View.Enums.FlipStrategy.None;
                Arrow.Visible = true;

                SetActivePlate(PlateFocus);
            }
        }

        public void StepClick()
        {
            Arrow.Reset();
            Arrow.Scale = .2;
            Arrow.Left = 949;
            Arrow.Top = 285;
            Arrow.Flip = Dungeon.View.Enums.FlipStrategy.Horizontally;
            Arrow.Visible = true;

            SetActivePlate(PlateClick);
        }

        public override bool AllKeysHandle => true;

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key == Key.D)
                PlateOrigin.Left += 1;
            if (key == Key.A)
                PlateOrigin.Left -= 1;
            if (key == Key.S)
                PlateOrigin.Top += 1;
            if (key == Key.W)
                PlateOrigin.Top -= 1;

            base.KeyDown(key, modifier, hold);
        }

        private class ArrowImage : ImageObject
        {
            public ArrowImage() : base("Backgrounds/arrow.png".AsmImg())
            {
                this.CacheAvailable = false;
            }

            public TimeSpan Time { get; set; }
            private bool down = false;

            public void Invert()
            {
                Image = "Backgrounds/arrow_white.png".AsmImg();
            }

            public void Reset()
            {
                Image = "Backgrounds/arrow.png".AsmImg();
            }

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

            public PlateObject(string title, string text, bool wordwrap=false, double height=125) : base($"Backgrounds/plate250{height}.png".AsmImg())
            {
                this.Width = 250;
                this.Height = height;

                Title = this.AddTextCenter(title.AsDrawText().Gabriela().InSize(16), vertical: false);
                Title.Top += 7;

                var txt = text.AsDrawText().Gabriela().InSize(12);

                if (wordwrap)
                    txt = txt.WithWordWrap();

                Description = this.AddTextCenter(txt, vertical: false);
                if (!wordwrap)
                {
                    Description.Width = 200;
                }
                else
                {
                    Description.Width = 220;
                    Description.Left = 15;
                }
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