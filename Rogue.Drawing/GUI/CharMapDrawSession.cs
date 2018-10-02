using System;
using System.Collections.Generic;
using System.Linq;
using Rogue.Drawing.Impl;
using Rogue.Settings;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.GUI
{
    public class CharMapDrawSession : DrawSession
    {
        private DrawingSize DrawingSize = new DrawingSize();

        public CharMapDrawSession()
        {
            this.DrawRegion = new Types.Rectangle
            {
                X = 0,
                Y = 29,
                Height = 5,
                Width = DrawingSize.WindowChars
            };
        }

        public IEnumerable<string> Commands { get; set; }

        public override IDrawSession Run()
        {
            var color = ConsoleColor.DarkGreen;

            string stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 3);
            stringBuffer = " " + DrawHelp.Border(true, 1) + "══════════════════════" + DrawHelp.Border(true, 0, "ThreeBot") + "══════════════════════" + DrawHelp.Border(true, 0, "ThreeBot") + "══════════════════════" + DrawHelp.Border(true, 0, "ThreeBot") + "═══════════════════════════" + DrawHelp.Border(true, 2);
            this.Write(0, 0, new DrawText(stringBuffer, color));

            //тело                
            for (int i = 1; i < 4; i++)
            {
                stringBuffer = DrawHelp.FullLine(100, " ", 3);
                stringBuffer = " " + DrawHelp.Border(true, 3) + stringBuffer.Remove(stringBuffer.Length - 1) + DrawHelp.Border(true, 3);
                if (i % 2 != 0) { stringBuffer = " ║                      ║                      ║                      ║                           ║"; }
                this.Write(i, 0, new DrawText(stringBuffer, color));
            }

            //носки 
            stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 3);
            stringBuffer = " " + DrawHelp.Border(true, 5) + "══════════════════════" + DrawHelp.Border(true, 0, "ThreeTop") + "══════════════════════" + DrawHelp.Border(true, 0, "ThreeTop") + "══════════════════════" + DrawHelp.Border(true, 0, "ThreeTop") + "═══════════════════════════" + DrawHelp.Border(true, 6);

            this.Write(4, 0, new DrawText(stringBuffer, color));

            stringBuffer = " " + DrawHelp.Border(true, 8) + "══════════════════════" + DrawHelp.Border(true, 0, "Cross") + "══════════════════════" + DrawHelp.Border(true, 0, "Cross") + "══════════════════════" + DrawHelp.Border(true, 0, "Cross") + "═══════════════════════════" + DrawHelp.Border(true, 7);
            this.Write(2, 0, new DrawText(stringBuffer, color));

            double columns = Math.Ceiling(Convert.ToDouble(this.Commands.Count()) / 2);

            var commands = new List<string>();

            int index = 1;
            int column = 1;

            foreach (var command in this.Commands)
            {
                int left = 0;
                if (column == 1) { left = 3; }
                else if (column == 2) { left = 26; }
                else if (column == 3) { left = 49; }
                else if (column == 4) { left = 72; }

                this.Write(index, left, new DrawText(command, ConsoleColor.DarkGreen));

                column++;
                if (column == 5)
                {
                    column = 1;
                    index += 2;
                }
            }

            return this;
        }
    }
}
