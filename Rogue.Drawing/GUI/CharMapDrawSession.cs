using System;
using System.Collections.Generic;
using Rogue.Drawing.Impl;
using Rogue.Settings;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.GUI
{
    public class CharMapDrawSession : DrawSession
    {
        public DrawingSize DrawingSize { get; set; }

        public CharMapDrawSession()
        {
            this.DrawRegion = new Types.Rectangle
            {
                X = 0,
                Y = 28,
                Height = 5,
                Width = DrawingSize.WindowChars
            };
        }

        public List<string> Commands { get; set; } = new List<string>();

        public override IDrawSession Run()
        {
            var color = ConsoleColor.DarkGreen;

            string stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 3);
            stringBuffer = " " + DrawHelp.Border(true, 1) + "══════════════════════" + DrawHelp.Border(true, 0, "ThreeBot") + "══════════════════════" + DrawHelp.Border(true, 0, "ThreeBot") + "══════════════════════" + DrawHelp.Border(true, 0, "ThreeBot") + "═══════════════════════════" + DrawHelp.Border(true, 2);
            this.Write(28, 0, new DrawText(stringBuffer, color));

            //тело                
            for (int i = 29; i < 32; i++)
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

            this.Write(32, 0, new DrawText(stringBuffer, color));

            stringBuffer = " " + DrawHelp.Border(true, 8) + "══════════════════════" + DrawHelp.Border(true, 0, "Cross") + "══════════════════════" + DrawHelp.Border(true, 0, "Cross") + "══════════════════════" + DrawHelp.Border(true, 0, "Cross") + "═══════════════════════════" + DrawHelp.Border(true, 7);
            this.Write(30, 0, new DrawText(stringBuffer, color));

            double columns = Math.Ceiling(Convert.ToDouble(this.Commands.Count) / 2);
            int index = 0;
            for (int i = 0; i < columns; i++)
            {
                try
                { if (this.Commands[index + 1] == null) { Commands.Add(""); } }
                catch { Commands.Add(""); }
                DrawColumn(i + 1, new List<string>() { Commands[index], Commands[index + 1] });
                if (index != 0) { index += 2; }
                else { index = 2; }
            }

            return this;
        }

        private void DrawColumn(int ColumnNumber, List<string> Strings)
        {
            int left = 0;
            if (ColumnNumber == 1) { left = 3; }
            else if (ColumnNumber == 2) { left = 26; }
            else if (ColumnNumber == 3) { left = 48; }
            else if (ColumnNumber == 4) { left = 71; }
            int i = 29;

            foreach (string s in Strings)
            {
                this.Write(i, left, new DrawText(s.Replace('&', ' '), ConsoleColor.DarkGreen));
                i += 2;
            }
        }
    }
}
