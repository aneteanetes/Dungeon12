using Dungeon.View;
using Dungeon12.Components;
using Dungeon12.Entities.MapRelated;

namespace Dungeon12.Entities
{
    public class Hero : IPhysical
    {
        public SpriteSheet WalkSpriteSheet { get; set; }

        public MapObject PhysicalObject { get; set; }
    }
}