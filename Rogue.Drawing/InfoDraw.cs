using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Drawing
{

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
                            bonuses = DrawHelp.TextBlock((value as MechEngine.Item.Scroll).Spell.Info.Replace('\n', '\0'), 21);
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
                    Count = (25 / 2) - ((value.ReputationName + ": " + value.ReputationSell.ToString()).Length / 2);
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
}
