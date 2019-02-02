namespace Rogue.Types
{
    public class Rectangle
    {
        public float X { get; set; }

        public float Y { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

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
