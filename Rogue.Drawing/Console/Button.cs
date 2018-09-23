using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Console
{
    /// <summary>
    /// Interface: Button; Clickable;
    /// </summary>
    public class Button : Interface
    {
        public Button(Window Window)
        {
            //this.AutoClear = false;
            this.Window = Window;
        }
        public Action OnClick;
        private List<List<ColouredChar>> constructed = new List<List<ColouredChar>>();
        public override List<List<ColouredChar>> Construct(bool Active)
        {
            short cl = 0;
            if (Active) { cl = Convert.ToInt16(this.ActiveColor); } else { cl = Convert.ToInt16(this.InactiveColor); }

            List<List<ColouredChar>> l = new List<List<ColouredChar>>();
            l.Add(GetColouredLine(Window.Border.UpperLeftCorner + GetLine(this.Width - 2, Window.Border.HorizontalLine) + Window.Border.UpperRightCorner, new List<short>() { Convert.ToInt16(Window.BorderColor) }, new List<int>(), new List<short>(), new List<int>()));
            l.Add(GetColouredLine(Window.Border.VerticalLine + this.Middle(this.Label) + Window.Border.VerticalLine, new List<short>() { Convert.ToInt16(Window.BorderColor), cl, Convert.ToInt16(Window.BorderColor) }, new List<int>() { (this.Width / 2) - (this.Label.Length / 2), this.Width - 1 }, new List<short>(), new List<int>()));
            l.Add(GetColouredLine(Window.Border.LowerLeftCorner + GetLine(this.Width - 2, Window.Border.HorizontalLine) + Window.Border.LowerRightCorner, new List<short>() { Convert.ToInt16(Window.BorderColor) }, new List<int>(), new List<short>(), new List<int>()));

            var debugText = string.Empty;
            foreach (var a in l)
            {
                debugText += Environment.NewLine + new string(a.Select(x => x.Char).ToArray());
            }

            constructed = l;

            return l;
        }

        public override IDrawSession Run()
        {
            this.DrawRegion = new Types.Rectangle
            {
                X = this.Window.Left+ this.Left,
                Y = 8,
                Width = this.Width,
                Height = this.Height
            };

            var lines = constructed;
            foreach (List<ColouredChar> line in lines)
            {
                var linePos = lines.IndexOf(line);

                foreach (ColouredChar c in line)
                {
                    var charPos = line.IndexOf(c);

                    this.Write(linePos, charPos, c.Char.ToString(), (ConsoleColor)c.Color, (ConsoleColor)c.BackColor);
                }
            }

            return base.Run();
        }
    }
}
