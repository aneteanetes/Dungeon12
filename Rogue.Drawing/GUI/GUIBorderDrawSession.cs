namespace Rogue.Drawing.GUI
{
    using System;
    using Rogue.Drawing.Impl;
    using Rogue.Settings;
    using Rogue.View.Interfaces;

    public class GUIBorderDrawSession : DrawSession
    {
        private DrawingSize DrawingSize = new DrawingSize();

        public GUIBorderDrawSession()
        {
            this.DrawRegion = new Types.Rectangle
            {
                X = 1,
                Y = 1,
                Height = DrawingSize.WindowLines-1,
                Width = DrawingSize.WindowChars-1
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
            stringBuffer = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 26);
            stringBuffer = DrawHelp.Border(true, 1) + stringBuffer.Remove(stringBuffer.Length - 2) + DrawHelp.Border(true, 2);
            this.Write(height, 0, new DrawText(stringBuffer, ConsoleColor.DarkGreen));
      
             
            for (int i = 1; i < 23; i++)
            {
                stringBuffer = DrawHelp.FullLine(100, " ", 2);
                stringBuffer = DrawHelp.Border(true, 3) + stringBuffer.Remove(stringBuffer.Length - 26) + DrawHelp.Border(true, 3);                
                this.Write(i, 0, new DrawText(stringBuffer, ConsoleColor.DarkGreen));
            }

            stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 26);
            stringBuffer = DrawHelp.Border(true, 5) + stringBuffer.Remove(stringBuffer.Length - 2) + DrawHelp.Border(true, 6);
            this.Write(23, 0, new DrawText(stringBuffer, ConsoleColor.DarkGreen));
        }

        private void CharacterBorder()
        {
            int height = 23;
            int startCharacterBorder = 74;
            int charaterWidth = 25;

            //up ========
            var stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(charaterWidth, DrawHelp.Border(true, 4));
            stringBuffer = DrawHelp.Border(true, 1) + stringBuffer.Remove(stringBuffer.Length - 2) + DrawHelp.Border(true, 2);
            this.Write(0, startCharacterBorder, new DrawText(stringBuffer, ConsoleColor.DarkGreen));

            //body |    |
            for (int i = 1; i < height; i++)
            {
                stringBuffer = DrawHelp.FullLine(charaterWidth, " ");
                stringBuffer = DrawHelp.Border(true, 3) + stringBuffer.Remove(stringBuffer.Length - 2) + DrawHelp.Border(true, 3);

                this.Write(i, startCharacterBorder, new DrawText(stringBuffer, ConsoleColor.DarkGreen));
            }

            //down ======
            stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(charaterWidth, DrawHelp.Border(true, 4));
            stringBuffer = DrawHelp.Border(true, 5) + stringBuffer.Remove(stringBuffer.Length - 2) + DrawHelp.Border(true, 6);
            this.Write(height, startCharacterBorder, new DrawText(stringBuffer, ConsoleColor.DarkGreen));
        }

        private void InfoBorder()
        {
            //info window
            var height = 24;

            string stringBuffer = string.Empty;

            stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 2);
            stringBuffer = DrawHelp.Border(true, 1) + stringBuffer.Remove(stringBuffer.Length - 2) + DrawHelp.Border(true, 2);
            this.Write(height, 0, new DrawText(stringBuffer, ConsoleColor.DarkGreen));

            //тело                
            stringBuffer = DrawHelp.FullLine(100, " ", 2);
            stringBuffer = DrawHelp.Border(true, 3) + stringBuffer.Remove(stringBuffer.Length - 2) + DrawHelp.Border(true, 3);
            this.Write(height + 1, 0, new DrawText(stringBuffer, ConsoleColor.DarkGreen));

            //носки 
            stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(100, DrawHelp.Border(true, 4), 2);
            stringBuffer = DrawHelp.Border(true, 5) + stringBuffer.Remove(stringBuffer.Length - 2) + DrawHelp.Border(true, 6);
            this.Write(height + 2, 0, new DrawText(stringBuffer, ConsoleColor.DarkGreen));
        }
    }
}