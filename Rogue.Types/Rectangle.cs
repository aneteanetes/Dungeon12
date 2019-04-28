using System;

namespace Rogue.Types
{
    public class Rectangle
    {
        public Rectangle()
        {

        }
        
        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public double X { get; set; }

        public float Xf => (float)X;

        public double Y { get; set; }

        public float Yf => (float)Y;

        public double Width { get; set; }

        public double Height { get; set; }

        public float Heightf => (float)Height;

        public float Widthf => (float)Width;

        public float xMax => Xf+ Widthf;

        public float yMax => Yf+ Heightf;

        public bool Overlaps(Rectangle other)
        {
            var x1 = Math.Max(this.X, other.X);
            var x2 = Math.Min(this.X + this.Width, other.X + other.Width);
            var y1 = Math.Max(this.Y, other.Y);
            var y2 = Math.Min(this.Y + this.Height, other.Y + other.Height);

            if (x2 >= x1 && y2 >= y1)
            {
                return true;
            }

            return false;
        }

        public Point Pos
        {
            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        public bool Contains(double x, double y)
        {
            return ((x >= X) && (x < X+Width) &&
                (y >= Y) && (y < Y+Height));
        }
    }
}
