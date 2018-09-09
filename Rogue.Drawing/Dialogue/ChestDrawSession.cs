using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Drawing.GUIInfo;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Dialogue
{
    public class ChestDrawSession : RightInfoDrawSession
    {
        public int Index { get; set; }

        public List<IDrawable>  Items { get; set; }

        protected override void Draw()
        {
            string stringBuffer = "Хранилище";            
            this.WriteStatFull(stringBuffer, 1, ConsoleColor.Yellow);

            int top = 2;
            int limit = "                  ".Length;

            for (int i = 0; i < 7; i++)
            {
                bool bold = false;
                ConsoleColor col = ConsoleColor.Gray;
                if (Index == i)
                {
                    bold = true;
                    col = ConsoleColor.DarkGreen;
                }

                top++;
                this.WriteStatFull(DrawHelp.Border(bold, 0, "TopCornLeft") + "──────────────────" + DrawHelp.Border(bold, 0, "TopCornRight"), top, col);

                top++;
                this.WriteStatFull("│                  │", top, col);

                try
                {
                    var itm = this.Items[i];

                    string itemname = string.Empty;

                    if (itm.Name.Length > limit)
                    {
                        itemname = itm.Name.Substring(0, limit);
                    }
                    else
                    {
                        itemname = itm.Name;
                    }

                    this.WriteStatFull(itemname, top, itm.ForegroundColor);
                }
                catch (ArgumentOutOfRangeException) { }

                top++;
                this.WriteStatFull(DrawHelp.Border(bold, 0, "BotCornLeft") + "──────────────────" + DrawHelp.Border(bold, 0, "BotCornRight"),top, col);
                bold = false;
            }
        }
    }
}
