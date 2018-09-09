using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Drawing
{


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
                int myint = ((29 - Rogue.RAM.RealLog.Count) * -1);
                for (int i = 0; i < myint; i++)
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
}
