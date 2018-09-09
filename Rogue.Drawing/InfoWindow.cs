using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Drawing.Data;

namespace Rogue.Drawing
{
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
            {
                Console.Write("Вы купили предмет: " + ItemName + " и потратили " + SpendGold + " золотых!");
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
            Draw.RunSession<CharacterDataDrawSession>();
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
                if (Old != null)
                {
                    Console.Write("Вы экипировали " + New.Name + " вместо " + Old.Name + " !");
                    Rogue.RAM.RealLog.Add("Вы экипировали " + New.Name + " вместо " + Old.Name + " !");
                }
                else
                {
                    Console.Write("Вы экипировали " + New.Name + " !");
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
}
