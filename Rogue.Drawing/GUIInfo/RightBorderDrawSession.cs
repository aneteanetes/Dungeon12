using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.GUIInfo
{
    public class RightBorderDrawSession : DrawSession
    {
        public RightBorderDrawSession()
        {
            this.DrawRegion = new Types.Rectangle
            {
                X = 0,
                Y = 0,
                Width = 25,
                Height = 24
            };
        }

        public override IDrawSession Run()
        {
            int height = 24;
            var color = ConsoleColor.DarkGreen;

            string stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(25, DrawHelp.Border(true, 4));
            stringBuffer = DrawHelp.Border(true, 1) + stringBuffer.Remove(stringBuffer.Length - 2) + DrawHelp.Border(true, 2);
            this.Write(0, 1, stringBuffer, color);

            //тело
            for (int i = 1; i < height; i++)
            {
                stringBuffer = DrawHelp.FullLine(25, " ");
                stringBuffer = DrawHelp.Border(true, 3) + stringBuffer.Remove(stringBuffer.Length - 2) + DrawHelp.Border(true, 3);
                this.Write(i, 1, stringBuffer, color);
            }

            //носки 
            stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(25, DrawHelp.Border(true, 4));
            stringBuffer = DrawHelp.Border(true, 5) + stringBuffer.Remove(stringBuffer.Length - 2) + DrawHelp.Border(true, 6);
            this.Write(height, 1, stringBuffer, color);

            return base.Run();
        }
    }
}
