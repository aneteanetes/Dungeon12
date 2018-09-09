using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Drawing
{

    public static class SaveLoadWindows
    {
        public static bool DrawLoad
        {
            set
            {
                DrawLeftWin = true;
                DrawRightWin = true;

                int height = 0;

                Console.ForegroundColor = Rogue.RAM.CUIColor;

                string Clear = string.Empty;
                Clear = DrawHelp.FullLine(82, DrawHelp.Border(true, 4), 27);
                Clear = " " + DrawHelp.Border(true, 1) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 10) + "═╗";
                Console.SetCursorPosition(20, height);
                Console.WriteLine(Clear);

                //тело                
                for (int q = 1; q < 27; q++)
                {
                    Clear = DrawHelp.FullLine(82, " ", 2);
                    Clear = " " + DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 26) + DrawHelp.Border(true, 3) + " ║";
                    Console.SetCursorPosition(20, q);
                    Console.WriteLine(Clear);
                }

                //носки 
                Clear = string.Empty;
                Clear = DrawHelp.FullLine(82, DrawHelp.Border(true, 4), 27);
                Clear = " " + DrawHelp.Border(true, 5) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 9) + "═╝";
                Console.SetCursorPosition(20, 26);
                Console.WriteLine(Clear);

                //носки объявления 
                Clear = string.Empty;
                Clear = DrawHelp.FullLine(82, DrawHelp.Border(true, 4), 27);
                Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 7) + "▓";
                Console.SetCursorPosition(20, 2);
                Console.WriteLine(Clear);

                CharacterDraw.DrawHeader("Загрузить");

                List<SystemEngine.SaveLoadFile> fl = SystemEngine.Helper.File.LoadFiles;

                int i = 0;
                int fc = 0;
                foreach (SystemEngine.SaveLoadFile f in fl)
                {
                    fc++;
                    if (fc < 7)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.SetCursorPosition(28, 3 + i);
                        Console.Write("┌───┐");
                        Console.SetCursorPosition(29 + "┌───┐".Length, 3 + i);
                        Console.ForegroundColor = f.FileType;
                        Console.Write(f.Title);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.SetCursorPosition(28, 4 + i);
                        Console.Write("│   │ " + f.Path);
                        Console.ForegroundColor = f.Color;
                        Console.SetCursorPosition(30, 4 + i);
                        Console.Write(f.Icon);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.SetCursorPosition(28, 5 + i);
                        Console.Write("└───┘");
                        Console.ForegroundColor = f.FileStatus;
                        Console.SetCursorPosition(29 + "┌───┐".Length, 5 + i);
                        Console.Write(f.Status);
                        Console.ForegroundColor = Rogue.RAM.CUIColor;
                        Clear = DrawHelp.FullLine(82, DrawHelp.Border(true, 4), 27);
                        Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 7);
                        Console.SetCursorPosition(20, 6 + i);
                        Console.WriteLine(Clear);
                        i = i + 4;
                    }
                }
                //носки 
                Clear = string.Empty;
                Clear = DrawHelp.FullLine(82, DrawHelp.Border(true, 4), 27);
                Clear = " " + DrawHelp.Border(true, 5) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 9) + "═╝";
                Console.SetCursorPosition(20, 26);
                Console.WriteLine(Clear);
            }
        }

        public static bool DrawSave
        {
            set
            {
                DrawLeftWin = true;
                DrawRightWinSave();

                int height = 0;

                Console.ForegroundColor = Rogue.RAM.CUIColor;

                string Clear = string.Empty;
                Clear = DrawHelp.FullLine(82, DrawHelp.Border(true, 4), 27);
                Clear = " " + DrawHelp.Border(true, 1) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 10) + "═╗";
                Console.SetCursorPosition(20, height);
                Console.WriteLine(Clear);

                //тело                
                for (int q = 1; q < 27; q++)
                {
                    Clear = DrawHelp.FullLine(82, " ", 2);
                    Clear = " " + DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 26) + DrawHelp.Border(true, 3) + " ║";
                    Console.SetCursorPosition(20, q);
                    Console.WriteLine(Clear);
                }

                //носки 
                Clear = string.Empty;
                Clear = DrawHelp.FullLine(82, DrawHelp.Border(true, 4), 27);
                Clear = " " + DrawHelp.Border(true, 5) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 9) + "═╝";
                Console.SetCursorPosition(20, 26);
                Console.WriteLine(Clear);

                //носки объявления 
                Clear = string.Empty;
                Clear = DrawHelp.FullLine(82, DrawHelp.Border(true, 4), 27);
                Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 7) + "▓";
                Console.SetCursorPosition(20, 2);
                Console.WriteLine(Clear);

                CharacterDraw.DrawHeader("Сохранить");

                List<SystemEngine.SaveLoadFile> fl = SystemEngine.Helper.File.LoadFiles;

                int i = 0;
                int fc = 0;
                foreach (SystemEngine.SaveLoadFile f in fl)
                {
                    fc++;
                    if (fc < 7)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.SetCursorPosition(28, 3 + i);
                        Console.Write("┌───┐");
                        Console.SetCursorPosition(29 + "┌───┐".Length, 3 + i);
                        Console.ForegroundColor = f.FileType;
                        Console.Write(f.Title);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.SetCursorPosition(28, 4 + i);
                        Console.Write("│   │ " + f.Path);
                        Console.ForegroundColor = f.Color;
                        Console.SetCursorPosition(30, 4 + i);
                        Console.Write(f.Icon);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.SetCursorPosition(28, 5 + i);
                        Console.Write("└───┘");
                        Console.ForegroundColor = f.FileStatus;
                        Console.SetCursorPosition(29 + "┌───┐".Length, 5 + i);
                        Console.Write(f.Status);
                        Console.ForegroundColor = Rogue.RAM.CUIColor;
                        Clear = DrawHelp.FullLine(82, DrawHelp.Border(true, 4), 27);
                        Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 7);
                        Console.SetCursorPosition(20, 6 + i);
                        Console.WriteLine(Clear);
                        i = i + 4;
                    }
                }
                //носки 
                Clear = string.Empty;
                Clear = DrawHelp.FullLine(82, DrawHelp.Border(true, 4), 27);
                Clear = " " + DrawHelp.Border(true, 5) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 9) + "═╝";
                Console.SetCursorPosition(20, 26);
                Console.WriteLine(Clear);
            }
        }

        public static bool DrawLeftWin
        {
            set
            {
                int height = 0;

                Console.ForegroundColor = Rogue.RAM.CUIColor;

                string Clear = string.Empty;
                Clear = DrawHelp.FullLine(17, DrawHelp.Border(true, 4), 1);
                Clear = " " + DrawHelp.Border(true, 1) + Clear.Remove(Clear.Length - 1) + "══╗";
                Console.SetCursorPosition(0, height);
                Console.WriteLine(Clear);

                //тело                
                for (int q = 1; q < 27; q++)
                {
                    Clear = DrawHelp.FullLine(17, " ", 1);
                    Clear = " " + DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 1) + "  ║";
                    Console.SetCursorPosition(0, q);
                    Console.WriteLine(Clear);
                }

                //носки 
                Clear = string.Empty;
                Clear = DrawHelp.FullLine(17, DrawHelp.Border(true, 4), 1);
                Clear = " " + DrawHelp.Border(true, 5) + Clear.Remove(Clear.Length - 1) + "══╝";
                Console.SetCursorPosition(0, 26);
                Console.WriteLine(Clear);

                //носки объявления 
                Clear = string.Empty;
                Clear = DrawHelp.FullLine(17, DrawHelp.Border(true, 4), 1);
                Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 1) + "══╣";
                Console.SetCursorPosition(0, 2);
                Console.WriteLine(Clear);

                int Count = (20 / 2) - ("Инфо".Length / 2);
                Console.SetCursorPosition(Count + 1, 1);
                Console.WriteLine(DrawHelp.FullLine("Инфо".Length, "Инфо", "Инфо".Length - 1));

                Console.SetCursorPosition(5, 4);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(" Тип файла");

                Console.SetCursorPosition(3, 6);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("▓");
                Console.SetCursorPosition(5, 6);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("- Сох. игра");

                Console.SetCursorPosition(3, 8);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("▓");
                Console.SetCursorPosition(5, 8);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("- Бонус кон.");

                Console.SetCursorPosition(3, 10);
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write("▓");
                Console.SetCursorPosition(5, 10);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("- Фан модуль");

                Console.SetCursorPosition(3, 14);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Состояние файла");

                Console.SetCursorPosition(3, 16);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("▓");
                Console.SetCursorPosition(5, 16);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("- Целый файл");

                Console.SetCursorPosition(3, 18);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("▓");
                Console.SetCursorPosition(5, 18);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("- Файл повреж");
            }
        }

        public static bool DrawRightWin
        {
            set
            {
                int height = 0;

                Console.ForegroundColor = Rogue.RAM.CUIColor;

                string Clear = string.Empty;
                Clear = DrawHelp.FullLine(17, DrawHelp.Border(true, 4), 1);
                Clear = " " + DrawHelp.Border(true, 1) + Clear.Remove(Clear.Length - 1) + "══╗";
                Console.SetCursorPosition(79, height);
                Console.WriteLine(Clear);

                //тело                
                for (int q = 1; q < 27; q++)
                {
                    Clear = DrawHelp.FullLine(17, " ", 1);
                    Clear = " " + DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 1) + "  ║";
                    Console.SetCursorPosition(79, q);
                    Console.WriteLine(Clear);
                }

                //носки 
                Clear = string.Empty;
                Clear = DrawHelp.FullLine(17, DrawHelp.Border(true, 4), 1);
                Clear = " " + DrawHelp.Border(true, 5) + Clear.Remove(Clear.Length - 1) + "══╝";
                Console.SetCursorPosition(79, 26);
                Console.WriteLine(Clear);

                //носки объявления 
                Clear = string.Empty;
                Clear = DrawHelp.FullLine(17, DrawHelp.Border(true, 4), 1);
                Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 1) + "══╣";
                Console.SetCursorPosition(79, 2);
                Console.WriteLine(Clear);

                int Count = (178 / 2) - ("Действия".Length / 2);
                Console.SetCursorPosition(Count + 1, 1);
                Console.WriteLine(DrawHelp.FullLine("Действия".Length, "Действия", "Действия".Length - 1));

                Console.SetCursorPosition(84, 4);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(" Загрузить");

                Console.SetCursorPosition(83, 6);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("{1}");
                Console.SetCursorPosition(87, 6);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("- Файл");

                Console.SetCursorPosition(83, 8);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("{2}");
                Console.SetCursorPosition(87, 8);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("- Файл");

                Console.SetCursorPosition(83, 10);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("{3}");
                Console.SetCursorPosition(87, 10);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("- Файл");

                Console.SetCursorPosition(83, 12);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("{4}");
                Console.SetCursorPosition(87, 12);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("- Файл");

                Console.SetCursorPosition(83, 14);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("{5}");
                Console.SetCursorPosition(87, 14);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("- Файл");

                Console.SetCursorPosition(83, 16);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("{6}");
                Console.SetCursorPosition(87, 16);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("- Файл");

                Console.SetCursorPosition(82, 18);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("{Esc}");
                Console.SetCursorPosition(88, 18);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("- В меню");
            }
        }

        public static void DrawRightWinSave()
        {
            int height = 0;

            Console.ForegroundColor = Rogue.RAM.CUIColor;

            string Clear = string.Empty;
            Clear = DrawHelp.FullLine(17, DrawHelp.Border(true, 4), 1);
            Clear = " " + DrawHelp.Border(true, 1) + Clear.Remove(Clear.Length - 1) + "══╗";
            Console.SetCursorPosition(79, height);
            Console.WriteLine(Clear);

            //тело                
            for (int q = 1; q < 27; q++)
            {
                Clear = DrawHelp.FullLine(17, " ", 1);
                Clear = " " + DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 1) + "  ║";
                Console.SetCursorPosition(79, q);
                Console.WriteLine(Clear);
            }

            //носки 
            Clear = string.Empty;
            Clear = DrawHelp.FullLine(17, DrawHelp.Border(true, 4), 1);
            Clear = " " + DrawHelp.Border(true, 5) + Clear.Remove(Clear.Length - 1) + "══╝";
            Console.SetCursorPosition(79, 26);
            Console.WriteLine(Clear);

            //носки объявления 
            Clear = string.Empty;
            Clear = DrawHelp.FullLine(17, DrawHelp.Border(true, 4), 1);
            Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 1) + "══╣";
            Console.SetCursorPosition(79, 2);
            Console.WriteLine(Clear);

            int Count = (178 / 2) - ("Действия".Length / 2);
            Console.SetCursorPosition(Count + 1, 1);
            Console.WriteLine(DrawHelp.FullLine("Действия".Length, "Действия", "Действия".Length - 1));

            Console.SetCursorPosition(84, 4);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" Сохранить");

            Console.SetCursorPosition(83, 6);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("{1}");
            Console.SetCursorPosition(87, 6);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("- Файл");

            Console.SetCursorPosition(83, 8);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("{2}");
            Console.SetCursorPosition(87, 8);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("- Файл");

            Console.SetCursorPosition(83, 10);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("{3}");
            Console.SetCursorPosition(87, 10);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("- Файл");

            Console.SetCursorPosition(83, 12);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("{4}");
            Console.SetCursorPosition(87, 12);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("- Файл");

            Console.SetCursorPosition(83, 14);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("{5}");
            Console.SetCursorPosition(87, 14);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("- Файл");

            Console.SetCursorPosition(83, 16);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("{6}");
            Console.SetCursorPosition(87, 16);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("- Файл");

            Console.SetCursorPosition(82, 18);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("{Esc}");
            Console.SetCursorPosition(88, 18);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("- В меню");
        }
    }
}
