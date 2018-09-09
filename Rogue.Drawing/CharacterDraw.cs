using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Drawing
{
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
                    Console.Write("Прогресс: " + p.min.ToString() + "/" + p.max.ToString());
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
        public static void DrawItems(int tab = 1, int index = 0)
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
            bold = false;

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
            bold = false;
            #endregion

            #region Helm
            if (index == 2) { bold = true; }
            if (pl.Equipment.Helm != null)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(28, 11);
                Console.Write(DrawHelp.Border(bold, 0, "TopCornLeft") + "───" + DrawHelp.Border(bold, 0, "TopCornRight") + " " + pl.Equipment.Helm.Name);
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
                Console.Write(DrawHelp.Border(bold, 0, "BotCornLeft") + "───" + DrawHelp.Border(bold, 0, "BotCornRight") + " " + stats);
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
                Console.Write(DrawHelp.Border(bold, 0, "TopCornLeft") + "───" + DrawHelp.Border(bold, 0, "TopCornRight") + " " + pl.Equipment.Armor.Name);
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
                Console.Write(DrawHelp.Border(bold, 0, "BotCornLeft") + "───" + DrawHelp.Border(bold, 0, "BotCornRight") + " " + stats);
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
                        if (Rogue.RAM.iTab.NowTab != 1 && Rogue.RAM.iTab.NowTab != 2 && Rogue.RAM.iTab.NowTab != 3 && Rogue.RAM.iTab.NowTab != 4) { dasr(Rogue.RAM.iTab.NowTab - 4); Rogue.RAM.iTab.NowTab -= 4; }
                        break;
                    }
                case SystemEngine.ArrowDirection.Right:
                    {
                        if (Rogue.RAM.iTab.NowTab != 5 && Rogue.RAM.iTab.NowTab != 6 && Rogue.RAM.iTab.NowTab != 7 && Rogue.RAM.iTab.NowTab != 8) { dasr(Rogue.RAM.iTab.NowTab + 4); Rogue.RAM.iTab.NowTab += 4; }
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
}
