using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Drawing.Console
{
    /// <summary>
    /// Interface: Button; Clickable;
    /// </summary>
    public class Button : Interface
    {
        public Button(Window Window)
        {
            this.Window = Window;
        }
        public Action OnClick;
        public override List<List<ColouredChar>> Construct(bool Active)
        {
            short cl = 0;
            if (Active) { cl = Convert.ToInt16(this.ActiveColor); } else { cl = Convert.ToInt16(this.InactiveColor); }

            List<List<ColouredChar>> l = new List<List<ColouredChar>>();
            l.Add(GetColouredLine(Window.Border.UpperLeftCorner + GetLine(this.Width - 2, Window.Border.HorizontalLine) + Window.Border.UpperRightCorner, new List<short>() { Convert.ToInt16(Window.BorderColor) }, new List<int>(), new List<short>(), new List<int>()));
            l.Add(GetColouredLine(Window.Border.VerticalLine + this.Middle(this.Label) + Window.Border.VerticalLine, new List<short>() { Convert.ToInt16(Window.BorderColor), cl, Convert.ToInt16(Window.BorderColor) }, new List<int>() { (this.Width / 2) - (this.Label.Length / 2), this.Width - 1 }, new List<short>(), new List<int>()));
            l.Add(GetColouredLine(Window.Border.LowerLeftCorner + GetLine(this.Width - 2, Window.Border.HorizontalLine) + Window.Border.LowerRightCorner, new List<short>() { Convert.ToInt16(Window.BorderColor) }, new List<int>(), new List<short>(), new List<int>()));

            return l;
        }
    }
}
