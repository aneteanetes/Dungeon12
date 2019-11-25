using Dungeon.Drawing;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon
{
    public static class DrawTextExtensions
    {
        public static DrawText AsDrawText(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                value = " ";
            return new DrawText(value);
        }

        public static DrawText InColor(this DrawText drawText, DrawColor drawColor)
        {
            drawText.ForegroundColor = drawColor;
            return drawText;
        }

        public static DrawText InColor(this DrawText drawText, IDrawColor drawColor)
        {
            drawText.ForegroundColor = drawColor;
            return drawText;
        }

        public static DrawText InColor(this DrawText drawText, ConsoleColor consoleColor)
        {
            drawText.ForegroundColor = new DrawColor(consoleColor);
            return drawText;
        }

        public static DrawText InSize(this DrawText drawText, long size=12)
        {
            drawText.Size = size;
            return drawText;
        }

        public static DrawText WithWordWrap(this DrawText drawText)
        {
            drawText.WordWrap = true;
            return drawText;
        }
    }
}
