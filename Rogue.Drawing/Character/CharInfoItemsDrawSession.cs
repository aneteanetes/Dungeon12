using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Character
{
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

            Console.ForegroundColor = ConsoleColor.Gray;
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
            ConsoleColor colorBuffer = ConsoleColor.Gray;
            Count = (23 / 2) - ("Экипировано:".Length / 2);
            output = "Экипировано:";
            this.Write(12, Count + 1, new DrawText(DrawHelp.FullLine(output.Length, output, output.Length - 1), colorBuffer));


            if (Rogue.RAM.Player.Equipment.Helm != null)
            {
                itm = Rogue.RAM.Player.Equipment.Helm.Name;
                ilvl = Rogue.RAM.Player.Equipment.Helm.ILvl.ToString();
                colorBuffer = Rogue.RAM.Player.Equipment.Helm.Color;
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
            if (Rogue.RAM.Player.Equipment.Armor != null)
            {
                itm = Rogue.RAM.Player.Equipment.Armor.Name;
                ilvl = Rogue.RAM.Player.Equipment.Armor.ILvl.ToString();
                colorBuffer = Rogue.RAM.Player.Equipment.Armor.Color;
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

            if (Rogue.RAM.Player.Equipment.Boots != null)
            {
                itm = Rogue.RAM.Player.Equipment.Boots.Name;
                ilvl = Rogue.RAM.Player.Equipment.Boots.ILvl.ToString();
                colorBuffer = Rogue.RAM.Player.Equipment.Boots.Color;
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
            if (Rogue.RAM.Player.Equipment.Weapon != null)
            {
                itm = Rogue.RAM.Player.Equipment.Weapon.Name;
                ilvl = Rogue.RAM.Player.Equipment.Weapon.ILvl.ToString();
                colorBuffer = Rogue.RAM.Player.Equipment.Weapon.Color;
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
            if (Rogue.RAM.Player.Equipment.OffHand != null)
            {
                itm = Rogue.RAM.Player.Equipment.OffHand.Name;
                ilvl = Rogue.RAM.Player.Equipment.OffHand.ILvl.ToString();
                colorBuffer = Rogue.RAM.Player.Equipment.OffHand.Color;
            }
            else
            {
                itm = " ";
                ilvl = " ";
            }
            Count = (23 / 2) - ((itm.Length + ilvl.Length + 1) / 2);
            output = itm + " " + ilvl;

            this.Write(18, Count + 1, new DrawText(DrawHelp.FullLine(output.Length, output, output.Length - 1), colorBuffer));

            colorBuffer = ConsoleColor.Red;
            int gear = MechEngine.Item.GetGearScore();
            Count = (23 / 2) - (("GearScore: ".Length + MechEngine.Item.GetGearScore().ToString().Length) / 2);
            output = "GearScore: " + MechEngine.Item.GetGearScore().ToString();

            this.Write(20, Count + 1, new DrawText(DrawHelp.FullLine(output.Length, output, output.Length - 1), colorBuffer));

            //Заполнение ячеек
            if (Rogue.RAM.Player.Equipment.Helm != null)
            {
                this.Write(3, 12, new DrawText(Rogue.RAM.Player.Equipment.Helm.Icon(), Rogue.RAM.Player.Equipment.Helm.Color));
            }

            if (Rogue.RAM.Player.Equipment.Armor != null)
            {
                this.Write(6, 12, new DrawText(Rogue.RAM.Player.Equipment.Helm.Icon(), Rogue.RAM.Player.Equipment.Helm.Color));
            }

            if (Rogue.RAM.Player.Equipment.Weapon != null)
            {
                this.Write(6, 6, new DrawText(Rogue.RAM.Player.Equipment.Helm.Icon(), Rogue.RAM.Player.Equipment.Helm.Color));
            }

            if (Rogue.RAM.Player.Equipment.OffHand != null)
            {
                this.Write(6, 18, new DrawText(Rogue.RAM.Player.Equipment.Helm.Icon(), Rogue.RAM.Player.Equipment.Helm.Color));
            }

            if (Rogue.RAM.Player.Equipment.Boots != null)
            {
                this.Write(9, 12, new DrawText(Rogue.RAM.Player.Equipment.Helm.Icon(), Rogue.RAM.Player.Equipment.Helm.Color));
            }

            //Ab points
            Console.ForegroundColor = ConsoleColor.Magenta;
            Count = (23 / 2) - (("Очки навыков: ".Length + Rogue.RAM.Player.AbPoint.ToString().Length) / 2);
            output = "Очки навыков: " + Rogue.RAM.Player.AbPoint.ToString();
            this.Write(22, Count + 1, new DrawText(DrawHelp.FullLine(output.Length, output, output.Length - 1), colorBuffer));

            //CrPoints
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Count = (23 / 2) - (("Очки профф: ".Length + Rogue.RAM.Player.CrPoint.ToString().Length) / 2);
            output = "Очки профф: " + Rogue.RAM.Player.CrPoint.ToString();

            this.Write(23, Count + 1, new DrawText(DrawHelp.FullLine(output.Length, output, output.Length - 1), colorBuffer));

            return base.Run();
        }
    }
}
