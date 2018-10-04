using System;
using Rogue.Drawing.Impl;
using Rogue.Entites.Alive.Character;
using Rogue.Entites.Items;
using Rogue.Items;
using Rogue.Items.Types;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Data
{
    public class CharacterDataDrawSession : DrawSession
    {
        public CharacterDataDrawSession()
        {
            this.DrawRegion = new Types.Rectangle
            {
                X = 76,
                Y = 1,
                Width = 22,
                Height = 23
            };
        }

        public Player Player { get; set; }

        public HotPanel HotPanel { get; set; }

        public override IDrawSession Run()
        {
            //name
            int positionInLine = (this.DrawRegion.Width / 2) - (Player.Name.Length / 2);
            this.Write(1, positionInLine, new DrawText(Player.Name, ConsoleColor.Cyan));

            //race class
            string WriteThis = $"{Player.Race.ToDisplay()} - {Player.ClassName}";
            positionInLine = (this.DrawRegion.Width / 2) - (WriteThis.Length / 2);
            this.Write(3, positionInLine, new DrawText(WriteThis, ConsoleColor.Gray));


            //level exp
            positionInLine = (this.DrawRegion.Width / 2) - ((4 + Player.Level.ToString().Length + 6 + Player.EXP.ToString().Length + 1 + Player.MaxExp.ToString().Length) / 2);
            WriteThis = "LVL: " + Player.Level.ToString() + " EXP: " + Player.EXP.ToString() + "/" + Player.MaxExp.ToString();
            this.Write(5, positionInLine, new DrawText(WriteThis, ConsoleColor.DarkGray));

            //hp
            positionInLine = (this.DrawRegion.Width / 2) - ((7 + Player.MaxHitPoints.ToString().Length + 1 + Player.MaxHitPoints.ToString().Length) / 2);
            WriteThis = "Жизнь: " + Player.MaxHitPoints.ToString() + "/" + Player.MaxHitPoints.ToString();
            this.Write(7, positionInLine, new DrawText(WriteThis, ConsoleColor.Red));

            //mp
            WriteThis = $"{Player.ResourceName}: {Player.Resource}";
            positionInLine = (this.DrawRegion.Width / 2) - ((WriteThis.Length) / 2);
            this.Write(8, positionInLine, new DrawText(WriteThis, Player.ResourceColor));

            //dmg
            positionInLine = (this.DrawRegion.Width / 2) - ((6 + Player.MinDMG.ToString().Length + 1 + Player.MaxDMG.ToString().Length) / 2);
            WriteThis = "Урон: " + Player.MinDMG.ToString() + "-" + Player.MaxDMG.ToString();
            this.Write(10, positionInLine, new DrawText(WriteThis, ConsoleColor.DarkYellow));

            //ad
            positionInLine = (this.DrawRegion.Width / 2) - ((12 + Player.AttackPower.ToString().Length) / 2);
            WriteThis = "Сила атаки: " + Player.AttackPower.ToString();
            this.Write(11, positionInLine, new DrawText(WriteThis, ConsoleColor.DarkRed));

            //ap
            positionInLine = (this.DrawRegion.Width / 2) - ((12 + Player.AbilityPower.ToString().Length) / 2);
            WriteThis = "Сила магии: " + Player.AbilityPower.ToString();
            this.Write(12, positionInLine, new DrawText(WriteThis, ConsoleColor.DarkCyan));

            //arm
            positionInLine = (this.DrawRegion.Width / 2) - ((11 + Player.Defence.ToString().Length) / 2);
            WriteThis = "Защита Ф : " + Player.Defence.ToString();
            this.Write(13, positionInLine, new DrawText(WriteThis, ConsoleColor.DarkGreen));

            //mrs
            positionInLine = (this.DrawRegion.Width / 2) - ((11 + Player.Barrier.ToString().Length) / 2);
            WriteThis = "Защита М : " + Player.Barrier.ToString();
            this.Write(14, positionInLine, new DrawText(WriteThis, ConsoleColor.DarkMagenta));

            //money
            positionInLine = (this.DrawRegion.Width / 2) - ((3 + Player.Gold.ToString().Length) / 2);
            WriteThis = "$: " + Player.Gold.ToString();
            this.Write(16, positionInLine, new DrawText(WriteThis, ConsoleColor.Yellow));

            //inv label
            positionInLine = (this.DrawRegion.Width / 2) - (("Инвентарь:".Length) / 2);
            this.Write(18, positionInLine, new DrawText("Инвентарь:", ConsoleColor.DarkRed));

            //Item Empt = Item.Empty;
            //Item[] CI = new Item[]{
            //    new Rune { Icon="*", ForegroundColor=new DrawColor(ConsoleColor.Red)  },
            //    new Rune { Icon="*", ForegroundColor=new DrawColor(ConsoleColor.Blue)  },
            //    new Rune { Icon="*", ForegroundColor=new DrawColor(ConsoleColor.Green)  },
            //    new Rune { Icon="*", ForegroundColor=new DrawColor(ConsoleColor.Magenta)  },
            //    new Rune { Icon="*", ForegroundColor=new DrawColor(ConsoleColor.Cyan)  },
            //    new Rune { Icon="*", ForegroundColor=new DrawColor(ConsoleColor.White)  }
            //}; //Current items
            //for (int i = 0; i < 6; i++)
            //{
            //    try
            //    {
            //        string wolvowhat = CI[i].Name;
            //    }
            //    catch (IndexOutOfRangeException)
            //    {
            //        Array.Resize(ref CI, CI.Length + 1);
            //        CI[i] = Empt;
            //    }
            //}

            ////вещи
            //Item[] M = new Item[]{
            //    new Rune { Icon="*", ForegroundColor=new DrawColor(ConsoleColor.Red)  },
            //    new Rune { Icon="*", ForegroundColor=new DrawColor(ConsoleColor.Blue)  },
            //    new Rune { Icon="*", ForegroundColor=new DrawColor(ConsoleColor.Green)  },
            //    new Rune { Icon="*", ForegroundColor=new DrawColor(ConsoleColor.Magenta)  },
            //    new Rune { Icon="*", ForegroundColor=new DrawColor(ConsoleColor.Cyan)  },
            //    new Rune { Icon="*", ForegroundColor=new DrawColor(ConsoleColor.White)  }
            //};
            ////HotPanel.ToArray();
            //Item N = Item.Empty;

            //for (int i = 0; i < 6; i++)
            //{
            //    try
            //    {
            //        M[i].ToString();
            //    }
            //    catch (IndexOutOfRangeException)
            //    {
            //        Array.Resize(ref M, M.Length + 1);
            //        M[i] = N;
            //    }
            //}


            //positionInLine = (this.DrawRegion.Width / 2) - (("┌───┬───┬───┐".Length) / 2);

            //this.Write(19, positionInLine, new DrawText("┌───┬───┬───┐", ConsoleColor.DarkRed));
            //this.Write(20, positionInLine, new DrawText("│ " + M[0].Icon.ToString() + " │ " + M[1].Icon.ToString() + " │ " +  M[2].Icon.ToString() + " │", ConsoleColor.DarkRed));
            //this.Write(21, positionInLine, new DrawText("├───┼───┼───┤", ConsoleColor.DarkRed));
            //this.Write(22, positionInLine, new DrawText("│ " + M[3].Icon.ToString() + " │ " + M[4].Icon.ToString() + " │ " +  M[5].Icon.ToString() + " │", ConsoleColor.DarkRed));
            //this.Write(23, positionInLine, new DrawText("└───┴───┴───┘", ConsoleColor.DarkRed));

            return this;
        }
    }
}