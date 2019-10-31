namespace Dungeon
{
    using Dungeon.Classes;
    using Dungeon.Items;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;

    public class MagicFindEquip : Equipment
    {
        public MagicFindEquip()
        {
            Color = new DrawColor(ConsoleColor.Blue);
        }

        public override string Title => $"+10% Шанс найти магический предмет";

        public void Apply(Character character)
        {

        }

        public void Discard(Character character)
        {

        }

        protected override void CallApply(dynamic obj) => this.Apply(obj);
        protected override void CallDiscard(dynamic obj)=> this.Discard(obj);

        public class DrawColor : IDrawColor
        {
            public DrawColor(ConsoleColor consoleColor)
            {
                var rgba = ConsoleMap[consoleColor];
                this.R = rgba.R;
                this.G = rgba.G;
                this.B = rgba.B;
                this.A = rgba.A;
            }


            public DrawColor(byte r, byte g, byte b)
            {
                this.R = r;
                this.G = g;
                this.B = b;
                this.A = 255;
            }

            public DrawColor(byte r, byte g, byte b, byte a)
            {
                this.R = r;
                this.G = g;
                this.B = b;
                this.A = a;
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
}