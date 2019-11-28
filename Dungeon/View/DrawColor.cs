using System;
using System.Collections.Generic;
using Dungeon.View.Interfaces;

namespace Dungeon.Drawing
{
    public partial class DrawColor : IDrawColor
    {
        public DrawColor() { }

        public DrawColor(ConsoleColor consoleColor)
        {
            var rgba = ConsoleMap[consoleColor];
            R = rgba.R;
            G = rgba.G;
            B = rgba.B;
            A = rgba.A;
        }


        public DrawColor(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
            A = 255;
        }

        public DrawColor(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public byte R { get; set; }

        public byte G { get; set; }

        public byte B { get; set; }

        public byte A { get; set; }

        public static implicit operator DrawColor(ConsoleColor consoleColor) => new DrawColor(consoleColor);

        private static Dictionary<ConsoleColor, (byte R, byte G, byte B, byte A)> ConsoleMap => new Dictionary<ConsoleColor, (byte R, byte G, byte B, byte A)>()
        {
            { ConsoleColor.Black, (0,0,0,255) },
            { ConsoleColor.Blue, (0,0,255,255) },
            { ConsoleColor.Cyan, (0,255,255,255) },
            { ConsoleColor.DarkBlue, (0,0,128,255) },
            { ConsoleColor.DarkCyan, (0,128,128,255) },
            { ConsoleColor.DarkGray, (169,169,169,255) },
            { ConsoleColor.DarkGreen, (0,128,0,255) },
            { ConsoleColor.DarkMagenta, (128,0,128,255) },
            { ConsoleColor.DarkRed, (128,0,0,255) },
            { ConsoleColor.DarkYellow, (128,128,0,255) },
            { ConsoleColor.Gray, (128,128,128,255) },
            { ConsoleColor.Green, (0,255,0,255) },
            { ConsoleColor.Magenta, (255,0,255,255) },
            { ConsoleColor.Red, (255,0,0,255) },
            { ConsoleColor.White, (255,255,255,255) },
            { ConsoleColor.Yellow, (255,255,0,255) },
        };

        public double Opacity { get; set; }
    }
}