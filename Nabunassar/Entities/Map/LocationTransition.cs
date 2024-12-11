using Dungeon.Types;

namespace Nabunassar.Entities.Map
{
    internal class LocationTransition
    {
        public Location From { get; set; }

        public Location To { get; set; }

        public string Name { get; set; }

        public Direction Direction { get; set; }

        public TransitionState State { get; set; }
    }
}
