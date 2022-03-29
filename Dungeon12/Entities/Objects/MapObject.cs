using Dungeon.GameObjects;
using Dungeon12.Entities.Enums;

namespace Dungeon12.Entities.Objects
{
    public class MapObject : GameComponent
    {
        public int Id { get; set; }

        public string Icon { get; set; }

        public ObjectType Type { get; set; }
    }
}