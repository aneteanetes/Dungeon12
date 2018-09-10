using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Combat
{
    public class CombatLogBorderDrawSession : DrawSession
    {
        public CombatLogBorderDrawSession()
        {
            this.DrawRegion = new Types.Rectangle
            {
                X=25,
                Y=0,
                Width=47,
                Height=26
            };
        }

        public override IDrawSession Run()
        {
            int height = 0;

            var color = ConsoleColor.DarkGreen;

            string stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
            stringBuffer = " " + DrawHelp.Border(true, 1) + stringBuffer.Remove(stringBuffer.Length - 1) + DrawHelp.Border(true, 2);
            this.Write(height, 25, stringBuffer, color);

            //тело                
            for (int i = 1; i < 24; i++)
            {
                stringBuffer = DrawHelp.FullLine(74, " ", 2);
                stringBuffer = " " + DrawHelp.Border(true, 3) + stringBuffer.Remove(stringBuffer.Length - 26) + DrawHelp.Border(true, 3);
                this.Write(i, 25, stringBuffer, color);
            }

            //носки 
            stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
            stringBuffer = " " + DrawHelp.Border(true, 5) + stringBuffer.Remove(stringBuffer.Length - 1) + DrawHelp.Border(true, 6);
            this.Write(25, 24, stringBuffer, color);

            //носки объявления 
            stringBuffer = string.Empty;
            stringBuffer = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
            stringBuffer = " " + DrawHelp.Border(true, 8) + stringBuffer.Remove(stringBuffer.Length - 1) + DrawHelp.Border(true, 7);
            this.Write(25, 2, stringBuffer, color);

            return base.Run();
        }
    }
}
