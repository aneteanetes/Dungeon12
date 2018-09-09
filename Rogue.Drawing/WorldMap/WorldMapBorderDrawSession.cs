using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Drawing.Impl;
using Rogue.Settings;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.WorldMap
{
    public class WorldMapBorderDrawSession : DrawSession
    {
        public DrawingSize DrawingSize { get; set; }
                
        public WorldMapBorderDrawSession()
        {
            this.DrawRegion = new Types.Rectangle
            {
                X = 0,
                Y = 0,
                Width = DrawingSize.MapChars,
                Height = DrawingSize.MapLines
            };
        }

        public override IDrawSession Run()
        {
            int height = 0;
            
            string stringbuffer = string.Empty;
            stringbuffer = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 27);
            stringbuffer = DrawHelp.Border(true, 1) + stringbuffer.Remove(stringbuffer.Length - 2) + DrawHelp.Border(true, 2);
            this.Write(height, 1, new DrawText(stringbuffer, ConsoleColor.DarkGreen));

            //тело                
            for (int i = 1; i < 24; i++)
            {
                stringbuffer = DrawHelp.FullLine(100, " ", 2);
                stringbuffer = DrawHelp.Border(true, 3) + stringbuffer.Remove(stringbuffer.Length - 27) + DrawHelp.Border(true, 3);
                this.Write(i, 1, new DrawText(stringbuffer, ConsoleColor.DarkGreen));
            }

            //носки 
            stringbuffer = string.Empty;
            stringbuffer = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 27);
            stringbuffer = DrawHelp.Border(true, 5) + stringbuffer.Remove(stringbuffer.Length - 2) + DrawHelp.Border(true, 6);
            this.Write(24, 1, new DrawText(stringbuffer, ConsoleColor.DarkGreen));


            //header 
            stringbuffer = string.Empty;
            stringbuffer = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 27);
            stringbuffer = "╠" + stringbuffer.Remove(stringbuffer.Length - 2) + "╣";

            this.Write(2, 1, new DrawText(stringbuffer, ConsoleColor.DarkGreen));

            int c = (75 / 2) - ("Карта мира".Length / 2);
            this.Write(1, 2+c, new DrawText(stringbuffer, ConsoleColor.DarkGreen));

            
            return this;
        }
    }
}
