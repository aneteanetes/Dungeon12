using Dungeon.Types;

namespace Dungeon.Physics
{
    public class PhysicalPosition
    {
        public double X { get; set; }

        public double Y { get; set; }

        public Point ToPoint() => new Point(X, Y);
    }
}