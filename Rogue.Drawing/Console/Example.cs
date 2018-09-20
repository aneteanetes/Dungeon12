using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Drawing.Console
{
    public static class Example
    {

        /// <summary>
        /// Example of window
        /// </summary>
        /// <param name="Border">Window will be with border or not</param>
        /// <param name="CustomBorder">Window border will be custom or one of additional</param>
        /// <param name="Header">Window will be with header or not</param>
        /// <param name="Animation">Window will be animated or not</param>
        /// <param name="AnimationType">Window with animation can have one of 3 animations (0-2)</param>
        public static void s(bool Border = true, bool CustomBorder = false, bool Header = false, bool Animation = false, int AnimationType = 0)
        {
            Window w = new Window
            {

                //Size
                Width = 30,
                Height = 30,

                //Position
                Left = 0,
                Top = 0
            };

            //Border settings
            var b = Additional.BoldBorder;
            if (Border) { w.BorderColor = ConsoleColor.DarkCyan; }
            if (CustomBorder)
            {
                b.UpperRightCorner = '@';
                b.UpperLeftCorner = '@';
                b.HorizontalLine = '~';
                b.PerpendicularLeftward = '@';
                b.PerpendicularRightward = '@';
            }
            w.Border = b;

            //header
            w.Header = Header;

            //Animation
            if (Animation)
            {
                w.Animated = true;
            }
            if (AnimationType <= 0) { w.Animation = Additional.StadartAnimation; }
            if (AnimationType == 1) { w.Animation = Additional.HorizontalAnimation; }
            if (AnimationType >= 2) { w.Animation = Additional.CenterAnimation; }


            //Text in window
            Text t = new Text(w);
            t.TextPosition = TextPosition.Center;
            t.BackgroundColor = ConsoleColor.Black;
            List<char> rainbow = new List<char>() { 'S', 'c', 'r', 'o', 'l', 'l' };
            for (int i = 0; i < 6; i++)
            {
                t.ForegroundColor = (ConsoleColor)i + 4;
                t.Write(Convert.ToString(rainbow[i]));
            }
            t.AppendLine();
            t.ForegroundColor = ConsoleColor.DarkCyan;
            t.WriteLine("Old scroll of power!");
            t.AppendLine();
            string s = "";
            for (int i = 0; i < w.Width - 4; i++) { s += '~'; }
            t.WriteLine(s);

            t.TextPosition = TextPosition.Center;

            t.ForegroundColor = ConsoleColor.DarkBlue;
            t.Write("Attack Damage: ");
            t.ForegroundColor = ConsoleColor.Green;
            t.Write("+5");
            t.AppendLine();

            t.ForegroundColor = ConsoleColor.DarkMagenta;
            t.Write("Ability Power: ");
            t.ForegroundColor = ConsoleColor.Green;
            t.Write("+2");
            t.AppendLine();

            t.AppendLine();

            t.ForegroundColor = ConsoleColor.DarkCyan;
            s = "";
            for (int i = 0; i < w.Width - 4; i++) { s += '='; }
            t.WriteLine(s);
            t.TextPosition = TextPosition.Right;
            t.ForegroundColor = ConsoleColor.DarkYellow;
            t.WriteLine("<Special Effect>");
            t.ForegroundColor = ConsoleColor.DarkCyan;

            t.WriteLine(s);

            t.TextPosition = TextPosition.Center;

            t.ForegroundColor = ConsoleColor.DarkGreen;
            t.Write("When you use ");
            t.ForegroundColor = ConsoleColor.Red;
            t.Write("*Fireball*");
            t.ForegroundColor = ConsoleColor.DarkGreen;
            t.Write(",");
            t.AppendLine();
            t.WriteLine("you have chance to get");
            t.WriteLine("special effect:");
            t.TextPosition = TextPosition.Left;
            t.ForegroundColor = ConsoleColor.Yellow;
            t.AppendLine();
            t.WriteLine("Battle trance:");
            t.TextPosition = TextPosition.Right;
            t.WriteLine("\"");
            t.TextPosition = TextPosition.Center;
            t.ForegroundColor = ConsoleColor.Red;
            t.Write("Hit Points: ");
            t.ForegroundColor = ConsoleColor.Green;
            t.Write("+1");
            t.AppendLine();
            t.ForegroundColor = ConsoleColor.Blue;
            t.Write("Mana Points: ");
            t.ForegroundColor = ConsoleColor.Green;
            t.Write("+1");
            t.AppendLine();
            t.TextPosition = TextPosition.Left;
            t.ForegroundColor = ConsoleColor.Yellow;
            t.AppendLine();
            t.WriteLine("\"");

            //finally
            w.Text = t;
            w.Publish();
        }
    }
}
