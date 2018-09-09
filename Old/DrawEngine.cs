using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.Globalization;
using Microsoft.Win32.SafeHandles;

namespace Rogue
{
    public static class DrawEngine
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

        public static class PopUpWindowDraw
        {
            public static void Trap_pt1()
            {
                GUIDraw.DrawLab();
                PopUpWindowV1("Выберите тип ловушки", 4, new List<string> { "Механическая", "Магическая", "Огненная", "Ядовитая" });
            }
            /// <summary>
            /// Draw aligment window
            /// </summary>
            public static void Banishment_pt1()
            {
                GUIDraw.DrawLab();
                PopUpWindowV1("Выберите мировоззрение цели", 2, new List<string> { "Добро", "Зло" });
            }
            /// <summary>
            /// Draw direction window
            /// </summary>
            public static void Banishment_pt2()
            {
                GUIDraw.DrawLab();
                Rogue.RAM.PopUpTab.NowTab = 1;
                PopUpWindowVA("Направление");
            }
            /// <summary>
            /// Draw popup window version one
            /// </summary>
            private static void PopUpWindowV1(string Title, int Buttons, List<string> bTitles)
            {

                Console.ForegroundColor = ConsoleColor.Gray;

                //top
                string Clear = string.Empty;
                Clear = DrawHelp.FullLine(75, DrawHelp.Border(true, 4), 27);
                Clear = DrawHelp.Border(true, 1) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 2);
                Console.SetCursorPosition(22, 2);
                Console.WriteLine(Clear);

                //ForTitle
                Clear = DrawHelp.FullLine(75, " ", 2);
                Clear = DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 27) + DrawHelp.Border(true, 3);
                Console.SetCursorPosition(22, 3);
                Console.WriteLine(Clear);

