using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Drawing.Console
{
    /// <summary>
    /// Interface: CheckBox; Boolean;
    /// </summary>
    public class CheckBox : Interface
    {
        public CheckBox(Window Window)
        {
            this.Window = Window;
        }
        public Action OnCheckAdditional;
        protected bool value = false;
        public bool Checked
        { get { return value; } }
        public void OnCheck()
        {
            if (value) { value = false; }
            else { value = true; }
        }
        public override List<List<ColouredChar>> Construct(bool Active)
        {
            short cl = 0;
            char cr = ' ';
            if (this.value) { cr = 'Y'; } else { cr = 'N'; }

            if (Active) { cl = Convert.ToInt16(this.ActiveColor); } else { cl = Convert.ToInt16(this.InactiveColor); }

            List<List<ColouredChar>> l = new List<List<ColouredChar>>();
            l.Add(GetColouredLine(Window.Border.UpperLeftCorner + GetLine(this.Width - 2, Window.Border.HorizontalLine) + Window.Border.UpperRightCorner, new List<short>() { Convert.ToInt16(Window.BorderColor) }, new List<int>(), new List<short>(), new List<int>()));
            l.Add(GetColouredLine(Window.Border.VerticalLine + Middle(cr + this.Label) + Window.Border.VerticalLine, new List<short>() { cl }, new List<int>(), new List<short>(), new List<int>()));
            l.Add(GetColouredLine(Window.Border.LowerLeftCorner + GetLine(this.Width - 2, Window.Border.HorizontalLine) + Window.Border.LowerRightCorner, new List<short>() { Convert.ToInt16(Window.BorderColor) }, new List<int>(), new List<short>(), new List<int>()));

            return l;
        }
    }
}
