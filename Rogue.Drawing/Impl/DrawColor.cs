using System;
using System.Collections.Generic;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Impl
{
    public class DrawColor : IDrawColor
    {
        public DrawColor(ConsoleColor consoleColor)
        {
            var rgba = this.ConsoleMap[consoleColor];
            this.R = rgba.R;
            this.G = rgba.G;
            this.B = rgba.B;
            this.A = rgba.A;
        }

        public DrawColor(int r, int g, int b, int a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        public int R { get; }

        public int G { get; }

        public int B { get; }

        public int A { get; }

        public static implicit operator DrawColor(ConsoleColor consoleColor) => new DrawColor(consoleColor);

        private Dictionary<ConsoleColor, (int R, int G, int B, int A)> ConsoleMap => new Dictionary<ConsoleColor, (int R, int G, int B, int A)>()
        {
            { ConsoleColor.Black, (0,0,0,255) },
            { ConsoleColor.Blue, (0,0,255,255) },
            { ConsoleColor.Cyan, (0,255,255,255) },
            { ConsoleColor.DarkBlue, (0,0,128,255) },
            { ConsoleColor.DarkCyan, (0,128,128,255) },
            { ConsoleColor.DarkGray, (128,128,0,255) },
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
    }
}