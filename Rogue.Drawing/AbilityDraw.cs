using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Drawing
{
    public static class AbilityDraw
    {
        public static void DrawMainWindow()
        {
            int height = 0;

            Console.ForegroundColor = Rogue.RAM.CUIColor;

            string Clear = string.Empty;
            Clear = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
            Clear = " " + DrawHelp.Border(true, 1) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 2);
            Console.SetCursorPosition(25, height);
            Console.WriteLine(Clear);

            //тело                
            for (int i = 1; i < 24; i++)
            {
                Clear = DrawHelp.FullLine(74, " ", 2);
                Clear = " " + DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 26) + DrawHelp.Border(true, 3);
                Console.SetCursorPosition(25, i);
                Console.WriteLine(Clear);
            }

            //носки 
            Clear = string.Empty;
            Clear = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
            Clear = " " + DrawHelp.Border(true, 5) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 6);
            Console.SetCursorPosition(25, 24);
            Console.WriteLine(Clear);

            //носки объявления 
            Clear = string.Empty;
            Clear = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
            Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 7);
            Console.SetCursorPosition(25, 2);
            Console.WriteLine(Clear);

            //носки актив/пассив
            Clear = string.Empty;
            Clear = DrawHelp.FullLine(74, DrawHelp.Border(false, 4), 27);
            Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 7);
            Console.SetCursorPosition(25, 22);
            Console.WriteLine(Clear);

            //носки Локация
            Clear = string.Empty;
            Clear = DrawHelp.FullLine(74, DrawHelp.Border(false, 4), 27);
            Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 7);
            Console.SetCursorPosition(25, 16);
            Console.WriteLine(Clear);

            //носки мультипликатор
            Clear = string.Empty;
            Clear = DrawHelp.FullLine(74, DrawHelp.Border(false, 4), 27);
            Clear = " " + DrawHelp.Border(true, 8) + "───────────────────────┬──────────────────────" + DrawHelp.Border(true, 7);
            Console.SetCursorPosition(25, 18);
            Console.WriteLine(Clear);
            Console.SetCursorPosition(25 + "                       ".Length, 19);
            Console.Write("  │");

            //носки стоимость
            Clear = string.Empty;
            Clear = DrawHelp.FullLine(74, DrawHelp.Border(false, 4), 27);
            Clear = " " + DrawHelp.Border(true, 8) + "───────────────────────┴──────────────────────" + DrawHelp.Border(true, 7);
            Console.SetCursorPosition(25, 20);
            Console.WriteLine(Clear);

            //носки Локация
            Clear = string.Empty;
            Clear = DrawHelp.FullLine(74, DrawHelp.Border(false, 4), 27);
            Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 7);
            Console.SetCursorPosition(25, 14);
            Console.WriteLine(Clear);

        }
        /// <summary>
        /// Draw ability window
        /// </summary>
        /// <param name="text">Info</param>
        /// <param name="Name">Name</param>
        /// <param name="Mode">Active/Passive to string</param>
        /// <param name="Cost">int to string</param>
        /// <param name="magrate">Сила магии: 1.54</param>
        /// <param name="LvRate">int to string</param>
        /// <param name="Duration">int to string</param>
        /// <param name="location">Location to string</param>
        public static void AdditionalInfoWindow(string text, string Name, string Mode, string Cost, string magrate, string LvRate, string Duration, string location)
        {
            AbilityDraw.DrawMainWindow();
            ConsoleColor Class = ConsoleColor.Gray;
            CharacterDraw.DrawColorHeader(Name, Class);
            string[] str = text.Split(new Char[] { '\n' });
            Console.ForegroundColor = Class;
            int i = 0;
            int c = 0;
            foreach (string s in str)
            {
                c = (50 / 2) - (s.Length / 2);
                Console.SetCursorPosition(c + 25, i + 4);
                foreach (char ch in s.ToCharArray())
                {
                    if (ch == '&') { Console.ForegroundColor = ConsoleColor.DarkCyan; continue; }
                    else if (ch == '^') { Console.ForegroundColor = ConsoleColor.Green; continue; }
                    else if (ch == '#') { Console.ForegroundColor = ConsoleColor.Yellow; continue; }
                    else if (ch == ';') { Console.ForegroundColor = ConsoleColor.DarkGray; continue; }
                    else if (ch == '<') { Console.ForegroundColor = ConsoleColor.DarkYellow; continue; }
                    else if (ch == '@') { Console.ForegroundColor = ConsoleColor.DarkMagenta; continue; }
                    else if (ch == '?') { Console.ForegroundColor = ConsoleColor.Cyan; continue; }
                    else if (ch == '$') { Console.ForegroundColor = ConsoleColor.Cyan; continue; }
                    else if (ch == '№') { Console.ForegroundColor = ConsoleColor.Cyan; continue; }
                    else if (ch == '~') { Console.ForegroundColor = ConsoleColor.Magenta; continue; }
                    else if (ch == '†') { Console.ForegroundColor = SystemEngine.Helper.Information.ClassC; continue; }

                    if (ch == ' ') { Console.ForegroundColor = Class; }

                    Console.Write(ch);
                }
                i++;
            }
            //Mode
            c = (50 / 2) - (Mode.Length / 2);
            Console.SetCursorPosition(c + 25, 15);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(Mode);
            //Cost
            c = (50 / 2) - ((Rogue.RAM.Player.ManaName.Length + Cost.Length + 1) / 2);
            Console.SetCursorPosition(c + 25, 17);
            Console.ForegroundColor = SystemEngine.Helper.Information.ClassC;
            Console.Write(Rogue.RAM.Player.ManaName + Cost);
            //Rate
            c = (25 / 2) - ((magrate.Length) / 2);
            Console.SetCursorPosition(c + 25, 19);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(magrate);
            //lvlRate
            c = (75 / 2) - (("Уровень: " + LvRate).Length / 2);
            Console.SetCursorPosition(c + 25, 19);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Уровень: " + LvRate);
            //duration
            if (Duration == "0") { Duration = "Мгновенно"; }
            if (Duration == "999") { Duration = "Постоянно"; }
            c = (50 / 2) - (("Время действия: " + Duration).Length / 2);
            Console.SetCursorPosition(c + 25, 21);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Время действия: " + Duration);
            //location
            c = (50 / 2) - (("Где используется: " + location).Length / 2);
            Console.SetCursorPosition(c + 25, 23);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("Где используется: " + location);

            Console.ReadKey(true);
        }
    }
}
