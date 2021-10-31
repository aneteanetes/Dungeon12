using Dungeon.Physics;

namespace Dungeon12.Entities.MapRelated
{
    public class MapObject : PhysicalObject<MapObject>
    {
        protected override MapObject Self => this;
    }
}
