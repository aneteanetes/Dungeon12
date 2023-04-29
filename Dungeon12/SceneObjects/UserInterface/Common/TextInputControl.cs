namespace Dungeon12.SceneObjects
{
    using Dungeon;
    using Dungeon.Control;
    using Dungeon.Control.Keys;
    using Dungeon.Drawing;
    using Dungeon.GameObjects;
    using Dungeon.SceneObjects;
    using Dungeon.SceneObjects.Base;
    using Dungeon.View.Interfaces;
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal class TextInputControl : ColoredRectangle<GameComponentEmpty>
    {
        private readonly int limit;
        private readonly bool capitalize;
        private readonly bool autofocus;
        private bool __focus = false;

        private bool freezing = false;
        public bool focus
        {
            get
            {
                return __focus;
            }
            set
            {
                if (value)
                {
                    if (!autofocus)
                    {
                        if (!_invisibleBack)
                            focusRect.Opacity = 0.001;

                        freezing = true;

                        Global.Freezer.Freeze(this);
                        //Global.Freezer.FreezeHandle(ControlEventType.Key, this);
                        //Global.BlockSceneControls = true;
                    }
                }
                else
                {
                    if (!autofocus)
                    {
                        freezing = false;

                        if (!_invisibleBack)
                            focusRect.Opacity = 0.5;

                        Global.Freezer.Unfreeze();
                        //Global.Freezer.UnfreezeHandle(ControlEventType.Key, this);
                        //Global.BlockSceneControls = false;
                    }
                }

                __focus = value;
            }
        }

        /// <summary>
        /// Освобождает <see cref="Freezer.HandleFreezes"/> и <see cref="Global.BlockSceneControls"/> если держит их
        /// </summary>
        public void FreeIfFreeze()
        {
            if (freezing)
            {
                focus = false;
            }
        }

        private readonly ColoredRectangle<GameComponentEmpty> focusRect;

        private static Action<TextInputControl> Change;

        private TypingText typingText;

        public Func<string, bool> Validation { get; set; }

        private bool _invisibleBack;
        private TextObject _placeholder;

        public TextInputControl(IDrawText drawText, 
            int chars, 
            bool capitalize = false, 
            bool autofocus = true, 
            bool absolute = true,
            bool onEnterOnBlur = false, 
            double width=0, 
            double height = 0,
            bool invisibleBack=false,
            IDrawText placeholder=null,
            bool carrige=false)
            :base(GameComponentEmpty.Empty)
        {
            _invisibleBack = invisibleBack;
            AbsolutePosition = absolute;
            limit = chars;

            Color = new DrawColor(ConsoleColor.Black);
            Depth = 1;
            Fill = true;

            Opacity = invisibleBack ? 000.1 : 0.5;
            Round = 5;

            this.capitalize = capitalize;
            this.autofocus = autofocus;

            drawText.SetText(new string(Enumerable.Range(0, chars).Select(c => 'G').ToArray()));

            var measure = MeasureText(drawText);

            if (width == 0)
            {
                width = measure.X;
            }

            if (height == 0)
            {
                height = measure.Y;
            }

            Width = width;
            Height = height;

            drawText.SetText("");

            typingText = new TypingText(drawText, carrige,placeholder);
            SetInputTextPosition();

            AddChild(typingText);

            if (placeholder != default)
            {
                _placeholder = this.AddTextCenter(placeholder);
            }

            if (!autofocus)
            {
                Change += sender =>
                {
                    if (sender != this && focus)
                    {
                        if (onEnterOnBlur)
                        {
                            OnEnter?.Invoke(Value);
                        }
                        focus = false;
                        if (!_invisibleBack)
                            focusRect.Opacity = 0.5;
                    }
                };
                focusRect = new BlurRect()
                {
                    Width = Width,
                    Height = Height
                };
                AddChild(focusRect);
                if (_invisibleBack)
                    focusRect.Opacity = 0.0001;
            }
        }

        private void SetInputTextPosition()
        {
            OnTyping?.Invoke(Value);
               var width = Width;
            var height = Height;

            try
            {
                var measure = MeasureText(typingText.Text);

                var left = width / 2 - measure.X / 2;
                typingText.Left = left;

                var top = height / 2 - measure.Y / 2;
                typingText.Top = top;
            }
            catch (ArgumentException)
            {
                //если мы не можем обработать этот символ - удаляем его
                typingText.Text.SetText(typingText.Text.StringData.Substring(0, typingText.Text.StringData.Length - 1));
                SetInputTextPosition();
            }
        }

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (!autofocus && !focus)
                return;

            //var text = typingText.Strings;

            if (key == Key.Enter)
            {
                EnterKeyDown();
            }

            if (key == Key.Escape)
            {
                typingText.HideCarriage();

                focus = false;
                if (!Value.IsNotEmpty() && _placeholder != default)
                    _placeholder.Visible = true;
            }

            //if (key == Key.Delete)
            //{
            //    text.SetText(string.Empty);
            //    SetInputTextPosition();
            //}

            //if (key == Key.Back)
            //{
            //    if (text.Length > 0)
            //    {
            //        text.SetText(text.StringData.Substring(0, text.StringData.Length - 1));
            //        SetInputTextPosition();
            //    }
            //    return;
            //}
        }

        private void EnterKeyDown()
        {
            typingText.HideCarriage();

            focus = false;
            OnEnter?.Invoke(Value);

            if (!Value.IsNotEmpty() && _placeholder != default)
                _placeholder.Visible = true;
        }

        public Action<string> OnTyping = x => { };

        public override void TextInput(string text)
        {
            if (!autofocus && !focus)
                return;

            var innerText = typingText.Text;

            if (innerText.Length >= limit && text!= "\b")
                return;

            if (innerText.StringData.Length == 0 && capitalize)
            {
                text = text.ToUpper();
            }

            if (Validation?.Invoke(text) ?? true)
            {
                if (text == "\u001b")
                    return;

                if (int.TryParse(text, out var num))
                    return;

                if (text == "\b")
                {
                    if (innerText.Length > 0)
                    {
                        innerText.SetText(innerText.StringData.Substring(0, innerText.StringData.Length - 1));
                        SetInputTextPosition();
                    }
                }                
                else if (CleanInput(text).IsNotEmpty() || text==" ")
                {
                    if (_placeholder != null)
                        _placeholder.Visible = false;

                    innerText.SetText(innerText.StringData + text);
                    SetInputTextPosition();
                }
            }
        }
        static string CleanInput(string strIn)
        {
            // Replace invalid characters with empty strings.
            try
            {
                return Regex.Replace(strIn, @"[^\w]", "",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            // If we timeout when replacing invalid characters,
            // we should return Empty.
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }

        public override void Click(PointerArgs args)
        {
            if (!focus)
                typingText.ShowCarriage();

            focus = true;

            if (_placeholder != default)
                _placeholder.Visible = false;
            //Change?.Invoke(this);
        }

        public override void GlobalClick(PointerArgs args)
        {
            if (!___focus)
                EnterKeyDown();
            base.GlobalClick(args);
        }

        public override void ClickRelease(PointerArgs args)
        {
            if (!autofocus)
            {
                Click(args);
            }
        }

        public override void GlobalClickRelease(PointerArgs args)
        {
            if (!autofocus)
            {
                //Change?.Invoke(null);
            }
        }

        public override bool AllKeysHandle => true;

        private bool ___focus = false;
        public override void Focus()
        {
            ___focus = true;
            Opacity = 0.7;
            UpdatePath();
        }

        public override void Unfocus()
        {
            ___focus = false;
            Opacity = 0.5;
            UpdatePath();
        }

        private string GetChar(Key key)
        {
            return key.ToString();
        }

        public string Value
        {
            get => typingText.Text.StringData;
            set
            {
                typingText.Text.SetText(value);
                SetInputTextPosition();
            }
        }

        public Action<string> OnEnter { get; set; }

        protected override ControlEventType[] Handles => new ControlEventType[] { 
            ControlEventType.Text, 
            ControlEventType.Key, 
            ControlEventType.Click, 
            ControlEventType.GlobalClick,
            ControlEventType.GlobalClickRelease, 
            ControlEventType.ClickRelease };

        public override void Destroy()
        {
            Change=null;
            base.Destroy();
        }

        private class TypingText : TextObject
        {
            public override bool CacheAvailable => false;

            public TextObject carriage;

            public TypingText(IDrawText text, bool IsCarriage, IDrawText placeholder = null) : base(text)
            {
                if (IsCarriage)
                {
                    var carriagetext = text.Copy();
                    carriagetext.SetText("|");

                    carriage = this.AddTextCenter(carriagetext);
                    carriage.Left = this.Width;
                    carriage.Visible = false;
                    if (placeholder!= null)
                        carriage.Top = this.MeasureText(placeholder).Y / 6;
                }
            }

            public void ShowCarriage()
            {
                if (carriage != default)
                {
                    carriage.Opacity = 1;
                    carriage.Visible = true;
                }
            }

            public void HideCarriage()
            {
                if (carriage != default)
                {
                    carriage.Visible = false;
                    Time= default;
                }
            }


            public override void Update(GameTimeLoop gameTime)
            {
                if (carriage != default)
                {
                    carriage.Left = this.MeasureText(this.Text).X;
                    if (this.Text.StringData.IsNotEmpty())
                    {
                        carriage.Top = this.MeasureText(Text).Y / 6;
                    }


                    if (Time == default || Time < default(TimeSpan))
                    {
                        Time = TimeSpan.FromMilliseconds(300);
                        down = !down;
                    }

                    Time -= gameTime.ElapsedGameTime;

                    if (down)
                    {
                        carriage.Opacity -= opacityMultiplier;
                        //Description.Opacity -= opacityMultiplier;
                        //Title.Opacity -= opacityMultiplier;

                    }
                    else
                    {
                        carriage.Opacity += opacityMultiplier;
                        //Description.Opacity += opacityMultiplier;
                        //Title.Opacity += opacityMultiplier;
                    }
                }
            }

            public TimeSpan Time { get; set; }
            private bool down = false;


            public double opacityMultiplier = 0.1;
        }

        private class BlurRect : DarkRectangle
        {
            public override bool CacheAvailable => false;

            public BlurRect()
            {
                Color = new DrawColor(ConsoleColor.Gray);
                Fill = true;
                Opacity = 0.5;
            }
        }
    }
}