using Dungeon.GameObjects;
using Dungeon12.Data.Region;

namespace Dungeon12.World.Map
{
    public class WorldMap : StoredGameComponent<Region>
    {
        public WorldMap(string name) : base(name)
        {
        }
    }
}
