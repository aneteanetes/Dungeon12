using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Drawing
{


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

            char ic = '\0';
            ConsoleColor cc = ConsoleColor.Black;
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
                        if (Rogue.RAM.MerchTab.NowTab != 1 && Rogue.RAM.MerchTab.NowTab != 2 && Rogue.RAM.MerchTab.NowTab != 3 && Rogue.RAM.MerchTab.NowTab != 4) { dasr(Rogue.RAM.MerchTab.NowTab - 4); Rogue.RAM.MerchTab.NowTab -= 4; }
                        break;
                    }
                case SystemEngine.ArrowDirection.Right:
                    {
                        if (Rogue.RAM.MerchTab.NowTab != 9 && Rogue.RAM.MerchTab.NowTab != 10 && Rogue.RAM.MerchTab.NowTab != 11 && Rogue.RAM.MerchTab.NowTab != 12) { dasr(Rogue.RAM.MerchTab.NowTab + 4); Rogue.RAM.MerchTab.NowTab += 4; }
                        break;
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
}
