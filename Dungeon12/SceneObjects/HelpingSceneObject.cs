using Dungeon;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using System;
using System.Collections.Generic;

namespace Dungeon12.SceneObjects
{
    public enum HintStates
    {
        Default,
        Click,
        LocationOpenedAndUsed,
        OriginSelected,
        HeroCreated
    }

    internal class HelpingSceneObject : EmptySceneControl
    {
        //private readonly ArrowImage Arrow;
        private readonly PlateObject PlateFocus;
        private readonly PlateObject PlateActivation;
        private readonly PlateObject PlateClick;
        private readonly PlateObject PlateTextInput;
        private readonly PlateObject PlateHexAppear;
        private readonly PlateObject PlateOrigin;
        private readonly PlateObject PlateUseOthers;
        private readonly PlateObject PlateCraft;
        private readonly PlateObject PlateConfirm;

        List<PlateObject> Plates = new List<PlateObject>();

        public bool IsEnabled { get; set; } = true;

        public HelpingSceneObject()
        {
            Width = DungeonGlobal.Resolution.Width;
            Height = DungeonGlobal.Resolution.Height;

            //Arrow = new ArrowImage();

            Plates.Add(AddChild(PlateFocus = new PlateObject("Перемещение", @"Для перемещения 
наведите курсор 
на плитку:")
            {
                Left = 1332,
                Top = 358
            }));

            Plates.Add(AddChild(PlateClick = new PlateObject("Активация", @"Для перемещения и
активации плитки
нажмите на неё:")
            {
                Left = 1026,
                Top = 331,
                Visible = false
            }));


            Plates.Add(AddChild(PlateActivation = new PlateObject("Локация",
                @"Каждая локация содержит в себе несколько плиток, выберите доступную нажмите для активации. Если плиток несколько вы сможете выбирать какую активировать.",
                true, 200)
            {
                Left = 848,
                Top = 101,
                Visible = false
            }));


            Plates.Add(AddChild(PlateTextInput = new PlateObject("Ввод текста",
                @"Для ввода текста нажмите на текстовое поле. Пока вы не нажмёте Enter или Esc другие элементы интерфейса не будут активны. После того как закончите ввод текста нажмите на Enter или Esc.

Как только будете готовы - подпишите свидетельство.",
                true, 300)
            {
                Left = 219,
                Top = 225,
                Visible = false
            }));

            Plates.Add(AddChild(PlateHexAppear = new PlateObject("Появление плиток",
                @"Некоторые плитки после использования могут открывать доступ до следующих. После подписания свидетельства вы должны указать своё происхождение",
                true, 200)
            {
                Left = 1146,
                Top = 147,
                Visible = false
            }));


            Plates.Add(AddChild(PlateOrigin = new PlateObject("Выбор региона",
                @"Для выбора региона наведите на него курсор и нажмите левую кнопку мыши. Не забудьте изучить бонусы каждого региона.",
                true, 200)
            {
                Left = 111,
                Top = 451,
                Visible = false
            }));


            Plates.Add(AddChild(PlateUseOthers = new PlateObject("Исследование",
                @"Продолжите исследование локации, как только все возможные плитки будут открыты вы сможете переместиться далее.",
                true, 200)
            {
                Left = 111,
                Top = 451,
                Visible = false
            }));

            Plates.Add(AddChild(PlateCraft = new PlateObject("Профессия",
                @"Профессия позволяет создавать предметы для сопартийцев и последователей определённого типа.",
                true, 200)
            {
                Left = 111,
                Top = 451,
                Visible = false
            }));

            Plates.Add(AddChild(PlateConfirm = new PlateObject("Подтверждение",
                @"Подтвердите создание персонажа, после этого изменить свой выбор вы не сможете.",
                true)
            {
                Left = 111,
                Top = 451,
                Visible = false
            }));

            //AddChild(Arrow);

            StepFocus();
        }

        private HintStates state = HintStates.Default;

        public void ChangeState(HintStates states)
        {
            state = states;
        }

        public void ConfirmCreate()
        {
            //Arrow.Reset();
            //Arrow.Visible = false;
            SetActivePlate(PlateConfirm);
        }

        public void Hide()
        {
            //Arrow.Reset();
            //Arrow.Visible = false;
            Plates.ForEach(x => x.Visible = false);
        }

        public void ShowCraft()
        {
            //Arrow.Reset();
           //Arrow.Visible = false;

            SetActivePlate(PlateCraft);
        }

        public void StepUseOther()
        {
            if (state != HintStates.OriginSelected)
            {
                state = HintStates.OriginSelected;
            }

            //Arrow.Reset();
            //Arrow.Visible = false;

            SetActivePlate(PlateUseOthers);
        }

        public void StepOriginSelect()
        {
            //Arrow.Reset();
            //Arrow.Visible = false;

            SetActivePlate(PlateOrigin);
        }

        public void StepNewHex()
        {
            if (state == HintStates.Click)
            {
                ChangeState(HintStates.LocationOpenedAndUsed);
            }

            //Arrow.Reset();
            //Arrow.Visible = true;
            //Arrow.Left = 1008;
            //Arrow.Top = 254;
            //Arrow.Flip = Dungeon.View.Enums.FlipStrategy.None;

            SetActivePlate(PlateHexAppear);
        }

        public void StepTextInput()
        {
            //Arrow.Reset();
            //Arrow.Visible = true;
            //Arrow.Left = 357;
            //Arrow.Top = 176;
            //Arrow.Flip = Dungeon.View.Enums.FlipStrategy.Horizontally;

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
                //Arrow.Invert();
                //Arrow.Visible = true;
                //Arrow.Left = 908;
                //Arrow.Top = 83;
                //Arrow.Flip = Dungeon.View.Enums.FlipStrategy.None;

                SetActivePlate(PlateActivation);
            }
            else if (state == HintStates.LocationOpenedAndUsed)
            {
                StepNewHex();
            }
            else if (state == HintStates.OriginSelected)
            {
                StepUseOther();
            }
        }

        public void StepFocus()
        {
            if (state == HintStates.Default)
            {
                //Arrow.Reset();
                //Arrow.Scale = .2;
                //Arrow.Left = 1157;
                //Arrow.Top = 266;
                //Arrow.Flip = Dungeon.View.Enums.FlipStrategy.None;
                //Arrow.Visible = true;

                SetActivePlate(PlateFocus);
            }
        }

        public void StepClick()
        {
            if (state != HintStates.HeroCreated)
            {
                //Arrow.Reset();
                //Arrow.Scale = .2;
                //Arrow.Left = 949;
                //Arrow.Top = 285;
                //Arrow.Flip = Dungeon.View.Enums.FlipStrategy.Horizontally;
                //Arrow.Visible = true;

                SetActivePlate(PlateClick);
            }
        }

        public override bool AllKeysHandle => true;

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key == Key.D)
                PlateHexAppear.Left += 1;
            if (key == Key.A)
                PlateHexAppear.Left -= 1;
            if (key == Key.S)
                PlateHexAppear.Top += 1;
            if (key == Key.W)
                PlateHexAppear.Top -= 1;

