using Dungeon.GameObjects;
using Dungeon.SceneObjects.Grouping;
using Dungeon12.Entities.Objects;

namespace Dungeon12.Entities.Turning
{
    internal class Turn : GameComponent
    {
        public Turn(GameObject @object)
        {
            Object=@object;
        }

        public bool Invisible { get; set; }

        public int Initiative { get; set; }

        public GameObject Object { get; private set; }

        public ObjectGroupProperty IsActive { get; set; } = new ObjectGroupProperty();

        public static implicit operator Turn(GameObject @object) => new Turn(@object);

        public override string ToString()
        {
            return $"{Object?.GlobalId}:{Object?.Name}";
        }
    }
}
