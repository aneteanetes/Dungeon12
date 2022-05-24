using Dungeon12.Entities.Talks;

namespace Dungeon12.Entities
{
    internal class Fame
    {
        public int Good { get; set; }

        public int Evil { get; set; }

        public int Trick { get; set; }

        public int Wisdom { get; set; }

        public void Add(int value, ReplicaType type)
        {
            switch (type)
            {
                case ReplicaType.Good:
                    Good += value;
                    break;
                case ReplicaType.Evil:
                    Evil += value;
                    break;
                case ReplicaType.Trick:
                    Trick += value;
                    break;
                case ReplicaType.Wisdom:
                    Wisdom += value;
                    break;
                default:
                    break;
            }
        }
    }
}
