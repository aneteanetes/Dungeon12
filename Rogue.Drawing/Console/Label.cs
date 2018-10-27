namespace Rogue.Drawing.Console
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Rogue.Drawing.Impl;
    using Rogue.View.Interfaces;

    public class Label : Interface
    {
        public TextPosition Align = TextPosition.Center;

        public Label(Interface @interface, string text = "")
        {
            this.Parent = @interface;
            this.Text = text;
        }

        public string Text;

        public ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// пока что только на mid текст
        /// </summary>
        /// <param name="Active"></param>
        /// <returns></returns>
        public override IEnumerable<IDrawText> Construct(bool Active)
        {
            if (this.SourceText != null)
            {
                BindRegion(this.SourceText);
                return new IDrawText[] { this.SourceText };
            }

            if (this.Width == 0 && this.Align == TextPosition.Center)
            {
                throw new Exception("Label with align=center can't have Width=0!");
            }
            else if (this.Align != TextPosition.Center)
            {
                this.Width = this.Text.Length;
            }

            this.DrawRegion = new Types.Rectangle
            {
                X = this.Parent.Left + this.Left,
                Y = this.Parent.Top + this.Top,
                Width = this.Width,
                Height = 1
            };

            var line = DrawText.Empty((int)this.Width, this.ForegroundColor);
            line.ReplaceAt(0, new DrawText(Middle(this.Text), this.ForegroundColor));

            BindRegion(line);

            return new IDrawText[] { line };
        }

        private void BindRegion(IDrawText drawText)
        {
            float x = this.Parent.FlatSum(i => i.Left) + this.Left;

            if (this.Align == TextPosition.Center)
            {
                x = this.Parent.FlatSum(i => i.Left) + this.Left + (this.Parent.Width / 2 - drawText.Length / 2);
            }

            drawText.Region = new Types.Rectangle
            {
                X = x,
                Y = this.Parent.FlatSum(i => i.Top) + this.Top,
                Height = 1,
                Width = drawText.Length
            };
        }

        public IDrawText SourceText { get; set; }

        public override IDrawSession Run()
        {
            return base.Run();
        }
    }
}