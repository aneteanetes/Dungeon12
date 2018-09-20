namespace Rogue.Drawing.GUI
{
    using System;
    using Rogue.Drawing.Impl;
    using Rogue.Settings;
    using Rogue.View.Interfaces;

    public class GUIBorderDrawSession : DrawSession
    {
        public DrawingSize DrawingSize { get; set; }

        public GUIBorderDrawSession()
        {
            this.DrawRegion = new Types.Rectangle
            {
                X = 0,
                Y = 0,
                Height = DrawingSize.WindowLines,
                Width = DrawingSize.WindowChars
            };
        }

        public override IDrawSession Run()
        {
            this.MapBorder();
            this.CharacterBorder();
            this.InfoBorder();

            return this;
        }
        
        private void MapBorder()
        {
            int height = 0;

            string stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 27);
            stringBuffer = DrawHelp.Border(true, 1) + stringBuffer.Remove(stringBuffer.Length - 2) + DrawHelp.Border(true, 2);
            this.Write(height, 1, new DrawText(stringBuffer, ConsoleColor.DarkGreen));
      
             
            for (int i = 1; i < 24; i++)
            {
                stringBuffer = DrawHelp.FullLine(100, " ", 2);
                stringBuffer = DrawHelp.Border(true, 3) + stringBuffer.Remove(stringBuffer.Length - 27) + DrawHelp.Border(true, 3);                
                this.Write(1, 1, new DrawText(stringBuffer, ConsoleColor.DarkGreen));
            }

            stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 27);
            stringBuffer = DrawHelp.Border(true, 5) + stringBuffer.Remove(stringBuffer.Length - 2) + DrawHelp.Border(true, 6);
            this.Write(24, 1, new DrawText(stringBuffer, ConsoleColor.DarkGreen));
        }

        private void CharacterBorder()
        {
            int height = 24;

            //up ========
            var stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(25, DrawHelp.Border(true, 4));
            stringBuffer = DrawHelp.Border(true, 1) + stringBuffer.Remove(stringBuffer.Length - 2) + DrawHelp.Border(true, 2);
            this.Write(0, 75, new DrawText(stringBuffer, ConsoleColor.DarkGreen));

            //body |    |
            for (int i = 1; i < height; i++)
            {
                stringBuffer = DrawHelp.FullLine(25, " ");
                stringBuffer = DrawHelp.Border(true, 3) + stringBuffer.Remove(stringBuffer.Length - 2) + DrawHelp.Border(true, 3);

                this.Write(i, 75, new DrawText(stringBuffer, ConsoleColor.DarkGreen));
            }

            //down ======
            stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(25, DrawHelp.Border(true, 4));
            stringBuffer = DrawHelp.Border(true, 5) + stringBuffer.Remove(stringBuffer.Length - 2) + DrawHelp.Border(true, 6);
            this.Write(height, 75, new DrawText(stringBuffer, ConsoleColor.DarkGreen));
        }

        private void InfoBorder()
        {
            //info window
            var height = 25;

            string stringBuffer = string.Empty;

            stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 2);
            stringBuffer = DrawHelp.Border(true, 1) + stringBuffer.Remove(stringBuffer.Length - 2) + DrawHelp.Border(true, 2);
            this.Write(height, 1, new DrawText(stringBuffer, ConsoleColor.DarkGreen));

            //тело                
            stringBuffer = DrawHelp.FullLine(100, " ", 2);
            stringBuffer = DrawHelp.Border(true, 3) + stringBuffer.Remove(stringBuffer.Length - 2) + DrawHelp.Border(true, 3);
            this.Write(height + 1, 1, new DrawText(stringBuffer, ConsoleColor.DarkGreen));

            //носки 
            stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 2);
            stringBuffer = DrawHelp.Border(true, 5) + stringBuffer.Remove(stringBuffer.Length - 2) + DrawHelp.Border(true, 6);
            this.Write(height + 2, 1, new DrawText(stringBuffer, ConsoleColor.DarkGreen));
        }
    }
}