using Dungeon.Drawing;
using System.Drawing;

namespace Dungeon12.Entities.Talks
{
    public enum ReplicaType
    {
        Good = 1,
        Evil = 2,
        Trick = 3,
        Wisdom = 4
    }

    public static class ReplicaTypeColor
    {
        public static DrawColor TypeColor(this ReplicaType type)
        {
            switch (type)
            {
                case ReplicaType.Good: return new DrawColor(54, 185, 132);
                case ReplicaType.Evil: return new DrawColor(201, 3, 35);
                case ReplicaType.Trick: return new DrawColor(167, 42, 129);
                case ReplicaType.Wisdom: return new DrawColor(3, 132, 172);
                default: return DrawColor.White;
            }
        }
    }
}