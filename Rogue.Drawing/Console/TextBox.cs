using System;
using System.Collections.Generic;
using System.Text;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Console
{
    /// <summary>
    /// Interface: TextBox; Writeable;
    /// </summary>
    public class TextBox : Interface
    {
        public TextBox(Window Window)
        {
            this.Window = Window;
            this.DrawRegion = new Types.Rectangle
            {
                X = this.Left,
                Y = this.Top,
                Width = this.Width,
                Height = this.Height
            };

            //this.OnFocus = () => { Console.CursorVisible = true; };
        }
        public Action OnKeyPress;
        public Action OnEndTyping;
        protected string String = "";
        public string Text
        {
            get
            {
                //string s = String;
                //String = "";
                return String;
            }
        }


        public override IDrawSession Run()
        {
            if (this.String != "")
            {
                string stringBuffer = string.Empty;

                if (this.String.Length >= this.Width - 2)
                {
                    stringBuffer= String.Substring(String.Length - (this.Width - 2));
                }
                else
                {
                    stringBuffer = String;
                }

                this.Write(Window.Top + this.Top + 1, (Window.Left + this.Left + 1), stringBuffer, this.ActiveColor);
            }

            //bool end = false;
            //while (!end)
            //{
            //    ConsoleKeyInfo k = Console.ReadKey(true);
            //    if (k.Key == ConsoleKey.Enter || k.Key == ConsoleKey.Escape) { if (this.OnEndTyping != null) { this.OnEndTyping(); } end = true; break; }
            //    if (
            //                    k.KeyChar != (char)27 &&
            //                    k.KeyChar != '\0' &&
            //                    k.KeyChar != '\t' &&
            //                    k.KeyChar != '\\' &&
            //                    k.KeyChar != '/' &&
            //                    k.KeyChar != ':' &&
            //                    k.KeyChar != '*' &&
            //                    k.KeyChar != '?' &&
            //                    k.KeyChar != '"' &&
            //                    k.KeyChar != '<' &&
            //                    k.KeyChar != '>' &&
            //                    k.KeyChar != '|' &&
            //                    k.KeyChar != '.' &&
            //                    k.KeyChar != ',' &&
            //                    k.KeyChar != '`' &&
            //                    k.KeyChar != '~' &&
            //                    k.KeyChar != '!' &&
            //                    k.KeyChar != '#' &&
            //                    k.KeyChar != '№' &&
            //                    k.KeyChar != '@' &&
            //                    k.KeyChar != '$' &&
            //                    k.KeyChar != ';' &&
            //                    k.KeyChar != '%' &&
            //                    k.KeyChar != '^' &&
            //                    k.KeyChar != '&' &&
            //                    k.KeyChar != '(' &&
            //                    k.KeyChar != ')' &&
            //                    k.KeyChar != '-' &&
            //                    k.KeyChar != '+' &&
            //                    k.KeyChar != '='
            //               )
            //    { this.String += k.KeyChar; }
            //    for (int i = 0; i < this.String.Length; i++)
            //    {
            //        try
            //        {
            //            if (String[i] == '\b') { String = String.Substring(0, String.Length - 2); }
            //        }
            //        catch { }
            //    }
            //    //if string more then textbox
            //    if (this.String.Length >= this.Width - 2)
            //    {
            //        //write substring
            //        this.Clear();
            //        Console.SetCursorPosition(Window.Left + this.Left + 1, Window.Top + this.Top + 1);
            //        Console.Write(String.Substring(this.String.Length - (this.Width - 2)));
            //    }
            //    else
            //    {
            //        if (k.KeyChar == '\b') { this.Clear(); }
            //        Console.SetCursorPosition(Window.Left + this.Left + 1, Window.Top + this.Top + 1);
            //        Console.Write(this.String);
            //    }
            //}
            //Console.CursorVisible = false;

            return base.Run();
        }
        public override List<List<ColouredChar>> Construct(bool Active)
        {
            short cl = 0;
            if (Active) { cl = Convert.ToInt16(this.ActiveColor); } else { cl = Convert.ToInt16(this.InactiveColor); }

            string printf = "";
            if (String != "") { if (String.Length >= this.Width - 2) { printf = String.Substring(0, this.Width - 2); } else { printf = String; } } else { printf = this.Label; }
            List<List<ColouredChar>> l = new List<List<ColouredChar>>();
            l.Add(GetColouredLine(Additional.LightBorder.UpperLeftCorner + GetLine(this.Width - 2, Additional.LightBorder.HorizontalLine) + Additional.LightBorder.UpperRightCorner, new List<short>() { Convert.ToInt16(Window.BorderColor) }, new List<int>(), new List<short>(), new List<int>()));
            l.Add(GetColouredLine(Additional.LightBorder.VerticalLine + printf + GetLine(this.Width - 2 - printf.Length, ' ') + Additional.LightBorder.VerticalLine, new List<short>() { Convert.ToInt16(Window.BorderColor), cl, Convert.ToInt16(Window.BorderColor) }, new List<int>() { 1, this.Width - 1 }, new List<short>(), new List<int>()));
            l.Add(GetColouredLine(Additional.LightBorder.LowerLeftCorner + GetLine(this.Width - 2, Additional.LightBorder.HorizontalLine) + Additional.LightBorder.LowerRightCorner, new List<short>() { Convert.ToInt16(Window.BorderColor) }, new List<int>(), new List<short>(), new List<int>()));
            return l;
        }
    }
}
