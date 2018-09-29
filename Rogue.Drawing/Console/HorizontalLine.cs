namespace Rogue.Drawing.Console
{
    using System.Collections.Generic;
    using Rogue.Drawing.Impl;
    using Rogue.View.Interfaces;

    public class HorizontalLine : Interface
    {
        public HorizontalLine(Window window)
        {
            this.Window = window;
        }

        public override IEnumerable<IDrawText> Construct(bool Active)
        {
            var line = DrawText.Empty(this.Width);
            line.ForegroundColor = new DrawColor(Window.BorderColor);
            line.ReplaceAt(0, new DrawText(Window.Border.PerpendicularLeftward.ToString(), Window.BorderColor));
            line.ReplaceAt(1, new DrawText(GetLine(this.Width - 2, Window.Border.HorizontalLine), this.Window.BorderColor));
            line.ReplaceAt(this.Width - 1, new DrawText(Window.Border.PerpendicularRightward.ToString(), Window.BorderColor));

            return new IDrawText[] { line };
        }

        public override IDrawSession Run()
        {
            return base.Run();
        }
    }
}