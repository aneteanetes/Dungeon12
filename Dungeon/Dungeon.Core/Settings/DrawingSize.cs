using System;

namespace Dungeon.Settings
{
    public class DrawingSize
    {
        public const int Lines = 29;

        public const int Chars = 48;

        public int WindowLines { get; set; } = 29;

        public int WindowChars { get; set; } = 48;
        
        public int MapChars { get; set; } = 35;

        public int MapLines { get; set; } = 19;

        public static int Cell { get; set; } = 32;

        public static float CellF { get; set; } = 32;
    }
}
