using Dungeon.View.Interfaces;
using Dungeon12.Entities.MapRelated;

namespace Dungeon12.Components
{
    public interface IPhysical
    {
        public MapObject PhysicalObject { get; set; }
    }
}