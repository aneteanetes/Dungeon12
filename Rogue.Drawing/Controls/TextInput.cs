namespace Rogue.Drawing.Controls
{
    using System;
    using Rogue.Drawing.Impl;
    using Rogue.Types;
    using Rogue.View.Interfaces;

    public class TextInput : BaseControl
    {
        public TextInput()
        {
            this.OnFocus = OnFocusEvent;
            this.OnUnfocus = OnUnfocusEvent;
        }

        public override bool IsControlable => true;

        public override string Tileset => $"Rogue.Resources.Images.GUI.textbox.png";

        public override Rectangle TileSetRegion => new Rectangle
        {
            X = 0,
            Y = 0,
            Height = 91,
            Width = 456
        };

        public float Size = 15;
        public float LetterSpacing = 12;

        /// <summary>
        /// Максимальное количество символов для ввода
        /// Не смотря на то что это выглядит адекватно - ХУЙ ТАМ, ТАМ БАГА С ОФФСЕТОМ
        /// </summary>
        public int Max = 0;

        public override bool Container => false;

        public IDrawText Label { get; set; }
        private string value = string.Empty;
        public string Placeholder;
        public IDrawColor ActiveColor;
        public IDrawColor InactiveColor;
        public bool Editable = false;

        public override IDrawSession Run()
        {
            IDrawColor currentColor = Editable
                ? ActiveColor
                : InactiveColor;

            if (this.Label == null)
            {
                this.Label = new DrawText(Placeholder, currentColor) { Size = Size, LetterSpacing = LetterSpacing };
            }
            else
            {
                var val = value.Length > 0 ? value : Placeholder;

                if ((val.Length+0.2f) * this.LetterSpacing >= this.Width*24)
                {
                    double offset = (this.value.Length + 0.2f) * this.LetterSpacing - this.Width * 24;
                    offset /= 24;
                    offset += Math.Round(offset, MidpointRounding.AwayFromZero) + 1;
                    val = value.Substring((int)offset);
                }

                this.Label = new DrawText(val, currentColor) { Size=Size,LetterSpacing=LetterSpacing };
            }


            this.Label.Region = new Rectangle
            {
                X = 0.2f,
                Y = 0.3f
            };

            this.Append(new Text
            {
                DrawText = Label
            });

            return base.Run();
        }

        public void AppendValue(string data)
        {
            if (this.value.Length <= this.Max)
                this.value += data;
        }

        public void BackslashValue(int count)
        {
            if (value.Length > 0)
                this.value = this.value.Substring(0, this.value.Length - count);
        }

        public string GetValue() => this.value;

        private void OnFocusEvent()
        {
            Editable = true;
            this.Label.Paint(ActiveColor, true);
            this.Run().Publish();
        }

        private void OnUnfocusEvent()
        {
            Editable = false;
            this.Label.Paint(InactiveColor, true);
            this.Run().Publish();
        }
    }
}