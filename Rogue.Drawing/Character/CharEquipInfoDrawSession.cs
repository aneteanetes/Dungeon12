using System;
using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Character
{
    public class CharEquipInfoDrawSession : CharPerksInfoDrawSession
    {
        public int Index { get; set; }

        public override IDrawSession Run()
        {
            this.WriteHeader("Экипировка персонажа");
            
            MechEngine.Character pl = Rogue.RAM.Player;


            this.DrawItem(pl.Equipment.Weapon, 3);
            this.DrawItem(pl.Equipment.OffHand, 7);
            this.DrawItem(pl.Equipment.Helm, 11);
            this.DrawItem(pl.Equipment.Armor, 15);
            this.DrawItem(pl.Equipment.Boots, 19);

            return base.Run();
        }

        private void DrawItem(Item item, int linePos)
        {
            bool bold = false;

            if (Index == 0) { bold = true; }

            if (item != null)
            {
                this.Write(linePos, 28, new DrawText(DrawHelp.Border(bold, 0, "TopCornLeft") + "───" + DrawHelp.Border(bold, 0, "TopCornRight") + " " + item.Name, ConsoleColor.Gray));
                
                this.Write(linePos, 28, new DrawText("│   │ " + "Уровень предмета: [" + item.ILvl + "]", ConsoleColor.Gray));

                this.Write(linePos, 30, new DrawText(item.Icon, item.Color));


                var p = item;
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
                linePos++;
                this.Write(linePos, 28, new DrawText(DrawHelp.Border(bold, 0, "BotCornLeft") + "───" + DrawHelp.Border(bold, 0, "BotCornRight") + " " + stats, ConsoleColor.Gray));

            }
            else
            {
                linePos--;
                this.Write(linePos, 28, new DrawText(DrawHelp.Border(bold, 0, "TopCornLeft") + "───" + DrawHelp.Border(bold, 0, "TopCornRight") + " Пусто", ConsoleColor.Gray));
                linePos++;
                this.Write(linePos, 28, new DrawText("│   │ Нет", ConsoleColor.Gray));
                linePos++;
                this.Write(linePos, 28, new DrawText(DrawHelp.Border(bold, 0, "BotCornLeft") + "───" + DrawHelp.Border(bold, 0, "BotCornRight") + " Нет", ConsoleColor.Gray));

            }

            string stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
            stringBuffer = " " + DrawHelp.Border(true, 8) + stringBuffer.Remove(stringBuffer.Length - 1) + DrawHelp.Border(true, 7);
            linePos++;
            this.Write(linePos, 25, new DrawText(stringBuffer, ConsoleColor.DarkGreen));
        }
    }
}