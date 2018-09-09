using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Character
{
    public class CharAbilityInfoSelectionDrawSession : DrawSession
    {
        //TODO: оттестить rectangle, потому что не понимая как он работает хер знает как это реализовать

        public override IDrawSession Run()
        {
            //switch (Dir)
            //{
            //    case SystemEngine.ArrowDirection.Top: { if (Rogue.RAM.iTab.NowTab != 1 && Rogue.RAM.iTab.NowTab != 5) { dasr(Rogue.RAM.iTab.NowTab - 1); Rogue.RAM.iTab.NowTab -= 1; } break; }
            //    case SystemEngine.ArrowDirection.Bot: { if (Rogue.RAM.iTab.NowTab != 4 && Rogue.RAM.iTab.NowTab != 8) { dasr(Rogue.RAM.iTab.NowTab + 1); Rogue.RAM.iTab.NowTab += 1; } break; }
            //    case SystemEngine.ArrowDirection.Left:
            //        {
            //            if (Rogue.RAM.iTab.NowTab != 1 && Rogue.RAM.iTab.NowTab != 2 && Rogue.RAM.iTab.NowTab != 3 && Rogue.RAM.iTab.NowTab != 4) { dasr(Rogue.RAM.iTab.NowTab - 4); Rogue.RAM.iTab.NowTab -= 4; }
            //            break;
            //        }
            //    case SystemEngine.ArrowDirection.Right:
            //        {
            //            if (Rogue.RAM.iTab.NowTab != 5 && Rogue.RAM.iTab.NowTab != 6 && Rogue.RAM.iTab.NowTab != 7 && Rogue.RAM.iTab.NowTab != 8) { dasr(Rogue.RAM.iTab.NowTab + 4); Rogue.RAM.iTab.NowTab += 4; }
            //            break;
            //        }
            //}

            return base.Run();
        }

        //private static void dasr(int index, bool Bold = true, bool ReDrawning = false)
        //{
        //    int side = 0;
        //    int vertical = 0;
        //    if (index == 1 || index == 2 || index == 3 || index == 4) { side = 28; } else { side = 52; }
        //    if (index == 1 || index == 5) { vertical = 5; }
        //    if (index == 2 || index == 6) { vertical = 10; }
        //    if (index == 3 || index == 7) { vertical = 15; }
        //    if (index == 4 || index == 8) { vertical = 20; }

        //    if (Bold) { Console.ForegroundColor = Rogue.RAM.CUIColor; } else { Console.ForegroundColor = ConsoleColor.Gray; }
        //    Console.SetCursorPosition(side, vertical);
        //    if (Bold) { Console.Write("╔───╗ "); } else { Console.Write("┌───┐ "); }
        //    Console.SetCursorPosition(side, vertical + 1);
        //    Console.Write("│");
        //    Console.SetCursorPosition(side + 4, vertical + 1);
        //    Console.Write("│");
        //    Console.SetCursorPosition(side, vertical + 2);
        //    if (Bold) { Console.Write("╚───╝"); } else { Console.Write("└───┘"); }
        //    if (!ReDrawning) { if (Bold) { dasr(Rogue.RAM.iTab.NowTab, false); } }
        //}
    }
}
