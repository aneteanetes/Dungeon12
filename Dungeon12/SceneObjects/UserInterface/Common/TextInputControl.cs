namespace Dungeon12.SceneObjects
{
    using Dungeon;
    using Dungeon.Control;
    using Dungeon.Control.Keys;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.GameObjects;
    using Dungeon.SceneObjects;
    using Dungeon.View.Interfaces;
    using System;
    using System.Linq;

    public class TextInputControl : ColoredRectangle<GameComponentEmpty>
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

        public TextInputControl(IDrawText drawText, int chars, bool capitalize = false, bool autofocus = true, bool absolute = true, bool onEnterOnBlur = false, double width=0, double height = 0)
            :base(GameComponentEmpty.Empty)
        {
            AbsolutePosition = absolute;
            limit = chars;

            Color = ConsoleColor.Black;
            Depth = 1;
            Fill = true;
            Opacity = 0.5;
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

            typingText = new TypingText(drawText);
            SetInputTextPosition();

            AddChild(typingText);

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
                        focusRect.Opacity = 0.5;
                    }
                };
                focusRect = new BlurRect()
                {
                    Width = Width,
                    Height = Height
                };
                AddChild(focusRect);
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

            //var text = typingText.Text;

            if (key == Key.Enter)
            {
                focus = false;
                OnEnter?.Invoke(Value);
            }

            if (key == Key.Escape)
            {
                focus = false;
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

        public Action<string> OnTyping;

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

                if (text == "\b")
                {
                    if (innerText.Length > 0)
                    {
                        innerText.SetText(innerText.StringData.Substring(0, innerText.StringData.Length - 1));
                        SetInputTextPosition();
                    }
                }
                else
                {
                    innerText.SetText(innerText.StringData + text);
                    SetInputTextPosition();
                }
            }
        }

        public override void Click(PointerArgs args)
        {
            focus = true;
            //Change?.Invoke(this);
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

        public override void Focus()
        {
            Opacity = 0.7;
            UpdatePath();
        }

        public override void Unfocus()
        {
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

        protected override ControlEventType[] Handles => new ControlEventType[] { ControlEventType.Text, ControlEventType.Key, ControlEventType.Click, ControlEventType.GlobalClickRelease, ControlEventType.ClickRelease };

        private class TypingText : TextControl
        {
            public override bool CacheAvailable => false;

            public TypingText(IDrawText text) : base(text)
            {
            }
        }

        private class BlurRect : DarkRectangle
        {
            public override bool CacheAvailable => false;

            public BlurRect()
            {
                Color = ConsoleColor.Gray;
                Fill = true;
                Opacity = 0.5;
            }
        }
    }
}