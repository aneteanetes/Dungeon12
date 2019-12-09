namespace Dungeon12.Items
{
    using Dungeon.Drawing;
    using Dungeon.View.Interfaces;
    using System;

    public class ColorAttribute : Attribute
    {
        public IDrawColor DrawColor { get; set; }
        
        public ColorAttribute(byte r, byte g, byte b)
        {
            DrawColor = new InnerDrawColor
            {
                R = r,
                G = g,
                B = b,
                A = 255
            };
        }

        private class InnerDrawColor : IDrawColor
        {
            public byte R { get; set; }

            public byte G { get; set; }

            public byte B { get; set; }

            public byte A { get; set; }

            public double Opacity { get; set; }
        }
    }
}