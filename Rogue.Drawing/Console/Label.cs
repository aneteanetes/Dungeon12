namespace Rogue.Drawing.Console
{
    using System;
    using System.Collections.Generic;
    using Rogue.Drawing.Impl;
    using Rogue.View.Interfaces;

    public class Label : Interface
    {
        public TextPosition Align = TextPosition.Center;

        public Label(Window window, string text)
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
            this.DrawRegion = new Types.Rectangle
            {
                X = this.Window.Left + this.Left,
                Y = this.Window.Top + this.Top,
                Width = this.Width,
                Height = 1
            };

            var line = DrawText.Empty(this.Width, this.ForegroundColor);
            line.ReplaceAt(0, new DrawText(Middle(this.Text), this.ForegroundColor));

            return new IDrawText[] { line };
        }

        public override IDrawSession Run()
        {
            return base.Run();
        }
    }
}