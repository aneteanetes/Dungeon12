using Dungeon;
using Dungeon.Types;

namespace SidusXII.Models.Map
{
    public enum MapCellPart
    {
        /// <summary>
        /// Center
        /// </summary>
        C = 0,

        /// <summary>
        /// Left
        /// </summary>
        L = 1,

        /// <summary>
        /// Left Top
        /// </summary>
        LT = 3,

        /// <summary>
        /// Left Bottom
        /// </summary>
        LB = 4,

        /// <summary>
        /// Right
        /// </summary>
        R = 2,

        /// <summary>
        /// Right Top
        /// </summary>
        RT = 5,

        /// <summary>
        /// Right Bottom
        /// </summary>
        RB = 6
    }

    public static class MapCellPartExt
    {
        public static MapCellPart ToMapCell(this Direction dir, Point from, Point to)
        {
            switch (dir)
            {
                case Direction.Up:
                    if (from.Y % 2 == 0)
                        return MapCellPart.RT;
                    else
                        return MapCellPart.LT;
                case Direction.Down:
                    if (from.Y % 2 == 0)
                        return MapCellPart.RB;
                    else
                        return MapCellPart.LB;

                case Direction.Left: return MapCellPart.L;
                case Direction.LeftUp: return MapCellPart.LT;
                case Direction.LeftDown: return MapCellPart.LB;

                case Direction.Right: return MapCellPart.R;
                case Direction.RightUp: return MapCellPart.RT;
                case Direction.RightDown: return MapCellPart.RB;

                default: return MapCellPart.C;
            }
        }

        public static MapCellPart Opposite(this MapCellPart part)
        {
            switch (part)
            {
                case MapCellPart.L: return MapCellPart.R;
                case MapCellPart.LT: return MapCellPart.RB;
                case MapCellPart.LB: return MapCellPart.RT;
                case MapCellPart.R: return MapCellPart.L;
                case MapCellPart.RT: return MapCellPart.LB;
                case MapCellPart.RB: return MapCellPart.LT;
                default: return MapCellPart.C;
            }
        }
    }
}