namespace Dungeon.Types
{
    public enum Direction
    {
        Idle=0,

        Up=1,
        Down=20,
        Left=300,
        Right=4000,

        UpLeft=301,
        UpRight = 4001,
        DownLeft = 320,
        DownRight =4020,

        LeftUp = 301,
        RightUp = 4001,
        LeftDown = 320,
        RightDown = 4020,
    }
}

namespace Dungeon
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