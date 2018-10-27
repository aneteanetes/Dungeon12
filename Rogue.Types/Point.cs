namespace Rogue.Types
{
    public class Point
    {
        public Point()
        {

        }

        public Point(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public float X { get; set; }

        public float Y { get; set; }
    }
}