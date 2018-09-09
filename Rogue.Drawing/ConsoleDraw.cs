using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Drawing
{
    public static class ConsoleDraw
    {
        public static void WriteTitle(string Title)
        {
            int leftOffSet = (Console.WindowWidth / 2);
            int topOffSet = (Console.WindowHeight / 2);
            int onestringconsole = (Console.WindowHeight / 30);
            string[] str = Title.Split(new Char[] { '\n' });
            int i = 0;
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            foreach (string strr in str)
            {
                try { Console.SetCursorPosition(leftOffSet - strr.Length / 2, topOffSet + (onestringconsole * i)); }
                catch (ArgumentOutOfRangeException) { Console.SetCursorPosition(0, topOffSet + (onestringconsole * i)); }
                Console.WriteLine(strr);
                i = i + 1;
            }
        }

        public static void CursorCenter()
        {
            int leftOffSet = (Console.WindowWidth / 2);
            int topOffSet = (Console.WindowHeight / 2);
            int onestringconsole = (Console.WindowHeight / 30);
            Console.SetCursorPosition(leftOffSet, topOffSet + onestringconsole);
        }

        public static string ReadCenter(bool clear = false, bool save = false)
        {
            if (clear) { Console.Clear(); }
            //Начало
            int leftOffSet = (Console.WindowWidth / 2);
            int topOffSet = (Console.WindowHeight / 2);
            int onestringconsole = (Console.WindowHeight / 30);
            Console.SetCursorPosition(leftOffSet, topOffSet + onestringconsole);

            //Цикл
            string OurResult = string.Empty;
            if (save)
            {
                OurResult = Rogue.RAM.Player.Name;
                Console.SetCursorPosition(0, topOffSet + onestringconsole);
                DrawEngine.ConsoleDraw.ClearLine(topOffSet + onestringconsole);
                Console.SetCursorPosition(leftOffSet - (OurResult.Length / 2), topOffSet + onestringconsole);
                Console.Write(OurResult);
            }
            bool LoopEnd = false;
            while (LoopEnd == false)
            {
                ConsoleKeyInfo Pushed = Console.ReadKey(true);
                if (Pushed.Key != ConsoleKey.Enter)
                {
                    if (Pushed.Key == ConsoleKey.Backspace)
                    {
                        try
                        {
                            OurResult = OurResult.Remove(OurResult.Length - 1);
                            Console.SetCursorPosition(0, topOffSet + onestringconsole);
                            DrawEngine.ConsoleDraw.ClearLine(topOffSet + onestringconsole);
                            Console.SetCursorPosition(leftOffSet - (OurResult.Length / 2), topOffSet + onestringconsole);
                            Console.Write(OurResult);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            Console.SetCursorPosition(0, topOffSet + onestringconsole);
                            DrawEngine.ConsoleDraw.ClearLine(topOffSet + onestringconsole);
                            Console.SetCursorPosition(leftOffSet - (OurResult.Length / 2), topOffSet + onestringconsole);
                            Console.Write(OurResult);
                        }
                    }
                    else
                    {
                        if (
                                Pushed.KeyChar != (char)27 &&
                                Pushed.KeyChar != '\0' &&
                                Pushed.KeyChar != '\t' &&
                                Pushed.KeyChar != '\\' &&
                                Pushed.KeyChar != '/' &&
                                Pushed.KeyChar != ':' &&
                                Pushed.KeyChar != '*' &&
                                Pushed.KeyChar != '?' &&
                                Pushed.KeyChar != '"' &&
                                Pushed.KeyChar != '<' &&
                                Pushed.KeyChar != '>' &&
                                Pushed.KeyChar != '|' &&
                                Pushed.KeyChar != '.' &&
                                Pushed.KeyChar != ',' &&
                                Pushed.KeyChar != '`' &&
                                Pushed.KeyChar != '~' &&
                                Pushed.KeyChar != '!' &&
                                Pushed.KeyChar != '#' &&
                                Pushed.KeyChar != '№' &&
                                Pushed.KeyChar != '@' &&
                                Pushed.KeyChar != '$' &&
                                Pushed.KeyChar != ';' &&
                                Pushed.KeyChar != '%' &&
                                Pushed.KeyChar != '^' &&
                                Pushed.KeyChar != '&' &&
                                Pushed.KeyChar != '(' &&
                                Pushed.KeyChar != ')' &&
                                Pushed.KeyChar != '-' &&
                                Pushed.KeyChar != '+' &&
                                Pushed.KeyChar != '='
                           )
                        { OurResult = OurResult + Pushed.KeyChar; }
                        Console.SetCursorPosition(0, topOffSet + onestringconsole);
                        DrawEngine.ConsoleDraw.ClearLine(topOffSet + onestringconsole);
                        Console.SetCursorPosition(leftOffSet - (OurResult.Length / 2), topOffSet + onestringconsole);
                        Console.Write(OurResult);
                    }

                }
                else
                {
                    LoopEnd = true;
                }
            }
            return OurResult;
        }

        public static void ClearLine(int NumberOfLine)
        {
            int width = Console.WindowWidth;
            string Clear = string.Empty;
            for (int i = 0; i < width; i++)
            {
                Clear += " ";
            }
            Console.SetCursorPosition(0, NumberOfLine);
            Console.WriteLine(Clear);
        }

        public static void WriteMenu(string Menu)
        {
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            int leftOffSet = (Console.WindowWidth / 2);
            int topOffSet = (Console.WindowHeight / 2);
            int onestringconsole = (Console.WindowHeight / 30);
            string[] str = Menu.Split(new Char[] { '\n' });
            int i = 0;
            int height = str.Length;
            height = (Console.WindowHeight / 2) - (height / 2);
            foreach (string strr in str)
            {
                if (strr == " ")
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(leftOffSet - strr.Length / 2, height + (onestringconsole * i));
                    Console.WriteLine(strr);
                    i = i + 1;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.SetCursorPosition(leftOffSet - strr.Length / 2, height + (onestringconsole * i));
                    Console.WriteLine(strr);
                    i = i + 1;
                }
            }
        }

        public static void WriteAdditionalInformation(string Info, ConsoleColor Color)
        {
            Console.SetCursorPosition(55, 3);
            Console.ForegroundColor = Color;
            Console.WriteLine(Info);
        }

        public static void WriteAdditionalLogo(string Info, ConsoleColor Color)
        {
            Console.SetCursorPosition(45, 4);
            Console.ForegroundColor = Color;
            Console.WriteLine(Info);
        }

        public static void WriteAddonName(ConsoleColor Color)
        {
            int c = (100 / 2) - (Rogue.RAM.AddonName.Length / 2);
            Console.SetCursorPosition(c, 7);
            Console.ForegroundColor = Color;
            Console.WriteLine(Rogue.RAM.AddonName);
        }

        public static string ReadFromInfoWindow(string Title)
        {
            Console.ForegroundColor = Rogue.RAM.Map.Biom;
            string clear = DrawHelp.FullLine(95, " ", 2);
            Console.SetCursorPosition(2, 24 + 2);
            Console.Write(clear);
            Console.SetCursorPosition(3, 24 + 2);
            Console.Write(Title);
            return Console.ReadLine();
        }
    }
}
