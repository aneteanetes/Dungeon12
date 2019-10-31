namespace Dungeon.Types
{
    public enum Direction
    {
        Idle=0,
        Up=1,
        Down=2,
        Left=3,
        Right=4
    }
}

namespace Rogue
{
    using Dungeon.Types;
    public static class DirectionExtensions
    {
        public static Direction Opposite(this Direction dir)
        {
            switch (dir)
            {
                case Direction.Up: return Direction.Down;
                case Direction.Down: return Direction.Up;
                case Direction.Left: return Direction.Right;
                case Direction.Right: return Direction.Left;
                default: return Direction.Idle;
            }
        }
    }
}