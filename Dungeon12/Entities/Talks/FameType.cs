using Dungeon.Drawing;
using System.Drawing;

namespace Dungeon12.Entities.Talks
{
    public enum FameType
    {
        Good = 1,
        Evil = 2,
        Trick = 3,
        Wisdom = 4
    }

    public static class FameTypeColor
    {
        public static DrawColor TypeColor(this FameType type)
        {
            switch (type)
            {
                case FameType.Good: return new DrawColor(54, 185, 132);
                case FameType.Evil: return new DrawColor(201, 3, 35);
                case FameType.Trick: return new DrawColor(167, 42, 129);
                case FameType.Wisdom: return new DrawColor(3, 132, 172);
                default: return DrawColor.White;
            }
        }
    }
}