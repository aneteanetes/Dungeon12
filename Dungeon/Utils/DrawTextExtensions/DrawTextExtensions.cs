using Dungeon.Drawing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon
{
    public static class DrawTextExtensions
    {
        public static DrawText AsDrawText(this string value)
        {
            return new DrawText(value);
        }

        public static DrawText InColor(this DrawText drawText, DrawColor drawColor)
        {
            drawText.ForegroundColor = drawColor;
            return drawText;
        }

        public static DrawText InSize(this DrawText drawText, long size=12)
        {
            drawText.Size = 12;
            return drawText;
        }
    }
}
