namespace Rogue.Drawing.Console
{
    using System;
    using System.Collections.Generic;
    using Rogue.Drawing.Impl;
    using Rogue.View.Interfaces;

    public class Label : Interface
    {
        public TextPosition Align = TextPosition.Center;

        public Label(Window window, string text="")
        {
            this.Window = window;
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
            if(this.Width==0 && this.Align== TextPosition.Center)
            {
                throw new Exception("Label with align=center can't have Width=0!");
            }
            else if (this.Align!= TextPosition.Center)
            {
                this.Width = this.Text.Length;
            }

            this.DrawRegion = new Types.Rectangle
            {
                X = this.Window.Left + this.Left,
                Y = this.Window.Top + this.Top,
                Width = this.Width,
                Height = 1
            };

            if (this.SourceText != null)
            {
                return new IDrawText[] { this.SourceText };
            }

            var line = DrawText.Empty(this.Width, this.ForegroundColor);
            line.ReplaceAt(0, new DrawText(Middle(this.Text), this.ForegroundColor));

            return new IDrawText[] { line };
        }

        public IDrawText SourceText { get; set; }

        public override IDrawSession Run()
        {
            return base.Run();
        }
    }
}