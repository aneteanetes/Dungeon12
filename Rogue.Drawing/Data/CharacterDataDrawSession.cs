using System;
using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Data
{
    public class CharacterDataDrawSession : DrawSession
    {
        public CharacterDataDrawSession()
        {
            this.DrawRegion = new Types.Rectangle
            {
                X = 75,
                Y = 1,
                Width = 23,
                Height = 17
            };
        }

        public override IDrawSession Run()
        {
            MechEngine.Character Player = Rogue.RAM.Player;
            
            //name
            int positionInLine = (this.DrawRegion.Width / 2) - (Rogue.RAM.Player.Name.Length / 2);
            this.Write(1, positionInLine, new DrawText(Rogue.RAM.Player.Name, ConsoleColor.Cyan));

            //race class
            positionInLine = (this.DrawRegion.Width / 2) - ((Player.GetClassRace(1).Length + Player.GetClassRace(2).Length + 3) / 2);
            string WriteThis = Player.GetClassRace(1) + " - " + Player.GetClassRace(2);
            this.Write(3, positionInLine, new DrawText(WriteThis, ConsoleColor.Gray));


            //level exp
            positionInLine = (this.DrawRegion.Width / 2) - ((4 + Player.Level.ToString().Length + 6 + Player.EXP.ToString().Length + 1 + Player.mEXP.ToString().Length) / 2);
            WriteThis = "LVL: " + Player.Level.ToString() + " EXP: " + Player.EXP.ToString() + "/" + Player.mEXP.ToString();
            this.Write(5, positionInLine, new DrawText(WriteThis, ConsoleColor.DarkGray));

            //hp
            positionInLine = (this.DrawRegion.Width / 2) - ((7 + Player.CHP.ToString().Length + 1 + Player.MHP.ToString().Length) / 2);
            WriteThis = "Жизнь: " + Player.CHP.ToString() + "/" + Player.MHP.ToString();
            this.Write(7, positionInLine, new DrawText(WriteThis, ConsoleColor.Red));

            //mp
            if (Player.Class != MechEngine.BattleClass.Warrior)
            {
                positionInLine = (this.DrawRegion.Width / 2) - ((Player.ManaName.Length + Player.CMP.ToString().Length + 1 + Player.MMP.ToString().Length) / 2);
                WriteThis = Player.ManaName + Player.CMP.ToString() + "/" + Player.MMP.ToString();
            }
            else
            {
                positionInLine = (this.DrawRegion.Width / 2) - ((Player.ManaName.Length + Player.CMP.ToString().Length) / 2);
                WriteThis = Player.ManaName + Player.CMP.ToString();
            }
            this.Write(8, positionInLine, new DrawText(WriteThis, SystemEngine.Helper.Information.ClassC));

            //dmg
            positionInLine = (this.DrawRegion.Width / 2) - ((6 + Player.MIDMG.ToString().Length + 1 + Player.MADMG.ToString().Length) / 2);
            WriteThis = "Урон: " + Player.MIDMG.ToString() + "-" + Player.MADMG.ToString();
            this.Write(10, positionInLine, new DrawText(WriteThis, ConsoleColor.DarkYellow));

            //ad
            positionInLine = (this.DrawRegion.Width / 2) - ((12 + Player.AD.ToString().Length) / 2);
            WriteThis = "Сила атаки: " + Player.AD.ToString();
            this.Write(11, positionInLine, new DrawText(WriteThis, ConsoleColor.DarkRed));

            //ap
            positionInLine = (this.DrawRegion.Width / 2) - ((12 + Player.AP.ToString().Length) / 2);
            WriteThis = "Сила магии: " + Player.AP.ToString();
            this.Write(12, positionInLine, new DrawText(WriteThis, ConsoleColor.DarkCyan));

            //arm
            positionInLine = (this.DrawRegion.Width / 2) - ((11 + Player.ARM.ToString().Length) / 2);
            WriteThis = "Защита Ф : " + Player.ARM.ToString();
            this.Write(13, positionInLine, new DrawText(WriteThis, ConsoleColor.DarkGreen));

            //mrs
            positionInLine = (this.DrawRegion.Width / 2) - ((11 + Player.MRS.ToString().Length) / 2);
            WriteThis = "Защита М : " + Player.MRS.ToString();
            this.Write(14, positionInLine, new DrawText(WriteThis, ConsoleColor.DarkMagenta));

            //money
            positionInLine = (this.DrawRegion.Width / 2) - ((3 + Player.Gold.ToString().Length) / 2);
            WriteThis = "$: " + Player.Gold.ToString();
            this.Write(16, positionInLine, new DrawText(WriteThis, ConsoleColor.Yellow));

            //inv label
            positionInLine = (this.DrawRegion.Width / 2) - (("Инвентарь:".Length) / 2);
            this.Write(18, positionInLine, new DrawText("Инвентарь:", ConsoleColor.DarkRed));

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
            

            positionInLine = (this.DrawRegion.Width / 2) - (("┌───┬───┬───┐".Length) / 2);

            this.Write(19, positionInLine, new DrawText("┌───┬───┬───┐", ConsoleColor.DarkRed));
            this.Write(20, positionInLine, new DrawText("│ " + "`" + M[0].Color.ToString() + "`" + M[0].Icon().ToString() + " │ " + "`" + M[1].Color.ToString() + "`" + M[1].Icon().ToString() + " │ " + "`" + M[2].Color.ToString() + "`" + M[2].Icon().ToString() + " │", ConsoleColor.DarkRed));
            this.Write(21, positionInLine, new DrawText("├───┼───┼───┤", ConsoleColor.DarkRed);
            this.Write(22, positionInLine, new DrawText("│ " + "`" + M[3].Color.ToString() + "`" + M[3].Icon().ToString() + " │ " + "`" + M[4].Color.ToString() + "`" + M[4].Icon().ToString() + " │ " + "`" + M[5].Color.ToString() + "`" + M[5].Icon().ToString() + " │", ConsoleColor.DarkRed));
            this.Write(23, positionInLine, new DrawText("└───┴───┴───┘", ConsoleColor.DarkRed));

            return this;
        }
    }
}
