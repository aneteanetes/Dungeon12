using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Merchant
{
    /// <summary>
    /// requires left window, charinfo border, member window
    /// </summary>
    public class MerchantGoodsDrawSession : DrawSession
    {
        public MerchantGoodsDrawSession()
        {
            this.DrawRegion = new Types.Rectangle
            {
                X = 24,
                Y = 4,
                Width = 47,
                Height = 22
            };
        }

        public int Position { get; set; }

        public int Index { get; set; }

        public int Selected { get; set; }

        public IDrawable Item { get; set; }

        public override IDrawSession Run()
        {
            int left = 0;
            switch (Position)
            {
                case 1: { Position = 4; left = 31; break; }
                case 2: { Position = 9; left = 31; break; }
                case 3: { Position = 14; left = 31; break; }
                case 4: { Position = 19; left = 31; break; }
                case 5: { Position = 4; left = 48; break; }
                case 6: { Position = 9; left = 48; break; }
                case 7: { Position = 14; left = 48; break; }
                case 8: { Position = 19; left = 48; break; }
                case 9: { Position = 4; left = 62; break; }
                case 10: { Position = 9; left = 62; break; }
                case 11: { Position = 14; left = 62; break; }
                case 12: { Position = 19; left = 62; break; }
            }
            
            this.Write(Position, left, "┌───┐ ", ConsoleColor.Gray);
            this.Write(Position+1, left, "│   │ ", ConsoleColor.Gray);
            this.Write(Position + 1, left + 2, Item.Icon, Item.ForegroundColor);
            this.Write(Position + 2, left, "└───┘", ConsoleColor.Gray);

            //name of item
            var itemName = Item.Name;

            if (Item.Name.Length > 7)
            {
                itemName = Item.Name.Substring(0, 7);
            }

            this.Write(Position + 3, left, itemName, Item.ForegroundColor);
            this.Write(Position + 4, left, "$:" + Item.Price, ConsoleColor.Yellow);

            return base.Run();
        }

        private void Selection()
        {
            //#region Vertical

            //int vertical = 0;
            //if (index == 1 || index == 5 || index == 9) { vertical = 4; }
            //if (index == 2 || index == 6 || index == 10) { vertical = 9; }
            //if (index == 3 || index == 7 || index == 11) { vertical = 14; }
            //if (index == 4 || index == 8 || index == 12) { vertical = 19; }

            //#endregion

            //#region Horizontal

            //int horizontal = 0;
            //if (index == 1 || index == 2 || index == 3 || index == 4) { horizontal = 31; }
            //if (index == 5 || index == 6 || index == 7 || index == 8) { horizontal = 48; }
            //if (index == 9 || index == 10 || index == 11 || index == 12) { horizontal = 62; }

            //#endregion

            //if (Bold) { Console.ForegroundColor = Rogue.RAM.CUIColor; } else { Console.ForegroundColor = ConsoleColor.Gray; }

            //Console.SetCursorPosition(horizontal, vertical);

            //if (Bold) { Console.Write("╔───╗ "); } else { Console.Write("┌───┐ "); }

            //Console.SetCursorPosition(horizontal, vertical + 1);
            //Console.Write("│");
            //Console.SetCursorPosition(horizontal + 4, vertical + 1);
            //Console.Write("│");
            //Console.SetCursorPosition(horizontal, vertical + 2);

            //if (Bold) { Console.Write("╚───╝"); } else { Console.Write("└───┘"); }

            //if (!ReDrawning) { if (Bold) { dasr(Rogue.RAM.MerchTab.NowTab, false); } }
        }
    }
}
