namespace Dungeon.Types
{
    public enum Direction
    {
        Idle = 0,

        Up = 1,
        Down = 20,
        Left = 300,
        Right = 4000,

        UpLeft = 301,
        UpRight = 4001,
        DownLeft = 320,
        DownRight = 4020,

        LeftUp = 301,
        RightUp = 4001,
        LeftDown = 320,
        RightDown = 4020
    }

    public enum Distance
    {
        Low = 0,
        Medium = 1,
        Hard = 1
    }

    public enum SimpleDirection
    {
        Up = 1,
        Down = 20,
        Left = 300,
        Right = 4000,
    }

    public enum Binary
    {
        Zero = 0,
        One = 1
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

                case Direction.UpLeft: return Direction.DownRight;
                case Direction.UpRight: return Direction.DownLeft;
                case Direction.DownLeft: return Direction.UpRight;
                case Direction.DownRight: return Direction.UpLeft;

                default: return Direction.Idle;
            }
        }
        public static string ToStringUpDown(this Direction dir)
        {
            if(dir== Direction.LeftDown)
                return "DownLeft";

            if (dir== Direction.LeftUp)
                return "UpLeft";

            if (dir== Direction.RightUp)
                return "UpRight";

            if (dir== Direction.RightDown)
                return "DownRight";

            return dir.ToString();
        }

        public static Direction OppositeX(this Direction dir)
        {
            switch (dir)
            {
                case Direction.Left: return Direction.Right;
                case Direction.Right: return Direction.Left;

                case Direction.Down: return Direction.Down;
                case Direction.Up: return Direction.Up;

                case Direction.UpLeft: return Direction.UpRight;
                case Direction.UpRight: return Direction.UpLeft;
                case Direction.DownLeft: return Direction.DownRight;
                case Direction.DownRight: return Direction.DownLeft;

                default: return Direction.Idle;
            }
        }

        public static bool IsDiagonal(this Direction dir)
        {
            switch (dir)
            {
                case Direction.UpLeft:
                case Direction.UpRight: 
                case Direction.DownLeft: 
                case Direction.DownRight:
                    return true;

                default: return false;
            }
        }

        public static Direction Rangom(this Direction dir)
        {
            switch (Random.Range(0,9))
            {
                case 1: return Direction.Up;
                case 2: return Direction.Down;
                case 3: return Direction.Left;
                case 4: return Direction.Right;
                case 5: return Direction.UpLeft;
                case 6: return Direction.UpRight;
                case 7: return Direction.DownLeft;
                case 8: return Direction.DownRight;
                default: return Direction.Idle;
            }
        }

        public static string ToStringX(this Direction direction) => direction switch
        {
            Direction.Up => nameof(Direction.Up),
            Direction.Down => nameof(Direction.Down),
            Direction.Left => nameof(Direction.Left),
            Direction.Right => nameof(Direction.Right),
            Direction.UpLeft => nameof(Direction.UpLeft),
            Direction.UpRight => nameof(Direction.UpRight),
            Direction.DownLeft => nameof(Direction.DownLeft),
            Direction.DownRight => nameof(Direction.DownRight),
            _ => nameof(Direction.Idle),
        };
    }
}