                Clear = string.Empty;
                Clear = DrawHelp.FullLine(75, DrawHelp.Border(true, 0, "WallHor"), 27);
                Clear = DrawHelp.Border(true, 0, "ThreeRight") + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 0, "ThreeLeft");
                Console.SetCursorPosition(22, 4);
                Console.WriteLine(Clear);

                int q = 0;
                for (int i = 5; i < 6 + (Buttons * 3); i++)
                {
                    Clear = DrawHelp.FullLine(75, " ", 2);
                    Clear = DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 27) + DrawHelp.Border(true, 3);
                    Console.SetCursorPosition(22, i);
                    Console.WriteLine(Clear);
                    q = i;
                }

                //bot
                Clear = string.Empty;
                Clear = DrawHelp.FullLine(75, DrawHelp.Border(true, 4), 27);
                Clear = DrawHelp.Border(true, 5) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 6);
                Console.SetCursorPosition(22, q);
                Console.WriteLine(Clear);

                DrawPopUpHeader(Title);

                ForReDraw = bTitles;
                foreach (string s in bTitles)
                { DrawButton(s, bTitles.IndexOf(s)); }
            }
            /// <summary>
            /// Draw title in PopUpWindowV1
            /// </summary>
            /// <param name="Text">Title string</param>
            private static void DrawPopUpHeader(string Text)
            {
                int Count = (50 / 2) - (Text.Length / 2);
                Console.SetCursorPosition(Count + 22, 3);
                Console.WriteLine(DrawHelp.FullLine(Text.Length, Text, Text.Length - 1));
            }
            /// <summary>
            /// Draw button on popUp window
            /// </summary>
            /// <param name="Title">Button title</param>
            /// <param name="Position">Button position {Be care with this param}</param>
            private static void DrawButton(string Title, int Position)
            {
                bool MyCoderIdiot = false;
                if (Position == Rogue.RAM.PopUpTab.NowTab) { Console.ForegroundColor = ConsoleColor.Yellow; MyCoderIdiot = true; } else { Console.ForegroundColor = ConsoleColor.Gray; }

                //top
                string Clear = string.Empty;
                Clear = DrawHelp.FullLine(69, DrawHelp.Border(false, 4, "WallHor"), 27);
                Clear = DrawHelp.Border(MyCoderIdiot, 1, "TopCornLeft") + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(MyCoderIdiot, 2, "TopCornRight");
                Console.SetCursorPosition(25, 5 + (Position * 3));
                Console.WriteLine(Clear);
                //bod
                Clear = DrawHelp.FullLine(69, " ", 27);
                Clear = DrawHelp.Border(false, 1, "WallVert") + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(false, 2, "WallVert");
                Console.SetCursorPosition(25, 6 + (Position * 3));
                Console.WriteLine(Clear);
                //title
                int Count = (50 / 2) - (Title.Length / 2);
                Console.SetCursorPosition(Count + 21, 6 + (Position * 3));
                Console.WriteLine(DrawHelp.FullLine(Title.Length, Title, Title.Length - 1));
                //bot
                Clear = DrawHelp.FullLine(69, DrawHelp.Border(false, 4, "WallHor"), 27);
                Clear = DrawHelp.Border(MyCoderIdiot, 1, "BotCornLeft") + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(MyCoderIdiot, 2, "BotCornRight");
                Console.SetCursorPosition(25, 7 + (Position * 3));
                Console.WriteLine(Clear);
            }
            /// <summary>
            /// Draw pop up window with direction arrows
            /// </summary>
            /// <param name="Title"></param>
            private static void PopUpWindowVA(string Title)
            {
                Console.ForegroundColor = ConsoleColor.Gray;

                //top
                string Clear = string.Empty;
                Clear = DrawHelp.FullLine(50, DrawHelp.Border(true, 4), 27);
                Clear = DrawHelp.Border(true, 1) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 2);
                Console.SetCursorPosition(36, 2);
                Console.WriteLine(Clear);

                //ForTitle
                Clear = DrawHelp.FullLine(50, " ", 2);
                Clear = DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 27) + DrawHelp.Border(true, 3);
                Console.SetCursorPosition(36, 3);
                Console.WriteLine(Clear);

                Clear = string.Empty;
                Clear = DrawHelp.FullLine(50, DrawHelp.Border(true, 0, "WallHor"), 27);
                Clear = DrawHelp.Border(true, 0, "ThreeRight") + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 0, "ThreeLeft");
                Console.SetCursorPosition(36, 4);
                Console.WriteLine(Clear);

                int q = 0;
                for (int i = 5; i < 7 + (2 * 3); i++)
                {
                    Clear = DrawHelp.FullLine(50, " ", 2);
                    Clear = DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 27) + DrawHelp.Border(true, 3);
                    Console.SetCursorPosition(36, i);
                    Console.WriteLine(Clear);
                    q = i;
                }

                //bot
                Clear = string.Empty;
                Clear = DrawHelp.FullLine(50, DrawHelp.Border(true, 4), 27);
                Clear = DrawHelp.Border(true, 5) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 6);
                Console.SetCursorPosition(36, q);
                Console.WriteLine(Clear);

                DrawPopUpHeader(Title);
                DrawArrowButton(1);
                //foreach (string s in bTitles)
                //{ DrawButton(s, bTitles.IndexOf(s)); }
            }
            /// <summary>
            /// Draw buttons on PopUpWindowVa
            /// </summary>
            private static void DrawArrowButton(int Position)
            {
                bool[] Walls = new bool[] { false, false, false, false };
                ConsoleColor[] Col = new ConsoleColor[] { ConsoleColor.Gray, ConsoleColor.Gray, ConsoleColor.Gray, ConsoleColor.Gray };
                if (Position == 1) { Walls[0] = true; Col[0] = ConsoleColor.Yellow; }
                if (Position == 2) { Walls[1] = true; Col[1] = ConsoleColor.Yellow; }
                if (Position == 3) { Walls[2] = true; Col[2] = ConsoleColor.Yellow; }
                if (Position == 4) { Walls[3] = true; Col[3] = ConsoleColor.Yellow; }

                //upper
                Console.ForegroundColor = Col[0];
                string Clear = DrawHelp.Border(Walls[0], 0, "TopCornLeft") + "───" + DrawHelp.Border(Walls[0], 0, "TopCornRight");
                Console.SetCursorPosition(45, 5);
                Console.WriteLine(Clear);
                Clear = "│ ↑ │";
                Console.SetCursorPosition(45, 6);
                Console.WriteLine(Clear);
                Clear = DrawHelp.Border(Walls[0], 0, "BotCornLeft") + "───" + DrawHelp.Border(Walls[0], 0, "BotCornRight");
                Console.SetCursorPosition(45, 7);
                Console.WriteLine(Clear);

                //midleft
                Console.ForegroundColor = Col[1];
                Clear = DrawHelp.Border(Walls[1], 0, "TopCornLeft") + "───" + DrawHelp.Border(Walls[1], 0, "TopCornRight");
                Console.SetCursorPosition(39, 7);
                Console.WriteLine(Clear);
                Clear = "│ ← │";
                Console.SetCursorPosition(39, 8);
                Console.WriteLine(Clear);
                Clear = DrawHelp.Border(Walls[1], 0, "BotCornLeft") + "───" + DrawHelp.Border(Walls[1], 0, "BotCornRight");
                Console.SetCursorPosition(39, 9);
                Console.WriteLine(Clear);

                //midright
                Console.ForegroundColor = Col[2];
                Clear = DrawHelp.Border(Walls[2], 0, "TopCornLeft") + "───" + DrawHelp.Border(Walls[2], 0, "TopCornRight");
                Console.SetCursorPosition(51, 7);
                Console.WriteLine(Clear);
                Clear = "│ → │";
                Console.SetCursorPosition(51, 8);
                Console.WriteLine(Clear);
                Clear = DrawHelp.Border(Walls[2], 0, "BotCornLeft") + "───" + DrawHelp.Border(Walls[2], 0, "BotCornRight");
                Console.SetCursorPosition(51, 9);
                Console.WriteLine(Clear);

                //bottom
                Console.ForegroundColor = Col[3];
                Clear = DrawHelp.Border(Walls[3], 0, "TopCornLeft") + "───" + DrawHelp.Border(Walls[3], 0, "TopCornRight");
                Console.SetCursorPosition(45, 9);
                Console.WriteLine(Clear);
                Clear = "│ ↓ │";
                Console.SetCursorPosition(45, 10);
                Console.WriteLine(Clear);
                Clear = DrawHelp.Border(Walls[3], 0, "BotCornLeft") + "───" + DrawHelp.Border(Walls[3], 0, "BotCornRight");
                Console.SetCursorPosition(45, 11);
                Console.WriteLine(Clear);
            }
            /// <summary>
            /// use for redraw
            /// </summary>
            private static List<string> ForReDraw;
            /// <summary>
            /// ReDraw choosen popup window with arrow direction
            /// </summary>
            /// <param name="PopUp">Version of popUp window</param>
            /// <param name="Dir">Arrow direction</param>
            public static void ReDrawPopUp(SystemEngine.PopUpWindow PopUp, SystemEngine.ArrowDirection Dir, MechEngine.BattleClass Class)
            {
                switch (PopUp)
                {
                    case SystemEngine.PopUpWindow.V1:
                        {
                            if (Class == MechEngine.BattleClass.Inquisitor)
                            {
                                switch (Dir)
                                {
                                    case SystemEngine.ArrowDirection.Top: { if (Rogue.RAM.PopUpTab.NowTab > 0) { Rogue.RAM.PopUpTab.NowTab -= 1; } break; }
                                    case SystemEngine.ArrowDirection.Bot: { if (Rogue.RAM.PopUpTab.NowTab < 1) { Rogue.RAM.PopUpTab.NowTab += 1; } break; }
                                }
                                foreach (string s in ForReDraw)
                                { DrawButton(s, ForReDraw.IndexOf(s)); }

                            }
                            else if (Class == MechEngine.BattleClass.Assassin)
                            {
                                switch (Dir)
                                {
                                    case SystemEngine.ArrowDirection.Top: { if (Rogue.RAM.PopUpTab.NowTab > 0) { Rogue.RAM.PopUpTab.NowTab -= 1; } break; }
                                    case SystemEngine.ArrowDirection.Bot: { if (Rogue.RAM.PopUpTab.NowTab < 3) { Rogue.RAM.PopUpTab.NowTab += 1; } break; }
                                }
                                foreach (string s in ForReDraw)
                                { DrawButton(s, ForReDraw.IndexOf(s)); }
                            }

                            break;
                        }
                    case SystemEngine.PopUpWindow.VA:
                        {
                            switch (Dir)
                            {
                                case SystemEngine.ArrowDirection.Top: { if (Rogue.RAM.PopUpTab.NowTab == 2 || Rogue.RAM.PopUpTab.NowTab == 3 || Rogue.RAM.PopUpTab.NowTab == 4) { DrawArrowButton(1); Rogue.RAM.PopUpTab.NowTab = 1; } break; }
                                case SystemEngine.ArrowDirection.Bot: { if (Rogue.RAM.PopUpTab.NowTab == 2 || Rogue.RAM.PopUpTab.NowTab == 3 || Rogue.RAM.PopUpTab.NowTab == 1) { DrawArrowButton(4); Rogue.RAM.PopUpTab.NowTab = 4; } break; }
                                case SystemEngine.ArrowDirection.Left: { if (Rogue.RAM.PopUpTab.NowTab == 1 || Rogue.RAM.PopUpTab.NowTab == 3 || Rogue.RAM.PopUpTab.NowTab == 4) { DrawArrowButton(2); Rogue.RAM.PopUpTab.NowTab = 2; } break; }
                                case SystemEngine.ArrowDirection.Right: { if (Rogue.RAM.PopUpTab.NowTab == 1 || Rogue.RAM.PopUpTab.NowTab == 2 || Rogue.RAM.PopUpTab.NowTab == 4) { DrawArrowButton(3); Rogue.RAM.PopUpTab.NowTab = 3; } break; }
                            }
                            break;
                        }
                }
            }

            public static void SwitchCraftAbilWindow(int index, List<MechEngine.Ability> Abils)
            {
                Console.ForegroundColor = Rogue.RAM.CUIColor;
                #region window
                string Clear = string.Empty;
                Clear = DrawHelp.FullLine(75, DrawHelp.Border(true, 4), 18);
                Clear = DrawHelp.Border(true, 1) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 2);
                Console.SetCursorPosition(20, 9);
                Console.WriteLine(Clear);

                //ForTitle
                Clear = DrawHelp.FullLine(75, " ", 2);
                Clear = DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 18) + DrawHelp.Border(true, 3);
                Console.SetCursorPosition(20, 10);
                Console.WriteLine(Clear);
                Clear = DrawHelp.FullLine(75, DrawHelp.Border(true, 0, "WallHor"), 18);
                Clear = DrawHelp.Border(true, 0, "ThreeRight") + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 0, "ThreeLeft");
                Console.SetCursorPosition(20, 11);
                Console.WriteLine(Clear);

                int Count = (50 / 2) - ("Выберите способность которую надо заменить".Length / 2);
                Console.SetCursorPosition(27, 10);
                Console.WriteLine(DrawHelp.FullLine("Выберите способность которую надо заменить".Length, "Выберите способность которую надо заменить", "Выберите способность которую надо заменить".Length - 1));

                int q = 0;
                for (int i = 12; i < 16; i++)
                {
                    Clear = DrawHelp.FullLine(75, " ", 2);
                    Clear = DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 18) + DrawHelp.Border(true, 3);
                    Console.SetCursorPosition(20, i);
                    Console.WriteLine(Clear);
                    q = i;
                }

                //bot
                Clear = string.Empty;
                Clear = DrawHelp.FullLine(75, DrawHelp.Border(true, 4), 18);
                Clear = DrawHelp.Border(true, 5) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 6);
                Console.SetCursorPosition(20, q);
                Console.WriteLine(Clear);
                #endregion

                bool[] bord = new bool[] { false, false, false };
                if (index == 0) { bord[0] = true; }
                if (index == 1) { bord[1] = true; }
                if (index == 2) { bord[2] = true; }

                for (int i = 0; i < 3; i++)
                {
                    Console.ForegroundColor = Rogue.RAM.CUIColor;
                    ConsoleColor Col = Rogue.RAM.CUIColor;
                    int left = 22;
                    if (i == 1) { left = 39; }
                    if (i == 2) { left = 56; }
                    //if (index == i)
                    //{
                    //    Col = ConsoleColor.Magenta;
                    //}
                    Console.SetCursorPosition(left, 12);
                    Console.ForegroundColor = Col;
                    Console.Write(DrawHelp.Border(bord[i], 0, "TopCornLeft"));
                    Console.SetCursorPosition(left + 1, 12);
                    Console.ForegroundColor = Rogue.RAM.CUIColor;
                    Console.Write(DrawHelp.Border(false, 0, "WallHor") + DrawHelp.Border(false, 0, "WallHor") + DrawHelp.Border(false, 0, "WallHor"));
                    Console.SetCursorPosition(left + 4, 12);
                    Console.ForegroundColor = Col;
                    Console.Write(DrawHelp.Border(bord[i], 0, "TopCornRight"));

                    Console.SetCursorPosition(left + 6, 12);
                    Console.ForegroundColor = Abils[i].Color;
                    Console.Write(Abils[i].Name);
                    Console.ForegroundColor = Rogue.RAM.CUIColor;

                    Console.SetCursorPosition(left, 13);
                    Console.Write(DrawHelp.Border(false, 0, "WallVert") + "   " + DrawHelp.Border(false, 0, "WallVert"));

                    Console.SetCursorPosition(left + 2, 13);
                    Console.ForegroundColor = Abils[i].Color;
                    Console.Write(Abils[i].Icon);
                    Console.ForegroundColor = Rogue.RAM.CUIColor;

                    Console.SetCursorPosition(left + 6, 13);
                    Console.ForegroundColor =  ConsoleColor.Gray;
                    Console.Write("LVL: "+Abils[i].Level.ToString());
                    Console.ForegroundColor = Rogue.RAM.CUIColor;

                    Console.SetCursorPosition(left, 14);
                    Console.ForegroundColor = Col;
                    Console.Write(DrawHelp.Border(bord[i], 0, "BotCornLeft"));
                    Console.SetCursorPosition(left+1, 14);
                    Console.ForegroundColor = Rogue.RAM.CUIColor;
                    Console.Write(DrawHelp.Border(false, 0, "WallHor") + DrawHelp.Border(false, 0, "WallHor") + DrawHelp.Border(false, 0, "WallHor"));
                    Console.ForegroundColor = Col;
                    Console.SetCursorPosition(left+4, 14);
                    Console.Write(DrawHelp.Border(bord[i], 0, "BotCornRight"));

                    Console.SetCursorPosition(left + 6, 14);
                    if (Abils[i].COE < 10) { Console.ForegroundColor = ConsoleColor.DarkGray; }
                    if (Abils[i].COE > 25) { Console.ForegroundColor = ConsoleColor.Green; }
                    if (Abils[i].COE > 50) { Console.ForegroundColor = ConsoleColor.Yellow; }
                    if (Abils[i].COE > 80) { Console.ForegroundColor = ConsoleColor.Red; }
                    Console.Write("COE: " + Abils[i].COE.ToString() + "%");
                }

                Console.SetCursorPosition(22, 14);
            }
        }

        public static class GUIDraw
        {
            /// <summary>
            /// Draw or ReDraw labirinth
            /// </summary>
            public static void DrawLab()
            {
                #region Map window

                int height = 0;

                Console.ForegroundColor = Rogue.RAM.CUIColor;

                string Clear = string.Empty;
                Clear = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 27);
                Clear = DrawHelp.Border(true, 1) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 2);
                Console.SetCursorPosition(1, height);
                Console.WriteLine(Clear);

                //тело                
                for (int i = 1; i < 24; i++)
                {
                    Clear = DrawHelp.FullLine(100, " ", 2);
                    Clear = DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 27) + DrawHelp.Border(true, 3);
                    Console.SetCursorPosition(1, i);
                    Console.WriteLine(Clear);
                }

                //носки 
                Clear = string.Empty;
                Clear = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 27);
                Clear = DrawHelp.Border(true, 5) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 6);
                Console.SetCursorPosition(1, 24);
                Console.WriteLine(Clear);

                #endregion
                winAPIDraw.DrawLab();
                return;
                #region OldDraw
                //var Lab = Rogue.RAM.Map;
                //for (int y = 0; y < 23; y++)
                //{
                //    for (int x = 0; x < 71; x++)
                //    {
                //        Console.ForegroundColor = Rogue.RAM.Map.Biom;
                //        if (Lab.Map[x][y].Enemy != null)
                //        {
                //            Console.ForegroundColor = Lab.Map[x][y].Enemy.Chest;
                //            Lab.Map[x][y].Vision = Lab.Map[x][y].Enemy.Icon;
                //        }
                //        else if (Lab.Map[x][y].Player != null)
                //        {
                //            Console.ForegroundColor = ConsoleColor.Red;
                //        }
                //        else if (Lab.Map[x][y].Loot != null)
                //        {
                //            Console.ForegroundColor = Lab.Map[x][y].Loot.Color;
                //            Lab.Map[x][y].Vision = Lab.Map[x][y].Loot.Icon();
                //        }
                //        else if (Lab.Map[x][y].Door != null)
                //        {
                //            if (Lab.Map[x][y].Door.Name == "Exit")
                //            {
                //                Console.ForegroundColor = ConsoleColor.DarkRed;
                //                Lab.Map[x][y].Vision = Lab.Map[x][y].Door.IconExit;
                //            }
                //            else
                //            {
                //                Console.ForegroundColor = Lab.Map[x][y].Door.TypeDoor;
                //                Lab.Map[x][y].Vision = Lab.Map[x][y].Door.Icon;
                //            }
                //        }
                //        else if (Lab.Map[x][y].Door == null && Lab.Map[x][y].Enemy == null && Lab.Map[x][y].Loot == null && Lab.Map[x][y].Player == null && Lab.Map[x][y].Wall == null && Lab.Map[x][y].Trap == null)
                //        {
                //            Lab.Map[x][y].Vision = ' ';
                //        }
                //        else if (Lab.Map[x][y].Trap != null)
                //        {
                //            Console.ForegroundColor = ConsoleColor.DarkGray;
                //            Lab.Map[x][y].Vision = Lab.Map[x][y].Trap.icon;
                //        }

                //        Console.SetCursorPosition(x + 2, y + 1);
                //        Console.Write(Lab.Map[x][y].Vision);
                //        Console.ForegroundColor = Rogue.RAM.Map.Biom;
                //    }
                //}
                //DrawEngine.InfoWindow.Location(Rogue.RAM.Map.Name);                
                #endregion
            }

            public static void DrawGUI(bool InfoWindow = true)
            {
                //if (!InfoWindow) { if (GUI.EnemyMoves.CheckMove()) { GUI.EnemyMoves.Move(false); } }

                MechEngine.Character Player = Rogue.RAM.Player;

                #region Полезности

                //char s = Encoding.GetEncoding(437).GetChars(new byte[] { 180 })[0];                                
                //int width = 25;//Convert.ToInt32(Convert.ToDouble(Console.WindowWidth)*0.25);  

                #endregion

                #region Character window

                int height = 24;
                Console.ForegroundColor = Rogue.RAM.CUIColor;

                string Clear = string.Empty;
                Clear = DrawHelp.FullLine(25, DrawHelp.Border(true, 4));
                Clear = DrawHelp.Border(true, 1) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 2);
                Console.SetCursorPosition(75, 0);
                Console.WriteLine(Clear);

                //тело
                for (int i = 1; i < height; i++)
                {
                    Clear = DrawHelp.FullLine(25, " ");
                    Clear = DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 3);
                    Console.SetCursorPosition(75, i);
                    Console.WriteLine(Clear);
                }

                //носки 
                Clear = string.Empty;
                Clear = DrawHelp.FullLine(25, DrawHelp.Border(true, 4));
                Clear = DrawHelp.Border(true, 5) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 6);
                Console.SetCursorPosition(75, height);
                Console.WriteLine(Clear);

                #endregion

                if (InfoWindow)
                {
                    #region Bottom window

                    height = 25;
                    Console.ForegroundColor = Rogue.RAM.CUIColor;

                    Clear = string.Empty;
                    Clear = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 2);
                    Clear = DrawHelp.Border(true, 1) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 2);
                    Console.SetCursorPosition(1, height);
                    Console.WriteLine(Clear);

                    //тело                
                    Clear = DrawHelp.FullLine(100, " ", 2);
                    Clear = DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 3);
                    Console.SetCursorPosition(1, height + 1);
                    Console.WriteLine(Clear);

                    //носки 
                    Clear = string.Empty;
                    Clear = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 2);
                    Clear = DrawHelp.Border(true, 5) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 6);
                    Console.SetCursorPosition(1, height + 2);
                    Console.WriteLine(Clear);

                    #endregion
                }

                #region Character info #ANTIPATTERN

                DrawEngine.GUIDraw.ReDrawCharStat();

                #endregion

                FightDraw.ReDrawBuffDeBuff();

                //if (!InfoWindow) { if (!GUI.EnemyMoves.CheckMove()) { GUI.EnemyMoves.Move(true); } }
            }

            public static void ReDrawMapWindow()
            {
                int height = 0;

                Console.ForegroundColor = Rogue.RAM.CUIColor;

                string Clear = string.Empty;
                Clear = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 27);
                Clear = DrawHelp.Border(true, 1) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 2);
                Console.SetCursorPosition(1, height);
                Console.WriteLine(Clear);

                //тело                
                for (int i = 1; i < 24; i++)
                {
                    Clear = DrawHelp.FullLine(100, " ", 2);
                    Clear = DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 27) + DrawHelp.Border(true, 3);
                    Console.SetCursorPosition(1, i);
                    Console.WriteLine(Clear);
                }

                //носки 
                Clear = string.Empty;
                Clear = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 27);
                Clear = DrawHelp.Border(true, 5) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 6);
                Console.SetCursorPosition(1, 24);
                Console.WriteLine(Clear);
            }
            /// <summary>
            /// Draw ch stats, inventory, buffs/debuffs
            /// </summary>
            public static void ReDrawCharStat()
            {
                #region Character window

                Console.ForegroundColor = Rogue.RAM.CUIColor;

                string Clear = string.Empty;
                Clear = DrawHelp.FullLine(25, DrawHelp.Border(true, 4));
                Clear = DrawHelp.Border(true, 1) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 2);
                Console.SetCursorPosition(75, 0);
                Console.WriteLine(Clear);

                //тело
                for (int i = 1; i < 17; i++)
                {
                    Clear = DrawHelp.FullLine(25, " ");
                    Clear = DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 3);
                    Console.SetCursorPosition(75, i);
                    Console.WriteLine(Clear);
                }

                #endregion
                drawstat();
                FightDraw.ReDrawBuffDeBuff();
            }

            public static void drawstat()
            {
                MechEngine.Character Player = Rogue.RAM.Player;
                winAPIDraw.DrawRightWindow.Clear();
                //name
                int Count = (23 / 2) - (Rogue.RAM.Player.Name.Length / 2);
                winAPIDraw.DrawRightWindow.AddLine(Count, 1, Rogue.RAM.Player.Name, ConsoleColor.Cyan);

                //race class
                Count = (23 / 2) - ((Player.GetClassRace(1).Length + Player.GetClassRace(2).Length + 3) / 2);
                string WriteThis = Player.GetClassRace(1) + " - " + Player.GetClassRace(2);
                winAPIDraw.DrawRightWindow.AddLine(Count, 3, WriteThis, ConsoleColor.Gray);


                //level exp
                Count = (23 / 2) - ((4 + Player.Level.ToString().Length + 6 + Player.EXP.ToString().Length + 1 + Player.mEXP.ToString().Length) / 2);
                WriteThis = "LVL: " + Player.Level.ToString() + " EXP: " + Player.EXP.ToString() + "/" + Player.mEXP.ToString();
                winAPIDraw.DrawRightWindow.AddLine(Count, 5, WriteThis, ConsoleColor.DarkGray);

                //hp
                Count = (23 / 2) - ((7 + Player.CHP.ToString().Length + 1 + Player.MHP.ToString().Length) / 2);
                WriteThis = "Жизнь: " + Player.CHP.ToString() + "/" + Player.MHP.ToString();
                winAPIDraw.DrawRightWindow.AddLine(Count, 7, WriteThis, ConsoleColor.Red);

                //mp
                if (Player.Class != MechEngine.BattleClass.Warrior)
                {
                    Count = (23 / 2) - ((Player.ManaName.Length + Player.CMP.ToString().Length + 1 + Player.MMP.ToString().Length) / 2);
                    WriteThis = Player.ManaName + Player.CMP.ToString() + "/" + Player.MMP.ToString();
                }
                else
                {
                    Count = (23 / 2) - ((Player.ManaName.Length + Player.CMP.ToString().Length) / 2);
                    WriteThis = Player.ManaName + Player.CMP.ToString();
                }
                winAPIDraw.DrawRightWindow.AddLine(Count, 8, WriteThis, SystemEngine.Helper.Information.ClassC);

                //dmg
                Count = (23 / 2) - ((6 + Player.MIDMG.ToString().Length + 1 + Player.MADMG.ToString().Length) / 2);
                WriteThis = "Урон: " + Player.MIDMG.ToString() + "-" + Player.MADMG.ToString();
                winAPIDraw.DrawRightWindow.AddLine(Count, 10, WriteThis, ConsoleColor.DarkYellow);

                //ad
                Count = (23 / 2) - ((12 + Player.AD.ToString().Length) / 2);
                WriteThis = "Сила атаки: " + Player.AD.ToString();
                winAPIDraw.DrawRightWindow.AddLine(Count, 11, WriteThis, ConsoleColor.DarkRed);

                //ap
                Count = (23 / 2) - ((12 + Player.AP.ToString().Length) / 2);
                WriteThis = "Сила магии: " + Player.AP.ToString();
                winAPIDraw.DrawRightWindow.AddLine(Count, 12, WriteThis, ConsoleColor.DarkCyan);

                //arm
                Count = (23 / 2) - ((11 + Player.ARM.ToString().Length) / 2);
                WriteThis = "Защита Ф : " + Player.ARM.ToString();
                winAPIDraw.DrawRightWindow.AddLine(Count, 13, WriteThis, ConsoleColor.DarkGreen);

                //mrs
                Count = (23 / 2) - ((11 + Player.MRS.ToString().Length) / 2);
                WriteThis = "Защита М : " + Player.MRS.ToString();
                winAPIDraw.DrawRightWindow.AddLine(Count, 14, WriteThis, ConsoleColor.DarkMagenta);

                //money
                Count = (23 / 2) - ((3 + Player.Gold.ToString().Length) / 2);
                WriteThis = "$: " + Player.Gold.ToString();
                winAPIDraw.DrawRightWindow.AddLine(Count, 16, WriteThis, ConsoleColor.Yellow);

                //inv label
                Count = (23 / 2) - (("Инвентарь:".Length) / 2);
                winAPIDraw.DrawRightWindow.AddLine(Count, 18, "Инвентарь:", ConsoleColor.DarkRed);

                MechEngine.Item Empt = new MechEngine.Item();
                MechEngine.Item[] CI = Player.Inventory.ToArray();  //Current items
                for (int i = 0; i < 6; i++)
                {
                    try
                    {
                        string wolvowhat = CI[i].Name;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Array.Resize(ref CI, CI.Length + 1);
                        CI[i] = Empt;
                    }
                }

                //вещи
                MechEngine.Item[] M = Player.Inventory.ToArray();
                MechEngine.Item N = new MechEngine.Item();

                for (int i = 0; i < 6; i++)
                {
                    try
                    {
                        M[i].ToString();
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Array.Resize(ref M, M.Length + 1);
                        M[i] = N;
                    }
                }

                //n1
                //winAPIDraw.DrawLeftWindow.AddLine(82, 20, , M[0].Color);
                //n2
                //winAPIDraw.DrawLeftWindow.AddLine(86, 20, M[1].Icon().ToString(), M[1].Color);
                //n3
                //winAPIDraw.DrawLeftWindow.AddLine(90, 20, M[2].Icon().ToString(), M[2].Color);
                //n4
                //winAPIDraw.DrawLeftWindow.AddLine(82, 22, M[3].Icon().ToString(), M[3].Color);
                //n5
                //winAPIDraw.DrawLeftWindow.AddLine(86, 22, M[4].Icon().ToString(), M[4].Color);
                //n6
                //winAPIDraw.DrawLeftWindow.AddLine(90, 22, M[5].Icon().ToString(), M[5].Color);


                Count = (23 / 2) - (("┌───┬───┬───┐".Length) / 2);
                winAPIDraw.DrawRightWindow.AddLine(Count, 19, "┌───┬───┬───┐", ConsoleColor.DarkRed);
                winAPIDraw.DrawRightWindow.AddLine(Count, 20, "│ " + "`" + M[0].Color.ToString() + "`" + M[0].Icon().ToString() + " │ " + "`" + M[1].Color.ToString() + "`" + M[1].Icon().ToString() + " │ " + "`" + M[2].Color.ToString() + "`" + M[2].Icon().ToString() + " │", ConsoleColor.DarkRed);
                winAPIDraw.DrawRightWindow.AddLine(Count, 21, "├───┼───┼───┤", ConsoleColor.DarkRed);
                winAPIDraw.DrawRightWindow.AddLine(Count, 22, "│ " + "`" + M[3].Color.ToString() + "`" + M[3].Icon().ToString() + " │ " + "`" + M[4].Color.ToString() + "`" + M[4].Icon().ToString() + " │ " + "`" + M[5].Color.ToString() + "`" + M[5].Icon().ToString() + " │", ConsoleColor.DarkRed);
                winAPIDraw.DrawRightWindow.AddLine(Count, 23, "└───┴───┴───┘", ConsoleColor.DarkRed);

                winAPIDraw.DrawRightWindow.Draw = true;
            }

            public static void ReDrawCharInventory()
            {
                char[] oldchar = new char[] { '\0', '\0', '\0', '\0', '\0', '\0' };
                Int16[] oldcol = new short[] { 0,0,0,0,0,0 };

                for (int i = 0; i < 6; i++)
                {
                    try
                    {
                        oldchar[i] = Rogue.RAM.Player.Inventory[i].Icon();
                        oldcol[i] = Convert.ToInt16(Rogue.RAM.Player.Inventory[i].Color);
                    }
                    catch { }
                }


                winAPIDraw.Helpers.ReDrawOneChar(81, 19, 81, 19, oldchar[0], oldchar[0], oldcol[0], oldcol[0]);

                winAPIDraw.Helpers.ReDrawOneChar(85, 19, 85, 19, oldchar[1], oldchar[1], oldcol[1], oldcol[1]);

                winAPIDraw.Helpers.ReDrawOneChar(89, 19, 89, 19, oldchar[2], oldchar[2], oldcol[2], oldcol[2]);

                winAPIDraw.Helpers.ReDrawOneChar(81, 21, 81, 21, oldchar[3], oldchar[3], oldcol[3], oldcol[3]);

                winAPIDraw.Helpers.ReDrawOneChar(85, 21, 85, 21, oldchar[4], oldchar[4], oldcol[4], oldcol[4]);

                winAPIDraw.Helpers.ReDrawOneChar(89, 21, 89, 21, oldchar[5], oldchar[5], oldcol[5], oldcol[5]);

                //Console.ResetColor();
                //MechEngine.Character Player = Rogue.RAM.Player;
                //int Count = 0;

                //Console.ForegroundColor = ConsoleColor.DarkRed;
                //Count = (23 / 2) - (("Инвентарь:".Length) / 2);
                //Console.SetCursorPosition(Count + 75, 18);
                //Console.WriteLine("Инвентарь:");

                //MechEngine.Item Empt = new MechEngine.Item();
                //MechEngine.Item[] CI = Player.Inventory.ToArray();  //Current items
                //for (int i = 0; i < 6; i++)
                //{
                //    try
                //    {
                //        string wolvowhat = CI[i].Name;
                //    }
                //    catch (IndexOutOfRangeException)
                //    {
                //        Array.Resize(ref CI, CI.Length + 1);
                //        CI[i] = Empt;
                //    }
                //}
                //Count = (23 / 2) - (("┌───┬───┬───┐".Length) / 2);
                //Console.SetCursorPosition(Count + 75, 19);
                //Console.WriteLine("┌───┬───┬───┐");
                //Console.SetCursorPosition(Count + 75, 20);
                //Console.WriteLine("│   │   │   │");
                //Console.SetCursorPosition(Count + 75, 21);
                //Console.WriteLine("├───┼───┼───┤");
                //Console.SetCursorPosition(Count + 75, 22);
                //Console.WriteLine("│   │   │   │");
                //Console.SetCursorPosition(Count + 75, 23);
                //Console.WriteLine("└───┴───┴───┘");

                ////вещи
                //MechEngine.Item[] M = Player.Inventory.ToArray();
                //MechEngine.Item N = new MechEngine.Item();

                //for (int i = 0; i < 6; i++)
                //{
                //    try
                //    {
                //        M[i].ToString();
                //    }
                //    catch (IndexOutOfRangeException)
                //    {
                //        Array.Resize(ref M, M.Length + 1);
                //        M[i] = N;
                //    }
                //}

                ////n1
                //Console.SetCursorPosition(81, 20);
                //Console.ForegroundColor = M[0].Color;
                //Console.Write(" " + M[0].Icon() + " ");
                ////n2
                //Console.SetCursorPosition(85, 20);
                //Console.ForegroundColor = M[1].Color;
                //Console.Write(" " + M[1].Icon() + " ");
                ////n3
                //Console.SetCursorPosition(89, 20);
                //Console.ForegroundColor = M[2].Color;
                //Console.Write(" " + M[2].Icon() + " ");
                ////n4
                //Console.SetCursorPosition(81, 22);
                //Console.ForegroundColor = M[3].Color;
                //Console.Write(" " + M[3].Icon() + " ");
                ////n5
                //Console.SetCursorPosition(85, 22);
                //Console.ForegroundColor = M[4].Color;
                //Console.Write(" " + M[4].Icon() + " ");
                ////n6
                //Console.SetCursorPosition(89, 22);
                //Console.ForegroundColor = M[5].Color;
                //Console.Write(" " + M[5].Icon() + " ");

            }
        }

        public static class winAPIDraw
        {

            [DllImport("Kernel32.dll", SetLastError = true)]
            public static extern SafeFileHandle CreateFile(
                string fileName,
                [MarshalAs(UnmanagedType.U4)] uint fileAccess,
                [MarshalAs(UnmanagedType.U4)] uint fileShare,
                IntPtr securityAttributes,
                [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
                [MarshalAs(UnmanagedType.U4)] int flags,
                IntPtr template);

            [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "WriteConsoleOutputW")]
            public static extern bool WriteConsoleOutput(
              SafeFileHandle hConsoleOutput,
              CharInfo[] lpBuffer,
              Coord dwBufferSize,
              Coord dwBufferCoord,
              ref SmallRect lpWriteRegion);

            [StructLayout(LayoutKind.Sequential)]
            public struct Coord
            {
                public short X;
                public short Y;

                public Coord(short X, short Y)
                {
                    this.X = X;
                    this.Y = Y;
                }
            };

            [StructLayout(LayoutKind.Explicit)]
            public struct CharUnion
            {
                [FieldOffset(0)]
                public char UnicodeChar;
                [FieldOffset(0)]
                public byte AsciiChar;
            }

            [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
            public struct CharInfo
            {
                [FieldOffset(0)]
                public char Char;
                [FieldOffset(2)]
                public short Attributes;

            }

            [StructLayout(LayoutKind.Sequential)]
            public struct SmallRect
            {
                public short Left;
                public short Top;
                public short Right;
                public short Bottom;
            }

            [STAThread]
            public static void DrawLab()
            {
                SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

                if (!h.IsInvalid)
                {
                    CharInfo[] buf = new CharInfo[72 * 23];
                    SmallRect rect = new SmallRect() { Left = 2, Top = 1, Right = 72, Bottom = 23 };

                    var Lab = Rogue.RAM.Map;

                    int i = 0;
                    for (int y = 0; y < 23; y++)
                    {
                        for (int x = 0; x < 72; x++)
                        {
                            //buf[i + x].Back = Convert.ToInt16(ConsoleColor.White);
                            if (Lab.Map[x][y].Enemy != null)
                            {
                                buf[i + x].Attributes = Convert.ToInt16(Lab.Map[x][y].Enemy.Chest);
                                buf[i + x].Char = Lab.Map[x][y].Enemy.Icon;
                            }
                            else if (Lab.Map[x][y].Player != null)
                            {
                                buf[i + x].Attributes = Convert.ToInt16(Lab.Map[x][y].Player.Color);
                                buf[i + x].Char = Lab.Map[x][y].Player.Icon;
                            }
                            else if (Lab.Map[x][y].Item != null)
                            {
                                buf[i + x].Attributes = Convert.ToInt16(Lab.Map[x][y].Item.Color);
                                buf[i + x].Char = Lab.Map[x][y].Item.Icon();
                            }
                            else if (Lab.Map[x][y].Object != null)
                            {
                                if (Lab.Map[x][y].Object.Name == "Exit")
                                {
                                    buf[i + x].Attributes = Convert.ToInt16(ConsoleColor.DarkRed);
                                    buf[i + x].Char = Lab.Map[x][y].Object.IconExit;
                                }
                                else if (Lab.Map[x][y].Object.Icon == '↨')
                                {
                                    Int16 f = (Lab.Map[x][y].Object as MechEngine.Member).ForegroundColor;
                                    Int16 ba = (Lab.Map[x][y].Object as MechEngine.Member).BackgroundColor;

                                    buf[i + x].Attributes = (short)(f | ba);
                                    buf[i + x].Char = Lab.Map[x][y].Object.Icon;
                                }
                                else
                                {
                                    buf[i + x].Attributes = Convert.ToInt16(Lab.Map[x][y].Object.Color);
                                    buf[i + x].Char = Lab.Map[x][y].Object.Icon;
                                }
                            }                                
                            else if (Lab.Map[x][y].Wall != null)
                            {
                                buf[i + x].Attributes = Convert.ToInt16(Lab.Biom);
                                buf[i + x].Char = '#';
                            }
                            else if (Lab.Map[x][y].Trap != null)
                            {
                                buf[i + x].Attributes = Convert.ToInt16(ConsoleColor.DarkGray);
                                buf[i + x].Char = Lab.Map[x][y].Trap.icon;
                            }
                            else
                            {
                                buf[i + x].Char = ' ';
                            }
                        }
                        i += 71;
                    }
                    bool b = WriteConsoleOutput(h, buf,
                      new Coord() { X = 71, Y = 23 },
                      new Coord() { X = 0, Y = 0 },
                      ref rect);
                }
            }

            public static void DrawRegion(string s)
            {
                SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

                if (!h.IsInvalid)
                {
                    CharInfo[] buf = new CharInfo[72 * 23];
                    SmallRect rect = new SmallRect() { Left = 2, Top = 3, Right = 72, Bottom = 23 };

                    int i = 0;
                    foreach (char c in s.ToCharArray())
                    {
                        if (c != '@')
                        {
                            buf[i].Attributes = Convert.ToInt16(Rogue.RAM.Map.Biom);
                        }
                        else { buf[i].Attributes = Convert.ToInt16(ConsoleColor.Red); }
                        if (c == '\r' || c == '\n') { }
                        else if (c == '2' || c == '3' || c == '4' || c == '5' || c == '6')
                        { buf[i].Char = ' '; }
                        else { buf[i].Char = c; }
                        i++;
                    }

                    bool b = WriteConsoleOutput(h, buf,
                      new Coord() { X = 71, Y = 23 },
                      new Coord() { X = 0, Y = 0 },
                      ref rect);

                }
            }            

            public static class Helpers
            {
                /// <summary>
                /// ReDraw object with information about color and char
                /// </summary>
                /// <param name="xo">X Old</param>
                /// <param name="yo">Y Old</param>
                /// <param name="xn">X New</param>
                /// <param name="yn">Y New</param>
                /// <param name="co">Char Old</param>
                /// <param name="cn">Char New</param>
                /// <param name="ao">Attribute Old</param>
                /// <param name="an">Attribute New</param>
                public static void ReDrawOneChar(Int16 xo = 0, Int16 yo = 0, Int16 xn = 0, Int16 yn = 0, char co = '\0', char cn = '\0', Int16 ao = 0, Int16 an = 0)
                {
                    SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

                    if (!h.IsInvalid)
                    {
                        CharInfo[] buf = new CharInfo[1 * 1];
                        SmallRect rect = new SmallRect() { Left = Convert.ToInt16(xo + 2), Top = Convert.ToInt16(yo + 1), Right = Convert.ToInt16(xo + 2), Bottom = Convert.ToInt16(yo + 1) };

                        buf[0].Attributes = ao;
                        buf[0].Char = co;

                        bool b = WriteConsoleOutput(h, buf,
                          new Coord() { X = Convert.ToInt16(xo + 2), Y = Convert.ToInt16(yo + 1) },
                          new Coord() { X = 0, Y = 0 },
                          ref rect);
                    }

                    if (!h.IsInvalid)
                    {
                        CharInfo[] buf = new CharInfo[1 * 1];
                        SmallRect rect = new SmallRect() { Left = Convert.ToInt16(xn + 2), Top = Convert.ToInt16(yn + 1), Right = Convert.ToInt16(xn + 2), Bottom = Convert.ToInt16(yn + 1) };

                        buf[0].Attributes = an;
                        buf[0].Char = cn;

                        bool b = WriteConsoleOutput(h, buf,
                          new Coord() { X = Convert.ToInt16(xn + 2), Y = Convert.ToInt16(yn + 1) },
                          new Coord() { X = 0, Y = 0 },
                          ref rect);
                    }
                }
            }

            public static class DrawRightWindow
            {                
                private static List<Line> Lines = new List<Line>(23);
                /// <summary>
                /// Add line to all lines
                /// </summary>
                /// <param name="Left">left from wall</param>
                /// <param name="Top">top from... top?</param>
                /// <param name="Line">string text</param>
                /// <param name="Color">color of line</param>
                public static void AddLine(int Left, int Top, string Line, ConsoleColor Color)
                {
                    string empty = string.Empty;
                    for (int i = 0; i < Left; i++)
                    { empty += '&'; }
                    Line = empty + Line;
                    Lines[Top - 1] = new Line() { lLine = Line, lColor = Color };
                }
                /// <summary>
                /// Clear lines
                /// </summary>
                public static void Clear()
                {
                    Start = true;
                }
                /// <summary>
                /// Start new lines
                /// </summary>
                public static bool Start
                {
                    set
                    {
                        if (value)
                        {
                            Lines = null;
                            Lines = new List<Line>(23);
                            for (int i = 0; i < 23; i++)
                            { Lines.Add(new Line() { lLine = "", lColor = ConsoleColor.Black }); }
                        }
                    }
                }
                /// <summary>
                /// Draw current lines
                /// </summary>
                public static bool Draw
                {
                    set
                    {
                        if (value)
                        {
                            SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

                            if (!h.IsInvalid)
                            {
                                CharInfo[] buf = new CharInfo[22 * 23];
                                SmallRect rect = new SmallRect() { Left = 76, Top = 1, Right = 97, Bottom = 23 };

                                int i = 0;
                                int q = 0;
                                bool newColor = false;
                                int g = 0;
                                Int16 Col = 0;
                                string col = "";
                                foreach (Line l in Lines)
                                {
                                    q = 0;
                                    foreach (char c in l.lLine.ToCharArray())
                                    {
                                        if (c != '`')
                                        {
                                            if (newColor)
                                            {
                                                col += c;
                                            }
                                            else
                                            {
                                                if (c != '&')
                                                {
                                                    if (Col == 0) { buf[i].Attributes = Convert.ToInt16(l.lColor); }
                                                    else { buf[i].Attributes = Col; Col = 0; }
                                                    buf[i].Char = c;
                                                }
                                                q++;
                                                i++;
                                            }
                                        }
                                        else { if (g != 0) { ConsoleColor V = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), col); Col = Convert.ToInt16(V); g = 0; newColor = false; col = ""; } else { g = 1; newColor = true; } }
                                    }
                                    i += (22 - q);
                                }


                                bool b = WriteConsoleOutput(h, buf,
                                  new Coord() { X = 22, Y = 23 },
                                  new Coord() { X = 0, Y = 0 },
                                  ref rect);
                            }
                        }
                    }
                }
            }


            public static void DRAW_MAIN_VOID(List<List<ColouredChar>> Lines, int left, int top, int WidthCountChars, int Heightcountlines)
            {
                SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

                if (!h.IsInvalid)
                {

                    CharInfo[] buf = new CharInfo[WidthCountChars * Heightcountlines];
                    SmallRect rect = new SmallRect() { Left = (short)(left), Top = (short)top, Right = (short)(WidthCountChars + left), Bottom = (short)((Heightcountlines + top) - 5) };

                    int i = 0;
                    int j = 0;
                    foreach (List<ColouredChar> Line in Lines)
                    {
                        j = 0;
                        foreach (ColouredChar c in Line)
                        {
                            buf[i].Attributes = c.Color;//(short)((short)c.Color | (short)c.BackColor);
                            buf[i].Char = c.Char;
                            i++;
                            j++;
                        }
                        i += (WidthCountChars - j);
                    }
                    //Array.Resize(ref buf, buf.Length - (4 * y));

                    bool b = WriteConsoleOutput(h, buf,
                      new Coord() { X = (short)WidthCountChars, Y = (short)Heightcountlines },
                      new Coord() { X = 0, Y = 0 },
                      ref rect);
                }
            }

            private class Line
            { public string lLine; public ConsoleColor lColor;}

            public static class DrawLeftWindow
            {
                
                private static List<Line> Lines = new List<Line>(23);
                /// <summary>
                /// Add line to all lines
                /// </summary>
                /// <param name="Left">left from wall</param>
                /// <param name="Top">top from... top?</param>
                /// <param name="Line">string text</param>
                /// <param name="Color">color of line</param>
                public static void AddLine(int Left, int Top, string Line, ConsoleColor Color)
                {
                    string empty = string.Empty;
                    for (int i = 0; i < Left; i++)
                    { empty += '&'; }
                    Line = empty + Line;
                    Lines[Top - 1] = new Line() { lLine = Line, lColor = Color };
                }
                /// <summary>
                /// Clear lines
                /// </summary>
                public static void Clear()
                {
                    Start = true;
                }
                /// <summary>
                /// Start new lines
                /// </summary>
                public static bool Start
                {
                    set
                    {
                        if (value)
                        {
                            Lines = null;
                            Lines = new List<Line>(23);
                            for (int i = 0; i < 23; i++)
                            { Lines.Add(new Line() { lLine = "", lColor = ConsoleColor.Black }); }
                        }
                    }
                }
                /// <summary>
                /// Draw current lines
                /// </summary>
                public static bool Draw
                {
                    set
                    {
                        if (value)
                        {
                            SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

                            if (!h.IsInvalid)
                            {
                                CharInfo[] buf = new CharInfo[22 * 23];
                                SmallRect rect = new SmallRect() { Left = 2, Top = 1, Right = 22, Bottom = 23 };

                                int i = 0;
                                int q = 0;
                                bool newColor = false;
                                int g = 0;
                                Int16 Col = 0;
                                string col = "";
                                foreach (Line l in Lines)
                                {
                                    q = 0;
                                    foreach (char c in l.lLine.ToCharArray())
                                    {
                                        if (c != '`')
                                        {
                                            if (newColor)
                                            {
                                                col += c;
                                            }
                                            else
                                            {
                                                if (c != '&')
                                                {
                                                    if (Col == 0) { buf[i].Attributes = Convert.ToInt16(l.lColor); }
                                                    else { buf[i].Attributes = Col; Col = 0; }
                                                    buf[i].Char = c;
                                                }
                                                q++;
                                                i++;
                                            }
                                        }
                                        else { if (g != 0) { ConsoleColor V = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), col); Col = Convert.ToInt16(V); g = 0; newColor = false; col = ""; } else { g = 1; newColor = true; } }
                                    }
                                    i += (22 - q);
                                }


                                bool b = WriteConsoleOutput(h, buf,
                                  new Coord() { X = 22, Y = 23 },
                                  new Coord() { X = 0, Y = 0 },
                                  ref rect);
                            }
                        }
                    }
                }
            }
        }

        public static class LabDraw
        {
            /// <summary>
            /// Redraw character location
            /// </summary>
            /// <param name="xold">Char was there (x)</param>
            /// <param name="yold">Char was there (y)</param>
            /// <param name="xnew">Char will be there (x)</param>
            /// <param name="ynew">Char will be there (y)</param>
            public static void ReDrawObject(int xold, int yold, int xnew, int ynew)
            {
                var Lab = Rogue.RAM.Map;
                //Console.SetCursorPosition(xold + 2, yold + 1);
                //Console.Write(' ');
                char oldchar = ' ';
                Int16 oldatr = 0;
                if (Lab.Map[xold][yold].Trap != null) { oldatr = Convert.ToInt16(ConsoleColor.DarkGray); oldchar = '?'; }
                if (Lab.Map[xold][yold].Object != null && Lab.Map[xold][yold].Object.Icon == '$') { oldatr = Convert.ToInt16(ConsoleColor.Yellow); oldchar = '$'; }
                if (Lab.Map[xold][yold].Item != null) { oldatr = Convert.ToInt16(Lab.Map[xold][yold].Item.Color); oldchar = Lab.Map[xold][yold].Item.Icon(); }

                //Console.ForegroundColor = Rogue.RAM.Player.Color;
                //Console.SetCursorPosition(xnew + 2, ynew + 1);
                //Console.Write(Rogue.RAM.Player.Icon);


                DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(Convert.ToInt16(xold), Convert.ToInt16(yold), Convert.ToInt16(xnew), Convert.ToInt16(ynew), oldchar, Rogue.RAM.Player.Icon, oldatr, Convert.ToInt16(Rogue.RAM.Player.Color));
            }
            /// <summary>
            /// ReDrawCharacter, without position
            /// </summary>
            public static void ReDrawObject()
            {
                var Lab = Rogue.RAM.Map;
                bool was = false;
                for (int y = 0; y < 23; y++)
                {
                    if (!was)
                    {
                        for (int x = 0; x < 71; x++)
                        {
                            if (Lab.Map[x][y].Player != null)
                            {
                                char mchar = Lab.Map[x][y].Player.Icon;
                                Int16 matr = Convert.ToInt16(Lab.Map[x][y].Player.Color);
                                DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(Convert.ToInt16(x), Convert.ToInt16(y), Convert.ToInt16(x), Convert.ToInt16(y), mchar, mchar, matr, matr);
                                return;
                            }
                        }
                    }
                }
            }
            /// <summary>
            /// Destroy object draw
            /// </summary>
            /// <param name="xold">x</param>
            /// <param name="yold">y</param>
            public static void ReDrawObject(int xold, int yold)
            {
                char c = '\0';
                Int16 col = 0;
                if (Rogue.RAM.Map.Map[xold][yold].Player != null)
                {
                    c = Rogue.RAM.Player.Icon;
                    col = Convert.ToInt16(Rogue.RAM.Player.Color);
                }
                DrawEngine.winAPIDraw.Helpers.ReDrawOneChar((Int16)xold, (Int16)yold, (Int16)xold, (Int16)yold, ' ', c, 0, col);
            }
            /// <summary>
            /// Draw new object in cell
            /// </summary>
            /// <param name="xnew">x location</param>
            /// <param name="ynew">y location</param>
            /// <param name="I">Item</param>
            /// <param name="M">Monster</param>
            /// <param name="D">Door</param>
            /// <param name="W">Wall</param>
            public static void ReDrawObject(int xnew, int ynew, MechEngine.Item I = null, MechEngine.Monster M = null, MechEngine.ActiveObject D = null, MechEngine.Wall W = null)
            {
                char mchar = new char();
                Int16 matr = new short();
                if (I != null)
                {
                    mchar = I.Icon();
                    matr = Convert.ToInt16(I.Color);
                }
                else if (M != null)
                {
                    mchar = M.Icon;
                    matr = Convert.ToInt16(M.Chest);
                }
                else if (D != null)
                {
                    mchar = D.Icon;
                    matr = Convert.ToInt16(D.Color);
                }
                else if (W != null)
                {
                    mchar = '#';
                    matr = Convert.ToInt16(Rogue.RAM.Map.Biom);
                }
                DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(Convert.ToInt16(xnew), Convert.ToInt16(ynew), Convert.ToInt16(xnew), Convert.ToInt16(ynew), mchar, mchar, matr, matr);
            }
        }

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

        public static class InfoWindow
        {
            public static void Location(string Header)
            {
                Console.ForegroundColor = Rogue.RAM.Map.Biom;
                string clear = DrawHelp.FullLine(90, " ", 2);
                Console.SetCursorPosition(2, (24) + 2);
                Console.Write(clear);
                Console.SetCursorPosition(3, (24) + 2);
                Console.Write("Вы попали в " + Header);
                Rogue.RAM.RealLog.Add("Вы попали в " + Header);                
            }

            public static void DestroyLoot(MechEngine.Item Item)
            {
                Console.ForegroundColor = Rogue.RAM.Map.Biom;
                string clear = DrawHelp.FullLine(90, " ", 2);
                Console.SetCursorPosition(2, 24 + 2);
                Console.Write(clear);
                Console.SetCursorPosition(3, 24 + 2);
                Console.Write("Вы разрушили предмет: " + Item.Name);
                Rogue.RAM.RealLog.Add("Вы разрушили предмет: " + Item.Name);
            }
            /// <summary>
            /// Draw into info window information about loot taking
            /// </summary>
            /// <param name="Take">Character take item or not</param>
            /// <param name="Loot">Item for draw name etc</param>
            public static void Loot(bool Take, MechEngine.Item Loot)
            {
                Console.ForegroundColor = Rogue.RAM.Map.Biom;
                string clear = DrawHelp.FullLine(90, " ", 2);
                Console.SetCursorPosition(2, 24 + 2);
                Console.Write(clear);
                if (Take == true)
                {
                    Console.SetCursorPosition(3, 24 + 2);
                    Console.Write("Вы взяли предмет: " + Loot.Name);
                    Rogue.RAM.RealLog.Add("Вы взяли предмет: " + Loot.Name);
                }
                else
                {
                    Console.SetCursorPosition(3, 24 + 2);
                    Console.Write("Инвентарь переполнен!");
                }
            }

            public static void Buy(int SpendGold, string ItemName)
            {
                Console.ForegroundColor = Rogue.RAM.Map.Biom;
                string clear = DrawHelp.FullLine(90, " ", 2);
                Console.SetCursorPosition(2, 24 + 2);
                Console.Write(clear);
                Console.SetCursorPosition(3, 24 + 2);
                if (SpendGold != 0 && ItemName != "null")
                { Console.Write("Вы купили предмет: " + ItemName + " и потратили " + SpendGold + " золотых!");
                Rogue.RAM.RealLog.Add("Вы купили предмет: " + ItemName + " и потратили " + SpendGold + " золотых!");
                }
                else
                { Console.Write("Недостаточно золота!"); }
            }

            public static void Sell(int SGold, string SellName)
            {
                Console.ForegroundColor = Rogue.RAM.Map.Biom;
                string clear = DrawHelp.FullLine(90, " ", 2);
                Console.SetCursorPosition(2, 24 + 2);
                Console.Write(clear);
                Console.SetCursorPosition(3, 24 + 2);
                Console.Write("Вы продали предмет: " + SellName + " и получили " + SGold + " золотых!");
                Rogue.RAM.RealLog.Add("Вы продали предмет: " + SellName + " и получили " + SGold + " золотых!");
            }

            public static void DropLoot(MechEngine.Item Loot)
            {
                Clear();
                Console.SetCursorPosition(3, 24 + 2);
                Console.ForegroundColor = Rogue.RAM.Map.Biom;
                Console.Write("Вы выбросили предмет: " + Loot.Name);
                Rogue.RAM.RealLog.Add("Вы выбросили предмет: " + Loot.Name);
            }

            public static void UseItem(MechEngine.Item Item)
            {
                Clear();
                DrawEngine.GUIDraw.ReDrawCharStat();
                Console.SetCursorPosition(3, 24 + 2);
                Console.ForegroundColor = Rogue.RAM.Map.Biom;
                MechEngine.Item.Potion temp = Item as MechEngine.Item.Potion;
                if (Item.Kind == MechEngine.Kind.Potion)
                {
                    Console.Write("Вы выпиваете: " + Item.Name + ". Эффект:" + temp.GetEffect());
                    Rogue.RAM.RealLog.Add("Вы выпиваете: " + Item.Name + ". Эффект:" + temp.GetEffect());
                }
                if (Item.Kind == MechEngine.Kind.Scroll)
                {
                    Console.Write("Вы используете: " + Item.Name);
                    Rogue.RAM.RealLog.Add("Вы используете: " + Item.Name);
                }
            }

            public static void UseEquip(MechEngine.Item Old, MechEngine.Item New, bool Success)
            {
                Clear();
                Console.SetCursorPosition(3, 24 + 2);
                Console.ForegroundColor = Rogue.RAM.Map.Biom;
                if (Success)
                {
                    if (Old != null) { Console.Write("Вы экипировали " + New.Name + " вместо " + Old.Name + " !");
                    Rogue.RAM.RealLog.Add("Вы экипировали " + New.Name + " вместо " + Old.Name + " !");
                    }
                    else { Console.Write("Вы экипировали " + New.Name + " !");
                    Rogue.RAM.RealLog.Add("Вы экипировали " + New.Name + " !");
                    }
                }
                else
                { Console.Write("Сейчас нельзя экипировать этот предмет!"); }
            }

            public static void ItemNotToUse(MechEngine.Item Item)
            {
                Clear();
                Console.SetCursorPosition(3, 24 + 2);
                Console.ForegroundColor = Rogue.RAM.Map.Biom;
                Console.Write(Item.Name + ": предмет нельзя использовать в данный момент!");
            }
            /// <summary>
            /// Write custom message into InfoWindow
            /// </summary>
            /// <param name="S">Message</param>
            public static void Custom(string S)
            {
                try { Console.ForegroundColor = Rogue.RAM.Map.Biom; }
                catch { Console.ForegroundColor = ConsoleColor.DarkGreen; }
                string clear = DrawHelp.FullLine(95, " ", 2);
                Console.SetCursorPosition(2, 24 + 2);
                Console.Write(clear);
                Console.SetCursorPosition(3, 24 + 2);
                Console.Write(S);
                Rogue.RAM.RealLog.Add(S);
            }
            /// <summary>
            /// new realisation of custom message
            /// </summary>
            public static string Message
            {
                set
                {
                    Console.ForegroundColor = Rogue.RAM.Map.Biom;
                    string clear = DrawHelp.FullLine(95, " ", 2);
                    Console.SetCursorPosition(2, 24 + 2);
                    Console.Write(clear);
                    Console.SetCursorPosition(3, 24 + 2);
                    Console.Write(value);
                    Rogue.RAM.RealLog.Add(value);
                }
            }
            /// <summary>
            /// Write custom warning into InfoWindow.
            /// </summary>
            public static string Warning
            {
                set
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    string clear = DrawHelp.FullLine(95, " ", 2);
                    Console.SetCursorPosition(2, 24 + 2);
                    Console.Write(clear);
                    Console.SetCursorPosition(3, 24 + 2);
                    Console.Write(value);
                    Rogue.RAM.RealLog.Add(value);
                }
            }
            /// <summary>
            /// new realisation of custom color message
            /// </summary>
            public static List<ColoredWord> cMessage
            {
                set
                {
                    Console.ForegroundColor = Rogue.RAM.Map.Biom;
                    string clear = DrawHelp.FullLine(95, " ", 2);
                    Console.SetCursorPosition(2, 26);
                    Console.Write(clear);
                    int next = 0;
                    string log = "";
                    foreach (ColoredWord word in value)
                    {
                        Console.SetCursorPosition(3 + next, 26);
                        Console.ForegroundColor = word.Color;
                        Console.Write(word.Word);
                        log += word.Word;
                        next += 1 + word.Word.Length;
                    }
                    Rogue.RAM.RealLog.Add(log);
                }
            }           

            public static void Clear()
            {
                string clear = DrawHelp.FullLine(100, " ", 4);
                Console.SetCursorPosition(2, 24 + 2);
                Console.Write(clear);
            }
        }

        public static class InfoDraw
        {
            public static MechEngine.NPC NPC
            {
                set
                {
                    FightDraw.DrawEnemyGUI();
                    
                    Console.ForegroundColor = value.Color;
                    int Count = (25 / 2) - (value.Name.Length / 2);
                    Console.SetCursorPosition(Count, 1);
                    Console.WriteLine(value.Name);

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Count = (25 / 2) - (value.Affix.Length / 2);
                    Console.SetCursorPosition(Count, 2);
                    Console.WriteLine(value.Affix);

                    DrawEngine.DrawHelp.DrawAvatar = new ColorChar() { Char = value.Icon, Color = value.Color };
                    int forCount = 14;
                    foreach (string s in DrawEngine.DrawHelp.TextBlock(value.Info, 21))
                    {
                        Console.SetCursorPosition(3, forCount++);
                        Console.WriteLine(s);
                    }
                }
            }

            public static MechEngine.Member Member
            {
                set
                {
                    FightDraw.DrawEnemyGUI();

                    Console.ForegroundColor = value.Color;
                    int Count = (25 / 2) - (value.Name.Length / 2);
                    Console.SetCursorPosition(Count, 1);
                    Console.WriteLine(value.Name);

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Count = (25 / 2) - (value.Affix.Length / 2);
                    Console.SetCursorPosition(Count, 2);
                    Console.WriteLine(value.Affix);

                    DrawEngine.DrawHelp.DrawAvatar = new ColorChar() { Char = value.Icon, Color = value.Color };
                    int forCount = 14;
                    foreach (string s in DrawEngine.DrawHelp.TextBlock(value.Info, 21))
                    {
                        Console.SetCursorPosition(3, forCount++);
                        Console.WriteLine(s);
                    }
                }
            }

            public static MechEngine.ActiveObject ActiveObject
            {
                set
                {
                    SoundEngine.Sound.Identify();
                    if (value.Name == "Exit")
                    {
                        FightDraw.DrawEnemyGUI();

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        int Count = (26 / 2) - ("Портал".Length / 2);
                        Console.SetCursorPosition(Count, 2);
                        Console.WriteLine("Портал");

                        DrawEngine.DrawHelp.DrawAvatar = new ColorChar() { Char = value.IconExit, Color = ConsoleColor.DarkRed };
                        int forCount = 14;
                        foreach (string s in DrawEngine.DrawHelp.TextBlock("Портал перенесёт вас в следующую локацию. Вернуться не представится возможным.", 21))
                        {
                            Console.SetCursorPosition(3, forCount++);
                            Console.WriteLine(s);
                        }
                    }
                    else
                    {
                        FightDraw.DrawEnemyGUI();

                        Console.ForegroundColor = value.Color;
                        int Count = (26 / 2) - (value.Name.Length / 2);
                        Console.SetCursorPosition(Count, 2);
                        Console.WriteLine(value.Name);

                        DrawEngine.DrawHelp.DrawAvatar = new ColorChar() { Char = value.Icon, Color = value.Color };
                        int forCount = 14;
                        foreach (string s in DrawEngine.DrawHelp.TextBlock(value.Info, 21))
                        {
                            Console.SetCursorPosition(3, forCount++);
                            Console.WriteLine(s);
                        }
                    }
                }
            }

            public static MechEngine.Merchant Trade
            {
                set
                {
                    FightDraw.DrawEnemyGUI();
                    SoundEngine.Sound.Identify();
                    MechEngine.Merchant e = value;
                    //name
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    int Count = (23 / 2) - (e.Name.Length / 2);
                    Console.SetCursorPosition(Count + 1, 1);
                    Console.WriteLine(DrawHelp.FullLine(" ".Length + e.Name.Length, " " + e.Name, " ".Length + e.Name.Length - 1));

                    DrawEngine.DrawHelp.DrawAvatar = new ColorChar() { Color = e.SpeachColor, Char = e.SpeachIcon };

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Count = (23 / 2) - ((9 + e.CurGold.ToString().Length) / 2);
                    Console.SetCursorPosition(Count + 1, 12);
                    string WriteThis = "Золотых: " + e.CurGold.ToString();
                    Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));
                    //ap
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Count = (23 / 2) - ((10 + e.MaxGold.ToString().Length) / 2);
                    Console.SetCursorPosition(Count + 1, 13);
                    WriteThis = "Максимум: " + e.MaxGold.ToString();
                    Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));
                }
            }

            public static MechEngine.Monster Enemy
            {
                set
                {
                    SoundEngine.Sound.Identify();
                    MechEngine.Monster e = value;

                    int height = 24;
                    Console.ForegroundColor = Rogue.RAM.CUIColor;

                    string Clear = string.Empty;
                    Clear = DrawHelp.FullLine(25, DrawHelp.Border(true, 4));
                    Clear = DrawHelp.Border(true, 1) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 2);
                    Console.SetCursorPosition(1, 0);
                    Console.WriteLine(Clear);

                    //тело
                    for (int ii = 1; ii < height; ii++)
                    {
                        Clear = DrawHelp.FullLine(25, " ");
                        Clear = DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 3);
                        Console.SetCursorPosition(1, ii);
                        Console.WriteLine(Clear);
                    }

                    //носки 
                    Clear = string.Empty;
                    Clear = DrawHelp.FullLine(25, DrawHelp.Border(true, 4));
                    Clear = DrawHelp.Border(true, 5) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 6);
                    Console.SetCursorPosition(1, height);
                    Console.WriteLine(Clear);

                    
                    //name
                    Console.ForegroundColor = e.Chest;
                    if (e.Name == "Валоран")
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    }
                    int Count = (23 / 2) - (e.Name.Length / 2);
                    Console.SetCursorPosition(Count + 1, 1);
                    Console.WriteLine(DrawHelp.FullLine(" ".Length + e.Name.Length, " " + e.Name, " ".Length + e.Name.Length - 1));
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Count = (100 / 2) - ((e.Name.Length + " VS ".Length + Rogue.RAM.Player.Name.Length) / 2);
                    Console.SetCursorPosition(Count + 1, 1);
                    //Console.WriteLine(DrawHelp.FullLine(e.Name.Length + " VS ".Length + Rogue.RAM.Player.Name.Length, e.Name + " VS " + Rogue.RAM.Player.Name, (e.Name.Length + " VS ".Length + Rogue.RAM.Player.Name.Length) - 1));

                    string WriteThis = "";
                    if (!(e.Name == "Валоран"))
                    {//race class
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Count = (23 / 2) - ((e.GetRace().Length) / 2);
                        Console.SetCursorPosition(Count + 1, 3);
                        WriteThis = " " + e.GetRace();
                        Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                        Count = (23 / 2) - (("  Хранитель мира  ".Length) / 2);
                        Console.SetCursorPosition(Count + 1, 2);
                        WriteThis = "  Хранитель мира  ";
                        Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));

                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Count = (23 / 2) - (("  [Предатель]  ".Length) / 2);
                        Console.SetCursorPosition(Count + 1, 3);
                        WriteThis = "  [Предатель]  ";
                        Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));
                        Console.ResetColor();
                    }

                    //level exp
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Count = (23 / 2) - ((9 + e.LVL.ToString().Length) / 2);
                    Console.SetCursorPosition(Count + 1, 5);
                    WriteThis = " Уровень: " + e.LVL.ToString();
                    Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));

                    //hp
                    Console.ForegroundColor = ConsoleColor.Red;
                    Count = (23 / 2) - ((7 + e.CHP.ToString().Length + 1 + e.MHP.ToString().Length) / 2);
                    Console.SetCursorPosition(Count + 1, 7);
                    WriteThis = "Жизнь: " + e.CHP.ToString() + "/" + e.MHP.ToString();
                    Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));

                    //dmg
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Count = (23 / 2) - ((6 + e.MIDMG.ToString().Length + 1 + e.MADMG.ToString().Length) / 2);
                    Console.SetCursorPosition(Count + 1, 9);
                    WriteThis = "Урон: " + e.MIDMG.ToString() + "-" + e.MADMG.ToString();
                    Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));
                    ////ad
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Count = (23 / 2) - ((12 + e.AD.ToString().Length) / 2);
                    Console.SetCursorPosition(Count + 1, 11);
                    WriteThis = "Сила атаки: " + e.AD.ToString();
                    Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));
                    //ap
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Count = (23 / 2) - ((12 + e.AP.ToString().Length) / 2);
                    Console.SetCursorPosition(Count + 1, 12);
                    WriteThis = "Сила магии: " + e.AP.ToString();
                    Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));
                    //arm
                    Console.ForegroundColor = Rogue.RAM.CUIColor;
                    Count = (23 / 2) - ((11 + e.ARM.ToString().Length) / 2);
                    Console.SetCursorPosition(Count + 1, 14);
                    WriteThis = "Защита Ф : " + e.ARM.ToString();
                    Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));
                    //mrs
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Count = (23 / 2) - ((11 + e.MRS.ToString().Length) / 2);
                    Console.SetCursorPosition(Count + 1, 15);
                    WriteThis = "Защита М : " + e.MRS.ToString();
                    Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));

                    ////money
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Count = (23 / 2) - ((12) / 2);
                    Console.SetCursorPosition(Count + 1, 17);
                    WriteThis = " Способности:";
                    Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));


                    int i = 19;
                    foreach (MechEngine.MonsterAbility a in e.Cast)
                    {
                        if (a != null)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Count = (23 / 2) - ((a.Name.Length) / 2);
                            Console.SetCursorPosition(Count + 1, i);
                            WriteThis = a.Name;
                            Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));
                            i++;
                        }
                    }
                    if (i == 19)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Count = (23 / 2) - ((4) / 2);
                        Console.SetCursorPosition(Count + 1, i);
                        WriteThis = " Нет";
                        Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));
                    }

                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    string clear = DrawHelp.FullLine(75, " ", 2);
                    Console.SetCursorPosition(2, 24 + 2);
                    Console.Write(clear);
                }
            }

            public static MechEngine.Item Item
            {
                set
                {
                    SoundEngine.Sound.Identify();
                    FightDraw.DrawEnemyGUI();

                    Console.ForegroundColor = value.Color;
                    string nname = "";
                    try
                    {
                        if (value.Name.Length > 20) { nname = value.Name.Substring(0, 20); } else { nname = value.Name; };
                    }
                    catch { nname = "Ингридиент для яда"; }
                    int Count = (25 / 2) - (nname.Length / 2);
                    Console.SetCursorPosition(Count, 2);
                    Console.WriteLine(nname);

                    string type = "";
                    List<string> bonuses = new List<string>();

                    switch (value.Kind)
                    {
                        case MechEngine.Kind.Armor:
                            {
                                type = "Броня";
                                string arm = DrawHelp.Sign((value as MechEngine.Item.Armor).ARM);
                                string mrs = DrawHelp.Sign((value as MechEngine.Item.Armor).MRS);
                                string hp = DrawHelp.Sign((value as MechEngine.Item.Armor).HP);
                                string mp = DrawHelp.Sign((value as MechEngine.Item.Armor).MP);
                                if (Rogue.RAM.Player.Equipment.Armor != null)
                                {
                                    arm = DrawHelp.Sign((value as MechEngine.Item.Armor).ARM - Rogue.RAM.Player.Equipment.Armor.ARM);
                                    mrs = DrawHelp.Sign((value as MechEngine.Item.Armor).MRS - Rogue.RAM.Player.Equipment.Armor.MRS);
                                    hp = DrawHelp.Sign((value as MechEngine.Item.Armor).HP - Rogue.RAM.Player.Equipment.Armor.HP);
                                    mp = DrawHelp.Sign((value as MechEngine.Item.Armor).MP - Rogue.RAM.Player.Equipment.Armor.MP);
                                }
                                bonuses.Add("Физ. Защита: " + arm);
                                bonuses.Add("Маг. Защита: " + mrs);
                                bonuses.Add("Здоровье: " + hp);
                                bonuses.Add("Ресурсы: " + mp);
                                break;
                            }
                        case MechEngine.Kind.Boots:
                            {
                                type = "Обувь";
                                string arm = DrawHelp.Sign((value as MechEngine.Item.Boots).ARM);
                                string mrs = DrawHelp.Sign((value as MechEngine.Item.Boots).MRS);
                                if (Rogue.RAM.Player.Equipment.Boots != null)
                                {
                                    arm = DrawHelp.Sign((value as MechEngine.Item.Boots).ARM - Rogue.RAM.Player.Equipment.Boots.ARM);
                                    mrs = DrawHelp.Sign((value as MechEngine.Item.Boots).MRS - Rogue.RAM.Player.Equipment.Boots.MRS);
                                }
                                bonuses.Add("Физ. Защита: " + arm);
                                bonuses.Add("Маг. Защита: " + mrs);
                                break;
                            }
                        case MechEngine.Kind.Helm:
                            {
                                type = "Шлем";
                                string arm = DrawHelp.Sign((value as MechEngine.Item.Helm).AD);
                                string mrs = DrawHelp.Sign((value as MechEngine.Item.Helm).AP);
                                string hp = DrawHelp.Sign((value as MechEngine.Item.Helm).HP);
                                string mp = DrawHelp.Sign((value as MechEngine.Item.Helm).MP);
                                if (Rogue.RAM.Player.Equipment.Helm != null)
                                {
                                    arm = DrawHelp.Sign((value as MechEngine.Item.Helm).AD - Rogue.RAM.Player.Equipment.Helm.AD);
                                    mrs = DrawHelp.Sign((value as MechEngine.Item.Helm).AP - Rogue.RAM.Player.Equipment.Helm.AP);
                                    hp = DrawHelp.Sign((value as MechEngine.Item.Helm).HP - Rogue.RAM.Player.Equipment.Helm.HP);
                                    mp = DrawHelp.Sign((value as MechEngine.Item.Helm).MP - Rogue.RAM.Player.Equipment.Helm.MP);
                                }
                                bonuses.Add("Сила атаки: " + arm);
                                bonuses.Add("Сила магии: " + mrs);
                                bonuses.Add("Здоровье: " + hp);
                                bonuses.Add("Ресурсы: " + mp);
                                break;
                            }
                        case MechEngine.Kind.OffHand:
                            {
                                type = "Левая рука";
                                string arm = DrawHelp.Sign((value as MechEngine.Item.OffHand).AD);
                                string mrs = DrawHelp.Sign((value as MechEngine.Item.OffHand).AP);
                                string hp = DrawHelp.Sign((value as MechEngine.Item.OffHand).ARM);
                                string mp = DrawHelp.Sign((value as MechEngine.Item.OffHand).MRS);
                                string mi = DrawHelp.Sign((value as MechEngine.Item.OffHand).MIDMG);
                                string ma = DrawHelp.Sign((value as MechEngine.Item.OffHand).MADMG);
                                if (Rogue.RAM.Player.Equipment.OffHand != null)
                                {
                                    arm = DrawHelp.Sign((value as MechEngine.Item.OffHand).AD - Rogue.RAM.Player.Equipment.OffHand.AD);
                                    mrs = DrawHelp.Sign((value as MechEngine.Item.OffHand).AP - Rogue.RAM.Player.Equipment.OffHand.AP);
                                    hp = DrawHelp.Sign((value as MechEngine.Item.OffHand).ARM - Rogue.RAM.Player.Equipment.OffHand.ARM);
                                    mp = DrawHelp.Sign((value as MechEngine.Item.OffHand).MRS - Rogue.RAM.Player.Equipment.OffHand.MRS);
                                    mi = DrawHelp.Sign((value as MechEngine.Item.OffHand).MIDMG - Rogue.RAM.Player.Equipment.OffHand.MIDMG);
                                    ma = DrawHelp.Sign((value as MechEngine.Item.OffHand).MADMG - Rogue.RAM.Player.Equipment.OffHand.MADMG);
                                }
                                bonuses.Add("Сила атаки: " + arm);
                                bonuses.Add("Сила магии: " + mrs);
                                bonuses.Add("Физ. Защита: " + hp);
                                bonuses.Add("Маг. Защита: " + mp);
                                bonuses.Add("Мин. Урон: " + mi);
                                bonuses.Add("Макс. Урон: " + ma);
                                break;
                            }
                        case MechEngine.Kind.Weapon:
                            {
                                type = "Оружие";
                                string arm = DrawHelp.Sign((value as MechEngine.Item.Weapon).AD);
                                string mrs = DrawHelp.Sign((value as MechEngine.Item.Weapon).AP);
                                string hp = DrawHelp.Sign((value as MechEngine.Item.Weapon).ARM);
                                string mi = DrawHelp.Sign((value as MechEngine.Item.Weapon).MIDMG);
                                string ma = DrawHelp.Sign((value as MechEngine.Item.Weapon).MADMG);
                                if (Rogue.RAM.Player.Equipment.Weapon != null)
                                {
                                    arm = DrawHelp.Sign((value as MechEngine.Item.Weapon).AD - Rogue.RAM.Player.Equipment.Weapon.AD);
                                    mrs = DrawHelp.Sign((value as MechEngine.Item.Weapon).AP - Rogue.RAM.Player.Equipment.Weapon.AP);
                                    hp = DrawHelp.Sign((value as MechEngine.Item.Weapon).ARM - Rogue.RAM.Player.Equipment.Weapon.ARM);
                                    mi = DrawHelp.Sign((value as MechEngine.Item.Weapon).MIDMG - Rogue.RAM.Player.Equipment.Weapon.MIDMG);
                                    ma = DrawHelp.Sign((value as MechEngine.Item.Weapon).MADMG - Rogue.RAM.Player.Equipment.Weapon.MADMG);
                                }
                                bonuses.Add("Сила атаки: " + arm);
                                bonuses.Add("Сила магии: " + mrs);
                                bonuses.Add("Физ. Защита: " + hp);
                                bonuses.Add("Мин. Урон: " + mi);
                                bonuses.Add("Макс. Урон: " + ma);
                                break;
                            }
                        case MechEngine.Kind.Key:
                            {
                                type = "Ключ";
                                bonuses = DrawHelp.TextBlock(value.Info, 21);
                                break;
                            }
                        case MechEngine.Kind.Potion:
                            {
                                type = "Зелье";
                                string arm = DrawHelp.Sign((value as MechEngine.Item.Potion).HP);
                                string mrs = DrawHelp.Sign((value as MechEngine.Item.Potion).MP);
                                bonuses.Add("Здоровье: " + arm);
                                bonuses.Add("Ресурсы: " + mrs);
                                break;
                            }
                        case MechEngine.Kind.Scroll:
                            {
                                type = "Свиток";
                                bonuses = DrawHelp.TextBlock((value as MechEngine.Item.Scroll).Spell.Info.Replace('\n','\0'), 21);
                                break;
                            }
                        case MechEngine.Kind.Resource:
                            {
                                type = "Ресурс";
                                bonuses = DrawHelp.TextBlock(value.Info, 21);
                                break;
                            }
                        case MechEngine.Kind.Rune:
                            {
                                type = "Руна";
                                bonuses = DrawHelp.TextBlock(value.Info, 21);
                                break;
                            }
                    }

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Count = (25 / 2) - (type.Length / 2);
                    Console.SetCursorPosition(Count, 3);
                    Console.WriteLine(type);

                    DrawEngine.DrawHelp.DrawAvatar = new ColorChar() { Char = value.Icon(), Color = value.Color };

                    if (value.ReputationSell != 0)
                    {
                        Count = (25 / 2) - ((value.ReputationName+": "+value.ReputationSell.ToString()).Length / 2);
                        Console.SetCursorPosition(Count, 12);
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(value.ReputationName + ": " + value.ReputationSell.ToString());
                    }
                    if (value.Enchanted)
                    {
                        Count = (25 / 2) - (("Зачарованно").Length / 2);
                        Console.SetCursorPosition(Count, 11);
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("Зачарованно");
                    }

                    int forCount = 14;
                    foreach (string s in bonuses)
                    {
                        if (s.IndexOf("+") > -1) { Console.ForegroundColor = ConsoleColor.Green; }
                        if (s.IndexOf("+0") > -1) { Console.ForegroundColor = ConsoleColor.Gray; }
                        if (s.IndexOf("-") > -1) { Console.ForegroundColor = ConsoleColor.Red; }
                        Count = (25 / 2) - (s.Length / 2);
                        Console.SetCursorPosition(Count, forCount++);
                        Console.WriteLine(s);
                    }
                }
            }
        }

        public static class CharMap
        {
            public static void DrawCMap(List<string> Commands)
            {
                #region Window
                Console.ForegroundColor = Rogue.RAM.CUIColor;

                string Clear = string.Empty;
                Clear = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 3);
                Clear = " " + DrawHelp.Border(true, 1) + "══════════════════════" + DrawHelp.Border(true, 0, "ThreeBot") + "══════════════════════" + DrawHelp.Border(true, 0, "ThreeBot") + "══════════════════════" + DrawHelp.Border(true, 0, "ThreeBot") + "═══════════════════════════" + DrawHelp.Border(true, 2);// + Clear.Remove(Clear.Length - 24) + DrawHelp.Border(true, 2);
                Console.SetCursorPosition(0, 28);
                Console.WriteLine(Clear);

                //тело                
                for (int i = 29; i < 32; i++)
                {
                    Clear = DrawHelp.FullLine(100, " ", 3);
                    Clear = " " + DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 3);
                    if (i % 2 != 0) { Clear = " ║                      ║                      ║                      ║                           ║"; }
                    Console.SetCursorPosition(0, i);
                    Console.WriteLine(Clear);
                }

                //носки 
                Clear = string.Empty;
                Clear = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 3);
                Clear = " " + DrawHelp.Border(true, 5) + "══════════════════════" + DrawHelp.Border(true, 0, "ThreeTop") + "══════════════════════" + DrawHelp.Border(true, 0, "ThreeTop") + "══════════════════════" + DrawHelp.Border(true, 0, "ThreeTop") + "═══════════════════════════" + DrawHelp.Border(true, 6);
                Console.SetCursorPosition(0, 32);
                Console.WriteLine(Clear);

                Clear = " " + DrawHelp.Border(true, 8) + "══════════════════════" + DrawHelp.Border(true, 0, "Cross") + "══════════════════════" + DrawHelp.Border(true, 0, "Cross") + "══════════════════════" + DrawHelp.Border(true, 0, "Cross") + "═══════════════════════════" + DrawHelp.Border(true, 7);
                Console.SetCursorPosition(0, 30);
                Console.WriteLine(Clear);
                #endregion

                double columns = Math.Ceiling(Convert.ToDouble(Commands.Count) / 2);
                int index = 0;
                for (int i = 0; i < columns; i++)
                {
                    try
                    { if (Commands[index + 1] == null) { Commands.Add(""); } }
                    catch { Commands.Add(""); }
                    DrawColumn(i + 1, new List<string>() { Commands[index], Commands[index + 1] });
                    if (index != 0) { index += 2; }
                    else { index = 2; }
                }
            }

            private static void DrawColumn(int ColumnNumber, List<string> Strings)
            {
                if (Strings.Count > 13) { Console.Clear(); DrawEngine.ConsoleDraw.WriteTitle("Кодер - мудак"); }
                int left = 0;
                if (ColumnNumber == 1) { left = 3; }
                else if (ColumnNumber == 2) { left = 26; }
                else if (ColumnNumber == 3) { left = 48; }
                else if (ColumnNumber == 4) { left = 71; }
                int i = 29;
                Console.ForegroundColor = Rogue.RAM.Map.Biom;
                foreach (string s in Strings)
                {
                    Console.SetCursorPosition(left, i);
                    Console.Write(s.Replace('&', ' '));
                    i += 2;

                }
            }
        }

        public static class FightDraw
        {
            public static void DrawFight()
            {
                DrawEnemyGUI();
                DrawCombatLog();
                DrawEnemyStat();
                ReDrawBuffDeBuff();
            }

            public static void DrawEnemyGUI()
            {
                int height = 24;
                Console.ForegroundColor = Rogue.RAM.CUIColor;

                string Clear = string.Empty;
                Clear = DrawHelp.FullLine(25, DrawHelp.Border(true, 4));
                Clear = DrawHelp.Border(true, 1) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 2);
                Console.SetCursorPosition(1, 0);
                Console.WriteLine(Clear);

                //тело
                for (int i = 1; i < height; i++)
                {
                    Clear = DrawHelp.FullLine(25, " ");
                    Clear = DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 3);
                    Console.SetCursorPosition(1, i);
                    Console.WriteLine(Clear);
                }

                //носки 
                Clear = string.Empty;
                Clear = DrawHelp.FullLine(25, DrawHelp.Border(true, 4));
                Clear = DrawHelp.Border(true, 5) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 6);
                Console.SetCursorPosition(1, height);
                Console.WriteLine(Clear);
            }

            public static void DrawEnemyStat()
            {
                MechEngine.Monster e = Rogue.RAM.Enemy;
                //name
                Console.ForegroundColor = e.Chest;
                if (e.Name == "Валоран")
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                }
                int Count = (23 / 2) - (e.Name.Length / 2);
                Console.SetCursorPosition(Count + 1, 1);
                Console.WriteLine(DrawHelp.FullLine(" ".Length + e.Name.Length, " " + e.Name, " ".Length + e.Name.Length - 1));
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Count = (100 / 2) - ((e.Name.Length + " VS ".Length + Rogue.RAM.Player.Name.Length) / 2);
                Console.SetCursorPosition(Count + 1, 1);
                Console.WriteLine(DrawHelp.FullLine(e.Name.Length + " VS ".Length + Rogue.RAM.Player.Name.Length, e.Name + " VS " + Rogue.RAM.Player.Name, (e.Name.Length + " VS ".Length + Rogue.RAM.Player.Name.Length) - 1));

                string WriteThis = "";
                if (!(e.Name == "Валоран"))
                {//race class
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Count = (23 / 2) - ((e.GetRace().Length) / 2);
                    Console.SetCursorPosition(Count + 1, 3);
                    WriteThis = " " + e.GetRace();
                    Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Count = (23 / 2) - (("  Хранитель мира  ".Length) / 2);
                    Console.SetCursorPosition(Count + 1, 2);
                    WriteThis = "  Хранитель мира  ";
                    Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));

                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Count = (23 / 2) - (("  [Предатель]  ".Length) / 2);
                    Console.SetCursorPosition(Count + 1, 3);
                    WriteThis = "  [Предатель]  ";
                    Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));
                    Console.ResetColor();
                }

                //level exp
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Count = (23 / 2) - ((9 + e.LVL.ToString().Length) / 2);
                Console.SetCursorPosition(Count + 1, 5);
                WriteThis = " Уровень: " + e.LVL.ToString();
                Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));

                //hp
                Console.ForegroundColor = ConsoleColor.Red;
                Count = (23 / 2) - ((7 + e.CHP.ToString().Length + 1 + e.MHP.ToString().Length) / 2);
                Console.SetCursorPosition(Count + 1, 7);
                WriteThis = "Жизнь: " + e.CHP.ToString() + "/" + e.MHP.ToString();
                Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));

                //dmg
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Count = (23 / 2) - ((6 + e.MIDMG.ToString().Length + 1 + e.MADMG.ToString().Length) / 2);
                Console.SetCursorPosition(Count + 1, 9);
                WriteThis = "Урон: " + e.MIDMG.ToString() + "-" + e.MADMG.ToString();
                Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));
                ////ad
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Count = (23 / 2) - ((12 + e.AD.ToString().Length) / 2);
                Console.SetCursorPosition(Count + 1, 11);
                WriteThis = "Сила атаки: " + e.AD.ToString();
                Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));
                //ap
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Count = (23 / 2) - ((12 + e.AP.ToString().Length) / 2);
                Console.SetCursorPosition(Count + 1, 12);
                WriteThis = "Сила магии: " + e.AP.ToString();
                Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));
                //arm
                Console.ForegroundColor = Rogue.RAM.CUIColor;
                Count = (23 / 2) - ((11 + e.ARM.ToString().Length) / 2);
                Console.SetCursorPosition(Count + 1, 14);
                WriteThis = "Защита Ф : " + e.ARM.ToString();
                Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));
                //mrs
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Count = (23 / 2) - ((11 + e.MRS.ToString().Length) / 2);
                Console.SetCursorPosition(Count + 1, 15);
                WriteThis = "Защита М : " + e.MRS.ToString();
                Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));

                ////money
                Console.ForegroundColor = ConsoleColor.Yellow;
                Count = (23 / 2) - ((12) / 2);
                Console.SetCursorPosition(Count + 1, 17);
                WriteThis = " Способности:";
                Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));


                int i = 19;
                foreach (MechEngine.MonsterAbility a in e.Cast)
                {
                    if (a != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Count = (23 / 2) - ((a.Name.Length) / 2);
                        Console.SetCursorPosition(Count + 1, i);
                        WriteThis = a.Name;
                        Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));
                        i++;
                    }
                }
                if (i == 19)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Count = (23 / 2) - ((4) / 2);
                    Console.SetCursorPosition(Count + 1, i);
                    WriteThis = " Нет";
                    Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));
                }

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                string clear = DrawHelp.FullLine(75, " ", 2);
                Console.SetCursorPosition(2, 24 + 2);
                Console.Write(clear);
            }

            public static void DrawCombatLog()
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
            }

            public static void DrawGlobalLog()
            {
                int height = 0;

                Console.ForegroundColor = Rogue.RAM.CUIColor;

                string Clear = string.Empty;
                Clear = DrawHelp.FullLine(101, DrawHelp.Border(true, 4), 3);
                Clear = " " + DrawHelp.Border(true, 1) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 2);
                Console.SetCursorPosition(0, height);
                Console.WriteLine(Clear);

                //тело                
                int suki = 0;
                for (int i = 1; i < 32; i++)
                {
                    Clear = DrawHelp.FullLine(101, " ", 3);
                    Clear = " " + DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 3);
                    Console.SetCursorPosition(0, i);
                    Console.WriteLine(Clear);
                    suki = i;
                }

                //носки 
                Clear = string.Empty;
                Clear = DrawHelp.FullLine(101, DrawHelp.Border(true, 4), 3);
                Clear = " " + DrawHelp.Border(true, 5) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 6);
                Console.SetCursorPosition(0, suki + 1);
                Console.WriteLine(Clear);

                //носки объявления 
                Clear = string.Empty;
                Clear = DrawHelp.FullLine(101, DrawHelp.Border(true, 4), 3);
                Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 7);
                Console.SetCursorPosition(0, 2);
                Console.WriteLine(Clear);

                int Count = (100 / 2) - ("Глобальный лог".Length / 2);
                Console.SetCursorPosition(Count + 1, 1);
                Console.WriteLine(DrawHelp.FullLine("Глобальный лог".Length, "Глобальный лог", ("Глобальный лог".Length) - 1));

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                int q = 3;
                if (Rogue.RAM.RealLog.Count > 29)
                {
                    int myint=((29 - Rogue.RAM.RealLog.Count)*-1);
                    for (int i = 0; i <myint ; i++)
                    {
                        Rogue.RAM.RealLog.Remove(Rogue.RAM.RealLog[0]);
                    }
                }
                for (int i = 0; i < Rogue.RAM.RealLog.Count; i++)
                {
                    Console.SetCursorPosition(2, q);
                    Console.WriteLine(Rogue.RAM.RealLog[i]);
                    q++;
                }
            }

            public static void ReDrawCombatLog()
            {
                DrawCombatLog();

                MechEngine.Monster e = Rogue.RAM.Enemy;
                Console.ForegroundColor = e.Chest;
                if (e.Name == "Валоран")
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                }
                int Count = (23 / 2) - (e.Name.Length / 2);
                Console.SetCursorPosition(Count + 1, 1);
                Console.WriteLine(DrawHelp.FullLine(" ".Length + e.Name.Length, " " + e.Name, " ".Length + e.Name.Length - 1));
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Count = (100 / 2) - ((e.Name.Length + " VS ".Length + Rogue.RAM.Player.Name.Length) / 2);
                Console.SetCursorPosition(Count + 1, 1);
                Console.WriteLine(DrawHelp.FullLine(e.Name.Length + " VS ".Length + Rogue.RAM.Player.Name.Length, e.Name + " VS " + Rogue.RAM.Player.Name, (e.Name.Length + " VS ".Length + Rogue.RAM.Player.Name.Length) - 1));

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                int q = 3;
                if (Rogue.RAM.Log.Count > 20)
                {
                    for (int i = 0; i < Rogue.RAM.Log.Count - 20; i++)
                    { Rogue.RAM.Log.Remove(Rogue.RAM.Log[i]); }
                }
                for (int i = 0; i < Rogue.RAM.Log.Count; i++)
                {
                    Console.SetCursorPosition(28, q);
                    if (Rogue.RAM.Log[i].Length > 37)
                    {
                        string first = Rogue.RAM.Log[i].Substring(37);
                        Rogue.RAM.Log.Add(first);
                        Rogue.RAM.Log[i] = Rogue.RAM.Log[i].Remove(37);
                        ReDrawCombatLog();
                        break;
                    }
                    else
                    {
                        Console.WriteLine(DateTime.Now.ToShortTimeString() + " > " + Rogue.RAM.Log[i]);
                    }
                    q++;
                }
            }

            public static void ReDrawBuffDeBuff()
            {
                int Count = 5;
                ClearBuffsDebuffs();
                if (Rogue.RAM.Enemy != null)
                {
                    if (Rogue.RAM.Enemy.DoT != null)
                    {
                        for (int i = 0; i < Rogue.RAM.Enemy.DoT.Count; i++)
                        {
                            MechEngine.Ability a = Rogue.RAM.Enemy.DoT[i];
                            Count++;
                            Console.SetCursorPosition(3, Count);
                            Console.ForegroundColor = a.Color;
                            Console.Write(a.Icon);
                        }
                    }
                }
                Count = 5;
                if (Rogue.RAM.Player.Buffs != null)
                {
                    for (int i = 0; i < Rogue.RAM.Player.Buffs.Count; i++)
                    {
                        MechEngine.Ability a = Rogue.RAM.Player.Buffs[i];
                        Count++;
                        Console.SetCursorPosition(96, Count);
                        Console.ForegroundColor = a.Color;
                        Console.Write(a.Icon);
                    }
                }
            }

            private static void ClearBuffsDebuffs()
            { for (int i = 5; i < 9; i++) { Console.SetCursorPosition(96, i); Console.Write(' '); Console.SetCursorPosition(3, i); Console.Write(' '); } }
        }

        public static class MerchantDraw
        {
            /// <summary>
            /// Draw goods window in merchant
            /// </summary>
            /// <param name="ReDraw">Set its true if you redrawning window or draw first time</param>
            public static void DrawGoodsWindow(bool ReDraw = false)
            {
                DrawMerchantWindow();
                CharacterDraw.DrawMainInfoCharWindow();
                CharacterDraw.DrawHeader("Торговля");
                int Count = 0;
                foreach (MechEngine.Item i in (Rogue.RAM.Merch as MechEngine.Merchant).Goods) { Count++; dah(Count, i); }
                if (ReDraw) { dasr(Rogue.RAM.MerchTab.NowTab, true, true); }
            }
            /// <summary>
            /// Draw or redraw merchant window
            /// </summary>
            public static void DrawMerchantWindow()
            {
                FightDraw.DrawEnemyGUI();

                var e = Rogue.RAM.Merch;
                //name
                Console.ForegroundColor = ConsoleColor.Yellow;
                int Count = (23 / 2) - (e.Name.Length / 2);
                Console.SetCursorPosition(Count + 1, 1);
                Console.WriteLine(DrawHelp.FullLine(" ".Length + e.Name.Length, " " + e.Name, " ".Length + e.Name.Length - 1));

                char ic='\0';
                ConsoleColor cc= ConsoleColor.Black;
                int cg = 0;
                int mg = 0;
                if (Rogue.RAM.Merch.GetType() == typeof(MechEngine.Merchant))
                {
                    ic = (e as MechEngine.Merchant).SpeachIcon;
                    cc = (e as MechEngine.Merchant).SpeachColor;
                    cg = (e as MechEngine.Merchant).CurGold;
                    mg = (e as MechEngine.Merchant).MaxGold;
                }
                else if (Rogue.RAM.Merch.GetType() == typeof(MechEngine.Member))
                {
                    ic = (e as MechEngine.Member).SpeachIcon;
                    cc = (e as MechEngine.Member).SpeachColor;
                    cg = (e as MechEngine.Member).CurGold;
                    mg = (e as MechEngine.Member).MaxGold;
                }
                DrawEngine.DrawHelp.DrawAvatar = new ColorChar() { Color = cc, Char = ic };

                Console.ForegroundColor = ConsoleColor.Yellow;
                Count = (23 / 2) - ((9 + cg.ToString().Length) / 2);
                Console.SetCursorPosition(Count + 1, 12);
                string WriteThis = "Золотых: " + cg.ToString();
                Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));
                //ap
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Count = (23 / 2) - ((10 + mg.ToString().Length) / 2);
                Console.SetCursorPosition(Count + 1, 13);
                WriteThis = "Максимум: " + mg.ToString();
                Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));
              

            }
            /// <summary>
            /// ReDraw only currnet gold
            /// </summary>
            public static void ReDrawMoney()
            {
                MechEngine.Merchant e = (Rogue.RAM.Merch as MechEngine.Merchant);
                Console.ForegroundColor = ConsoleColor.Yellow;
                int Count = (23 / 2) - ((9 + e.CurGold.ToString().Length) / 2);
                Console.SetCursorPosition(Count + 1, 12);
                string WriteThis = "Золотых: " + e.CurGold.ToString();
                Console.WriteLine(DrawHelp.FullLine(WriteThis.Length, WriteThis, WriteThis.Length - 1));
            }
            /// <summary>
            /// (d)raw (a)bility (h)elper
            /// </summary>
            /// <param name="Position">Count from top position (1,2,3,4)</param>
            /// <param name="Ability">subj</param>
            /// <param name="LeftRight">Left side = false, Right side = true</param>
            private static void dah(int Position, MechEngine.Item Good)
            {
                int left = 0;
                switch (Position)
                {
                    case 1: { Position = 4; left = 31; break; }
                    case 2: { Position = 9; left = 31; break; }
                    case 3: { Position = 14; left = 31; break; }
                    case 4: { Position = 19; left = 31; break; }
                    case 5: { Position = 4; left = 48; break; }
                    case 6: { Position = 9; left = 48; break; }
                    case 7: { Position = 14; left = 48; break; }
                    case 8: { Position = 19; left = 48; break; }
                    case 9: { Position = 4; left = 62; break; }
                    case 10: { Position = 9; left = 62; break; }
                    case 11: { Position = 14; left = 62; break; }
                    case 12: { Position = 19; left = 62; break; }
                }
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(left, Position);
                Console.Write("┌───┐ ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(left, Position + 1);
                Console.Write("│   │ ");
                //level
                //Console.ForegroundColor = ConsoleColor.DarkCyan;
                //Console.SetCursorPosition(Side + 6, Position + 1);
                //Console.Write("$: ");
                //Console.ForegroundColor = ConsoleColor.DarkYellow;
                //Console.SetCursorPosition(Side + 11, Position + 1);
                //Console.Write(Good.Sell);
                //
                Console.SetCursorPosition(left + 2, Position + 1);
                Console.ForegroundColor = Good.Color;
                Console.Write(Good.Icon());
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(left, Position + 2);
                Console.Write("└───┘");
                //name of item
                Console.SetCursorPosition(left, Position + 3);
                Console.ForegroundColor = Good.Color;
                if (Good.Name.Length > 7) { Console.Write(Good.Name.Substring(0, 7)); } else { Console.Write(Good.Name); };
                Console.SetCursorPosition(left, Position + 4);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("$:" + Good.Sell);

                //rate of COE
                //Console.SetCursorPosition(Side + 6, Position + 2);
                //Console.Write("Кол: (1)");
            }
            /// <summary>
            /// Draw selected ability | Be CARE! Here Change _NOW-TAB!!!! |
            /// </summary>
            /// <param name="Dir">ArrowDirection object equal ConsoleKey.*Arrow</param>
            public static void DrawAbilitySelect(SystemEngine.ArrowDirection Dir)
            {
                switch (Dir)
                {
                    case SystemEngine.ArrowDirection.Top: { if (Rogue.RAM.MerchTab.NowTab != 1 && Rogue.RAM.MerchTab.NowTab != 5 && Rogue.RAM.MerchTab.NowTab != 9) { dasr(Rogue.RAM.MerchTab.NowTab - 1); Rogue.RAM.MerchTab.NowTab -= 1; } break; }
                    case SystemEngine.ArrowDirection.Bot: { if (Rogue.RAM.MerchTab.NowTab != 4 && Rogue.RAM.MerchTab.NowTab != 8 && Rogue.RAM.MerchTab.NowTab != 12) { dasr(Rogue.RAM.MerchTab.NowTab + 1); Rogue.RAM.MerchTab.NowTab += 1; } break; }
                    case SystemEngine.ArrowDirection.Left:
                        {
                            if (Rogue.RAM.MerchTab.NowTab != 1 && Rogue.RAM.MerchTab.NowTab != 2 && Rogue.RAM.MerchTab.NowTab != 3 && Rogue.RAM.MerchTab.NowTab != 4) { dasr(Rogue.RAM.MerchTab.NowTab - 4); Rogue.RAM.MerchTab.NowTab -= 4; } break;
                        }
                    case SystemEngine.ArrowDirection.Right:
                        {
                            if (Rogue.RAM.MerchTab.NowTab != 9 && Rogue.RAM.MerchTab.NowTab != 10 && Rogue.RAM.MerchTab.NowTab != 11 && Rogue.RAM.MerchTab.NowTab != 12) { dasr(Rogue.RAM.MerchTab.NowTab + 4); Rogue.RAM.MerchTab.NowTab += 4; } break;
                        }
                }
            }
            /// <summary>
            /// 
            /// (d)raw (a)bility (s)elected (r)eal method
            /// </summary>
            /// <param name="index">Index of ability: 1-4 combat, 5-8 map</param>
            /// <param name="Bold">Only for self-recursion</param>
            /// <param name="ReDrawning">Set its true if you redrawning window or draw first time</param>
            private static void dasr(int index, bool Bold = true, bool ReDrawning = false)
            {
                #region Vertical

                int vertical = 0;
                if (index == 1 || index == 5 || index == 9) { vertical = 4; }
                if (index == 2 || index == 6 || index == 10) { vertical = 9; }
                if (index == 3 || index == 7 || index == 11) { vertical = 14; }
                if (index == 4 || index == 8 || index == 12) { vertical = 19; }

                #endregion

                #region Horizontal

                int horizontal = 0;
                if (index == 1 || index == 2 || index == 3 || index == 4) { horizontal = 31; }
                if (index == 5 || index == 6 || index == 7 || index == 8) { horizontal = 48; }
                if (index == 9 || index == 10 || index == 11 || index == 12) { horizontal = 62; }

                #endregion

                if (Bold) { Console.ForegroundColor = Rogue.RAM.CUIColor; } else { Console.ForegroundColor = ConsoleColor.Gray; }

                Console.SetCursorPosition(horizontal, vertical);

                if (Bold) { Console.Write("╔───╗ "); } else { Console.Write("┌───┐ "); }

                Console.SetCursorPosition(horizontal, vertical + 1);
                Console.Write("│");
                Console.SetCursorPosition(horizontal + 4, vertical + 1);
                Console.Write("│");
                Console.SetCursorPosition(horizontal, vertical + 2);

                if (Bold) { Console.Write("╚───╝"); } else { Console.Write("└───┘"); }

                if (!ReDrawning) { if (Bold) { dasr(Rogue.RAM.MerchTab.NowTab, false); } }
            }
        }

        public static class QuestgiverDraw
        {
            /// <summary>
            /// Draw quest giver window
            /// </summary>
            public static void DrawGiverWindow()
            {
                FightDraw.DrawEnemyGUI();

                MechEngine.Questgiver e = Rogue.RAM.qGiver;

                Console.ForegroundColor = e.SpeachColor;
                int Count = (23 / 2) - (e.Name.Length / 2);
                Console.SetCursorPosition(Count + 1, 2);
                Console.WriteLine(DrawHelp.FullLine(" ".Length + e.Name.Length, " " + e.Name, " ".Length + e.Name.Length - 1));

                DrawEngine.DrawHelp.DrawAvatar = new ColorChar() { Char = e.SpeachIcon, Color = e.SpeachColor };


                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Count = (23 / 2) - (" Задание:".Length / 2);
                Console.SetCursorPosition(Count + 1, 12);
                Console.WriteLine(" Задание:");
                //text of quest
                int forCount = 14;
                foreach (string s in DrawEngine.DrawHelp.TextBlock(e.Quest.Speach, 21))
                {
                    Console.SetCursorPosition(3, forCount++);
                    Console.WriteLine(s);
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Count = (23 / 2) - (" Награда:".Length / 2);
                Console.SetCursorPosition(Count + 1, forCount++);
                Console.WriteLine(" Награда:");

                int exemplar = 0;
                foreach (MechEngine.Item itm in e.Quest.Rewards.Items)
                {
                    foreach (MechEngine.Item search in e.Quest.Rewards.Items)
                    { if (itm.Name == search.Name) { exemplar++; } }
                    Console.ForegroundColor = itm.Color;
                    string nameofreward = string.Empty;
                    if (itm.Name.Length > 14) { nameofreward = itm.Name.Substring(0, 14); } else { nameofreward = itm.Name; };
                    Count = (23 / 2) - ((nameofreward + " (" + exemplar.ToString() + ")").Length / 2);
                    Console.SetCursorPosition(Count + 1, forCount++);
                    Console.WriteLine(nameofreward + " (" + exemplar + ")");
                }

                foreach (MechEngine.Perk prk in e.Quest.Rewards.Perks)
                {
                    Console.ForegroundColor = prk.Color;
                    string nameofreward = string.Empty;
                    if (prk.Name.Length > 14) { nameofreward = prk.Name.Substring(0, 14); } else { nameofreward = prk.Name; };
                    Count = (23 / 2) - (("Перк: " + nameofreward).Length / 2);
                    Console.SetCursorPosition(Count + 1, forCount++);
                    Console.WriteLine("Перк: " + nameofreward);
                }

                foreach (MechEngine.Ability abl in e.Quest.Rewards.Abilityes)
                {
                    Console.ForegroundColor = abl.Color;
                    string nameofreward = string.Empty;
                    if (abl.Name.Length > 14) { nameofreward = abl.Name.Substring(0, 14); } else { nameofreward = abl.Name; };
                    Count = (23 / 2) - (("Нав: <" + nameofreward + ">").Length / 2);
                    Console.SetCursorPosition(Count + 1, forCount++);
                    Console.WriteLine("Нав <" + nameofreward + ">");
                }

                foreach (int epx in e.Quest.Rewards.Exp)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    string nameofreward = string.Empty;
                    if (epx.ToString().Length > 14) { nameofreward = epx.ToString().Substring(0, 12) + "~"; } else { nameofreward = epx.ToString(); };
                    Count = (23 / 2) - (("EXP: " + nameofreward).Length / 2);
                    Console.SetCursorPosition(Count + 1, forCount++);
                    Console.WriteLine("EXP: " + nameofreward);
                }

                foreach (int epx in e.Quest.Rewards.Gold)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    string nameofreward = string.Empty;
                    if (epx.ToString().Length > 14) { nameofreward = epx.ToString().Substring(0, 12) + "~"; } else { nameofreward = epx.ToString(); };
                    Count = (23 / 2) - (("$: " + nameofreward).Length / 2);
                    Console.SetCursorPosition(Count + 1, forCount++);
                    Console.WriteLine("$: " + nameofreward);
                }
            }
        }

        public static class ActiveObjectDraw
        {
            public static void Draw(List<string> repl, MechEngine.CapitalDoor.GateKeeper obj)
            {
                FightDraw.DrawEnemyGUI();

                MechEngine.CapitalDoor.GateKeeper e = obj;

                Console.ForegroundColor = e.Color;
                int Count = (23 / 2) - (e.Name.Length / 2);
                Console.SetCursorPosition(Count + 1, 2);
                Console.WriteLine(DrawHelp.FullLine(" ".Length + e.Name.Length, " " + e.Name, " ".Length + e.Name.Length - 1));

                DrawEngine.DrawHelp.DrawAvatar = new ColorChar() { Char = e.Icon, Color = e.Color };

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Count = (23 / 2) - (" Куда переместиться:".Length / 2);
                Console.SetCursorPosition(Count + 1, 12);
                Console.WriteLine(" Куда переместиться:");

                int forCount = 14;

                foreach (string loc in repl)
                {
                    Count = (23 / 2) - (loc.Length / 2);
                    Console.SetCursorPosition(Count + 1, forCount++);
                    Console.WriteLine(loc);
                }
            }

            public static void Draw(List<string> repl, MechEngine.ActiveObject obj, ConsoleColor TextColor)
            {
                FightDraw.DrawEnemyGUI();

                MechEngine.ActiveObject e = obj;

                Console.ForegroundColor = e.Color;
                int Count = (23 / 2) - (e.Name.Length / 2);
                Console.SetCursorPosition(Count + 1, 2);
                Console.WriteLine(DrawHelp.FullLine(" ".Length + e.Name.Length, " " + e.Name, " ".Length + e.Name.Length - 1));

                DrawEngine.DrawHelp.DrawAvatar = new ColorChar() { Char = e.Icon, Color = e.Color };

                Console.ForegroundColor = TextColor;
                Count = (23 / 2) - ("Действия:".Length / 2);
                Console.SetCursorPosition(Count + 1, 12);
                Console.WriteLine("Действия:");

                int forCount = 14;

                foreach (string loc in repl)
                {
                    Count = (23 / 2) - (loc.Length / 2);
                    Console.SetCursorPosition(Count + 1, forCount++);
                    Console.WriteLine(loc);
                }
            }

            public static void Draw(Replica R, MechEngine.ActiveObject obj)
            {
                FightDraw.DrawEnemyGUI();

                MechEngine.ActiveObject e = obj;

                Console.ForegroundColor = e.Color;
                int Count = (23 / 2) - (e.Name.Length / 2);
                Console.SetCursorPosition(Count + 1, 2);
                Console.WriteLine(DrawHelp.FullLine(" ".Length + e.Name.Length, " " + e.Name, " ".Length + e.Name.Length - 1));

                DrawEngine.DrawHelp.DrawAvatar = new ColorChar() { Char = e.Icon, Color = e.Color };
                int forCount = 10;
                Console.ForegroundColor = R.TextColor;
                foreach (string s in DrawEngine.DrawHelp.TextBlock(R.Text, 21))
                {
                    Count = (25 / 2) - (s.Replace('<', '\0').Replace('<', '\0').Length / 2);
                    Count++;
                    forCount++;
                    if (s != "" && s != " ")
                    {
                        foreach (char c in s)
                        {
                            if (c == '<') { Console.ForegroundColor = ConsoleColor.Green; continue; }
                            else if (c == '>') { Console.ForegroundColor = R.TextColor; continue; }

                            Console.SetCursorPosition(Count++, forCount);
                            Console.WriteLine(c);

                        }
                    }
                    // { Console.Write(s.Substring(1, s.Length - 1)); } else { Console.WriteLine(s); } }
                }
                forCount += 2;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Count = (23 / 2) - ("Действия:".Length / 2);
                Console.SetCursorPosition(Count + 1, forCount);
                Console.WriteLine("Действия:");
                forCount++;
                Console.ForegroundColor = R.OptionsColor;
                foreach (string repl in R.Options)
                {
                    if (repl.Length > 20)
                    {
                        Count = (23 / 2) - (repl.Substring(0,20).Length / 2);
                        Console.SetCursorPosition(Count + 1, forCount++);
                        Console.WriteLine(repl.Substring(0, 20));
                    }
                    else
                    {
                        Count = (23 / 2) - (repl.Length / 2);
                        Console.SetCursorPosition(Count + 1, forCount++);
                        Console.WriteLine(repl);
                    }
                }
            }

            public static void DrawChest(MechEngine.Chest obj,int index)
            {
                //winAPIDraw.DrawRightWindow.Clear(); old method
                winAPIDraw.DrawLeftWindow.Start = true;

                
                Console.ForegroundColor = obj.Color;
                int Count = (23 / 2) - (obj.Name.Length / 2);
                //Console.SetCursorPosition(Count + 1, 1);
                //Console.WriteLine(DrawHelp.FullLine(" ".Length + obj.Name.Length, " " + obj.Name, " ".Length + obj.Name.Length - 1));
                winAPIDraw.DrawLeftWindow.AddLine(Count + 1, 1, obj.Name, obj.Color);

                int top = 2;
                int limit = "                  ".Length;

                for (int i = 0; i < 7; i++)
                {
                    bool bold = false;
                    ConsoleColor col = ConsoleColor.Gray;
                    if (index == i) { bold = true; col = Rogue.RAM.CUIColor; }

                    //Console.ForegroundColor = col;
                    top++;
                    //Console.SetCursorPosition(3, top);
                    //Console.WriteLine(DrawHelp.Border(bold, 0, "TopCornLeft") + "──────────────────" + DrawHelp.Border(bold, 0, "TopCornRight"));
                    winAPIDraw.DrawLeftWindow.AddLine(1, top, DrawHelp.Border(bold, 0, "TopCornLeft") + "──────────────────" + DrawHelp.Border(bold, 0, "TopCornRight"), col);
                    top++;
                    //Console.SetCursorPosition(3, top);
                    //Console.WriteLine("│                  │");
                    winAPIDraw.DrawLeftWindow.AddLine(1, top, "│                  │", col);

                    try
                    {
                        //itm
                        MechEngine.Item it = obj.Items[i];
                        string itm = string.Empty;
                        if (it.Name.Length > limit) { itm = it.Name.Substring(0, limit); } else { itm = it.Name; }
                        int count = (22 / 2) - (itm.Length / 2);
                        //Console.SetCursorPosition(count, top);
                        //Console.ForegroundColor = it.Color;
                        //Console.Write(itm);                
                        winAPIDraw.DrawLeftWindow.AddLine(count, top, itm, it.Color);
                    }
                    catch (ArgumentOutOfRangeException) { }
                    //Console.ForegroundColor = col;

                    top++;
                    //Console.SetCursorPosition(3, top);
                    //Console.WriteLine(DrawHelp.Border(bold, 0, "BotCornLeft") + "──────────────────" + DrawHelp.Border(bold, 0, "BotCornRight"));
                    winAPIDraw.DrawLeftWindow.AddLine(1, top, DrawHelp.Border(bold, 0, "BotCornLeft") + "──────────────────" + DrawHelp.Border(bold, 0, "BotCornRight"), col);
                    bold = false;
                }

                winAPIDraw.DrawLeftWindow.Draw = true;
            }
        }

        public static class CharacterDraw
        {
            public static void DrawInterface()
            {
                DrawItemsWindow();
                DrawMainInfoCharWindow();
                //DrawPerks();
            }
            /// <summary>
            /// Draw left window in character info
            /// </summary>
            public static void DrawItemsWindow()
            {
                int height = 24;
                Console.ForegroundColor = Rogue.RAM.CUIColor;

                string Clear = string.Empty;
                Clear = DrawHelp.FullLine(25, DrawHelp.Border(true, 4));
                Clear = DrawHelp.Border(true, 1) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 2);
                Console.SetCursorPosition(1, 0);
                int t = Clear.Length;
                Console.WriteLine(Clear);

                //тело
                for (int i = 1; i < height; i++)
                {
                    Clear = DrawHelp.FullLine(25, " ");
                    Clear = DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 3);
                    Console.SetCursorPosition(1, i);
                    Console.WriteLine(Clear);
                }

                //носки 
                Clear = string.Empty;
                Clear = DrawHelp.FullLine(25, DrawHelp.Border(true, 4));
                Clear = DrawHelp.Border(true, 5) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 6);
                Console.SetCursorPosition(1, height);
                Console.WriteLine(Clear);

                //Инвентарь

                Console.ForegroundColor = ConsoleColor.Gray;
                int Count = (23 / 2) - ("Экипировка:".Length / 2);
                Console.SetCursorPosition(Count + 1, 1);
                Console.WriteLine(DrawHelp.FullLine("Экипировка:".Length, "Экипировка:", "Экипировка:".Length - 1));

                //Слоты инвентаря
                Count = (23 / 2) - ("┌───┐".Length / 2);
                Console.SetCursorPosition(Count + 1, 2);
                Console.WriteLine(DrawHelp.FullLine("┌───┐".Length, "┌───┐", "┌───┐".Length - 1));
                Console.SetCursorPosition(Count + 1, 3);
                Console.WriteLine(DrawHelp.FullLine("│   │".Length, "│   │", "│   │".Length - 1));
                Console.SetCursorPosition(Count + 1, 4);
                Console.WriteLine(DrawHelp.FullLine("└───┘".Length, "└───┘", "└───┘".Length - 1));
                Count = (23 / 2) - ("┌───┐ ┌───┐ ┌───┐".Length / 2);
                Console.SetCursorPosition(Count + 1, 5);
                Console.WriteLine(DrawHelp.FullLine("┌───┐ ┌───┐ ┌───┐".Length, "┌───┐ ┌───┐ ┌───┐", "┌───┐ ┌───┐ ┌───┐".Length - 1));
                Console.SetCursorPosition(Count + 1, 6);
                Console.WriteLine(DrawHelp.FullLine("│   │ │   │ │   │".Length, "│   │ │   │ │   │", "│   │ │   │ │   │".Length - 1));
                Console.SetCursorPosition(Count + 1, 7);
                Console.WriteLine(DrawHelp.FullLine("└───┘ └───┘ └───┘".Length, "└───┘ └───┘ └───┘", "└───┘ └───┘ └───┘".Length - 1));
                Count = (23 / 2) - ("┌───┐".Length / 2);
                Console.SetCursorPosition(Count + 1, 8);
                Console.WriteLine(DrawHelp.FullLine("┌───┐".Length, "┌───┐", "┌───┐".Length - 1));
                Console.SetCursorPosition(Count + 1, 9);
                Console.WriteLine(DrawHelp.FullLine("│   │".Length, "│   │", "│   │".Length - 1));
                Console.SetCursorPosition(Count + 1, 10);
                Console.WriteLine(DrawHelp.FullLine("└───┘".Length, "└───┘", "└───┘".Length - 1));

                string itm = " ";
                string ilvl = " ";
                string forwr = " ";
                //Rogue.RAM.Player.Equipment.Armor = (MechEngine.Item.Armor)DataBase.ItemBase.GetItem();
                //Thread.Sleep(500);
                //Rogue.RAM.Player.Equipment.Helm = (MechEngine.Item.Helm)DataBase.ItemBase.GetItem();
                //Thread.Sleep(500);
                //Rogue.RAM.Player.Equipment = DataBase.ItemBase.GetItem();
                //Thread.Sleep(500);
                //Rogue.RAM.Player.Equipment = DataBase.ItemBase.GetItem();
                //Thread.Sleep(500);
                //Rogue.RAM.Player.Equipment = DataBase.ItemBase.GetItem();

                //head
                Console.ForegroundColor = ConsoleColor.Gray;
                Count = (23 / 2) - ("Экипировано:".Length / 2);
                forwr = "Экипировано:";
                Console.SetCursorPosition(Count + 1, 12);
                Console.WriteLine(DrawHelp.FullLine(forwr.Length, forwr, forwr.Length - 1));
                if (Rogue.RAM.Player.Equipment.Helm != null)
                {
                    itm = Rogue.RAM.Player.Equipment.Helm.Name;
                    ilvl = Rogue.RAM.Player.Equipment.Helm.ILvl.ToString();
                    Console.ForegroundColor = Rogue.RAM.Player.Equipment.Helm.Color;
                }
                else
                {
                    itm = " ";
                    ilvl = " ";
                }
                Count = (23 / 2) - ((itm.Length + 1 + ilvl.Length) / 2);
                forwr = itm + " " + ilvl;
                Console.SetCursorPosition(Count + 1, 14);
                Console.WriteLine(DrawHelp.FullLine(forwr.Length, forwr, forwr.Length - 1));

                //chest               
                if (Rogue.RAM.Player.Equipment.Armor != null)
                {
                    itm = Rogue.RAM.Player.Equipment.Armor.Name;
                    ilvl = Rogue.RAM.Player.Equipment.Armor.ILvl.ToString();
                    Console.ForegroundColor = Rogue.RAM.Player.Equipment.Armor.Color;
                }
                else
                {
                    itm = " ";
                    ilvl = " ";
                }
                Count = (23 / 2) - ((itm.Length + 1 + ilvl.Length) / 2);
                forwr = itm + " " + ilvl;
                Console.SetCursorPosition(Count + 1, 15);
                Console.WriteLine(DrawHelp.FullLine(forwr.Length, forwr, forwr.Length - 1));

                //boots

                if (Rogue.RAM.Player.Equipment.Boots != null)
                {
                    itm = Rogue.RAM.Player.Equipment.Boots.Name;
                    ilvl = Rogue.RAM.Player.Equipment.Boots.ILvl.ToString();
                    Console.ForegroundColor = Rogue.RAM.Player.Equipment.Boots.Color;
                }
                else
                {
                    itm = " ";
                    ilvl = " ";
                }
                Count = (23 / 2) - ((itm.Length + ilvl.Length + 1) / 2);
                forwr = itm + " " + ilvl;
                Console.SetCursorPosition(Count + 1, 16);
                Console.WriteLine(DrawHelp.FullLine(forwr.Length, forwr, forwr.Length - 1));

                //weapon
                if (Rogue.RAM.Player.Equipment.Weapon != null)
                {
                    itm = Rogue.RAM.Player.Equipment.Weapon.Name;
                    ilvl = Rogue.RAM.Player.Equipment.Weapon.ILvl.ToString();
                    Console.ForegroundColor = Rogue.RAM.Player.Equipment.Weapon.Color;
                }
                else
                {
                    itm = " ";
                    ilvl = " ";
                }
                Count = (23 / 2) - ((itm.Length + ilvl.Length + 1) / 2);
                forwr = itm + " " + ilvl;
                Console.SetCursorPosition(Count + 1, 17);
                Console.WriteLine(DrawHelp.FullLine(forwr.Length, forwr, forwr.Length - 1));

                //Offhand
                if (Rogue.RAM.Player.Equipment.OffHand != null)
                {
                    itm = Rogue.RAM.Player.Equipment.OffHand.Name;
                    ilvl = Rogue.RAM.Player.Equipment.OffHand.ILvl.ToString();
                    Console.ForegroundColor = Rogue.RAM.Player.Equipment.OffHand.Color;
                }
                else
                {
                    itm = " ";
                    ilvl = " ";
                }
                Count = (23 / 2) - ((itm.Length + ilvl.Length + 1) / 2);
                forwr = itm + " " + ilvl;
                Console.SetCursorPosition(Count + 1, 18);
                Console.WriteLine(DrawHelp.FullLine(forwr.Length, forwr, forwr.Length - 1));

                Console.ForegroundColor = ConsoleColor.Red;
                int gear = MechEngine.Item.GetGearScore();
                Count = (23 / 2) - (("GearScore: ".Length + MechEngine.Item.GetGearScore().ToString().Length) / 2);
                forwr = "GearScore: " + MechEngine.Item.GetGearScore().ToString();
                Console.SetCursorPosition(Count + 1, 20);
                Console.WriteLine(DrawHelp.FullLine(forwr.Length, forwr, forwr.Length - 1));

                //Заполнение ячеек
                if (Rogue.RAM.Player.Equipment.Helm != null)
                {
                    Console.SetCursorPosition(12, 3);
                    Console.ForegroundColor = Rogue.RAM.Player.Equipment.Helm.Color;
                    Console.Write(Rogue.RAM.Player.Equipment.Helm.Icon());
                }

                if (Rogue.RAM.Player.Equipment.Armor != null)
                {
                    Console.SetCursorPosition(12, 6);
                    Console.ForegroundColor = Rogue.RAM.Player.Equipment.Armor.Color;
                    Console.Write(Rogue.RAM.Player.Equipment.Armor.Icon());
                }

                if (Rogue.RAM.Player.Equipment.Weapon != null)
                {
                    Console.SetCursorPosition(6, 6);
                    Console.ForegroundColor = Rogue.RAM.Player.Equipment.Weapon.Color;
                    Console.Write(Rogue.RAM.Player.Equipment.Weapon.Icon());
                }

                if (Rogue.RAM.Player.Equipment.OffHand != null)
                {
                    Console.SetCursorPosition(18, 6);
                    Console.ForegroundColor = Rogue.RAM.Player.Equipment.OffHand.Color;
                    Console.Write(Rogue.RAM.Player.Equipment.OffHand.Icon());
                }

                if (Rogue.RAM.Player.Equipment.Boots != null)
                {
                    Console.SetCursorPosition(12, 9);
                    Console.ForegroundColor = Rogue.RAM.Player.Equipment.Boots.Color;
                    Console.Write(Rogue.RAM.Player.Equipment.Boots.Icon());
                }

                //Ab points
                Console.ForegroundColor = ConsoleColor.Magenta;
                Count = (23 / 2) - (("Очки навыков: ".Length + Rogue.RAM.Player.AbPoint.ToString().Length) / 2);
                forwr = "Очки навыков: " + Rogue.RAM.Player.AbPoint.ToString();
                Console.SetCursorPosition(Count + 1, 22);
                Console.WriteLine(DrawHelp.FullLine(forwr.Length, forwr, forwr.Length - 1));
                //CrPoints
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Count = (23 / 2) - (("Очки профф: ".Length + Rogue.RAM.Player.CrPoint.ToString().Length) / 2);
                forwr = "Очки профф: " + Rogue.RAM.Player.CrPoint.ToString();
                Console.SetCursorPosition(Count + 1, 23);
                Console.WriteLine(DrawHelp.FullLine(forwr.Length, forwr, forwr.Length - 1));

                #region Template
                //                string s = @"┌┐│─└┘├┤┴┬┼
                //      ┌───┐
                //      │─H─│
                //      └───┘
                //┌───┐ ┌───┐ ┌───┐
                //│ A │ │ B │ │ S │
                //└───┘ └───┘ └───┘
                //      ┌───┐
                //      │ B │
                //      └───┘";
                #endregion
            }

            public static void DrawMainInfoCharWindow()
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
            }

            public static void DrawMainInfoCharWindow(bool Overload)
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
                    Clear = " " + DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 49) + DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 50) + DrawHelp.Border(true, 3);
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
            }

            public static void DrawPerks(int tab = 1)
            {
                DrawMainInfoCharWindow();
                DrawHeader("Особенности персонажа");
                Rogue.RAM.iTab.NowTab = tab;
                Rogue.RAM.iTab.MaxTab = (Rogue.RAM.Player.Perks.Count / 5) + 1;
                //if (tab == 0) tab = 1;
                int ctab = (tab * 5) - 5;
                int i = 0;
                if (Rogue.RAM.Player.Perks.Count > 5)
                {
                    string Clear = string.Empty;
                    Console.ForegroundColor = Rogue.RAM.CUIColor;
                    Clear = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
                    Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 7);
                    Console.SetCursorPosition(25, 22);
                    Console.Write(Clear);
                    if (Rogue.RAM.iTab.NowTab >= Rogue.RAM.iTab.MaxTab && Rogue.RAM.iTab.NowTab != 1)
                    {
                        Console.SetCursorPosition(27, 23);
                        Console.Write(" <=");
                    }
                    if (Rogue.RAM.iTab.NowTab < Rogue.RAM.iTab.MaxTab)
                    {

                        Console.SetCursorPosition(70, 23);
                        Console.Write("=> ");
                    }
                }
                for (int q = ctab; q < tab * 5; q++)
                {
                    try
                    {
                        MechEngine.Perk p = Rogue.RAM.Player.Perks[q];
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.SetCursorPosition(28, 3 + i);
                        Console.Write("┌───┐ " + p.Name);
                        Console.SetCursorPosition(28, 4 + i);
                        Console.Write("│   │ " + p.History);
                        Console.ForegroundColor = p.Color;
                        Console.SetCursorPosition(30, 4 + i);
                        Console.Write(p.Icon);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.SetCursorPosition(28, 5 + i);
                        string stats = string.Empty;
                        if (p.AP != 0)
                        {
                            stats += " AP" + GetSign(p.AP);
                        }
                        if (p.AD != 0)
                        {
                            stats += " AD" + GetSign(p.AD);
                        }
                        if (p.ARM != 0)
                        {
                            stats += " ARM" + GetSign(p.ARM);
                        }
                        if (p.HP != 0)
                        {
                            stats += " HP" + GetSign(p.HP);
                        }
                        if (p.MADMG != 0)
                        {
                            stats += " DMG↑" + GetSign(p.MADMG);
                        }
                        if (p.MIDMG != 0)
                        {
                            stats += " DMG↓" + GetSign(p.MIDMG);
                        }
                        if (p.MP != 0)
                        {
                            stats += " MP" + GetSign(p.MP);
                        }
                        if (p.MRS != 0)
                        {
                            stats += " MRS" + GetSign(p.MRS);
                        }
                        Console.Write("└───┘ " + stats);
                        //
                        string Clear = string.Empty;
                        Console.ForegroundColor = Rogue.RAM.CUIColor;
                        Clear = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
                        Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 7);
                        Console.SetCursorPosition(25, 6 + i);
                        Console.WriteLine(Clear);
                        i = i + 4;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        //пофигу
                    }
                }
            }

            public static void DrawBuffs(int tab = 1)
            {
                DrawMainInfoCharWindow();
                DrawHeader("Эффекты персонажа");
                Rogue.RAM.iTab.NowTab = tab;
                Rogue.RAM.iTab.MaxTab = (Rogue.RAM.Player.Buffs.Count / 5) + 1;
                //if (tab == 0) tab = 1;
                int ctab = (tab * 5) - 5;
                int i = 0;
                if (Rogue.RAM.Player.Buffs.Count > 5)
                {
                    string Clear = string.Empty;
                    Console.ForegroundColor = Rogue.RAM.CUIColor;
                    Clear = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
                    Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 7);
                    Console.SetCursorPosition(25, 22);
                    Console.Write(Clear);
                    if (Rogue.RAM.iTab.NowTab >= Rogue.RAM.iTab.MaxTab && Rogue.RAM.iTab.NowTab != 1)
                    {
                        Console.SetCursorPosition(27, 23);
                        Console.Write(" <=");
                    }
                    if (Rogue.RAM.iTab.NowTab < Rogue.RAM.iTab.MaxTab)
                    {

                        Console.SetCursorPosition(70, 23);
                        Console.Write("=> ");
                    }
                }
                for (int q = ctab; q < tab * 5; q++)
                {
                    try
                    {
                        MechEngine.Ability p = Rogue.RAM.Player.Buffs[q];
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.SetCursorPosition(28, 3 + i);
                        Console.Write("┌───┐ " + p.Name);
                        Console.SetCursorPosition(28, 4 + i);
                        Console.Write("│   │ Уровень навыка:" + p.Level.ToString());
                        Console.ForegroundColor = p.Color;
                        Console.SetCursorPosition(30, 4 + i);
                        Console.Write(p.Icon);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.SetCursorPosition(28, 5 + i);
                        Console.Write("└───┘ " + "Время действия: " + p.Duration.ToString());
                        //
                        string Clear = string.Empty;
                        Console.ForegroundColor = Rogue.RAM.CUIColor;
                        Clear = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
                        Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 7);
                        Console.SetCursorPosition(25, 6 + i);
                        Console.WriteLine(Clear);
                        i = i + 4;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        //пофигу
                    }
                }
            }

            public static void DrawRepute(int tab = 1)
            {
                DrawMainInfoCharWindow();
                DrawHeader("Дипломатическая репутация");
                Rogue.RAM.iTab.NowTab = tab;
                Rogue.RAM.iTab.MaxTab = (Rogue.RAM.Player.Repute.Count / 5) + 1;
                //if (tab == 0) tab = 1;
                int ctab = (tab * 5) - 5;
                int i = 0;
                if (Rogue.RAM.Player.Repute.Count > 5)
                {
                    string Clear = string.Empty;
                    Console.ForegroundColor = Rogue.RAM.CUIColor;
                    Clear = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
                    Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 7);
                    Console.SetCursorPosition(25, 22);
                    Console.Write(Clear);
                    if (Rogue.RAM.iTab.NowTab >= Rogue.RAM.iTab.MaxTab && Rogue.RAM.iTab.NowTab != 1)
                    {
                        Console.SetCursorPosition(27, 23);
                        Console.Write(" <=");
                    }
                    if (Rogue.RAM.iTab.NowTab < Rogue.RAM.iTab.MaxTab)
                    {

                        Console.SetCursorPosition(70, 23);
                        Console.Write("=> ");
                    }
                }
                for (int q = ctab; q < tab * 5; q++)
                {
                    try
                    {
                        MechEngine.Reputation p = Rogue.RAM.Player.Repute[q];
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.SetCursorPosition(28, 3 + i);
                        Console.Write("Фракция: " + p.name);
                        Console.SetCursorPosition(28, 4 + i);
                        Console.Write("Прогресс: " +p.min.ToString()+"/"+p.max.ToString());
                        Console.SetCursorPosition(28, 5 + i);
                        Console.Write("Цель: " + SystemEngine.Helper.String.ToString(p.race));
                        //
                        string Clear = string.Empty;
                        Console.ForegroundColor = Rogue.RAM.CUIColor;
                        Clear = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
                        Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 7);
                        Console.SetCursorPosition(25, 6 + i);
                        Console.WriteLine(Clear);
                        i = i + 4;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        //пофигу
                    }
                }
            }

            public static void DrawQuests(int tab = 1)
            {
                DrawMainInfoCharWindow();
                DrawHeader("Задания персонажа");
                Rogue.RAM.Qtab.NowTab = tab;
                Rogue.RAM.Qtab.MaxTab = (Rogue.RAM.Player.QuestBook.Count / 5) + 1;
                //if (tab == 0) tab = 1;
                int ctab = (tab * 5) - 5;
                int i = 0;
                if (Rogue.RAM.Player.QuestBook.Count > 5)
                {
                    string Clear = string.Empty;
                    Console.ForegroundColor = Rogue.RAM.CUIColor;
                    Clear = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
                    Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 7);
                    Console.SetCursorPosition(25, 22);
                    Console.Write(Clear);
                    if (Rogue.RAM.Qtab.NowTab >= Rogue.RAM.Qtab.MaxTab && Rogue.RAM.Qtab.NowTab != 1)
                    {
                        Console.SetCursorPosition(27, 23);
                        Console.Write(" <=");
                    }
                    if (Rogue.RAM.Qtab.NowTab < Rogue.RAM.Qtab.MaxTab)
                    {

                        Console.SetCursorPosition(70, 23);
                        Console.Write("=> ");
                    }
                }
                for (int q = ctab; q < tab * 5; q++)
                {
                    try
                    {
                        MechEngine.Quest Q = Rogue.RAM.Player.QuestBook[q];
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.SetCursorPosition(28, 3 + i);
                        Console.Write("┌───┐");
                        Console.SetCursorPosition(28 + 6, 3 + i);
                        Console.ForegroundColor = Q.Difficult;
                        Console.Write(Q.Name);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.SetCursorPosition(28, 4 + i);
                        Console.Write("│   │");
                        Console.SetCursorPosition(28 + 6, 4 + i);
                        Console.ForegroundColor = ConsoleColor.Red;
                        if (Q.Progress >= Math.Floor(Q.TargetCount * 0.2) || Q.Progress == 0) { Console.ForegroundColor = ConsoleColor.Red; }
                        if (Q.Progress >= Math.Floor(Q.TargetCount * 0.4)) { Console.ForegroundColor = ConsoleColor.Yellow; }
                        if (Q.Progress >= Math.Floor(Q.TargetCount * 0.8)) { Console.ForegroundColor = ConsoleColor.Green; }
                        Console.Write("Прогресс: <<" + Q.Progress + "/" + Q.TargetCount + ">>");
                        Console.ForegroundColor = Q.Color;
                        Console.SetCursorPosition(30, 4 + i);
                        Console.Write(Q.Icon);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.SetCursorPosition(28, 5 + i);
                        Console.Write("└───┘ " + Q.Target);
                        //
                        string Clear = string.Empty;
                        Console.ForegroundColor = Rogue.RAM.CUIColor;
                        Clear = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
                        Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 7);
                        Console.SetCursorPosition(25, 6 + i);
                        Console.WriteLine(Clear);
                        i = i + 4;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        //пофигу
                    }
                }
            }
            /// <summary>
            /// Draw 'Equipment' window
            /// </summary>
            /// <param name="tab"></param>
            public static void DrawItems(int tab = 1,int index=0)
            {
                DrawMainInfoCharWindow();
                DrawHeader("Экипировка персонажа");
                //
                MechEngine.Character pl = Rogue.RAM.Player;
                bool bold = false;
                #region Weapon
                if (index == 0) { bold = true; }
                if (pl.Equipment.Weapon != null)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.SetCursorPosition(28, 3);
                    Console.Write(DrawHelp.Border(bold, 0, "TopCornLeft") + "───" + DrawHelp.Border(bold, 0, "TopCornRight") + " " + pl.Equipment.Weapon.Name);
                    Console.SetCursorPosition(28, 4);
                    Console.Write("│   │ " + "Уровень предмета: [" + pl.Equipment.Weapon.ILvl + "]");
                    Console.SetCursorPosition(30, 4);
                    Console.ForegroundColor = pl.Equipment.Weapon.Color;
                    Console.Write(pl.Equipment.Weapon.Icon());
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.SetCursorPosition(28, 5);
                    MechEngine.Item.Weapon p = pl.Equipment.Weapon;
                    string stats = string.Empty;
                    if (p.AP != 0)
                    {
                        stats += " AP" + GetSign(p.AP);
                    }
                    if (p.AD != 0)
                    {
                        stats += " AD" + GetSign(p.AD);
                    }
                    if (p.ARM != 0)
                    {
                        stats += " ARM" + GetSign(p.ARM);
                    }
                    if (p.MADMG != 0)
                    {
                        stats += " DMG↑" + GetSign(p.MADMG);
                    }
                    if (p.MIDMG != 0)
                    {
                        stats += " DMG↓" + GetSign(p.MIDMG);
                    }
                    Console.Write(DrawHelp.Border(bold, 0, "BotCornLeft") + "───" + DrawHelp.Border(bold, 0, "BotCornRight") + " " + stats);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.SetCursorPosition(28, 3);
                    Console.Write(DrawHelp.Border(bold, 0, "TopCornLeft") + "───" + DrawHelp.Border(bold, 0, "TopCornRight") + " Пусто");
                    Console.SetCursorPosition(28, 4);
                    Console.Write("│   │ Нет");
                    Console.SetCursorPosition(28, 5);
                    Console.Write(DrawHelp.Border(bold, 0, "BotCornLeft") + "───" + DrawHelp.Border(bold, 0, "BotCornRight") + " Нет");
                }
                string Clear = string.Empty;
                Console.ForegroundColor = Rogue.RAM.CUIColor;
                Clear = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
                Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 7);
                Console.SetCursorPosition(25, 6);
                Console.WriteLine(Clear);
                bold=false;

                #endregion

                #region Offhand
                if (index == 1) { bold = true; }
                if (pl.Equipment.OffHand != null)
                {                    
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.SetCursorPosition(28, 7);
                    Console.Write(DrawHelp.Border(bold, 0, "TopCornLeft") + "───" + DrawHelp.Border(bold, 0, "TopCornRight") + " " + pl.Equipment.OffHand.Name);
                    Console.SetCursorPosition(28, 8);
                    Console.Write("│   │ " + "Уровень предмета: [" + pl.Equipment.OffHand.ILvl + "]");
                    Console.SetCursorPosition(30, 8);
                    Console.ForegroundColor = pl.Equipment.OffHand.Color;
                    Console.Write(pl.Equipment.OffHand.Icon());
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.SetCursorPosition(28, 9);
                    MechEngine.Item.OffHand p = pl.Equipment.OffHand;
                    string stats = string.Empty;
                    if (p.AP != 0)
                    {
                        stats += " AP" + GetSign(p.AP);
                    }
                    if (p.AD != 0)
                    {
                        stats += " AD" + GetSign(p.AD);
                    }
                    if (p.ARM != 0)
                    {
                        stats += " ARM" + GetSign(p.ARM);
                    }
                    if (p.MADMG != 0)
                    {
                        stats += " DMG↑" + GetSign(p.MADMG);
                    }
                    if (p.MIDMG != 0)
                    {
                        stats += " DMG↓" + GetSign(p.MIDMG);
                    }
                    if (p.MRS != 0)
                    {
                        stats += " DMG↓" + GetSign(p.MIDMG);
                    }
                    Console.Write(DrawHelp.Border(bold, 0, "BotCornLeft") + "───" + DrawHelp.Border(bold, 0, "BotCornRight") + " " + stats);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.SetCursorPosition(28, 7);
                    Console.Write(DrawHelp.Border(bold, 0, "TopCornLeft") + "───" + DrawHelp.Border(bold, 0, "TopCornRight") + " Пусто");
                    Console.SetCursorPosition(28, 8);
                    Console.Write("│   │ Нет");
                    Console.SetCursorPosition(28, 9);
                    Console.Write(DrawHelp.Border(bold, 0, "BotCornLeft") + "───" + DrawHelp.Border(bold, 0, "BotCornRight") + " Нет");
                }
                Clear = string.Empty;
                Console.ForegroundColor = Rogue.RAM.CUIColor;
                Clear = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
                Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 7);
                Console.SetCursorPosition(25, 10);
                Console.WriteLine(Clear);
                bold=false;
                #endregion

                #region Helm
                if (index == 2) { bold = true; }
                if (pl.Equipment.Helm != null)
                {                    
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.SetCursorPosition(28, 11);
                    Console.Write(DrawHelp.Border(bold, 0, "TopCornLeft") + "───" + DrawHelp.Border(bold, 0, "TopCornRight")+" " + pl.Equipment.Helm.Name);
                    Console.SetCursorPosition(28, 12);
                    Console.Write("│   │ " + "Уровень предмета: [" + pl.Equipment.Helm.ILvl + "]");
                    Console.SetCursorPosition(30, 12);
                    Console.ForegroundColor = pl.Equipment.Helm.Color;
                    Console.Write(pl.Equipment.Helm.Icon());
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.SetCursorPosition(28, 13);
                    MechEngine.Item.Helm p = pl.Equipment.Helm;
                    string stats = string.Empty;
                    if (p.AP != 0)
                    {
                        stats += " AP" + GetSign(p.AP);
                    }
                    if (p.AD != 0)
                    {
                        stats += " AD" + GetSign(p.AD);
                    }
                    if (p.HP != 0)
                    {
                        stats += " HP" + GetSign(p.HP);
                    }
                    if (p.MP != 0)
                    {
                        stats += " MP" + GetSign(p.MP);
                    }
                    Console.Write(DrawHelp.Border(bold, 0, "BotCornLeft") + "───" + DrawHelp.Border(bold, 0, "BotCornRight")+" " + stats);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.SetCursorPosition(28, 11);
                    Console.Write(DrawHelp.Border(bold, 0, "TopCornLeft") + "───" + DrawHelp.Border(bold, 0, "TopCornRight") + " Пусто");
                    Console.SetCursorPosition(28, 12);
                    Console.Write("│   │ Нет");
                    Console.SetCursorPosition(28, 13);
                    Console.Write(DrawHelp.Border(bold, 0, "BotCornLeft") + "───" + DrawHelp.Border(bold, 0, "BotCornRight") + " Нет");
                }
                Clear = string.Empty;
                Console.ForegroundColor = Rogue.RAM.CUIColor;
                Clear = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
                Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 7);
                Console.SetCursorPosition(25, 14);
                Console.WriteLine(Clear);
                bold = false;
                #endregion

                #region Armor
                if (index == 3) { bold = true; }
                if (pl.Equipment.Armor != null)
                {                    
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.SetCursorPosition(28, 15);
                    Console.Write(DrawHelp.Border(bold, 0, "TopCornLeft") + "───" + DrawHelp.Border(bold, 0, "TopCornRight")+" "+ pl.Equipment.Armor.Name);
                    Console.SetCursorPosition(28, 16);
                    Console.Write("│   │ " + "Уровень предмета: [" + pl.Equipment.Armor.ILvl + "]");
                    Console.SetCursorPosition(30, 16);
                    Console.ForegroundColor = pl.Equipment.Armor.Color;
                    Console.Write(pl.Equipment.Armor.Icon());
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.SetCursorPosition(28, 17);
                    MechEngine.Item.Armor p = pl.Equipment.Armor;
                    string stats = string.Empty;
                    if (p.ARM != 0)
                    {
                        stats += " ARM" + GetSign(p.ARM);
                    }
                    if (p.MRS != 0)
                    {
                        stats += " MRS" + GetSign(p.MRS);
                    }
                    if (p.HP != 0)
                    {
                        stats += " HP" + GetSign(p.HP);
                    }
                    if (p.MP != 0)
                    {
                        stats += " MP" + GetSign(p.MP);
                    }
                    Console.Write(DrawHelp.Border(bold, 0, "BotCornLeft") + "───" + DrawHelp.Border(bold, 0, "BotCornRight")+" " + stats);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.SetCursorPosition(28, 15);
                    Console.Write(DrawHelp.Border(bold, 0, "TopCornLeft") + "───" + DrawHelp.Border(bold, 0, "TopCornRight") + " Пусто");
                    Console.SetCursorPosition(28, 16);
                    Console.Write("│   │ Нет");
                    Console.SetCursorPosition(28, 17);
                    Console.Write(DrawHelp.Border(bold, 0, "BotCornLeft") + "───" + DrawHelp.Border(bold, 0, "BotCornRight") + " Нет");
                }
                Clear = string.Empty;
                Console.ForegroundColor = Rogue.RAM.CUIColor;
                Clear = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
                Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 7);
                Console.SetCursorPosition(25, 18);
                Console.WriteLine(Clear);
                bold = false;
                #endregion

                #region Boots
                if (index == 4) { bold = true; }
                if (pl.Equipment.Boots != null)
                {                    
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.SetCursorPosition(28, 19);
                    Console.Write(DrawHelp.Border(bold, 0, "TopCornLeft") + "───" + DrawHelp.Border(bold, 0, "TopCornRight") + " " + pl.Equipment.Boots.Name);
                    Console.SetCursorPosition(28, 20);
                    Console.Write("│   │ " + "Уровень предмета: [" + pl.Equipment.Boots.ILvl + "]");
                    Console.SetCursorPosition(30, 20);
                    Console.ForegroundColor = pl.Equipment.Boots.Color;
                    Console.Write(pl.Equipment.Boots.Icon());
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.SetCursorPosition(28, 21);
                    MechEngine.Item.Boots p = pl.Equipment.Boots;
                    string stats = string.Empty;
                    if (p.ARM != 0)
                    {
                        stats += " ARM" + GetSign(p.ARM);
                    }
                    if (p.MRS != 0)
                    {
                        stats += " MRS" + GetSign(p.MRS);
                    }
                    Console.Write(DrawHelp.Border(bold, 0, "BotCornLeft") + "───" + DrawHelp.Border(bold, 0, "BotCornRight") + " " + stats);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.SetCursorPosition(28, 19);
                    Console.Write(DrawHelp.Border(bold, 0, "TopCornLeft") + "───" + DrawHelp.Border(bold, 0, "TopCornRight") + " Пусто");
                    Console.SetCursorPosition(28, 20);
                    Console.Write("│   │ Нет");
                    Console.SetCursorPosition(28, 21);
                    Console.Write(DrawHelp.Border(bold, 0, "BotCornLeft") + "───" + DrawHelp.Border(bold, 0, "BotCornRight") + " Нет");
                }
                Clear = string.Empty;
                Console.ForegroundColor = Rogue.RAM.CUIColor;
                Clear = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
                Clear = " " + DrawHelp.Border(true, 8) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 7);
                Console.SetCursorPosition(25, 22);
                Console.WriteLine(Clear);
                bold = false;
                #endregion
            }
            /// <summary>
            /// Draw ability window in character menu
            /// </summary>
            /// <param name="ReDraw">Set its true if you redrawning window or draw first time</param>
            public static void DrawAbility(bool ReDraw = false)
            {
                DrawMainInfoCharWindow(true);
                DrawHeader("Способности персонажа");
                int count = 0;
                foreach (MechEngine.Ability a in Rogue.RAM.Player.Ability)
                { count += 1; dah(count, a, false); }
                count = 0;
                foreach (MechEngine.Ability a in Rogue.RAM.Player.CraftAbility)
                { count += 1; dah(count, a, true); }
                if (ReDraw) { dasr(Rogue.RAM.iTab.NowTab, true, true); }
            }
            /// <summary>
            /// (d)raw (a)bility (h)elper
            /// </summary>
            /// <param name="Position">Count from top position (1,2,3,4)</param>
            /// <param name="Ability">subj</param>
            /// <param name="LeftRight">Left side = false, Right side = true</param>
            private static void dah(int Position, MechEngine.Ability Ability, bool LeftRight)
            {
                switch (Position) { case 1: { Position = 5; break; } case 2: { Position = 10; break; } case 3: { Position = 15; break; } case 4: { Position = 20; break; } }
                int Side = 0;
                if (LeftRight) { Side = 52; } else { Side = 28; }

                //Title
                if (LeftRight)
                {
                    Console.ForegroundColor = Rogue.RAM.CUIColor;
                    Console.SetCursorPosition(Side + 4, 3);
                    Console.Write("Способности");
                }
                else
                {
                    Console.ForegroundColor = Rogue.RAM.CUIColor;
                    Console.SetCursorPosition(Side + 4, 3);
                    Console.Write("Навыки");
                }
                //
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(Side, Position);
                Console.Write("┌───┐ ");
                //name of ability
                Console.SetCursorPosition(Side + 6, Position);
                Console.ForegroundColor = Ability.Color;
                Console.Write(Ability.Name);
                //
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(Side, Position + 1);
                Console.Write("│   │ ");
                //level
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.SetCursorPosition(Side + 6, Position + 1);
                Console.Write("Lvl: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(Side + 11, Position + 1);
                Console.Write(Ability.Level.ToString());
                //
                Console.SetCursorPosition(Side + 2, Position + 1);
                Console.ForegroundColor = Ability.Color;
                Console.Write(Ability.Icon);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(Side, Position + 2);
                Console.Write("└───┘");
                //rate of COE
                Console.SetCursorPosition(Side + 6, Position + 2);
                if (Ability.COE < 10) { Console.ForegroundColor = ConsoleColor.DarkGray; }
                if (Ability.COE > 25) { Console.ForegroundColor = ConsoleColor.Green; }
                if (Ability.COE > 50) { Console.ForegroundColor = ConsoleColor.Yellow; }
                if (Ability.COE > 80) { Console.ForegroundColor = ConsoleColor.Red; }
                Console.Write("COE: " + Ability.COE.ToString() + "%");
            }
            /// <summary>
            /// Draw selected ability | Be CARE! Here Change _NOW-TAB!!!! |
            /// </summary>
            /// <param name="Dir">ArrowDirection object equal ConsoleKey.*Arrow</param>
            public static void DrawAbilitySelect(SystemEngine.ArrowDirection Dir)
            {
                switch (Dir)
                {
                    case SystemEngine.ArrowDirection.Top: { if (Rogue.RAM.iTab.NowTab != 1 && Rogue.RAM.iTab.NowTab != 5) { dasr(Rogue.RAM.iTab.NowTab - 1); Rogue.RAM.iTab.NowTab -= 1; } break; }
                    case SystemEngine.ArrowDirection.Bot: { if (Rogue.RAM.iTab.NowTab != 4 && Rogue.RAM.iTab.NowTab != 8) { dasr(Rogue.RAM.iTab.NowTab + 1); Rogue.RAM.iTab.NowTab += 1; } break; }
                    case SystemEngine.ArrowDirection.Left:
                        {
                            if (Rogue.RAM.iTab.NowTab != 1 && Rogue.RAM.iTab.NowTab != 2 && Rogue.RAM.iTab.NowTab != 3 && Rogue.RAM.iTab.NowTab != 4) { dasr(Rogue.RAM.iTab.NowTab - 4); Rogue.RAM.iTab.NowTab -= 4; } break;
                        }
                    case SystemEngine.ArrowDirection.Right:
                        {
                            if (Rogue.RAM.iTab.NowTab != 5 && Rogue.RAM.iTab.NowTab != 6 && Rogue.RAM.iTab.NowTab != 7 && Rogue.RAM.iTab.NowTab != 8) { dasr(Rogue.RAM.iTab.NowTab + 4); Rogue.RAM.iTab.NowTab += 4; } break;
                        }
                }
            }
            /// <summary>
            /// 
            /// (d)raw (a)bility (s)elected (r)eal method
            /// </summary>
            /// <param name="index">Index of ability: 1-4 combat, 5-8 map</param>
            /// <param name="Bold">Only for self-recursion</param>
            /// <param name="ReDrawning">Set its true if you redrawning window or draw first time</param>
            private static void dasr(int index, bool Bold = true, bool ReDrawning = false)
            {
                int side = 0;
                int vertical = 0;
                if (index == 1 || index == 2 || index == 3 || index == 4) { side = 28; } else { side = 52; }
                if (index == 1 || index == 5) { vertical = 5; }
                if (index == 2 || index == 6) { vertical = 10; }
                if (index == 3 || index == 7) { vertical = 15; }
                if (index == 4 || index == 8) { vertical = 20; }

                if (Bold) { Console.ForegroundColor = Rogue.RAM.CUIColor; } else { Console.ForegroundColor = ConsoleColor.Gray; }
                Console.SetCursorPosition(side, vertical);
                if (Bold) { Console.Write("╔───╗ "); } else { Console.Write("┌───┐ "); }
                Console.SetCursorPosition(side, vertical + 1);
                Console.Write("│");
                Console.SetCursorPosition(side + 4, vertical + 1);
                Console.Write("│");
                Console.SetCursorPosition(side, vertical + 2);
                if (Bold) { Console.Write("╚───╝"); } else { Console.Write("└───┘"); }
                if (!ReDrawning) { if (Bold) { dasr(Rogue.RAM.iTab.NowTab, false); } }
            }

            public static void DrawHeader(string Text)
            {
                int Count = (100 / 2) - (Text.Length / 2);
                Console.SetCursorPosition(Count + 1, 1);
                Console.WriteLine(DrawHelp.FullLine(Text.Length, Text, Text.Length - 1));
            }

            public static void DrawColorHeader(string Text, ConsoleColor Color)
            {
                Console.ForegroundColor = Color;
                int Count = (100 / 2) - (Text.Length / 2);
                Console.SetCursorPosition(Count + 1, 1);
                Console.WriteLine(DrawHelp.FullLine(Text.Length, Text, Text.Length - 1));
            }

            public static void DrawLevelUp(int gold, int hp, int mp, int ldmg, int mdmg)
            {
                DrawMainInfoCharWindow();
                DrawItemsWindow();
                Console.ForegroundColor = Rogue.RAM.CUIColor;
                DrawHeader("Повышение уровня:");

                Console.ForegroundColor = ConsoleColor.Cyan;
                //Class                
                hlu(Rogue.RAM.Player.GetClassRace(2), 4);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                //Level
                hlu("Уровень: +1", 6);
                Console.ForegroundColor = ConsoleColor.Yellow;
                //Gold
                hlu("Золото: +" + gold.ToString(), 8);
                Console.ForegroundColor = ConsoleColor.Red;
                //Health points
                hlu("Жизнь: +" + hp.ToString(), 10);
                Console.ForegroundColor = ConsoleColor.Blue;
                //Mana
                hlu(Rogue.RAM.Player.ManaName + ": +" + mp.ToString(), 11);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                //Min dmg
                hlu("DMG↓: +" + ldmg.ToString(), 13);
                //Max dmg
                hlu("DMG↑: +" + mdmg.ToString(), 14);
                Console.ForegroundColor = ConsoleColor.Magenta;
                //Skills
                hlu("Очки навыков: +1", 16);
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                //Craft skills
                hlu("Очки крафта: +1 ", 17);

                InfoWindow.Custom("Нажмите любую клавишу для продолжения...");
                Console.ReadKey(true);
            }
            /// <summary>
            /// Help draw level up window.
            /// </summary>
            /// <param name="NameOfStat">Drawning stat name</param>
            /// <param name="Height">Drawning stat position</param>
            private static void hlu(string NameOfStat, int Height)
            {
                string ths = NameOfStat;
                int Count = 50 - (ths.Length / 2);
                Console.SetCursorPosition(Count + 1, Height);
                Console.Write(ths);
            }

            private static string GetSign(int z)
            {
                string s = string.Empty;
                if (z > 0)
                {
                    s = "+" + z.ToString();
                }
                else
                {
                    s = z.ToString();
                }
                return s;
            }
        }

        public static class SplashScreen
        {
            public static void StartGame()
            {
                Console.Clear();

                List<string> Scb = new List<string>();
                //+ - оранжевый, # - черные, * - желтые, % - красные
                Scb.Add("    =                                                                                               ");
                Scb.Add(" #########                                                                          ####*  *####=   ");
                Scb.Add(" *****######                                                                    ########* ######### ");
                Scb.Add("  ####***####* =###  ###= #######      ####     ####      ####   #######         ***####*####  ####*");
                Scb.Add("   ###*   ####*####* ###* ########* #########*######### #######* #########          ####*=###**###**");
                Scb.Add("   ####*  ####*####* ###* ###**###* *##* =##** ###*##** ###* ###*####*###*          ####*  #**###** ");
                Scb.Add("  ###***  ###**=###* ###* ###**###*  ##* ###** ####**   ###* ###*###**###*          ####*    ##** ##");
                Scb.Add("###########*** ########## ####*#### #########*########*#######** ####*####*        #####*  ########*");
                Scb.Add("   ****#***     *#****#**  ##** #*** *##**####* ***#**  **###**   *#** ##**       ###****##########*");
                Scb.Add("                                     #######**              *                      *                ");
                Scb.Add("                                    #**###**                                                        ");

                List<string> Scw = new List<string>();
                // *- желтая, + - оранжевый, остальное тоже наверно оранжевое
                Scw.Add("....-...............................................................................................");
                Scw.Add(".*+++++++*..........................................................................-*++#..-*+++-...");
                Scw.Add(".---:#++++++....................................................................++++++++*.*+#+++++-.");
                Scw.Add("..#+++#:-++++-.-++#..++#-.:+#-++:......*++:.....:++*......-++:...-++-#+*.........---#+++*+++#..++++*");
                Scw.Add("...+++#...++++--+++#.+++#.*+++#+++*.-+++#++++--+++#+++:.+++#+++-.:+++#+++#..........*+++*-+++#*#++#*");
                Scw.Add("...+++#-..:+++#-+++:.+++*.*++#-+++#.-++#.-++#-.++#-*+#*.+++*.+++-:++#:#++#..........#+++*..:#:*++#:.");
                Scw.Add("..+++#*:..+++##-++#-.+++*.*++#-+++#..++#.-++#-.++++:*...+++*.:++#:++#-+++#..........#+++*....++#*.:-");
                Scw.Add(":#+++++++++#*-.:+++++++++.++++-++++.:+++#*+++--++++++#-#++++++**.#+++:++++-........-++++*..++++++++#");
                Scw.Add("...-:*#+##:.....*+##:-+#*..###-.+##-.:#+#*:++#-.-*#+##..-:#+##-...*+#:.###-.......#++###*+++++++++#*");
                Scw.Add(".....................................+++++###:..............-......................-................");
                Scw.Add("....................................-*#+++#*........................................................");

                List<string> Scl = new List<string>();
                Scl.Add("    -                                                                                               ");
                Scl.Add(" *.......*                                                                          -*..#  -*...-   ");
                Scl.Add(" ---:#......                                                                    ........* *.#.....- ");
                Scl.Add("  #...#:-....- -..#  ..#- :.#-..:      *..:     :..*      -..:   -..-#.*         ---#...*...#  ....*");
                Scl.Add("   ...#   ....--...# ...# *...#...* -...#....--...#...: ...#...- :...#...#          *...*-...#*#..#*");
                Scl.Add("   ...#-  :...#-...: ...* *..#-...# -..# -..#- ..#-*.#* ...* ...-:..#:#..#          #...*  :#:*..#: ");
                Scl.Add("  ...#*:  ...##-..#- ...* *..#-...#  ..# -..#- ....:*   ...* :..#:..#-...#          #...*    ..#* :-");
                Scl.Add(":#.........#*- :......... ....-.... :...#*...--......#-#......** #...:....-        -....*  ........#");
                Scl.Add("   -:*#.##:     *.##:-.#*  ###- .##- :#.#*:..#- -*#.##  -:#.##-   *.#: ###-       #..###*.........#*");
                Scl.Add("                                     .....###:              -                      -                ");
                Scl.Add("                                    -*#...#*                                                        ");

                int c = 8;
                foreach (string s in Scb)
                {
                    c++;
                    var sar = s.ToCharArray();
                    Console.SetCursorPosition(0, c);
                    for (int i = 0; i < sar.Length; i++)
                    {
                        if (sar[i] == '+') { Console.ForegroundColor = ConsoleColor.DarkRed; }
                        if (sar[i] == '#') { Console.ForegroundColor = ConsoleColor.DarkRed; }
                        if (sar[i] == '*') { Console.ForegroundColor = ConsoleColor.DarkCyan; }
                        if (sar[i] == '@') { Console.ForegroundColor = Rogue.RAM.CUIColor; }
                        if (sar[i] == ':') { Console.ForegroundColor = ConsoleColor.DarkGray; }
                        if (sar[i] == '%') { Console.ForegroundColor = ConsoleColor.Red; }
                        Console.Write(sar[i]);
                    }
                }


                Thread.Sleep(1250);
                c = 8;
                foreach (string s in Scb)
                {
                    c++;
                    var sar = s.ToCharArray();
                    Console.SetCursorPosition(0, c);
                    for (int i = 0; i < sar.Length; i++)
                    {
                        if (sar[i] == '+') { Console.ForegroundColor = ConsoleColor.DarkRed; }
                        if (sar[i] == '#') { Console.ForegroundColor = ConsoleColor.DarkRed; }
                        if (sar[i] == '*') { Console.ForegroundColor = ConsoleColor.DarkCyan; }
                        if (sar[i] == '@') { Console.ForegroundColor = Rogue.RAM.CUIColor; }
                        if (sar[i] == ':') { Console.ForegroundColor = ConsoleColor.DarkGray; }
                        if (sar[i] == '%') { Console.ForegroundColor = ConsoleColor.Red; }
                        Console.Write(sar[i]);
                    }
                }
                Thread.Sleep(1250);
                Console.Clear();
                c = 8;
                foreach (string s in Scw)
                {
                    c++;
                    var sar = s.ToCharArray();
                    Console.SetCursorPosition(0, c);
                    for (int i = 0; i < sar.Length; i++)
                    {
                        if (sar[i] == '+') { Console.ForegroundColor = ConsoleColor.DarkRed; }
                        if (sar[i] == '#') { Console.ForegroundColor = ConsoleColor.DarkRed; }
                        if (sar[i] == '*') { Console.ForegroundColor = ConsoleColor.DarkCyan; }
                        if (sar[i] == '@') { Console.ForegroundColor = Rogue.RAM.CUIColor; }
                        if (sar[i] == ':') { Console.ForegroundColor = ConsoleColor.DarkGray; }
                        if (sar[i] == '.') { Console.ForegroundColor = ConsoleColor.Black; }
                        Console.Write(sar[i]);
                    }
                }


                Thread.Sleep(1250);
                Console.Clear();
                c = 8;
                foreach (string s in Scl)
                {
                    c++;
                    var sar = s.ToCharArray();
                    Console.SetCursorPosition(0, c);
                    for (int i = 0; i < sar.Length; i++)
                    {
                        if (sar[i] == '+') { Console.ForegroundColor = ConsoleColor.DarkRed; }
                        if (sar[i] == '#') { Console.ForegroundColor = ConsoleColor.DarkRed; }
                        if (sar[i] == '*') { Console.ForegroundColor = ConsoleColor.DarkCyan; }
                        if (sar[i] == '@') { Console.ForegroundColor = Rogue.RAM.CUIColor; }
                        if (sar[i] == ':') { Console.ForegroundColor = ConsoleColor.DarkGray; }
                        if (sar[i] == '.') { Console.ForegroundColor = ConsoleColor.Black; }
                        Console.Write(sar[i]);
                    }
                }

                Thread.Sleep(800);
                Console.Clear();
                Thread.Sleep(800);
            }

            public static void CompanyLogo()
            {
                // . - белая/серая
                // # - черная
                // * - зелёная
                // @ - темно-зелёная
                // : - темно-серая
                List<string> Logo = new List<string>();
                Logo.Add(" ###.        @#       +#####  ##       ######.=##.    ");
                Logo.Add("    -######=%####=    ####==..###=    .###=*.-###@=     ");
                Logo.Add("     #####*   *##*  %####=     ##=    ###=     ##=      ");
                Logo.Add("     ####@   *.##-  ####@*     =@*   *####      %=      ");
                Logo.Add("     #########=.#%.#####%  ######     ###########       ");
                Logo.Add("     #####@@##=    #####= ##==####=    *##########%     ");
                Logo.Add("     #####+  #%.   @#####-##= #####-  ## .==  #####*    ");
                Logo.Add("     #####=         #####-    #####=  ##=      ####-    ");
                Logo.Add("    ######@  %=      ####=   +####=  -###-    %###=     ");
                Logo.Add("  #############=      :#########==  .###########==      ");
                Logo.Add("  .=.       -             *++:-     ##=.   .--          ");
                Logo.Add("                                ##  %####=              ");
                Logo.Add("              .##.            :.#=    *%##@             ");
                Logo.Add("            .###=            .=####     +#=.            ");
                Logo.Add("     -###:#:=###%#%:##  @#-     *###*   ##      ###     ");
                Logo.Add("   ###=##%= =###=#:########==+##@ ###= ####= ###=@###+  ");
                Logo.Add("   #####=    ###-  :###.###= .###.###= :###. @##=-##%.  ");
                Logo.Add("    =#####= .###:   ###.###= .### ###= -###  %##%.##%   ");
                Logo.Add("   ##. ###= -###:   ##%.###= .##=.###= +###. @##=.##@   ");
                Logo.Add(" .#######== ######-#########=######===.####=.######===  ");
                Logo.Add(" *## =#=       ##-   +#= *#=    =#+      @=     %=.");
                //                Console.BackgroundColor = ConsoleColor.White;
                int c = 4;
                foreach (string s in Logo)
                {
                    c++;
                    var sar = s.ToCharArray();
                    Console.SetCursorPosition(20, c);
                    for (int i = 0; i < sar.Length; i++)
                    {
                        if (sar[i] == '.') { Console.ForegroundColor = ConsoleColor.Gray; }
                        if (sar[i] == '#') { Console.ForegroundColor = ConsoleColor.DarkMagenta; }
                        if (sar[i] == '*') { Console.ForegroundColor = ConsoleColor.Green; }
                        if (sar[i] == '@') { Console.ForegroundColor = Rogue.RAM.CUIColor; }
                        if (sar[i] == ':') { Console.ForegroundColor = ConsoleColor.DarkGray; }
                        if (sar[i] == '%') { Console.ForegroundColor = ConsoleColor.DarkGray; }
                        Console.Write(sar[i]);
                    }
                }

                Thread.Sleep(2000);
            }
            /// <summary>
            /// Environment.Exit(0);
            /// </summary>
            public static void EndOfGame()
            {
                Console.Clear();
                List<string> End = new List<string>();
                End.Add(" *%@%**%-  -@=*:::*%=-     -+%@@@%*-      -*%@@=:#*      -+%%%*- :%:::-:-::::=:  :=@@%:%+-@*:+==*-    ");
                End.Add("##@+-+#%-   -*#####=*-   :##%=###==##-   =##%+-:#@*    -@##=:-##@--@###%=:%##=**##@=:-@#+--###%*###-  ");
                End.Add("##+  -%=-    *#=+##%-   -###*-###* ###: +###* -+*%:    =##@:  =##@--###=  =#=-:###=--+:=*  ###= @##%  ");
                End.Add("##+####@- -=###*-###@*  +###=-###*:###=-########*     :###%-  *###* *###:-#@* ########+   -####=##%*  ");
                End.Add("##+@%@##@-:%##%##@##@+= -###%:###+*###+ =###*-:%%*     @###:  =###*  @##=+#=- *###+--=%+   ###+:###:  ");
                End.Add("##+ -###%- =##*-**###+ ::=##=*###=*##@: -###+  :##:    -###* -###=-  +####@:   %##@- -##+ :###%-@###  ");
                End.Add("*#####@=:######+#######=###@######@+##%-  *#####@#*      +##@##%+- :########-   :@####@#=#######%##@  ");
                End.Add("   ---   -:-  ----    -- -- --    -          --- =#%-       ---     --   ---        -- *##*   ---##@  ");
                int c = 9;
                foreach (string s in End)
                {
                    c++;
                    var sar = s.ToCharArray();
                    Console.SetCursorPosition(0, c);
                    for (int i = 0; i < sar.Length; i++)
                    {
                        if (sar[i] == '.') { Console.ForegroundColor = ConsoleColor.DarkGray; }
                        if (sar[i] == '#') { Console.ForegroundColor = ConsoleColor.DarkRed; }
                        if (sar[i] == '*') { Console.ForegroundColor = Rogue.RAM.CUIColor; }
                        if (sar[i] == '@') { Console.ForegroundColor = Rogue.RAM.CUIColor; }
                        if (sar[i] == ':') { Console.ForegroundColor = Rogue.RAM.CUIColor; }
                        if (sar[i] == '%') { Console.ForegroundColor = Rogue.RAM.CUIColor; }
                        Console.Write(sar[i]);
                    }
                }
                SoundEngine.Sound.GameOver();
                Thread.Sleep(3000);
                Environment.Exit(0);
            }

            public static void MainScreen()
            {
                List<string> sb = new List<string>();

                //sb.Add("    %%%=                                                                                             @=++++");
                //sb.Add("    %%%%                                                                                             @==+++");
                //sb.Add("   %=====                                                                             +++*          @%=====");
                //sb.Add("  @++*+=+%                                                                          @+%%%=%      @%%%==%%==");
                //sb.Add(" %***=%%%+%                                    =                                   %@ +==%=    %==========+");
                sb.Add("**+=%%@%*@                             ==   ++                    @%%===%%@         ==      @+======");
                sb.Add("*+=%%%                            ==@@==@   ***                 @%==+++*++++===%    %=      +*++++++");
                sb.Add("+===+=                             =++=    @****                =*++********+==@    %%%     *******:");
                sb.Add("+++=*:%                             %=@     *+***             %=+******::**=       %=+=    %:::**+*-");
                sb.Add("     ::*                               =%   =++*:           @*+=+********@        @=%=*+   +--:*+*-:");
                sb.Add("      +::=                         ==***@    ++%*-+%     %=:**++*:::::*%          +==%+**@ :-:*+-:*:");
                sb.Add("%@@    ::**           %=%         %:*:*@      =*=*-::::*:::::::::::::+           %++*==+***.-*:--::*");
                sb.Add("%%@@@@@::**:%=+===   %====@     =*+%:-:*=@      =*=@%=======+*::::::*@         =*++*:=%=++-.-.-:*+=%");
                sb.Add("@%@@%%=-:*:-:+++++%  @=+*::::*+*-*+++===*:*=%%    +=@   @===+*::-::::%@@    @+++==:-@    =..-::*%=%%");
                sb.Add("     +-:**:--*+**:+   ++**++*:--:*+%@@@%%=::%%@   %=@   @*++++*-----:=@@@   @%+*+:-+     %.-+ +=@@@ ");
                sb.Add("   =:-:::*:-.*++*:-%  ++**:-.-*+*:*+*===++=:-:%    %    %::*+*::---:=   %:@ +:**-:-+      *-+@=@@@  ");
                sb.Add("       @*::--:+*+==+:::-----:++=%%==+*=+++++*-.-*@      ::**+++**..:% @=:::=::*:--::*        =%%@ @%");

                List<List<ColouredChar>> list = new List<List<ColouredChar>>();

                int c = 20;
                foreach (string s in sb)
                {
                    List<ColouredChar> q = new List<ColouredChar>();
                    c++;
                    var sar = s.ToCharArray();
                    ConsoleColor col= ConsoleColor.Black;
                    for (int i = 0; i < sar.Length; i++)
                    {
                        if (sar[i] == '"') { col = ConsoleColor.DarkGray; }
                        if (sar[i] == '=') { col = ConsoleColor.DarkRed; }
                        if (sar[i] == '*') { col = Rogue.RAM.CUIColor; }
                        if (sar[i] == '@') { col = ConsoleColor.Yellow; }
                        if (sar[i] == ':') { col = ConsoleColor.Red; }
                        if (sar[i] == '*') { col = ConsoleColor.Red; }
                        q.Add(new ColouredChar() { BackColor = 0, Char = sar[i], Color = Convert.ToInt16(col) });
                    }
                    list.Add(q);                    
                }
                winAPIDraw.DRAW_MAIN_VOID(list, 0, 20, 100, 32);
            }

            public static void MainScreen2()
            {
                List<string> sb = new List<string>();
                sb.Add("          '''                                                                           ''''         ");
                sb.Add("         ' :                                                                             : '         ");
                sb.Add("         ':^$'                                                                         '$^:'         ");
                sb.Add("           ^^$:                                                                       :$$^           ");
                sb.Add("         '$? $?                                                                       ^$ ?$'         ");
                sb.Add("       ':$?^^?%$                                                                     $??$^?$:'       ");
                sb.Add("        ^%+++/+^:                                                                   :^+/+%+%^        ");
                sb.Add("       '%++++/:?:$'                                                               ':$?:/+++++        ");
                sb.Add("   ' : :^%+//:::?^ $                                                             $ ^?::://+%?: : '   ");
                sb.Add("   : ''':%%%:++%+^^::                                                           ::^^+%++:%%%:''' :   ");
                sb.Add("   '$$$$$?%?/%?+%$                                                                 $?+?%/???$$$$$'   ");
                sb.Add("   :^:^???^:^?^++^'                                                   :''         '^%+^?^$^???^:^:   ");
                sb.Add("  ^^^%%%%$ '$^?+/?:'                 '                       ' ''    :^:'        ' ?/+?^$  $%%%%^^^  ");
                sb.Add("^?+//+//^$ ':+///%$'               ::^: ^$                :::$$     ' ?:         '$%/+/+$' $^//+//+?^");
                sb.Add("%+//////%$$$?+:///?:'  '$          ': $:'$                $^?^        :$'  '''  ' ?///:+?$$$%///////%");
                sb.Add("%%+/*:/++$$?+//:/%+^$''?$: ''        :$$?:                $%%?$     ' %?$''''?'':^+%/://+?$$++/:*/+%%");
                sb.Add("?%%+//:+:/^%:::**/++?%?:$ ::^$ ''''   $/%$                $/+%:' '  : $: ':$':?%?++/:*:::%^+:+///+%%?");
                sb.Add("^?+/%://::+:/%:**:%%%%+'  :^?^$: $?'   ^:/                /*/+$^$''' '^%:$^?  +%%+%/**:+/:+:://:%/+%^");
                sb.Add(":$//::::/::::/***:+///%%?^%%?$^  :?    $/*                **:/^+^'''''$::^?$'%%+//+:***+:::://:::+:$:");
                sb.Add("^%/://*::*..****::++/*+%%???^$%$'' ?$^%++/                /*:/?%$:$ :'$^:$?$'%+:/%/:*****..*::*//:/%^");
                sb.Add("++%%/:**:****::/:::/::+??:'''^?*+^$^$^%/+%                /://^%$?? ^^^??^^^:?+::/:::/::****:**:/%%++");
                sb.Add("/^??/++//*****+?%+//++?:  :''^$?++^$:?^??%                %?$^^^+?^++%^$$:$:':?/+//+%?+**:**//++/??^/");

                List<List<ColouredChar>> list = new List<List<ColouredChar>>();

                int c = 0;
                foreach (string s in sb)
                {
                    List<ColouredChar> q = new List<ColouredChar>();
                    c++;
                    var sar = s.ToCharArray();
                    ConsoleColor col = ConsoleColor.Black;
                    for (int i = 0; i < sar.Length; i++)
                    {
                        if (sar[i] == '\'') { col = ConsoleColor.DarkGray; }
                        if (sar[i] == '=') { col = ConsoleColor.DarkRed; }
                        if (sar[i] == '*') { col = Rogue.RAM.CUIColor; }
                        if (sar[i] == '/' || sar[i] == '+') { col = ConsoleColor.Yellow; }
                        if (sar[i] == ':') { col = ConsoleColor.DarkRed; }
                        if (sar[i] == '*') { col = ConsoleColor.Red; }
                        if (sar[i] == '?' || sar[i] == '$' || sar[i] == '%') { col = ConsoleColor.Red; }
                        q.Add(new ColouredChar() { BackColor = 0, Char = sar[i], Color = Convert.ToInt16(col) });
                    }
                    list.Add(q);
                }
                winAPIDraw.DRAW_MAIN_VOID(list, 0, 0, 100, 24);
            }

            public static void Bug()
            {
                try
                {
                    SystemEngine.Helper.File.DeleteFile = "BugSave"; DrawEngine.ConsoleDraw.WriteTitle("Игра сохранена в файл: Save\\BugSave.d12ml"); Thread.Sleep(2000);
                }
                catch { }
                Console.Clear();
                List<string> Bug = new List<string>();

                Bug.Add("        #######=       =#%=         =##=        #######=      ");
                Bug.Add("              ##=    ######        =######     ##=            ");
                Bug.Add("              =##  #######          =######%  =#%             ");
                Bug.Add("               #%=####%                =%#### ##=             ");
                Bug.Add("                                             =#=              ");
                Bug.Add("                      Вот это баааааг! :оО                    ");
                Bug.Add("               #=                            =%%              ");
                Bug.Add("             =#####%                     %########=           ");
                Bug.Add("          #==#############%==== ===%##############=#%         ");
                Bug.Add("        =#%=###################%###################=##=       ");
                Bug.Add("        ## ##%%%%%%########%%%%%%%%%#######%%%%%%###=##=      ");
                Bug.Add("       =#=%#%%%%%%%######%%%%%%%%%%%%%#####%%%%%%%%#=##=      ");
                Bug.Add("       ## ###%%%%%#######%%%%%%%%%%%%%%######%%%%%###=##      ");
                Bug.Add(" #######  ###############%%%%%%%%%%%%%############### =#######");
                Bug.Add("         =#################%%%%%%%%%%################         ");
                Bug.Add("         =####################%%%####################         ");
                Bug.Add("         =####################%  ###################%         ");
                Bug.Add("         %%###################=  ################### %        ");
                Bug.Add("        %#=#####%%%%%%%%######% =######%%%%%%%%####=%#        ");
                Bug.Add("        =## ###%%%%%%%%%%#####=  %####%%%%%%%%%###% ##        ");
                Bug.Add("         %## ###%%%%%%%%%%####   =###%%%%%%%%%%##% ##=        ");
                Bug.Add("        #%###=%##%%%%%%%%#####   =###%%%%%%%%###= ##=         ");
                Bug.Add("          =##% %#############%    ############%  %#=          ");
                Bug.Add("          =##     ###########=    =#########%    =##=         ");
                Bug.Add("        ###=        =########     =#######=        %###       ");
                Bug.Add("                        =%##%      ##%                        ");

                int c = 1;
                foreach (string s in Bug)
                {
                    c++;
                    var sar = s.ToCharArray();
                    Console.SetCursorPosition(20, c);
                    for (int i = 0; i < sar.Length; i++)
                    {
                        if (sar[i] == '.') { Console.ForegroundColor = ConsoleColor.Gray; }
                        if (sar[i] == '#') { Console.ForegroundColor = ConsoleColor.DarkGreen; }
                        if (sar[i] == '*') { Console.ForegroundColor = ConsoleColor.Green; }
                        if (sar[i] == '@') { Console.ForegroundColor = Rogue.RAM.CUIColor; }
                        if (sar[i] == '=') { Console.ForegroundColor = ConsoleColor.DarkGray; }
                        if (sar[i] == '%') { Console.ForegroundColor = ConsoleColor.DarkYellow; }
                        Console.Write(sar[i]);
                    }
                }

                Thread.Sleep(5000);
                Environment.Exit(0);
            }

            public static bool DrawKeyMap
            {
                set
                {
                    if (value)
                    {
                        int height = 0;

                        Console.ForegroundColor = Rogue.RAM.CUIColor;

                        string Clear = string.Empty;
                        Clear = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 3);
                        Clear = " " + DrawHelp.Border(true, 1) + "══════════════════════" + DrawHelp.Border(true, 0, "ThreeBot") + "══════════════════════" + DrawHelp.Border(true, 0, "ThreeBot") + "══════════════════════" + DrawHelp.Border(true, 0, "ThreeBot") + "═══════════════════════════" + DrawHelp.Border(true, 2);// + Clear.Remove(Clear.Length - 24) + DrawHelp.Border(true, 2);
                        Console.SetCursorPosition(0, height);
                        Console.WriteLine(Clear);

                        //тело                
                        for (int i = 1; i < 28; i++)
                        {
                            Clear = DrawHelp.FullLine(100, " ", 3);
                            Clear = " " + DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 1) + DrawHelp.Border(true, 3);
                            Console.SetCursorPosition(0, i);
                            Console.WriteLine(Clear);
                        }

                        //носки 
                        Clear = string.Empty;
                        Clear = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 3);
                        Clear = " " + DrawHelp.Border(true, 5) + "══════════════════════" + DrawHelp.Border(true, 0, "ThreeTop") + "══════════════════════" + DrawHelp.Border(true, 0, "ThreeTop") + "══════════════════════" + DrawHelp.Border(true, 0, "ThreeTop") + "═══════════════════════════" + DrawHelp.Border(true, 6);
                        Console.SetCursorPosition(0, 28);
                        Console.WriteLine(Clear);

                        //носки объявления
                        for (int i = 1; i < 14; i++)
                        {
                            Clear = string.Empty;
                            Clear = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 3);
                            Clear = " " + DrawHelp.Border(true, 8) + "══════════════════════" + DrawHelp.Border(true, 0, "Cross") + "══════════════════════" + DrawHelp.Border(true, 0, "Cross") + "══════════════════════" + DrawHelp.Border(true, 0, "Cross") + "═══════════════════════════" + DrawHelp.Border(true, 7);
                            Console.SetCursorPosition(0, i * 2);
                            Console.WriteLine(Clear);
                        }

                        DrawColumn
                            (
                                1,
                                new List<string>() 
                                { 
                                    "[O] - Открыть дверь ", 
                                    "[O] - Торг,  квесты ", 
                                    "[M] - Глобальная карт ", 
                                    "[T] - Подобрать вещь ",
                                    "[A] - Атаковать врага ",
                                    "[D] - Уничтожить вещь ",
                                    "[С] - Информация перс ",
                                    "[Numpad 1-6] - Инвент ",
                                    "[Q] - Классовый нав.1 ",
                                    "[W] - Классовый нав.2 ",
                                    "[E] - Классовый нав.3 ",
                                    "[R] - Классовый нав.4 ",
                                    "[Shift+↑] - Общ нав. ",
                                },
                                "    Главный экран    "
                            );
                        DrawColumn
                            (
                                2,
                                new List<string>() 
                                { 
                                    "[A] - Атака оружием ", 
                                    "[D] - Защитная стойка ", 
                                    "[S] - Попытка сбежать ", 
                                    "[Q] - Классовый нав.1 ",
                                    "[W] - Классовый нав.2 ",
                                    "[E] - Классовый нав.3 ",
                                    "[R] - Классовый нав.4 ",
                                    "    Окно персонажа  & ",
                                    "[P] - Особенности п. ",
                                    "[I] - Экипировка пер ",
                                    "[S] - Способности п. ",
                                    "[Q] - Задания персон. ",
                                    "[G] - Эффекты персон. ",
                                },
                                "    Экран сражения    "
                            );
                        DrawColumn
                            (
                                3,
                                new List<string>() 
                                { 
                                    "[R-Arrow] - Листать → ", 
                                    "[L-Arrow] - Листать ← ", 
                                    "    Экран заданий &   ", 
                                    " ←     Листать      → ", 
                                    "    Экран навыков &   ", 
                                    "[N] - Инфо о навыке.   ",
                                    "[Enter] - Навык +1 ",
                                    "    Экран инвентаря & ",
                                    "[A+Numpad] - Надеть ♦ ",
                                    "[H+Numpad] - Надеть ▲",
                                    "[W+Numpad] - Надеть }",
                                    "[O+Numpad] - Надеть ]",
                                    "[B+Numpad] - Надеть ▼",
                                },
                                "  Экран особенностей "
                            );
                        DrawColumn
                            (
                                4,
                                new List<string>() 
                                { 
                                    //"      [E]&", 
                                    //"Сохран. в txt файл",
                                    "    [Escape]&",
                                    "Вернуться в меню" 
                                },
                                " Действия здесь: * "
                            );
                    }
                }
            }

            private static void DrawColumn(int ColumnNumber, List<string> Strings, string Title)
            {
                if (Strings.Count > 13) { Console.Clear(); DrawEngine.ConsoleDraw.WriteTitle("Кодер - мудак"); }
                int left = 0;
                if (ColumnNumber == 1) { left = 3; }
                else if (ColumnNumber == 2) { left = 26; }
                else if (ColumnNumber == 3) { left = 49; }
                else if (ColumnNumber == 4) { left = 49 + 28; }
                if (Rogue.RAM.CUIColor == ConsoleColor.DarkYellow) { Console.ForegroundColor = ConsoleColor.Green; }
                else if (Rogue.RAM.CUIColor == ConsoleColor.DarkGreen) { Console.ForegroundColor = ConsoleColor.Yellow; }
                if (Title.IndexOf('*') > -1) { Console.ForegroundColor = ConsoleColor.Cyan; }
                Console.SetCursorPosition(left, 1);
                Console.Write(Title.Replace('*', ' '));
                Console.ForegroundColor = Rogue.RAM.CUIColor;
                Console.SetCursorPosition(left + 21, 1);
                Console.Write("║");
                int i = 3;
                foreach (string s in Strings)
                {
                    if (s.IndexOf('&') > -1)
                    {
                        if (Rogue.RAM.CUIColor == ConsoleColor.DarkYellow) { Console.ForegroundColor = ConsoleColor.Green; }
                        else if (Rogue.RAM.CUIColor == ConsoleColor.DarkGreen) { Console.ForegroundColor = ConsoleColor.Yellow; }
                    }
                    else { Console.ForegroundColor = ConsoleColor.Gray; }
                    if (s.IndexOf('*') > -1) { Console.ForegroundColor = ConsoleColor.Cyan; }
                    Console.SetCursorPosition(left, i);
                    Console.Write(s.Replace('&', ' '));
                    Console.ForegroundColor = Rogue.RAM.CUIColor;
                    Console.SetCursorPosition(left + 21, i);
                    Console.Write("║");
                    i += 2;

                }
            }

            private static bool DrawBorder
            {
                set
                {
                    int height = 0;

                    //mid

                    Console.ForegroundColor = Rogue.RAM.CUIColor;

                    string Clear = string.Empty;
                    Clear = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 2);
                    Clear = DrawHelp.Border(true, 1) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 2);
                    Console.SetCursorPosition(1, height);
                    Console.WriteLine(Clear);

                    //тело                
                    for (int i = 1; i < 28; i++)
                    {
                        Clear = DrawHelp.FullLine(100, " ", 2);
                        Clear = DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 3);
                        Console.SetCursorPosition(1, i);
                        Console.WriteLine(Clear);
                    }

                    //носки 
                    Clear = string.Empty;
                    Clear = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 2);
                    Clear = DrawHelp.Border(true, 5) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 6);
                    Console.SetCursorPosition(1, 28);
                    Console.WriteLine(Clear);
                }
            }

            private static void DrawText(string Title)
            {
                int leftOffSet = (Console.WindowWidth / 2);
                int topOffSet = (Console.WindowHeight / 2);
                string[] str = Title.Split(new Char[] { '\n' });
                int i = 2;
                Console.ResetColor();
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Clear();
                DrawBorder = true;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                foreach (string strr in str)
                {
                    int q = 3;
                    foreach (char c in strr)
                    {
                        try { Console.SetCursorPosition(q, i); }
                        catch (ArgumentOutOfRangeException) { Console.SetCursorPosition(0, i); }
                        Console.Write(c);
                        q++;
                    }
                    i = i + 1;
                }
            }

            public static void History_part1()
            {
                DrawBorder = true;
                DrawText(@"Привет, ты - будующий герой планеты Аурегон.

Как ты попал сюда? Кто ты? Что ждёт тебя впереди?

Меня зовут Контор, я здесь уже очень давно, и я помогу тебе разобраться.
Понимаешь в чем дело, наш мир очень неусточив:
каждые 200 лет верхний мир испаряется, и из Бездны к нам приходит новый,
неизвестный до этого момента мир.

Для того что бы поддерживать другие миры в спокойствии, они и создают Вас.
Вы - последние спасённые души из умирающего верхнего мира. Здесь, в Санктуарии,
Вам вибирают новую внешность и дают тело. Взамен Вы должны выполнить то, что
они попросят от Вас.

Так было до недавнего момента. Теперь всё изменилось. Новый мир родил ужасное зло.

Вся эта космология, планеты, миры, планы. По сути всё это система описывающая хаос который
творится здесь уже миллионы лет. Но эта система работает, маги перемещаются между
планами и планетами, и все живут хорошо. Но что будет если дать этому описанию не верное
трактование? Что может получиться если могущественному магу рассказать идею об уничтожении
всего этого хаоса?

Я знаю что будет. Я видел это. Теперь все миры кроме Санктуария и Хирона уничтожены.


Нажмите клавишу для продолжения...");
                Console.ReadKey(true);
                DrawText(@"Давай я раскажу тебе как это было, пока ты способен слушать.

То время для меня было не плохим, как обычно умирал старый мир, души стали приходить сюда,
просить о том что бы их вернули, а заодно общались со мной. Всё было как обычно, даже 
довольно весело.

Но потом пришёл он. Он ворвался в Санктуарий как буд-то это может сделать каждый.
Как буд-то это обычный мир в который можно попасть используя простейшую магию.

И дальше началась резня.

Он убил абсолютно всех хранителей, истребил все способные к перерождению души...
Меня-то это не коснулось, ведь я не существую впринципе. Но остальных было жалко.

Знаешь, со мной кроме душ никто не общался. Это устраивало хранителей, а меня
всё время раздражало. Но в тот момент, верховный жрец всё-таки обратил внимание на меня.
Он умолял меня спрятать тебя. 
Это был довольно быстрый разговор, но я буду помнить этот голос вечно.
Я спрятал тебя в самый сокровенный угол Санктуария, в своё сердце. Тебе видимо
немного неудобно здесь, но что поделать, большими размерами я не отличаюсь.

Позже, спустя некоторое время, я узнал что Санктуарий был последним миром который пал
перед этим ужасным существом. И только тогда я понял зачем нужно было прятать такую
слабую душу вроде тебя.

Нажмите клавишу для продолжения...");
                Console.ReadKey(true);
                DrawText(@"Пойми, ты - это его вторая половина. А только вторая половина знает как убить первую.
Тебе предначертано стать героем целого мира, такая ответственность, представляешь?

Давай я расскажу тебе что надо сделать.
У меня не получается так же хорошо рассказывать о судьбе как это выходило у хранителей, но
всё же кроме меня тебе никто этого не расскажет.

Я сохранил твою душу для того что бы ты сразился с ним. В данный момент он находится на
огромной планете Аурегон - твой путь лежит прямо туда. Тебе надо будет найти его, и 
уничтожить любой ценой. Для того что бы ты мог его найти, я наделю тебя способностью
видеть магический путь, используй магический путь для того что бы перемещаться по планете
с такой же скоростью как и он, но будь осторожен, любое пространство через которое он проходит
изменено навсегда. Никто не знает что он может оставить после себя, так что будь осторожнее.

По поводу твоего внешнего вида - у меня тут осталась парочка тел, так что можешь 
выбрать кем ты будешь в своей новой жизни.

Сейчас я телепортирую тебя на планету, помни, тебе нужно любой ценой найти его. 
И только после того как ты найдёшь его, я смогу восстановить все миры до 
состояния какими они были прежде, чем он родился.

Обещаю, что после того как ты справишься с ним, я лично спущусь к тебе, и расскажу всю
историю от начала и до конца.

А теперь лети. У тебя не так много времени.
Нажмите клавишу для продолжения...");
                Console.ReadKey(true);
            }

            public static void AngelEnd()
            {
                Console.Clear();
                List<string> sb = new List<string>();
                sb.Add("                           Ваш персональный счет - " + SystemEngine.score.ToString());
                sb.Add("                                                                                ");
                sb.Add("                                                                                ");
                sb.Add("            +?+++??+??+                                 :++?+?+???+             ");
                sb.Add("        ~=+ +?+??+?+?=++?+                           =+=====+++++?I I?+         ");
                sb.Add("     :~  ~=++? ?+???+???+??                         +======+=++? I+I?=  ?+      ");
                sb.Add("       := ~+?+I I ?+?+?+=?+?                       ===~=+=+== I I7II+ ?+        ");
                sb.Add("      =  =+~?I?I??77I=?????++                     +~++===++???III7I+I?= ++      ");
                sb.Add("       :~ =+:?=?IIIII?I++?+?+?:                 :+=======????~I?=I +I ++   =    ");
                sb.Add("         + +    ??+?7IIII??+?=??   ::++=:=~    ?=:===++????????    7 I          ");
                sb.Add("                 =????III?+????=??+?+=I ?:==+++~==+===+?+~+???                  ");
                sb.Add("                  + =III?I???=??????:+I??+:==++=+~+=== ?=?: ?                   ");
                sb.Add("                    ? I?7II??????+?++=I +I+++=+==+==+?? + +                     ");
                sb.Add("                     + IIIII+??+??+++=   ~+==++====???+I ~                      ");
                sb.Add("                       :I+I77III??+?       +==++=+??+~? +                       ");
                sb.Add("                       ? ? ?I+?IIIII        ?+?+? +++? ?                        ");
                sb.Add("                          ? +II~~I?         ++ =?+~ +                           ");
                sb.Add("                                ~             :                                 ");

                int c = 8;
                foreach (string s in sb)
                {
                    c++;
                    var sar = s.ToCharArray();
                    Console.SetCursorPosition(10, c);
                    for (int i = 0; i < sar.Length; i++)
                    {
                        if (sar[i] == '?') { Console.ForegroundColor = ConsoleColor.DarkGray; }
                        if (sar[i] == '+') { Console.ForegroundColor = ConsoleColor.White; }
                        if (sar[i] == 'I') { Console.ForegroundColor = ConsoleColor.Gray; }
                        Console.Write(sar[i]);
                    }
                }
                Console.ReadKey(true);
                Rogue.Main(null);
            }

            public static void DeamonEnd()
            {
                Console.Clear();
                List<string> sb = new List<string>();

                sb.Add("                           Ваш персональный счет - " + SystemEngine.score.ToString());
                sb.Add("                                                                                ");
                sb.Add("     :                                                                  :       ");
                sb.Add("      I.                                                              .         ");
                sb.Add("      ., ..                                                        .. I~        ");
                sb.Add("      ..:.,...?                                                ,.....+..        ");
                sb.Add("        .........                                            .........          ");
                sb.Add("       ............                                        ......... ..         ");
                sb.Add("       ..............I                                  =..............         ");
                sb.Add("        ...............+                              :.............?           ");
                sb.Add("        .................+                          ,.................          ");
                sb.Add("           ................I                      I.................            ");
                sb.Add("            .................                    .................?             ");
                sb.Add("          ....................~                ....................             ");
                sb.Add("             ..................I              ~.................                ");
                sb.Add("               I................      ..      ...............                   ");
                sb.Add("             ~.. I..............+    ....    ..................,=               ");
                sb.Add("                  . .............?   ...+   :...........I..                     ");
                sb.Add("                   ................  ~..   ...............                      ");
                sb.Add("                     . ....?.........:..................  .                     ");
                sb.Add("                    . . ... :~....,...,.........    =?=                         ");
                sb.Add("                               .......  ?..,.                                   ");
                sb.Add("                                 ~:.,.:~...:                                    ");
                sb.Add("                                I=::....~+?.                                    ");
                sb.Add("                                ~? ~,..:,= ..                                   ");
                sb.Add("                               ~=  +,...,  ...                                  ");
                sb.Add("                              =+   =,,..,  ...                                  ");
                sb.Add("                             +     :,....~  ,.                                  ");
                sb.Add("                            ??    =~~,,..   ?.                                  ");

                int c = 2;
                foreach (string s in sb)
                {
                    c++;
                    var sar = s.ToCharArray();
                    Console.SetCursorPosition(10, c);
                    for (int i = 0; i < sar.Length; i++)
                    {
                        if (sar[i] == '.') { Console.ForegroundColor = ConsoleColor.DarkRed; }
                        if (sar[i] == ':') { Console.ForegroundColor = ConsoleColor.Yellow; }
                        if (sar[i] == 'I' || sar[i] == '+' || sar[i] == '?' || sar[i] == '^' || sar[i] == '~' || sar[i] == '=') { Console.ForegroundColor = ConsoleColor.Red; }
                        Console.Write(sar[i]);
                    }
                }
                Console.ReadKey(true);
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Black;
                DrawEngine.GUIDraw.DrawGUI();
                DrawEngine.GUIDraw.drawstat();
            }
        }

        public static class WorldMapDraw
        {
            public static bool DrawWorldMap
            {
                set
                {
                    #region Map window

                    int height = 0;

                    Console.ForegroundColor = Rogue.RAM.CUIColor;

                    string Clear = string.Empty;
                    Clear = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 27);
                    Clear = DrawHelp.Border(true, 1) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 2);
                    Console.SetCursorPosition(1, height);
                    Console.WriteLine(Clear);

                    //тело                
                    for (int i = 1; i < 24; i++)
                    {
                        Clear = DrawHelp.FullLine(100, " ", 2);
                        Clear = DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 27) + DrawHelp.Border(true, 3);
                        Console.SetCursorPosition(1, i);
                        Console.WriteLine(Clear);
                    }

                    //носки 
                    Clear = string.Empty;
                    Clear = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 27);
                    Clear = DrawHelp.Border(true, 5) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 6);
                    Console.SetCursorPosition(1, 24);
                    Console.WriteLine(Clear);

                    //header 
                    Clear = string.Empty;
                    Clear = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 27);
                    Clear = "╠" + Clear.Remove(Clear.Length - 2) + "╣";
                    Console.SetCursorPosition(1, 2);
                    Console.WriteLine(Clear);
                    DrawHelp.WriteCenterPosition(2, 1, 75, "Карта мира");

                    #endregion
                    #region Bottom window

                    height = Console.WindowHeight - 5;
                    Console.ForegroundColor = Rogue.RAM.CUIColor;

                    Clear = string.Empty;
                    Clear = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 2);
                    Clear = DrawHelp.Border(true, 1) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 2);
                    Console.SetCursorPosition(1, height);
                    Console.WriteLine(Clear);

                    //тело                
                    Clear = DrawHelp.FullLine(100, " ", 2);
                    Clear = DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 3);
                    Console.SetCursorPosition(1, height + 1);
                    Console.WriteLine(Clear);

                    //носки 
                    Clear = string.Empty;
                    Clear = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 2);
                    Clear = DrawHelp.Border(true, 5) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 6);
                    Console.SetCursorPosition(1, height + 2);
                    Console.WriteLine(Clear);

                    #endregion
                    #region Character window

                    height = 24;
                    Console.ForegroundColor = Rogue.RAM.CUIColor;

                    Clear = string.Empty;
                    Clear = DrawHelp.FullLine(25, DrawHelp.Border(true, 4));
                    Clear = DrawHelp.Border(true, 1) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 2);
                    Console.SetCursorPosition(75, 0);
                    Console.WriteLine(Clear);

                    //тело
                    for (int i = 1; i < height; i++)
                    {
                        Clear = DrawHelp.FullLine(25, " ");
                        Clear = DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 3);
                        Console.SetCursorPosition(75, i);
                        Console.WriteLine(Clear);
                    }

                    //носки 
                    Clear = string.Empty;
                    Clear = DrawHelp.FullLine(25, DrawHelp.Border(true, 4));
                    Clear = DrawHelp.Border(true, 5) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 6);
                    Console.SetCursorPosition(75, height);
                    Console.WriteLine(Clear);

                    #endregion
                    InfoWindow.Location(Rogue.RAM.Map.Name);
                    
                    Console.SetCursorPosition(0, 3);
                    switch (Rogue.RAM.Map.Biom)
                    {
                        case ConsoleColor.DarkCyan: { winAPIDraw.DrawRegion(WorldTemplates.Mount); break; }
                        case ConsoleColor.DarkGreen: { winAPIDraw.DrawRegion(WorldTemplates.Three); break; }
                        case ConsoleColor.DarkYellow: { winAPIDraw.DrawRegion(WorldTemplates.Demon); break; }
                        case ConsoleColor.Gray: { winAPIDraw.DrawRegion(WorldTemplates.Dwarf); break; }
                        case ConsoleColor.DarkMagenta: { winAPIDraw.DrawRegion(WorldTemplates.Drow); break; }
                        case ConsoleColor.DarkGray: { winAPIDraw.DrawRegion(WorldTemplates.Dead); break; }
                    }

                    #region Character window

                    height = 24;
                    Console.ForegroundColor = Rogue.RAM.CUIColor;

                    Clear = string.Empty;
                    Clear = DrawHelp.FullLine(25, DrawHelp.Border(true, 4));
                    Clear = DrawHelp.Border(true, 1) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 2);
                    Console.SetCursorPosition(75, 0);
                    Console.WriteLine(Clear);

                    //тело
                    for (int i = 1; i < height; i++)
                    {
                        Clear = DrawHelp.FullLine(25, " ");
                        Clear = DrawHelp.Border(true, 3) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 3);
                        Console.SetCursorPosition(75, i);
                        Console.WriteLine(Clear);
                    }

                    //носки 
                    Clear = string.Empty;
                    Clear = DrawHelp.FullLine(25, DrawHelp.Border(true, 4));
                    Clear = DrawHelp.Border(true, 5) + Clear.Remove(Clear.Length - 2) + DrawHelp.Border(true, 6);
                    Console.SetCursorPosition(75, height);
                    Console.WriteLine(Clear);

                    #endregion

                    winAPIDraw.DrawRightWindow.Clear();

                    //name
                    int Count = (23 / 2) - ((Rogue.RAM.Map._Name.Length) / 2);
                    string WriteThis = Rogue.RAM.Map._Name;
                    winAPIDraw.DrawRightWindow.AddLine(Count, 1, WriteThis, Rogue.RAM.Map.Biom);

                    //name
                    Count = (23 / 2) - ((Rogue.RAM.Map._Affics.Length) / 2);
                    WriteThis = Rogue.RAM.Map._Affics;
                    winAPIDraw.DrawRightWindow.AddLine(Count, 2, WriteThis, Rogue.RAM.Map.Biom);

                    //objects
                    Count = (23 / 2) - (("Объекты:".Length) / 2);
                    WriteThis = "Объекты:";
                    winAPIDraw.DrawRightWindow.AddLine(Count, 4, WriteThis, Rogue.RAM.Map.Biom);

                    //door               
                    Count = (23 / 2) - (("▒  -  Обычная дверь".Length) / 2);
                    WriteThis = "`" + ConsoleColor.Gray.ToString() + "`▒ - Обычная дверь";
                    winAPIDraw.DrawRightWindow.AddLine(Count, 6, WriteThis, Rogue.RAM.Map.Biom);

                    //magic door
                    Count = (23 / 2) - (("? - Волшебная дверь".Length) / 2);
                    WriteThis = "`" + ConsoleColor.DarkMagenta.ToString() + "`░ - Волшебная дверь";
                    winAPIDraw.DrawRightWindow.AddLine(Count, 7, WriteThis, Rogue.RAM.Map.Biom);

                    //wall               
                    Count = (23 / 2) - (("#   -   Препятствие".Length) / 2);
                    WriteThis = "# - Препятствие";
                    winAPIDraw.DrawRightWindow.AddLine(Count, 8, WriteThis, Rogue.RAM.Map.Biom);

                    //merch              
                    Count = (23 / 2) - (("$     -    Торговец".Length) / 2);
                    WriteThis = "`" + ConsoleColor.Yellow.ToString() + "`$ - Торговец";
                    winAPIDraw.DrawRightWindow.AddLine(Count, 9, WriteThis, Rogue.RAM.Map.Biom);

                    //quest              
                    //Count = (23 / 2) - (("!     -     Задание".Length) / 2);
                    //WriteThis = "`" + ConsoleColor.Red.ToString() + "`! - Задание";
                    //winAPIDraw.DrawLeftWindow.AddLine(Count, 10, WriteThis, Rogue.RAM.Map.Biom);

                    //objects
                    Count = (23 / 2) - (("Жители:".Length) / 2);
                    WriteThis = "Жители:";
                    winAPIDraw.DrawRightWindow.AddLine(Count, 12, WriteThis, Rogue.RAM.Map.Biom);

                    int q = 14;

                    foreach (SystemEngine.Helper.Information.Mob m in SystemEngine.Helper.Information.MobsHere())
                    {
                        Count = (23 / 2) - (("? - " + m.Name).Length / 2);
                        WriteThis = "`" + m.Color.ToString() + "`" + m.Icon + " - " + m.Name;
                        winAPIDraw.DrawRightWindow.AddLine(Count, q, WriteThis, Rogue.RAM.Map.Biom);
                        q++;
                    }

                    winAPIDraw.DrawRightWindow.Draw = true;

                    DrawEngine.CharMap.DrawCMap(new List<string>() 
                                { 
                                    "[O] - Действие",
                                    "[M] - Карта ", 
                                    "[T] - Взять вещь ",
                                    "[A] - Атаковать ",
                                    "[С] - Персонаж ",
                                    "[I] - Идентификация ",
                                    "[1-6] - Инвентарь ",
                                    "[Q,W,E,R],+[Shift] - Навыки",
                                });
                }
            }

            public static class WorldTemplates
            {
                public static string Mount
                {
                    get
                    {
                        string s =

@"
                +*  *#######@#= ##:                              %##* 
               @#####: *#=   +#= #@                              ##   
     2       #@:#= %#:   * ###:@# +#*                           +#@   
           ##:##   ##  ####:*:%#  #=                      :=###@:     
          +#*  %#: :####  *##* ##:##       3          =###@*         %
          ##     :###+ @#*      #@*#%               =#@  *+:       =##
        +#%    +##: ##*         :#@ %##%####+      +#* %#%:    +%%##  
      :##:##: @#:    :###:        ##   *:   %#%    =#::#*   :##=      
  +#####   *%*+       #%%#*       *#% ####@   ##   @#   *####*        
 ##  *+                            +#:@#  %##= =####=   @#          :=
                                   :#= ##@@@%#=   ::  *=#%         ##*
                              :%    =#*     :  +#####@+*          #@  
                    4        =#@#+   =#%  =###%+                 :=   
                   %###      #% =#*    +%%+    %+  %%:       %###:    
                            *#=  %#+###%: =##@####@+%#*      #@ =#+###
    =##:       5            @#   :#@:  +@#@  %#*   * :@%###@#@*####@::
     6        ::            *: :####   %#%  =%:  :####*@*    +#= =###@
                               *#* @###+ @##++#%##*  +########@*#%:  @
   *=###        =#     @####@@###  +#:    +#%  *+=*  =:  =#+  *##%  +%
 %@=** =##%  *##@##:  +#+ *%@#%*#% :%###:    *#%:*###@###%       ###%@
".Replace('@', '&');
                        if (Rogue.RAM.Map.Name.IndexOf("Руины") > -1) { s = s.Replace('6', '@'); }
                        if (Rogue.RAM.Map.Name.IndexOf("Поселение") > -1) { s = s.Replace('5', '@'); }
                        if (Rogue.RAM.Map.Name.IndexOf("Катакомбы") > -1) { s = s.Replace('4', '@'); }
                        if (Rogue.RAM.Map.Name.IndexOf("Поле боя") > -1) { s = s.Replace('3', '@'); }
                        if (Rogue.RAM.Map.Name.IndexOf("Храм") > -1) { s = s.Replace('2', '@'); }
                        return s;
                    }
                }

                public static string Three
                {
                    get
                    {
                        string s =
@"
   -##:               -%#####+:*- :######=-          +##:            
   =#=             -%##=- :#@      +#=  -@##%-        -##-           
  %#+     6      +#########%-      +######*-@#%        :##-          
 +#%           :##:     -*=%@#######%*-      *##:       -##:         
 ##-          +##-    :##%*:--  5  -*@##%      %#=       +#=         
 ##          =#=     -##               +#%      %#*      +#+         
 ##         %#=     +@*##@:-+#####@::###*+##    -##-     :#%         
-##        +#+     %#%##@%###-   -*@@*=##@+#@    @#-      %#+        
-#@        +#+     ##- -*##-        =#%-   #@ *####:      -#@        
-#@        =#:     ##    ##-   4    +#+    #@+#= @#-      -#@        
-#@        =#####* ##    -##-      -##-   -##@#--##       -##-       
-##        +#%- *#%@#*    -##@-  -@#@-   -##-+####:        *#=       
 =#%        %#+-=#%-@#=     -##- %#*    -@#*  -*##         :#%       
  :##-      -###@-   *##:*++=#@  %#@@%-@#@-    =#=     3   @#:       
   -##-     -##%       +##****-   ---*##+    -@#+         +#%        
    :##- -=##@-:###-    ##      2    -##     @##-       *##*         
     -####%:%###*-@##:  ##            ##   *@:-%#@-   =##*           
   -###*-%##=-      +#####@@@@@@@@@@%%##%##@@##+-@#####*             
 :##%-:##%@#+          -+###+     -::###%-    :##=:##%-              
".Replace('@', '&');
                        if (Rogue.RAM.Map.Name.IndexOf("Руины") > -1) { s = s.Replace('6', '@'); }
                        if (Rogue.RAM.Map.Name.IndexOf("Поселение") > -1) { s = s.Replace('5', '@'); }
                        if (Rogue.RAM.Map.Name.IndexOf("Катакомбы") > -1) { s = s.Replace('4', '@'); }
                        if (Rogue.RAM.Map.Name.IndexOf("Поле боя") > -1) { s = s.Replace('3', '@'); }
                        if (Rogue.RAM.Map.Name.IndexOf("Храм") > -1) { s = s.Replace('2', '@'); }
                        return s;
                    }
                }

                public static string Demon
                {
                    get
                    {
                        string s =
@"
                                  ##################@                
           ###                  ###########*     %####@              
     +##########        ###########-               %#######-         
    #######: ###     :##########-                    :#######-       
   +###    -####     ####-                                ####       
   ####  6 ####     #####               4                  ###       
    ####+  ####    ####-                                 *####       
     :####  ###@   +####                               ######        
      #########     ####                              ####*          
       ######       -###%                            ####            
                    ####          3                  ####            
        +#####      ###                              ####            
     -##########:   ####                              ####           
    #####    ####    ####**                         2  ####          
   ####      -####*  :######%                     *+++* ####         
   ####        %#####    #######                ############         
   ####    5    @####    #######               ####  #####-         
   ####           ####        ####               ###*                
  -####       #######:        *###             #####*                
 -####      ######            ####           ######:                 
   #############               +##################                    
".Replace('@', '&');
                        if (Rogue.RAM.Map.Name.IndexOf("Руины") > -1) { s = s.Replace('6', '@'); }
                        if (Rogue.RAM.Map.Name.IndexOf("Поселение") > -1) { s = s.Replace('5', '@'); }
                        if (Rogue.RAM.Map.Name.IndexOf("Катакомбы") > -1) { s = s.Replace('4', '@'); }
                        if (Rogue.RAM.Map.Name.IndexOf("Поле боя") > -1) { s = s.Replace('3', '@'); }
                        if (Rogue.RAM.Map.Name.IndexOf("Храм") > -1) { s = s.Replace('2', '@'); }
                        return s;
                    }
                }

                public static string Dead
                {
                    get
                    {
                        string s =
@"
        #                               ##                           
         ####                       #### ##       #####              
             ##           #####  ########  #####  #    ###           
           ###       #####   ##          ##    #  ##    ##           
         ##         ##  ######     #####  #   ##  ##   ##            
       ##   #####   ##  ####     ##  ####  ###  ##     #####         
       #####     #   ### ##     ## ##  #####    #####     ##         
       ##   ##### ##########   ##  ##  ####         ##     ##        
        #####    ## ###        ## 4##  ## #   #  ### ##    ##        
       ####  ####  ###          ####  ##   #####  ##  ##   ##        
                  ##         ###  #  ## 2  ##  ####  ## ## #         
                  ####  ###### ##  ###   ##     ####  #### ####      
                   ##  ##    ##    #    ##  ####    ##   ##   ##     
                    ##  ##    #### ##### ## ######  ##     ###       
            5       ##  ##       ##     ##     ##   ### ## ###       
                    ##   #########  ###  ###   ##      # ##          
                     ##     6      ###      ########     ####        
          3      #####        ####     ########   ##        ##       
                ##     #### ##   #####  ##   #    #     ######       
                ##    ## ## #         ##  ## #   ##    ## ##         
".Replace('@', '&');
                        if (Rogue.RAM.Map.Name.IndexOf("Руины") > -1) { s = s.Replace('6', '@'); }
                        if (Rogue.RAM.Map.Name.IndexOf("Поселение") > -1) { s = s.Replace('5', '@'); }
                        if (Rogue.RAM.Map.Name.IndexOf("Катакомбы") > -1) { s = s.Replace('4', '@'); }
                        if (Rogue.RAM.Map.Name.IndexOf("Поле боя") > -1) { s = s.Replace('3', '@'); }
                        if (Rogue.RAM.Map.Name.IndexOf("Храм") > -1) { s = s.Replace('2', '@'); }
                        return s;
                    }
                }

                public static string Dwarf
                {
                    get
                    {
                        string s =
@"
 +#####%%###            =@#########=======@##%%===@####@@@%====##+   
    *=%##=##+                    @#@==%#########%=#######@#######*   
     %###2##=                     @#####*      ##%###@%+             
    @##%####*                                 *##==%%@######=        
     +@#@+                         @###@      +##=######===@##%      
                                  ##@=%##+    ##%##%  ##%=6=@#@      
                                 *##%==##=   %##=##+  @#######=      
                              =####@==##@   %#####*     *+++         
                             +##%===%##*   ###@#@      +=%=*         
             *@###*  *#########%@%==##+   =##=##*   #####@##%        
      *########%%##* ##%=@#########%###*  +##=##++=##@====@#%        
      ##%==%#####%##+##=###++*    %##@##% *########@=@@@@@##+        
      %######*=##=@####@=@#####     %####    *%#######@@@@=          
            *###===5######@%%##*     ##@##########3###########@*     
           %##%=====@#@  +@###*     *##=%@############%==%%%%%##@    
            ########@####%           @####=%###@+    %###=====##@    
              *====%###%##                ##@%%###+    *#######=     
                   +##%##@=%########%####+=###%==###                 
          +%@@@@+ @##==####@%@####%@@@=@####%4###=##                 
       %####@@@@###@###%==#%%##%+#############=##%##                 
     @##%@###################=##+       ######@#@##@                 
".Replace('@', '&');
                        if (Rogue.RAM.Map.Name.IndexOf("Руины") > -1) { s = s.Replace('6', '@'); }
                        if (Rogue.RAM.Map.Name.IndexOf("Поселение") > -1) { s = s.Replace('5', '@'); }
                        if (Rogue.RAM.Map.Name.IndexOf("Катакомбы") > -1) { s = s.Replace('4', '@'); }
                        if (Rogue.RAM.Map.Name.IndexOf("Поле боя") > -1) { s = s.Replace('3', '@'); }
                        if (Rogue.RAM.Map.Name.IndexOf("Храм") > -1) { s = s.Replace('2', '@'); }
                        return s;
                    }
                }

                public static string Drow
                {
                    get
                    {
                        string s =
@"
#####################################################################
#####################################################################
#####################################################################
#################### 6**+@###########################################
#########################*###########################################
#########################:########-      -###########################
########################=:#######=        :#####%    %###############
########################@    =###=    3    -+=+:  4  *###############
########################-    -###@        @#####@    *###############
########################*     #####=-    :###########################
########################=     #######-%##############################
###########################%*=#######################################
############################@%#######=###############################
#######################--=@#########--@##############################
#######################:##########*     -+++#########################
#######################*##########:  5  %####=#######################
#####################=  -#########      =###=@#######################
#####################= 2 +#########%===#####=@#######################
############################################@+#######################
#####################################################################
#####################################################################
#####################################################################
#####################################################################

".Replace('@', '&');
                        if (Rogue.RAM.Map.Name.IndexOf("Руины") > -1) { s = s.Replace('6', '@'); }
                        if (Rogue.RAM.Map.Name.IndexOf("Поселение") > -1) { s = s.Replace('5', '@'); }
                        if (Rogue.RAM.Map.Name.IndexOf("Катакомбы") > -1) { s = s.Replace('4', '@'); }
                        if (Rogue.RAM.Map.Name.IndexOf("Поле боя") > -1) { s = s.Replace('3', '@'); }
                        if (Rogue.RAM.Map.Name.IndexOf("Храм") > -1) { s = s.Replace('2', '@'); }
                        return s;
                    }
                }
            }
        }

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

        public static class DigitalArt
        {
            public static void Banishment(int x, int y)
            {
                char[] fst = new char[] { '¤', '√', '▼', '■' };
                char[] scd = new char[] { '▓', '▒', '░', ' ' };

                Thread.Sleep(1000);

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                foreach (char ch in fst)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(ch);
                    Thread.Sleep(180);
                }

                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                foreach (char ch in fst)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(ch);
                    Thread.Sleep(220);
                }

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                foreach (char ch in scd)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(ch);
                    Thread.Sleep(340);
                }
            }

            public static void HolyNova(int x, int y)
            {
                #region Visual
                //                string qw = @"
                //•••
                //•@•
                //•••
                //
                //\↑/
                //←@→
                ///↓\
                //
                //░░░
                //░@░
                //░░░
                //";
                #endregion

                char[][] top = 
                {
                    new char[]{'*','*','*'}, 
                    new char[]{'\\','↑','/'},
                    new char[]{'░','░','░'}
                };
                char[][] mid =
                {
                    new char[]{'*','@','*',},
                    new char[]{'←','@','→',},
                    new char[]{'░','@','░',}
                };
                char[][] bot =
                {
                    new char[]{'*','*','*',},
                    new char[]{'/','↓','\\',},
                    new char[]{'░','░','░',},
                };

                Console.ForegroundColor = ConsoleColor.Yellow;

                for (int i = 0; i < 3; i++)
                {
                    //Top
                    Console.SetCursorPosition(x - 1, y - 1);
                    Console.Write(top[i][0]);
                    Console.SetCursorPosition(x, y - 1);
                    Console.Write(top[i][1]);
                    Console.SetCursorPosition(x + 1, y - 1);
                    Console.Write(top[i][2]);
                    //Mid
                    Console.SetCursorPosition(x - 1, y);
                    Console.Write(mid[i][0]);
                    Console.SetCursorPosition(x, y);
                    Console.Write(mid[i][1]);
                    Console.SetCursorPosition(x + 1, y);
                    Console.Write(mid[i][2]);
                    //Bot
                    Console.SetCursorPosition(x - 1, y + 1);
                    Console.Write(bot[i][0]);
                    Console.SetCursorPosition(x, y + 1);
                    Console.Write(bot[i][1]);
                    Console.SetCursorPosition(x + 1, y + 1);
                    Console.Write(bot[i][2]);
                    Thread.Sleep(500);
                }
                GUIDraw.DrawLab();
            }

            public static void FireShield(int x, int y)
            {
                #region Visual
                //1
                // *
                //(@)
                //
                //2
                //*
                // @
                //
                //3
                //**
                // @
                //
                //4
                //***
                // @
                //
                //5
                //***
                // @*
                //
                //6
                //***
                // @*
                //  *
                //7
                //***
                // @*
                // **
                //8
                //***
                // @*
                //***
                //9
                //***
                //*@*
                //***

                //";
                #endregion

                char[][] top = 
                {
                    new char[]{' ','*',' '}, 
                    new char[]{'*',' ',' '},
                    new char[]{'*','*',' '},
                    new char[]{'*','*','*'},
                    new char[]{'*','*','*'},
                    new char[]{'*','*','*'},
                    new char[]{'*','*','*'},
                    new char[]{'*','*','*'},
                    new char[]{'*','*','*'}
                };
                char[][] mid =
                {
                    new char[]{'(','@',')'}, 
                    new char[]{' ','@',' '},
                    new char[]{' ','@',' '},
                    new char[]{' ','@',' '},
                    new char[]{' ','@','*'},//
                    new char[]{' ','@','*'},
                    new char[]{' ','@','*'},
                    new char[]{' ','@','*'},
                    new char[]{'*','@','*'}
                };
                char[][] bot =
                {
                    new char[]{' ',' ',' '}, 
                    new char[]{' ',' ',' '},
                    new char[]{' ',' ',' '},
                    new char[]{' ',' ',' '},
                    new char[]{' ',' ',' '},
                    new char[]{' ',' ','*'},
                    new char[]{' ','*','*'},
                    new char[]{'*','*','*'},
                    new char[]{'*','*','*'}
                };

                Console.ForegroundColor = ConsoleColor.Red;

                #region First

                //Top
                Console.SetCursorPosition(x - 1, y - 1);
                Console.Write(top[0][0]);
                Console.SetCursorPosition(x, y - 1);
                Console.Write(top[0][1]);
                Console.SetCursorPosition(x + 1, y - 1);
                Console.Write(top[0][2]);
                //Mid
                Console.SetCursorPosition(x - 1, y);
                Console.Write(mid[0][0]);
                Console.SetCursorPosition(x, y);
                Console.Write(mid[0][1]);
                Console.SetCursorPosition(x + 1, y);
                Console.Write(mid[0][2]);
                //Bot
                Console.SetCursorPosition(x - 1, y + 1);
                Console.Write(bot[0][0]);
                Console.SetCursorPosition(x, y + 1);
                Console.Write(bot[0][1]);
                Console.SetCursorPosition(x + 1, y + 1);
                Console.Write(bot[0][2]);
                Thread.Sleep(450);

                #endregion

                #region Other

                for (int q = 0; q < 3; q++)
                {
                    for (int i = 1; i < 9; i++)
                    {
                        //Top
                        Console.SetCursorPosition(x - 1, y - 1);
                        Console.Write(top[i][0]);
                        Console.SetCursorPosition(x, y - 1);
                        Console.Write(top[i][1]);
                        Console.SetCursorPosition(x + 1, y - 1);
                        Console.Write(top[i][2]);
                        //Mid
                        Console.SetCursorPosition(x - 1, y);
                        Console.Write(mid[i][0]);
                        Console.SetCursorPosition(x, y);
                        Console.Write(mid[i][1]);
                        Console.SetCursorPosition(x + 1, y);
                        Console.Write(mid[i][2]);
                        //Bot
                        Console.SetCursorPosition(x - 1, y + 1);
                        Console.Write(bot[i][0]);
                        Console.SetCursorPosition(x, y + 1);
                        Console.Write(bot[i][1]);
                        Console.SetCursorPosition(x + 1, y + 1);
                        Console.Write(bot[i][2]);
                        Thread.Sleep(150);
                    }
                }

                #endregion

                //GUIDraw.DrawLab();
            }

            public static void PoisonNova(int x, int y)
            {
                #region Visual
                //1
                // *
                //(@)
                //
                //2
                //*
                // @
                //
                //3
                //**
                // @
                //
                //4
                //***
                // @
                //
                //5
                //***
                // @*
                //
                //6
                //***
                // @*
                //  *
                //7
                //***
                // @*
                // **
                //8
                //***
                // @*
                //***
                //9
                //***
                //*@*
                //***

                //";
                #endregion

                char[][] top = 
                {
                    new char[]{' ','*',' '}, 
                    new char[]{'«',' ',' '},
                    new char[]{'*','»',' '},
                    new char[]{'*','*','«'},
                    new char[]{'«','*','*'},
                    new char[]{'»','*','*'},
                    new char[]{'*','«','»'},
                    new char[]{'*','*','«'},
                    new char[]{'*','*','*'}
                };
                char[][] mid =
                {
                    new char[]{'(','@',')'}, 
                    new char[]{' ','@',' '},
                    new char[]{' ','@',' '},
                    new char[]{' ','@',' '},
                    new char[]{' ','@','*'},//
                    new char[]{' ','@','*'},
                    new char[]{' ','@','»'},
                    new char[]{' ','@','*'},
                    new char[]{'*','@','*'}
                };
                char[][] bot =
                {
                    new char[]{' ',' ',' '}, 
                    new char[]{' ',' ',' '},
                    new char[]{' ',' ',' '},
                    new char[]{' ',' ',' '},
                    new char[]{' ',' ',' '},
                    new char[]{' ',' ','['},
                    new char[]{' ','*','*'},
                    new char[]{'*','*','*'},
                    new char[]{'*','[','*'}
                };

                Console.ForegroundColor = ConsoleColor.Green;

                #region First

                //Top
                Console.SetCursorPosition(x - 1, y - 1);
                Console.Write(top[0][0]);
                Console.SetCursorPosition(x, y - 1);
                Console.Write(top[0][1]);
                Console.SetCursorPosition(x + 1, y - 1);
                Console.Write(top[0][2]);
                //Mid
                Console.SetCursorPosition(x - 1, y);
                Console.Write(mid[0][0]);
                Console.SetCursorPosition(x, y);
                Console.Write(mid[0][1]);
                Console.SetCursorPosition(x + 1, y);
                Console.Write(mid[0][2]);
                //Bot
                Console.SetCursorPosition(x - 1, y + 1);
                Console.Write(bot[0][0]);
                Console.SetCursorPosition(x, y + 1);
                Console.Write(bot[0][1]);
                Console.SetCursorPosition(x + 1, y + 1);
                Console.Write(bot[0][2]);
                Thread.Sleep(450);

                #endregion

                #region Other

                for (int q = 0; q < 1; q++)
                {
                    for (int i = 1; i < 9; i++)
                    {
                        //Top
                        Console.SetCursorPosition(x - 1, y - 1);
                        Console.Write(top[i][0]);
                        Console.SetCursorPosition(x, y - 1);
                        Console.Write(top[i][1]);
                        Console.SetCursorPosition(x + 1, y - 1);
                        Console.Write(top[i][2]);
                        //Mid
                        Console.SetCursorPosition(x - 1, y);
                        Console.Write(mid[i][0]);
                        Console.SetCursorPosition(x, y);
                        Console.Write(mid[i][1]);
                        Console.SetCursorPosition(x + 1, y);
                        Console.Write(mid[i][2]);
                        //Bot
                        Console.SetCursorPosition(x - 1, y + 1);
                        Console.Write(bot[i][0]);
                        Console.SetCursorPosition(x, y + 1);
                        Console.Write(bot[i][1]);
                        Console.SetCursorPosition(x + 1, y + 1);
                        Console.Write(bot[i][2]);
                        Thread.Sleep(150);
                    }
                }

                #endregion

                //GUIDraw.DrawLab();
            }
        }

        public static class DrawHelp
        {
            /// <summary>
            /// Something about full line writing
            /// </summary>
            /// <param name="Width">i think result line width</param>
            /// <param name="Line">text</param>
            /// <param name="Spase">int for minus full line width</param>
            /// <returns></returns>
            public static string FullLine(int Width, string Line, int Spase = 1)
            {
                string r = string.Empty;
                for (int i = 0; i < Width - Spase; i++)
                {
                    r += Line;
                }
                return r;
            }
            /// <summary>
            /// Get line
            /// </summary>
            /// <param name="Width">Line width</param>
            /// <param name="Char">Char for line</param>
            /// <returns>string.length==width and string.indexOf(Char)==width</returns>
            public static string GetLine(int Width, char Char)
            {
                string s = "";
                for (int i = 0; i < Width; i++) { s += Char; }
                return s;
            }
            /// <summary>
            /// return one char in string for console borders
            /// </summary>
            /// <param name="Bold">Bold = true</param>
            /// <param name="Wall">Int position of wall or=></param>
            /// <param name="NameOfWall">=> or string name of wall like 'TopConLeft', it's mean Upper Corner Left</param>
            /// <returns></returns>
            public static string Border(bool Bold, int Wall, string NameOfWall = "")
            {
                char r = new char();

                if (NameOfWall == "")
                {
                    if (Bold == true)
                    {
                        switch (Wall)
                        {
                            case 1: { r = '╔'; break; }
                            case 2: { r = '╗'; break; }
                            case 3: { r = '║'; break; }
                            case 4: { r = '═'; break; }
                            case 5: { r = '╚'; break; }
                            case 6: { r = '╝'; break; }
                            case 7: { r = '╣'; break; }
                            case 8: { r = '╠'; break; }
                            case 9: { r = '╩'; break; }
                            case 10: { r = '╦'; break; }
                            case 11: { r = '╬'; break; }
                        }
                    }
                    else
                    {
                        switch (Wall)
                        {
                            case 1: { r = '┌'; break; }
                            case 2: { r = '┐'; break; }
                            case 3: { r = '│'; break; }
                            case 4: { r = '─'; break; }
                            case 5: { r = '└'; break; }
                            case 6: { r = '┘'; break; }
                            case 7: { r = '├'; break; }
                            case 8: { r = '┤'; break; }
                            case 9: { r = '┴'; break; }
                            case 10: { r = '┬'; break; }
                            case 11: { r = '┼'; break; }
                        }
                    }
                }
                else
                {
                    if (Bold == true)
                    {
                        switch (NameOfWall)
                        {
                            case "TopCornLeft": { r = '╔'; break; }
                            case "TopCornRight": { r = '╗'; break; }
                            case "WallVert": { r = '║'; break; }
                            case "WallHor": { r = '═'; break; }
                            case "BotCornLeft": { r = '╚'; break; }
                            case "BotCornRight": { r = '╝'; break; }
                            case "ThreeLeft": { r = '╣'; break; }
                            case "ThreeRight": { r = '╠'; break; }
                            case "ThreeTop": { r = '╩'; break; }
                            case "ThreeBot": { r = '╦'; break; }
                            case "Cross": { r = '╬'; break; }
                        }
                    }
                    else
                    {
                        switch (NameOfWall)
                        {
                            case "TopCornLeft": { r = '┌'; break; }
                            case "TopCornRight": { r = '┐'; break; }
                            case "WallVert": { r = '│'; break; }
                            case "WallHor": { r = '─'; break; }
                            case "BotCornLeft": { r = '└'; break; }
                            case "BotCornRight": { r = '┘'; break; }
                            case "ThreeLeft": { r = '┤'; break; }
                            case "ThreeRight": { r = '├'; break; }
                            case "ThreeTop": { r = '┴'; break; }
                            case "ThreeBot": { r = '┬'; break; }
                            case "Cross": { r = '┼'; break; }
                        }
                    }
                }
                return r.ToString();
            }
            /// <summary>
            /// Return text block with lines whitch equal RowLimit
            /// </summary>
            /// <param name="Text">Text</param>
            /// <param name="RowLimit">Limit</param>
            /// <returns></returns>
            public static List<string> TextBlock(string Text, int RowLimit)
            {
                List<string> tb = new List<string>();
                int lines = Convert.ToInt32(Text.Length / RowLimit) + 1;
                for (int i = 0; i < lines; i++)
                {
                    try
                    { tb.Add(Text.Substring(i * RowLimit, RowLimit)); }
                    catch (ArgumentOutOfRangeException) { tb.Add(Text.Substring(i * RowLimit)); }
                }
                return tb;
            }
            /// <summary>
            /// Write text in ConsolePosition x,y in center
            /// </summary>
            /// <param name="Left">Позиция столбца курсора.</param>            
            /// <param name="Top">Позиция строки курсора.</param>
            /// <param name="Length">Допустимая длина.</param>
            /// <param name="Text">Текст для записи.</param>
            public static void WriteCenterPosition(int Left, int Top, int Length, string Text)
            {
                int c = (Length / 2) - (Text.Length / 2);
                Console.SetCursorPosition(c + Left, Top);
                Console.Write(Text);
            }
            /// <summary>
            /// Draw avatar in left window, set char
            /// </summary>
            public static ColorChar DrawAvatar
            {
                set
                {
                    Console.ForegroundColor = Rogue.RAM.CUIColor;
                    int Count = (23 / 2) - ("╔═══════╗".Length / 2);
                    Console.SetCursorPosition(Count + 1, 4);
                    Console.WriteLine("╔═══════╗");
                    int Countt = 5;
                    for (int i = 0; i < 5; i++)
                    {
                        Console.SetCursorPosition(Count + 1, Countt++);
                        Console.WriteLine("║       ║");
                        if (i == 2)
                        {
                            Console.ForegroundColor = value.Color;
                            Console.SetCursorPosition(Count + 5, Countt - 1);
                            Console.Write(value.Char);
                            Console.ForegroundColor = Rogue.RAM.CUIColor;
                        }
                    }
                    Console.SetCursorPosition(Count + 1, Countt);
                    Console.WriteLine("╚═══════╝");
                }
            }
            /// <summary>
            /// Return int value with sign ( - or + )
            /// </summary>
            /// <param name="Number">int value</param>
            /// <returns></returns>
            public static string Sign(int Number)
            {
                if (Number >= 0) { return "+" + Number.ToString(); }
                else { return Number.ToString(); }
            }
            /// <summary>
            /// Return double value with sign ( - or + )
            /// </summary>
            /// <param name="Number">double value</param>
            /// <returns></returns>
            public static string Sign(double Number)
            {
                if (Number >= 0) { return "+" + Number.ToString(); }
                else { return Number.ToString(); }
            }
        }

        public static class StringMenus
        {
            public static StringMenu DrawMenu
            {
                set
                {
                    Console.BackgroundColor = ConsoleColor.Black;

                    Console.SetCursorPosition(45, 4);
                    Console.ForegroundColor = value.Logo.Color;
                    Console.WriteLine(value.Logo.Word);

                    Console.SetCursorPosition(55, 3);
                    Console.ForegroundColor = value.Additional.Color;
                    Console.WriteLine(value.Additional.Word);

                    int c = (100 / 2) - (value.Addon.Word.Length / 2);
                    Console.SetCursorPosition(c, 7);
                    Console.ForegroundColor = value.Addon.Color;
                    Console.WriteLine(value.Addon.Word);

                    int height = 10;
                    for (int i = 0; i < value.Options.Count; i++)
                    {
                        if (i != value.Index)
                        {
                            Console.BackgroundColor = value.OptionsColorBackground;
                            Console.ForegroundColor = value.OptionsColor;
                        }
                        else
                        {
                            Console.BackgroundColor = value.OptionsColorSelectedBackground;
                            Console.ForegroundColor = value.OptionsColorSelected;
                        }
                        int width = (100 / 2) - (value.Options[i].Length / 2);
                        Console.SetCursorPosition(width, height);
                        Console.Write(value.Options[i]);
                        height+=2;
                    }
                }
            }
        }

        public static class DialogueDraw
        {

            public static void Helpers()
            {
                PlayEngine.Enemy = false;

                ConsoleWindows.Window w = new ConsoleWindows.Window();
                w.Animated = true;
                w.Animation = ConsoleWindows.Additional.StadartAnimation;
                w.Border = ConsoleWindows.Additional.BoldBorder;
                w.BorderColor = ConsoleColor.DarkGreen;
                w.Header = true;
                w.Height = 20;
                w.Width = 60;
                w.Left = 20;
                w.Top = 5;

                ConsoleWindows.Text t = new ConsoleWindows.Text(w);
                t.BackgroundColor = ConsoleColor.Black;
                t.TextPosition = ConsoleWindows.TextPosition.Center;
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("Добро пожаловать в ");
                t.ForegroundColor = ConsoleColor.DarkCyan;
                t.Write("Dungeon");
                t.ForegroundColor = ConsoleColor.Red;
                t.Write(" 12");
                //t.ForegroundColor = ConsoleColor.Gray;
                //t.Write(" [");
                t.ForegroundColor = ConsoleColor.DarkMagenta;
                t.Write("YANA");
                t.ForegroundColor = ConsoleColor.Gray;
                //t.Write("] !");
                t.WriteLine("!");
                t.AppendLine();
                t.WriteLine("Вы желатете пройти обучение?");
                t.AppendLine();
                t.AppendLine();

                t.Write("Нажмите ");
                t.ForegroundColor = ConsoleColor.DarkYellow;
                t.Write("[Tab]");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write(" для перемещения между кнопками.");
                t.AppendLine();

                t.Write("Текущую активную кнопку можно узнать по ");
                t.ForegroundColor = ConsoleColor.Yellow;
                t.Write("желтой");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write(" надписи.");
                t.AppendLine();

                t.Write("Нажмите ");
                t.ForegroundColor = ConsoleColor.DarkYellow;
                t.Write("[Enter]");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write(" для выбора варианта.");
                t.AppendLine();

                w.Text = t;

                //Controls 
                ConsoleWindows.Button by = new ConsoleWindows.Button(w);
                by.Top = 11;
                by.Left = 12;
                by.Width = 8;
                by.Height = 5;
                by.ActiveColor = ConsoleColor.Yellow;
                by.InactiveColor = ConsoleColor.DarkGray;
                by.CloseAfterUse = true;
                by.Label = "Да";
                w.AddControl(by);

                ConsoleWindows.Button bn = new ConsoleWindows.Button(w);
                bn.Top = 11;
                bn.Left = 40;
                bn.Width = 8;
                bn.Height = 12;
                bn.ActiveColor = ConsoleColor.Yellow;
                bn.InactiveColor = ConsoleColor.DarkGray;
                bn.Label = "Нет";
                bn.CloseAfterUse = true;
                w.AddControl(bn);

                w.Draw();

                if (w.Sender.Label == "Нет")
                {
                    w = new ConsoleWindows.Window();
                    w.Animated = true;
                    w.Animation = ConsoleWindows.Additional.StadartAnimation;
                    w.Border = ConsoleWindows.Additional.BoldBorder;
                    w.BorderColor = ConsoleColor.DarkGreen;
                    w.Header = true;
                    w.Height = 20;
                    w.Width = 60;
                    w.Left = 20;
                    w.Top = 5;

                    t = new ConsoleWindows.Text(w);
                    t.BackgroundColor = ConsoleColor.Black;
                    t.TextPosition = ConsoleWindows.TextPosition.Center;
                    t.ForegroundColor = ConsoleColor.Gray;
                    t.Write("Добро пожаловать в ");
                    t.ForegroundColor = ConsoleColor.DarkCyan;
                    t.Write("Dungeon");
                    t.ForegroundColor = ConsoleColor.Red;
                    t.Write(" 12");
                    //t.ForegroundColor = ConsoleColor.Gray;
                    //t.Write(" [");
                    t.ForegroundColor = ConsoleColor.DarkMagenta;
                    t.Write("YANA");
                    t.ForegroundColor = ConsoleColor.Gray;
                    //t.Write("] !");
                    t.WriteLine("!");
                    t.AppendLine();
                    t.AppendLine();
                    t.AppendLine();
                    t.AppendLine();
                    t.WriteLine("Не забудьте поговорить со своим ангелом-хранителем!");
                    t.WriteLine("Команда разработчиков желает вам приятной игры!");                    
                    w.Text = t;

                    //Controls 
                    by = new ConsoleWindows.Button(w);
                    by.Top = 11;
                    by.Left = 20;
                    by.Width = 20;
                    by.Height = 5;
                    by.ActiveColor = ConsoleColor.Yellow;
                    by.InactiveColor = ConsoleColor.DarkGray;
                    by.Label = "Продолжить";
                    by.CloseAfterUse = true;
                    w.AddControl(by);

                    w.Draw();
                }
                else
                {
                    Education();
                }

                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Black;
                DrawEngine.GUIDraw.DrawGUI();
                DrawEngine.GUIDraw.drawstat();

                PlayEngine.Enemy = true;
            }
            public static void Education()
            {
                prt1();
                prt2();
                prt3();
                prt4();
                prt5();
                prt6();
                prt7();
                prt8();
                prt9();
                prt10();
                prt11();
            }
            private static void prt1()
            {
                ConsoleWindows.Window w = new ConsoleWindows.Window();
                w.Animated = true;
                w.Animation = ConsoleWindows.Additional.StadartAnimation;
                w.Border = ConsoleWindows.Additional.BoldBorder;
                w.BorderColor = ConsoleColor.DarkGreen;
                w.Header = true;
                w.Height = 20;
                w.Width = 65;
                w.Left = 15;
                w.Top = 5;

                ConsoleWindows.Text t = new ConsoleWindows.Text(w);
                t.BackgroundColor = ConsoleColor.Black;
                t.TextPosition = ConsoleWindows.TextPosition.Center;
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("Обучение: Интерфейс - ");
                t.ForegroundColor = ConsoleColor.Red;
                t.Write("1");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("/");
                t.ForegroundColor = ConsoleColor.Green;
                t.Write("4");
                t.AppendLine();

                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("Справа (");
                t.ForegroundColor = ConsoleColor.Yellow;
                t.Write("→");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write(") располагается окно персонажа.");
                t.AppendLine();

                t.WriteLine("В окне персонажа показываются ваши текущие характеристики:");

                t.ForegroundColor = ConsoleColor.Cyan;
                t.Write("Имя персонажа,"); t.ForegroundColor = ConsoleColor.Gray; t.Write(" Раса, Класс,"); t.ForegroundColor = ConsoleColor.DarkGray; t.Write(" Уровень, Опыт, ");
                t.ForegroundColor = ConsoleColor.Red;
                t.Write("Здоровье,"); t.ForegroundColor = ConsoleColor.White; t.Write(" Ресурсы,");
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.DarkYellow;
                t.Write("Урон от удара,"); t.ForegroundColor = ConsoleColor.DarkRed; t.Write("Сила атаки,"); t.ForegroundColor = ConsoleColor.DarkCyan; t.Write(" Сила магии,"); t.ForegroundColor = ConsoleColor.DarkGreen; t.Write(" Два вида защиты,");
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.Yellow;
                t.Write("Золото,"); t.ForegroundColor = ConsoleColor.DarkRed; t.Write(" Инвентарь.");
                t.AppendLine();
                t.AppendLine();

                t.ForegroundColor = ConsoleColor.Gray;
                t.WriteLine("Подробнее о них вы узнаете в процессе игры.");

                w.Text = t;

                //Controls 
                ConsoleWindows.Button by = new ConsoleWindows.Button(w);
                by.Top = 12;
                by.Left = 22;
                by.Width = 20;
                by.Height = 5;
                by.ActiveColor = ConsoleColor.Yellow;
                by.InactiveColor = ConsoleColor.DarkGray;
                by.Label = "Продолжить";
                by.CloseAfterUse = true;
                w.AddControl(by);

                w.Draw();

            }
            private static void prt2()
            {
                ConsoleWindows.Window w = new ConsoleWindows.Window();
                w.Animated = true;
                w.Animation = ConsoleWindows.Additional.StadartAnimation;
                w.Border = ConsoleWindows.Additional.BoldBorder;
                w.BorderColor = ConsoleColor.DarkGreen;
                w.Header = true;
                w.Height = 20;
                w.Width = 78;
                w.Left = 12;
                w.Top = 5;

                ConsoleWindows.Text t = new ConsoleWindows.Text(w);
                t.BackgroundColor = ConsoleColor.Black;
                t.TextPosition = ConsoleWindows.TextPosition.Center;
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("Обучение: Интерфейс - ");
                t.ForegroundColor = ConsoleColor.DarkYellow;
                t.Write("2");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("/");
                t.ForegroundColor = ConsoleColor.Green;
                t.Write("4");
                t.AppendLine();

                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("Слева от окна персонажа (");
                t.ForegroundColor = ConsoleColor.Yellow;
                t.Write("←");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write(") располагается окно карты.");
                t.AppendLine();

                t.WriteLine("Это главное окно игры, в нём вы можете взаимодействовать с миром.");
                t.WriteLine("Для того что бы узнать условные обозначения, нажмите клавишу [M]");
                t.WriteLine("в режиме активной игры, и вы увидите основные обозначение объектов.");
                t.WriteLine("Для того что бы с ними взаимодействовать, посмотрите нижнее окно.");
                t.WriteLine("Это окно доступных клавиш. В зависимости от игрового экрана, на");
                t.WriteLine("нем отображается список доступных действий. На главном экране,");
                t.WriteLine("Основными клавишами являются: [C],[I],[Q,W,E,R],[O],[1-6].");

                w.Text = t;

                //Controls 
                ConsoleWindows.Button by = new ConsoleWindows.Button(w);
                by.Top = 12;
                by.Left = 27;
                by.Width = 20;
                by.Height = 5;
                by.ActiveColor = ConsoleColor.Yellow;
                by.InactiveColor = ConsoleColor.DarkGray;
                by.Label = "Продолжить";
                by.CloseAfterUse = true;
                w.AddControl(by);

                w.Draw();
            }
            private static void prt3()
            {
                ConsoleWindows.Window w = new ConsoleWindows.Window();
                w.Animated = true;
                w.Animation = ConsoleWindows.Additional.StadartAnimation;
                w.Border = ConsoleWindows.Additional.BoldBorder;
                w.BorderColor = ConsoleColor.DarkGreen;
                w.Header = true;
                w.Height = 20;
                w.Width = 78;
                w.Left = 12;
                w.Top = 5;

                ConsoleWindows.Text t = new ConsoleWindows.Text(w);
                t.BackgroundColor = ConsoleColor.Black;
                t.TextPosition = ConsoleWindows.TextPosition.Center;
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("Обучение: Интерфейс - ");
                t.ForegroundColor = ConsoleColor.Yellow;
                t.Write("3");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("/");
                t.ForegroundColor = ConsoleColor.Green;
                t.Write("4");
                t.AppendLine();

                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("Под окном карты и персонажа (");
                t.ForegroundColor = ConsoleColor.Yellow;
                t.Write("↓");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write(") располагается окно информации.");
                t.AppendLine();

                t.WriteLine("В окне информации вы получаете основной отклик своих действий.");
                t.WriteLine("Цвет текста сообщения имеет значение:");
                t.AppendLine();
                List<char> rainbow = new List<char>() { 'Ц', 'в', 'е', 'т', 'н', 'о','й' };
                for (int i = 0; i < 7; i++)
                {
                    t.ForegroundColor = (ConsoleColor)i + 5;
                    t.Write(Convert.ToString(rainbow[i]));
                }
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write(" текст - в котором различные слова цветные, обычный.");
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.DarkGreen ;
                t.Write("Цвет карты");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write(" - чаще всего такой цвет означает не важные действия.");
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.White;
                t.Write("Белый");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write(" - цвет предупреждений. Обращайте внимание на белые сообщения.");
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.Red;
                t.Write("Красный");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write(" - цвет критических предупреждений. Сообщение очень важно.");
                t.AppendLine();

                w.Text = t;

                //Controls 
                ConsoleWindows.Button by = new ConsoleWindows.Button(w);
                by.Top = 12;
                by.Left = 27;
                by.Width = 20;
                by.Height = 5;
                by.ActiveColor = ConsoleColor.Yellow;
                by.InactiveColor = ConsoleColor.DarkGray;
                by.Label = "Продолжить";
                by.CloseAfterUse = true;
                w.AddControl(by);

                w.Draw();
            }
            private static void prt4()
            {
                ConsoleWindows.Window w = new ConsoleWindows.Window();
                w.Animated = true;
                w.Animation = ConsoleWindows.Additional.StadartAnimation;
                w.Border = ConsoleWindows.Additional.BoldBorder;
                w.BorderColor = ConsoleColor.DarkGreen;
                w.Header = true;
                w.Height = 20;
                w.Width = 78;
                w.Left = 12;
                w.Top = 5;

                ConsoleWindows.Text t = new ConsoleWindows.Text(w);
                t.BackgroundColor = ConsoleColor.Black;
                t.TextPosition = ConsoleWindows.TextPosition.Center;
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("Обучение: Интерфейс - ");
                t.ForegroundColor = ConsoleColor.Green;
                t.Write("4");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("/");
                t.ForegroundColor = ConsoleColor.Green;
                t.Write("4");
                t.AppendLine();

                t.ForegroundColor = ConsoleColor.Gray;
                t.AppendLine();
                t.AppendLine();
                t.WriteLine("Существует множество разных окон и экранов.");
                t.WriteLine("Для того что бы в них разобраться, следует чаще смотреть");
                t.WriteLine("на окно доступных клавиш. Так же, следует знать, что основные");
                t.WriteLine("клавиши перемещения по объектам на экране - стрелки.");

                w.Text = t;

                //Controls 
                ConsoleWindows.Button by = new ConsoleWindows.Button(w);
                by.Top = 11;
                by.Left = 17;
                by.Width = 40;
                by.Height = 5;
                by.ActiveColor = ConsoleColor.Yellow;
                by.InactiveColor = ConsoleColor.DarkGray;
                by.Label = "Перейти к обучению на карте.";
                by.CloseAfterUse = true;
                w.AddControl(by);

                w.Draw();
            }
            private static void prt5()
            {
                ConsoleWindows.Window w = new ConsoleWindows.Window();
                w.Animated = true;
                w.Animation = ConsoleWindows.Additional.StadartAnimation;
                w.Border = ConsoleWindows.Additional.BoldBorder;
                w.BorderColor = ConsoleColor.DarkGreen;
                w.Header = true;
                w.Height = 20;
                w.Width = 78;
                w.Left = 12;
                w.Top = 5;

                ConsoleWindows.Text t = new ConsoleWindows.Text(w);
                t.BackgroundColor = ConsoleColor.Black;
                t.TextPosition = ConsoleWindows.TextPosition.Center;
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("Обучение: Карта - ");
                t.ForegroundColor = ConsoleColor.Red;
                t.Write("1");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("/");
                t.ForegroundColor = ConsoleColor.Green;
                t.Write("3");
                t.AppendLine();

                t.ForegroundColor = ConsoleColor.Gray;
                t.AppendLine();
                t.AppendLine();
                t.WriteLine("Передвижение.");
                t.WriteLine("Для того что бы сдвинуться с места, закройте это окно, и");
                t.Write("нажмите на клавишу ");
                t.ForegroundColor = ConsoleColor.Yellow;
                t.Write("[↓]");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write(" .");
                t.AppendLine();

                w.Text = t;

                //Controls 
                ConsoleWindows.Button by = new ConsoleWindows.Button(w);
                by.Top = 11;
                by.Left = 17;
                by.Width = 40;
                by.Height = 5;
                by.ActiveColor = ConsoleColor.Yellow;
                by.InactiveColor = ConsoleColor.DarkGray;
                by.Label = "Закрыть окно.";
                by.CloseAfterUse = true;
                w.AddControl(by);

                w.Draw();

                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Black;
                DrawEngine.GUIDraw.DrawGUI();
                DrawEngine.GUIDraw.DrawLab();
                DrawEngine.GUIDraw.drawstat();
                DrawEngine.CharMap.DrawCMap(new List<string>() 
                                { 
                                    "[↓] - Идти вниз "
                                });
                bool end = false;
                while (!end)
                {
                    ConsoleKey r = Console.ReadKey(true).Key;
                    switch (r)
                    {
                        case ConsoleKey.DownArrow:
                            {
                                Rogue.RAM.Step.Step();
                                PlayEngine.GamePlay.MoveCharacter(2);
                                end = true; break;
                            }
                        default: { break; }
                    }
                }                
            }
            private static void prt6()
            {
                ConsoleWindows.Window w = new ConsoleWindows.Window();
                w.Animated = true;
                w.Animation = ConsoleWindows.Additional.StadartAnimation;
                w.Border = ConsoleWindows.Additional.BoldBorder;
                w.BorderColor = ConsoleColor.DarkGreen;
                w.Header = true;
                w.Height = 20;
                w.Width = 80;
                w.Left = 12;
                w.Top = 5;

                ConsoleWindows.Text t = new ConsoleWindows.Text(w);
                t.BackgroundColor = ConsoleColor.Black;
                t.TextPosition = ConsoleWindows.TextPosition.Center;
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("Обучение: Карта - ");
                t.ForegroundColor = ConsoleColor.Yellow;
                t.Write("2");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("/");
                t.ForegroundColor = ConsoleColor.Green;
                t.Write("3");
                t.AppendLine();

                t.ForegroundColor = ConsoleColor.Gray;
                t.AppendLine();
                t.AppendLine();
                t.WriteLine("Передвижение.");
                t.WriteLine("Ваш персонаж переместился вниз, теперь он может ходить по всем направлениям.");
                t.Write("Закройте окно, найдите любого NPC, и нажмите ");
                t.ForegroundColor = ConsoleColor.Yellow;
                t.Write("[I]");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write(" .");
                t.AppendLine();
                w.Text = t;

                ConsoleWindows.Button by = new ConsoleWindows.Button(w);
                by.Top = 11;
                by.Left = 17;
                by.Width = 40;
                by.Height = 5;
                by.ActiveColor = ConsoleColor.Yellow;
                by.InactiveColor = ConsoleColor.DarkGray;
                by.Label = "Закрыть окно.";
                by.CloseAfterUse = true;
                w.AddControl(by);

                w.Draw();

                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Black;
                DrawEngine.GUIDraw.DrawGUI();
                DrawEngine.GUIDraw.DrawLab();
                DrawEngine.GUIDraw.drawstat();
                DrawEngine.CharMap.DrawCMap(new List<string>() 
                                {
                                    "[I] - Идентификация ",
                                    "[↓] - Идти вниз ",
                                    "[↑] - Идти вверх ",
                                    "[→] - Идти влево ",
                                    "[←] - Идти вправо "
                                });

                bool end = false;
                while (!end)
                {
                    ConsoleKey r = Console.ReadKey(true).Key;
                    switch (r)
                    {
                        case ConsoleKey.UpArrow:
                            {
                                PlayEngine.GamePlay.MoveCharacter(1);
                                break;
                            }
                        case ConsoleKey.DownArrow:
                            {
                                PlayEngine.GamePlay.MoveCharacter(2);
                                break;
                            }
                        case ConsoleKey.LeftArrow:
                            {
                                PlayEngine.GamePlay.MoveCharacter(3);
                                break;
                            }
                        case ConsoleKey.RightArrow:
                            {
                                PlayEngine.GamePlay.MoveCharacter(4);
                                break;
                            }
                        case ConsoleKey.I:
                            {                                
                                PlayEngine.GamePlay.GetInfo.GetInfoFromMap();
                                PlayEngine.Enemy = false;
                                end = true;
                                break;
                            }
                    }
                }
            }
            private static void prt7()
            {
                ConsoleWindows.Window w = new ConsoleWindows.Window();
                w.Animated = true;
                w.Animation = ConsoleWindows.Additional.StadartAnimation;
                w.Border = ConsoleWindows.Additional.BoldBorder;
                w.BorderColor = ConsoleColor.DarkGreen;
                w.Header = true;
                w.Height = 20;
                w.Width = 80;
                w.Left = 12;
                w.Top = 5;

                ConsoleWindows.Text t = new ConsoleWindows.Text(w);
                t.BackgroundColor = ConsoleColor.Black;
                t.TextPosition = ConsoleWindows.TextPosition.Center;
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("Обучение: Карта - ");
                t.ForegroundColor = ConsoleColor.Green;
                t.Write("3");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("/");
                t.ForegroundColor = ConsoleColor.Green;
                t.Write("3");
                t.AppendLine();

                t.ForegroundColor = ConsoleColor.Gray;
                t.WriteLine("Передвижение.");
                t.WriteLine("Все активные команды:");
                t.WriteLine("Начать разговор, Атаковать, Инфо о персонаже - работают аналогично");
                t.AppendLine();
                t.Write("Чаще используйте команду - ");
                t.ForegroundColor = ConsoleColor.Yellow;
                t.Write("[I]");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write(" .");
                t.AppendLine();
                t.WriteLine("Многие непонятные детали станут сразу объяснимы.");
                t.AppendLine();
                w.Text = t;

                ConsoleWindows.Button by = new ConsoleWindows.Button(w);
                by.Top = 11;
                by.Left = 17;
                by.Width = 40;
                by.Height = 5;
                by.ActiveColor = ConsoleColor.Yellow;
                by.InactiveColor = ConsoleColor.DarkGray;
                by.Label = "Перейти к обучению в битве.";
                by.CloseAfterUse = true;
                w.AddControl(by);

                w.Draw();
            }
            private static void prt8()
            {
                ConsoleWindows.Window w = new ConsoleWindows.Window();
                w.Animated = true;
                w.Animation = ConsoleWindows.Additional.StadartAnimation;
                w.Border = ConsoleWindows.Additional.BoldBorder;
                w.BorderColor = ConsoleColor.DarkGreen;
                w.Header = true;
                w.Height = 20;
                w.Width = 80;
                w.Left = 12;
                w.Top = 5;

                ConsoleWindows.Text t = new ConsoleWindows.Text(w);
                t.BackgroundColor = ConsoleColor.Black;
                t.TextPosition = ConsoleWindows.TextPosition.Center;
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("Обучение: Битва - ");
                t.ForegroundColor = ConsoleColor.Red;
                t.Write("1");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("/");
                t.ForegroundColor = ConsoleColor.Green;
                t.Write("3");
                t.AppendLine();

                t.ForegroundColor = ConsoleColor.Gray;
                t.WriteLine("Битва.");
                t.WriteLine("Для того что бы начать битву, персонаж должен атаковать врага.");
                t.WriteLine("В битве у вас будет доступно несколько команд:");
                t.ForegroundColor = ConsoleColor.Red;
                t.Write("[A] : Атака");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write(" - Персонаж будет наносить удар оружием/рукой.");
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.DarkCyan;
                t.Write("[D] : Защита");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write(" - Защищаться. Защита персонажа увеличена в двое.");
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.DarkGray;
                t.Write("[S] : Побег");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write(" - Попытка сбержать. Защитные хар-ки уменьшены в трое.");
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.DarkMagenta;
                t.Write("[Q,W,E,R] : Навыки");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write(" - Подробне в следующем этапе обучения.");
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.Gray;
                t.WriteLine("Теперь закройте окно и попробуйте сразиться с гоблином.");
                w.Text = t;

                ConsoleWindows.Button by = new ConsoleWindows.Button(w);
                by.Top = 11;
                by.Left = 17;
                by.Width = 40;
                by.Height = 5;
                by.ActiveColor = ConsoleColor.Yellow;
                by.InactiveColor = ConsoleColor.DarkGray;
                by.Label = "Перейти к битве.";
                by.CloseAfterUse = true;
                w.AddControl(by);

                w.Draw();

                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Black;
                DrawEngine.GUIDraw.DrawGUI();
                DrawEngine.GUIDraw.drawstat();
                Rogue.RAM.Enemy = DataBase.MobBase.Goblin;
                DrawEngine.FightDraw.DrawFight();
                DrawEngine.CharMap.DrawCMap(new List<string>() 
                                        {
                                            "[A] - Удар рукой",
                                            "[D] - Защищаться ",
                                            "[S] - Сбежать"
                                        });
            }
            private static void prt9()
            {
                ConsoleWindows.Window w = new ConsoleWindows.Window();
                w.Animated = true;
                w.Animation = ConsoleWindows.Additional.StadartAnimation;
                w.Border = ConsoleWindows.Additional.BoldBorder;
                w.BorderColor = ConsoleColor.DarkGreen;
                w.Header = true;
                w.Height = 20;
                w.Width = 80;
                w.Left = 12;
                w.Top = 5;

                ConsoleWindows.Text t = new ConsoleWindows.Text(w);
                t.BackgroundColor = ConsoleColor.Black;
                t.TextPosition = ConsoleWindows.TextPosition.Center;
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("Обучение: Битва - ");
                t.ForegroundColor = ConsoleColor.Yellow;
                t.Write("2");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("/");
                t.ForegroundColor = ConsoleColor.Green;
                t.Write("3");
                t.AppendLine();

                t.ForegroundColor = ConsoleColor.Gray;
                t.WriteLine("Битва.");
                t.WriteLine("Итак, вы вступили в битву.");
                t.WriteLine("Закройте окно и атакуйте врага.");
                w.Text = t;

                ConsoleWindows.Button by = new ConsoleWindows.Button(w);
                by.Top = 11;
                by.Left = 17;
                by.Width = 40;
                by.Height = 5;
                by.ActiveColor = ConsoleColor.Yellow;
                by.InactiveColor = ConsoleColor.DarkGray;
                by.Label = "Закрыть.";
                by.CloseAfterUse = true;
                w.AddControl(by);

                w.Draw();

                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Black;
                DrawEngine.GUIDraw.DrawGUI();
                DrawEngine.GUIDraw.drawstat();
                DrawEngine.FightDraw.DrawFight();
                DrawEngine.CharMap.DrawCMap(new List<string>() 
                                        {
                                            "[A] - Удар рукой",
                                            "[D] - Защищаться ",
                                            "[S] - Сбежать"
                                        });

                bool end = false;
                while (!end)
                {
                    ConsoleKey k = Console.ReadKey(true).Key;
                    switch (k)
                    {
                        case ConsoleKey.A: { PlayEngine.GamePlay.Strike(); end = true; break; }
                        default: { break; }
                    }
                }                
            }
            private static void prt10()
            {
                ConsoleWindows.Window w = new ConsoleWindows.Window();
                w.Animated = true;
                w.Animation = ConsoleWindows.Additional.StadartAnimation;
                w.Border = ConsoleWindows.Additional.BoldBorder;
                w.BorderColor = ConsoleColor.DarkGreen;
                w.Header = true;
                w.Height = 20;
                w.Width = 80;
                w.Left = 12;
                w.Top = 5;

                ConsoleWindows.Text t = new ConsoleWindows.Text(w);
                t.BackgroundColor = ConsoleColor.Black;
                t.TextPosition = ConsoleWindows.TextPosition.Center;
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("Обучение: Битва - ");
                t.ForegroundColor = ConsoleColor.Green;
                t.Write("3");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write("/");
                t.ForegroundColor = ConsoleColor.Green;
                t.Write("3");
                t.AppendLine();

                t.ForegroundColor = ConsoleColor.Gray;
                t.WriteLine("Битва.");
                t.WriteLine("Великолепно! Теперь вы точно знаете как атаковать.");
                t.WriteLine("Но не все классы специализируются на атаке оружием.");
                t.WriteLine("Маги и приближенные к ним классы, используют в бою навыки.");
                t.WriteLine("Для того что бы использовать навык (в бою или нет),");
                t.Write("нужно нажать одну из клавиш - ");
                t.ForegroundColor = ConsoleColor.Yellow;
                t.Write("[Q] [W] [E] [R]");
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.Gray;
                t.WriteLine("Закройте окно и закончите бой используя навыки.");

                w.Text = t;

                ConsoleWindows.Button by = new ConsoleWindows.Button(w);
                by.Top = 11;
                by.Left = 17;
                by.Width = 40;
                by.Height = 5;
                by.ActiveColor = ConsoleColor.Yellow;
                by.InactiveColor = ConsoleColor.DarkGray;
                by.Label = "Закрыть.";
                by.CloseAfterUse = true;
                w.AddControl(by);

                w.Draw();

                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Black;
                DrawEngine.GUIDraw.DrawGUI();
                DrawEngine.GUIDraw.drawstat();
                DrawEngine.FightDraw.DrawFight();
                DrawEngine.CharMap.DrawCMap(new List<string>() 
                                        {
                                            "[Q] - "+Rogue.RAM.Player.Ability[0].Name,
                                            "[W] - "+Rogue.RAM.Player.Ability[1].Name, 
                                            "[E] - "+Rogue.RAM.Player.Ability[2].Name,
                                            "[R] - "+Rogue.RAM.Player.Ability[3].Name,
                                            "[A] - Удар рукой",
                                            "[D] - Защищаться ",
                                            "[S] - Сбежать",
                                        });
                PlayEngine.GamePlay.Fight(true,true);
            }
            private static void prt11()
            {
                PlayEngine.Enemy = false;
                ConsoleWindows.Window w = new ConsoleWindows.Window();
                w.Animated = true;
                w.Animation = ConsoleWindows.Additional.StadartAnimation;
                w.Border = ConsoleWindows.Additional.BoldBorder;
                w.BorderColor = ConsoleColor.DarkGreen;
                w.Header = true;
                w.Height = 22;
                w.Width = 82;
                w.Left = 12;
                w.Top = 5;

                ConsoleWindows.Text t = new ConsoleWindows.Text(w);
                t.BackgroundColor = ConsoleColor.Black;
                t.TextPosition = ConsoleWindows.TextPosition.Center;
                t.ForegroundColor = ConsoleColor.DarkCyan;
                t.Write("Dungeon");
                t.ForegroundColor = ConsoleColor.Red;
                t.Write(" 12");
                //t.ForegroundColor = ConsoleColor.Gray;
                //t.Write(" [");
                t.ForegroundColor = ConsoleColor.DarkMagenta;
                t.Write("YANA");
                t.AppendLine();
                t.AppendLine();
                t.ForegroundColor = ConsoleColor.Gray;
                t.WriteLine("На этом небольшой экскурс и обучение закончено.");
                t.WriteLine("В своём приключении вы встретите ещё много других,");
                t.WriteLine("увлекательных и занимательных вещей.");
                t.WriteLine("Но о них вы должны будете узнать сами.");
                t.Write("Если у вас возникнут трудности в игре, не забывайте о команде - ");          
                t.ForegroundColor = ConsoleColor.Yellow;
                t.Write("[I]");
                t.ForegroundColor = ConsoleColor.Gray;
                t.Write(".");
                t.AppendLine();
                t.AppendLine();
                t.Write("Когда вы будете готовы, закройте окно и поговорите с ");
                t.ForegroundColor = ConsoleColor.Red;
                t.Write("персонажем в центре ");
                t.ForegroundColor = ConsoleColor.White;
                t.Write("(@)");
                t.ForegroundColor = ConsoleColor.Gray;
                t.WriteLine(" !");

                w.Text = t;

                ConsoleWindows.Button by = new ConsoleWindows.Button(w);
                by.Top = 12;
                by.Left = 20;
                by.Width = 40;
                by.Height = 5;
                by.ActiveColor = ConsoleColor.Yellow;
                by.InactiveColor = ConsoleColor.DarkGray;
                by.Label = "Начать играть!";
                by.CloseAfterUse = true;
                w.AddControl(by);

                w.Draw();

                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Black;
                DrawEngine.GUIDraw.DrawGUI();
                DrawEngine.GUIDraw.drawstat();
            }
        }

        public class ColorChar
        { public char Char; public ConsoleColor Color;}
        public class ColoredWord
        { public string Word; public ConsoleColor Color;}
        public class Replica
        { public String Text; public List<String> Options = new List<string>(); public ConsoleColor TextColor; public ConsoleColor OptionsColor;}
        public class StringMenu
        {
            public ColoredWord Logo;
            public ColoredWord Additional;
            public ColoredWord Addon;            
            public List<String> Options;
            public ConsoleColor OptionsColor;
            public ConsoleColor OptionsColorBackground;
            public ConsoleColor OptionsColorSelected;
            public ConsoleColor OptionsColorSelectedBackground;
            public Int32 Index;
        }
                /// <summary>
        /// Just more cose then winAPI char
        /// </summary>
        public class ColouredChar
        {
            public short Color;
            public short BackColor;
            public char Char;
        }
    }
}