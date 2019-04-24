namespace Rogue.Types
{
    public class Rectangle
    {
        public double X { get; set; }

        public float Xf => (float)X;

        public double Y { get; set; }

        public float Yf => (float)X;

        public double Width { get; set; }

        public double Height { get; set; }

        public float Heightf => (float)Height;

        public float Widthf => (float)Width;

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
