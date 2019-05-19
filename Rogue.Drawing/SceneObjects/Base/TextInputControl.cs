namespace Rogue.Drawing.SceneObjects.Base
{
    using Rogue.Control.Events;
    using Rogue.Control.Keys;
    using Rogue.View.Interfaces;
    using System;
    using System.Linq;

    public class TextInputControl : ColoredRectangle
    {        
        private readonly int limit;
        private readonly bool capitalize;

        private TypingText typingText;
        
        public TextInputControl(IDrawText drawText, int chars, bool capitalize=false)
        {
            limit = chars;

            Color = ConsoleColor.Black;
            Depth = 1;
            Fill = true;
            Opacity = 0.5;
            Round = 5;

            this.capitalize = capitalize;

            drawText.SetText(new string(Enumerable.Range(0, chars).Select(c => 'G').ToArray()));

            var measure = MeasureText(drawText);

            var width = measure.X / 32;
            var height = measure.Y / 32;

            this.Width = width + 0.5;
            this.Height = height + 0.5;

            drawText.SetText("");

            typingText = new TypingText(drawText);
            SetInputTextPosition();

            this.AddChild(typingText);
        }

        private void SetInputTextPosition()
        {
            var width = this.Width * 32;
            var height = this.Height * 32;

            var measure = MeasureText(typingText.Text);

            var left = width / 2 - measure.X / 2;
            typingText.Left = left / 32;

            var top = height / 2 - measure.Y / 2;
            typingText.Top = top / 32;
        }

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            var text = typingText.Text;

            if(key== Key.Enter)
            {
                OnEnter?.Invoke(Value);
            }

            if (key == Key.Back)
            {
                if (text.Length > 0)
                {
                    text.SetText(text.StringData.Substring(0, text.StringData.Length - 1));
                    SetInputTextPosition();
                }
                return;
            }
        }

        public override void TextInput(string text)
        {
            var innerText = typingText.Text;

            if (innerText.Length >= limit)
                return;

            if (innerText.StringData.Length == 0 && capitalize)
            {
                text = text.ToUpper();
            }

            innerText.SetText(innerText.StringData + text);
            SetInputTextPosition();
        }

        public override bool AllKeysHandle => true;

        public override void Focus()
        {
            this.Opacity = 0.7;
            this.UpdatePath();
        }

        public override void Unfocus()
        {
            this.Opacity = 0.5;
            this.UpdatePath();
        }

        private string GetChar(Key key)
        {
            return key.ToString();
        }

        public string Value => this.typingText.Text.StringData;

        public Action<string> OnEnter { get; set; }

        protected override ControlEventType[] Handles => new ControlEventType[] { ControlEventType.Text, ControlEventType.Key };

        private class TypingText : TextControl
        {
            public override bool CacheAvailable => false;

            public TypingText(IDrawText text) : base(text)
            {
            }
        }
    }
}