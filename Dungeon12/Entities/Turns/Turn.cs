using Dungeon.GameObjects;
using Dungeon.SceneObjects.Grouping;
using Dungeon12.Entities.Objects;

namespace Dungeon12.Entities.Turns
{
    internal class Turn : GameComponent
    {
        public Turn(GameObject @object)
        {
            Object=@object;
        }

        public GameObject Object { get; private set; }

        public ObjectGroupProperty IsActive { get; set; } = new ObjectGroupProperty();

        public int Initialive { get; set; }

        public static implicit operator Turn(GameObject @object) => new Turn(@object);
    }
}
