using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Control.Keys;
using Rogue.Drawing.Impl;
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

            this.OnFocus = () => { this.Active = true; };
        }

        public Action OnEndTyping;

        public void OnKeyPress(KeyArgs args)
        {

        }

        protected string String = "";
        public string Text
        {
            get
            {
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

            var y = 0;
            var lines = this.Construct(this.Active);
            foreach (var line in lines)
            {
                this.Write(y, 0, line);
                y++;
            }

            return base.Run();
        }

        public override IEnumerable<IDrawText> Construct(bool Active)
        {
            this.DrawRegion = new Types.Rectangle
            {
                X = this.Window.Left + this.Left,
                Y = this.Window.Top + this.Top,
                Width = this.Width,
                Height = this.Height
            };

            var color = Active
                ? this.ActiveColor
                : this.InactiveColor;


            string printf = "";
            if (String != "")
            {
                if (String.Length >= this.Width - 2)
                {
                    printf = String.Substring(0, this.Width - 2);
                }
                else
                {
                    printf = String;
                }
            }
            else
            {
                printf = this.Label;
            }

            var top = new DrawText(Additional.LightBorder.UpperLeftCorner + GetLine(this.Width - 2, Additional.LightBorder.HorizontalLine) + Additional.LightBorder.UpperRightCorner, Window.BorderColor);

            var mid = DrawText.Empty(this.Width, Window.BorderColor);
            mid.ReplaceAt(0, new DrawText(Additional.LightBorder.ToString(), Window.BorderColor));
            mid.ReplaceAt(1, new DrawText(printf + GetLine(this.Width - 2 - printf.Length, ' '), color));
            mid.ReplaceAt(this.Width - 1, new DrawText(Additional.LightBorder.VerticalLine.ToString(), Window.BorderColor));

            var bot = new DrawText(Additional.LightBorder.LowerLeftCorner + GetLine(this.Width - 2, Additional.LightBorder.HorizontalLine) + Additional.LightBorder.LowerRightCorner, Window.BorderColor);

            return new IDrawText[] { top, mid, bot };
        }
    }
}
