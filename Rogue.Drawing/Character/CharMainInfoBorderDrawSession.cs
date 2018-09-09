using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Character
{
    public class CharMainInfoBorderDrawSession : DrawSession
    {
        public CharMainInfoBorderDrawSession()
        {
            this.DrawRegion = new Types.Rectangle
            {

            };
        }

        protected virtual string InLoop(string stringBuffer)
        {
            return " " + DrawHelp.Border(true, 3) + stringBuffer.Remove(stringBuffer.Length - 26) + DrawHelp.Border(true, 3);
        }

        public override IDrawSession Run()
        {
            int height = 0;

            ConsoleColor color = ConsoleColor.DarkGreen;

            string stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
            stringBuffer = " " + DrawHelp.Border(true, 1) + stringBuffer.Remove(stringBuffer.Length - 1) + DrawHelp.Border(true, 2);
            this.Write(height, 25, new DrawText(stringBuffer, color));

            //тело                
            for (int i = 1; i < 24; i++)
            {
                stringBuffer = DrawHelp.FullLine(74, " ", 2);
                stringBuffer = InLoop(stringBuffer);

                this.Write(i, 25, new DrawText(stringBuffer, color));
            }

            //носки 
            stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
            stringBuffer = " " + DrawHelp.Border(true, 5) + stringBuffer.Remove(stringBuffer.Length - 1) + DrawHelp.Border(true, 6);

            this.Write(24, 25, new DrawText(stringBuffer, color));

            //носки объявления 
            stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
            stringBuffer = " " + DrawHelp.Border(true, 8) + stringBuffer.Remove(stringBuffer.Length - 1) + DrawHelp.Border(true, 7);

            this.Write(25, 2, new DrawText(stringBuffer, color));

            return base.Run();
        }
    }
}
