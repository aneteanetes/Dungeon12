namespace Rogue.Drawing.Character
{
    using System;
    using Rogue.Drawing.Impl;
    using Rogue.Entites.Items;
    using Rogue.View.Interfaces;

    public class CharInfoItemsDrawSession :DrawSession
    {
        public CharInfoItemsDrawSession()
        {
            this.DrawRegion = new Types.Rectangle
            {
                X = 0,
                Y = 0,
                Width = 25,
                Height = 24
            };

        }

        public Equipment Equipment { get; set; }

        public override IDrawSession Run()
        {
            int height = 24;
            var color = ConsoleColor.DarkGreen;

            string stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(25, DrawHelp.Border(true, 4));
            stringBuffer = DrawHelp.Border(true, 1) + stringBuffer.Remove(stringBuffer.Length - 2) + DrawHelp.Border(true, 2);
            this.Write(0, 1, new DrawText(stringBuffer, color));

            //тело
            for (int i = 1; i < height; i++)
            {
                stringBuffer = DrawHelp.FullLine(25, " ");
                stringBuffer = DrawHelp.Border(true, 3) + stringBuffer.Remove(stringBuffer.Length - 2) + DrawHelp.Border(true, 3);
                this.Write(i, 1, new DrawText(stringBuffer, color));
            }

            //носки 
            stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(25, DrawHelp.Border(true, 4));
            stringBuffer = DrawHelp.Border(true, 5) + stringBuffer.Remove(stringBuffer.Length - 2) + DrawHelp.Border(true, 6);
            this.Write(height, 1, new DrawText(stringBuffer, color));

            //Инвентарь

            int Count = (23 / 2) - ("Экипировка:".Length / 2);
            this.Write(1, Count+1, new DrawText(DrawHelp.FullLine("Экипировка:".Length, "Экипировка:", "Экипировка:".Length - 1), color));

            //Слоты инвентаря
            Count = (23 / 2) - ("┌───┐".Length / 2);
            this.Write(2, Count + 1, new DrawText(DrawHelp.FullLine("┌───┐".Length, "┌───┐", "┌───┐".Length - 1), color));
            this.Write(3, Count + 1, new DrawText(DrawHelp.FullLine("│   │".Length, "│   │", "│   │".Length - 1), color));
            this.Write(4, Count + 1, new DrawText(DrawHelp.FullLine("└───┘".Length, "└───┘", "└───┘".Length - 1), color));

            Count = (23 / 2) - ("┌───┐ ┌───┐ ┌───┐".Length / 2);
            this.Write(5, Count + 1, new DrawText(DrawHelp.FullLine("┌───┐ ┌───┐ ┌───┐".Length, "┌───┐ ┌───┐ ┌───┐", "┌───┐ ┌───┐ ┌───┐".Length - 1), color));
            this.Write(6, Count + 1, new DrawText(DrawHelp.FullLine("│   │ │   │ │   │".Length, "│   │ │   │ │   │", "│   │ │   │ │   │".Length - 1), color));
            this.Write(7, Count + 1, new DrawText(DrawHelp.FullLine("└───┘ └───┘ └───┘".Length, "└───┘ └───┘ └───┘", "└───┘ └───┘ └───┘".Length - 1), color));
       

            Count = (23 / 2) - ("┌───┐".Length / 2);
             this.Write(8, Count + 1, new DrawText(DrawHelp.FullLine("┌───┐".Length, "┌───┐", "┌───┐".Length - 1), color));
             this.Write(9, Count + 1, new DrawText(DrawHelp.FullLine("│   │".Length, "│   │", "│   │".Length - 1), color));
            this.Write(10, Count + 1, new DrawText(DrawHelp.FullLine("└───┘".Length, "└───┘", "└───┘".Length - 1), color));

            string itm = " ";
            string ilvl = " ";
            string output = " ";

            //head
            IDrawColor colorBuffer = new DrawColor(ConsoleColor.Gray);
            Count = (23 / 2) - ("Экипировано:".Length / 2);
            output = "Экипировано:";
            this.Write(12, Count + 1, new DrawText(DrawHelp.FullLine(output.Length, output, output.Length - 1), colorBuffer));


            if (Equipment.Helm != null)
            {
                itm = Equipment.Helm.Name;
                ilvl = Equipment.Helm.Level.ToString();
                colorBuffer = Equipment.Helm.ForegroundColor;
            }
            else
            {
                itm = " ";
                ilvl = " ";
            }
            Count = (23 / 2) - ((itm.Length + 1 + ilvl.Length) / 2);
            output = itm + " " + ilvl;

            this.Write(14, Count + 1, new DrawText(DrawHelp.FullLine(output.Length, output, output.Length - 1), colorBuffer));

            //chest               
            if (Equipment.Armor != null)
            {
                itm = Equipment.Armor.Name;
                ilvl = Equipment.Armor.Level.ToString();
                colorBuffer = Equipment.Armor.ForegroundColor;
            }
            else
            {
                itm = " ";
                ilvl = " ";
            }
            Count = (23 / 2) - ((itm.Length + 1 + ilvl.Length) / 2);
            output = itm + " " + ilvl;

            this.Write(15, Count + 1, new DrawText(DrawHelp.FullLine(output.Length, output, output.Length - 1), colorBuffer));

            //boots

            if (Equipment.Boots != null)
            {
                itm = Equipment.Boots.Name;
                ilvl = Equipment.Boots.Level.ToString();
                colorBuffer = Equipment.Boots.ForegroundColor;
            }
            else
            {
                itm = " ";
                ilvl = " ";
            }
            Count = (23 / 2) - ((itm.Length + ilvl.Length + 1) / 2);
            output = itm + " " + ilvl;
            this.Write(16, Count + 1, new DrawText(DrawHelp.FullLine(output.Length, output, output.Length - 1), colorBuffer));

            //weapon
            if (Equipment.Weapon != null)
            {
                itm = Equipment.Weapon.Name;
                ilvl = Equipment.Weapon.Level.ToString();
                colorBuffer = Equipment.Weapon.ForegroundColor;
            }
            else
            {
                itm = " ";
                ilvl = " ";
            }
            Count = (23 / 2) - ((itm.Length + ilvl.Length + 1) / 2);
            output = itm + " " + ilvl;


            this.Write(17, Count + 1, new DrawText(DrawHelp.FullLine(output.Length, output, output.Length - 1), colorBuffer));


            //Offhand
            if (Equipment.OffHand != null)
            {
                itm = Equipment.OffHand.Name;
                ilvl = Equipment.OffHand.Level.ToString();
                colorBuffer = Equipment.OffHand.ForegroundColor;
            }
            else
            {
                itm = " ";
                ilvl = " ";
            }
            Count = (23 / 2) - ((itm.Length + ilvl.Length + 1) / 2);
            output = itm + " " + ilvl;

            this.Write(18, Count + 1, new DrawText(DrawHelp.FullLine(output.Length, output, output.Length - 1), colorBuffer));

            colorBuffer = new DrawColor(ConsoleColor.Red);
            int gear = 0;// MechEngine.Item.GetGearScore();
            Count = (23 / 2) - (("GearScore: 0".Length) / 2);
            output = "GearScore: 0";

            this.Write(20, Count + 1, new DrawText(DrawHelp.FullLine(output.Length, output, output.Length - 1), colorBuffer));

            //Заполнение ячеек
            if (Equipment.Helm != null)
            {
                this.Write(3, 12, new DrawText(Equipment.Helm.Icon, Equipment.Helm.ForegroundColor));
            }

            if (Equipment.Armor != null)
            {
                this.Write(6, 12, new DrawText(Equipment.Helm.Icon, Equipment.Armor.ForegroundColor));
            }

            if (Equipment.Weapon != null)
            {
                this.Write(6, 6, new DrawText(Equipment.Helm.Icon, Equipment.Weapon.ForegroundColor));
            }

            if (Equipment.OffHand != null)
            {
                this.Write(6, 18, new DrawText(Equipment.Helm.Icon, Equipment.OffHand.ForegroundColor));
            }

            if (Equipment.Boots != null)
            {
                this.Write(9, 12, new DrawText(Equipment.Helm.Icon, Equipment.Boots.ForegroundColor));
            }

            //Ab points
            // нет больше, теперь таланты
            //Console.ForegroundColor = ConsoleColor.Magenta;
            //Count = (23 / 2) - (("Очки навыков: ".Length + AbPoint.ToString().Length) / 2);
            //output = "Очки навыков: " + AbPoint.ToString();
            //this.Write(22, Count + 1, new DrawText(DrawHelp.FullLine(output.Length, output, output.Length - 1), colorBuffer));

            ////CrPoints
            //Console.ForegroundColor = ConsoleColor.DarkMagenta;
            //Count = (23 / 2) - (("Очки профф: ".Length + CrPoint.ToString().Length) / 2);
            //output = "Очки профф: " + CrPoint.ToString();

            this.Write(23, Count + 1, new DrawText(DrawHelp.FullLine(output.Length, output, output.Length - 1), colorBuffer));

            return base.Run();
        }
    }
}
