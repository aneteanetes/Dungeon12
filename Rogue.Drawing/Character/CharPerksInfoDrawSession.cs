using System;
using System.Diagnostics;
using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Character
{
    //maininfoborder
    public class CharPerksInfoDrawSession : DrawSession
    {
        public CharPerksInfoDrawSession()
        {
            this.DrawRegion = new Types.Rectangle
            {
                X = 25,
                Y = 3,
                Width = 50,
                Height = 12
            };
        }

        public int Tab { get; set; }

        public override IDrawSession Run()
        {
            this.WriteHeader("Особенности персонажа");
            this.WriteTab();


            int ctab = (Tab * 5) - 5;
            int i = 0;
            for (int q = ctab; q < Tab * 5; q++)
            {
                try
                {
                    MechEngine.Perk p = Rogue.RAM.Player.Perks[q];

                    this.Write(3 + i, 28, new DrawText("┌───┐ " + p.Name, ConsoleColor.Gray));
                    this.Write(4 + i, 28, new DrawText("│   │ " + p.History, ConsoleColor.Gray));
                    this.Write(4 + i, 30, new DrawText(p.Icon, p.Color));

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
                    this.Write(5 + i, 28, new DrawText("└───┘ " + stats, ConsoleColor.Gray));

                    //
                    stringBuffer = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
                    stringBuffer = " " + DrawHelp.Border(true, 8) + stringBuffer.Remove(stringBuffer.Length - 1) + DrawHelp.Border(true, 7);

                    this.Write(6 + i, 25, new DrawText(stringBuffer, ConsoleColor.DarkGreen));

                    i = i + 4;
                }
                catch (ArgumentOutOfRangeException)
                {
                    //ниуя не пофигу
                    Debugger.Break();
                    //пофигу
                }
            }

            return base.Run();
        }

        protected string GetSign(int z)
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

        protected void WriteTab()
        {
            string stringBuffer = string.Empty;

            Rogue.RAM.iTab.NowTab = Tab;
            Rogue.RAM.iTab.MaxTab = (Rogue.RAM.Player.Perks.Count / 5) + 1;

            if (Rogue.RAM.Player.Perks.Count > 5)
            {
                stringBuffer = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
                stringBuffer = " " + DrawHelp.Border(true, 8) + stringBuffer.Remove(stringBuffer.Length - 1) + DrawHelp.Border(true, 7);
                this.Write(22, 25, new DrawText(stringBuffer, ConsoleColor.DarkGreen));
                
                if (Rogue.RAM.iTab.NowTab >= Rogue.RAM.iTab.MaxTab && Rogue.RAM.iTab.NowTab != 1)
                {
                    this.Write(23, 27, new DrawText(" <=", ConsoleColor.DarkGreen));
                }

                if (Rogue.RAM.iTab.NowTab < Rogue.RAM.iTab.MaxTab)
                {
                    this.Write(23, 70, new DrawText("=> ", ConsoleColor.DarkGreen));
                }
            }
        }
    }
}
