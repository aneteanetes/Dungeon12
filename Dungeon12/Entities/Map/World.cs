using Dungeon.Tiled;

namespace Dungeon12.Entities.Map
{
    internal class World
    {
        public TiledMap Map { get; set; }

        public World(TiledMap tiledMap)
        {
            Map = tiledMap;
        }
    }
}
