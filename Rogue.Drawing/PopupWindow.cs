using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Drawing.Labirinth;

namespace Rogue.Drawing
{
    public static class PopUpWindowDraw
    {
        public static void Trap_pt1()
        {
            Draw.RunSession<LabirinthDrawSession>();

            PopUpWindowV1("Выберите тип ловушки", 4, new List<string> { "Механическая", "Магическая", "Огненная", "Ядовитая" });
        }
        /// <summary>
        /// Draw aligment window
        /// </summary>
        public static void Banishment_pt1()
        {
            Draw.RunSession<LabirinthDrawSession>();
            PopUpWindowV1("Выберите мировоззрение цели", 2, new List<string> { "Добро", "Зло" });
        }
        /// <summary>
        /// Draw direction window
        /// </summary>
        public static void Banishment_pt2()
        {
            Draw.RunSession<LabirinthDrawSession>();
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
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("LVL: " + Abils[i].Level.ToString());
                Console.ForegroundColor = Rogue.RAM.CUIColor;

                Console.SetCursorPosition(left, 14);
                Console.ForegroundColor = Col;
                Console.Write(DrawHelp.Border(bord[i], 0, "BotCornLeft"));
                Console.SetCursorPosition(left + 1, 14);
                Console.ForegroundColor = Rogue.RAM.CUIColor;
                Console.Write(DrawHelp.Border(false, 0, "WallHor") + DrawHelp.Border(false, 0, "WallHor") + DrawHelp.Border(false, 0, "WallHor"));
                Console.ForegroundColor = Col;
                Console.SetCursorPosition(left + 4, 14);
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
}