            if(key== Key.Space)
                Console.WriteLine();

            base.KeyDown(key, modifier, hold);
        }

        public override void Throw(Exception ex)
        {
            throw ex;
        }

        private class ArrowImage : ImageObject
        {
            public ArrowImage() : base("Backgrounds/arrow.png".AsmImg())
            {
                CacheAvailable = false;
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

#warning закоментированно анимация-мерцание
            //public override void InternalUpdate(GameTimeLoop gameTime)
            //{
            //    if (Time == default || Time < default(TimeSpan))
            //    {
            //        Time = TimeSpan.FromMilliseconds(800);
            //        down = !down;
            //    }

            //    Time -= gameTime.ElapsedGameTime;

            //    if (down)
            //    {
            //        Opacity -= opacityMultiplier;

            //    }
            //    else
            //    {
            //        Opacity += opacityMultiplier;
            //    }
            //}
            public double opacityMultiplier = 0.004;
        }

        private class PlateObject : ImageObject
        {
            public TextObject Title { get; private set; }

            public TextObject Description { get; private set; }

            public ImageObject Line { get; private set; }

            public PlateObject(string title, string text, bool wordwrap = false, double height = 125) : base($"Backgrounds/plate250{height}.png".AsmImg())
            {
                Width = 250;
                Height = height;

                Title = AddTextCenter(title.AsDrawText().Gabriela().InSize(16), vertical: false);
                Title.Top += 7;

                var txt = text.AsDrawText().Gabriela().InSize(12);

                if (wordwrap)
                    txt = txt.WithWordWrap();

                Description = AddTextCenter(txt, vertical: false);
                if (!wordwrap)
                {
                    Description.Width = 200;
                }
                else
                {
                    Description.Width = 220;
                    Description.Left = 15;
                }
                Description.Top += 46;

                AddChild(Line = new ImageObject("Backgrounds/line230.png".AsmImg())
                {
                    Left = 20,
                    Top = 39
                });
            }

            public TimeSpan Time { get; set; }

            private bool down = false;

            //public override void InternalUpdate(GameTimeLoop gameTime)
            //{
            //    if (Time == default || Time < default(TimeSpan))
            //    {
            //        Time = TimeSpan.FromMilliseconds(800);
            //        down = !down;
            //    }

            //    Time -= gameTime.ElapsedGameTime;

            //    if (down)
            //    {
            //        Opacity -= opacityMultiplier;
            //        //Description.Opacity -= opacityMultiplier;
            //        //Title.Opacity -= opacityMultiplier;

            //    }
            //    else
            //    {
            //        Opacity += opacityMultiplier;
            //        //Description.Opacity += opacityMultiplier;
            //        //Title.Opacity += opacityMultiplier;
            //    }
            //}

            public double opacityMultiplier = 0.004;
        }
    }
}