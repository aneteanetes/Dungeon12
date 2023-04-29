using Dungeon.GameObjects;

namespace Dungeon12.Entities.Objects
{
    internal class MapObject : GameComponent
    {
        public string Id { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public virtual bool IsSelected { get; set; }

        public virtual void Select() { }

        public GameObject GameObject { get; set; }
    }
}