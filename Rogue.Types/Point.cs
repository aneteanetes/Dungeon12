namespace Rogue.Types
{
    public class Point
    {
        public Point()
        {

        }

        public Point(Point fromCopy)
        {
            this.X = fromCopy.X;
            this.Y = fromCopy.Y;
        }

        public Point(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public float Xf => (float)X;

        public float Yf => (float)Y;
    }
}