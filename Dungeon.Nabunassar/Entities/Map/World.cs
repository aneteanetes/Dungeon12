using Dungeon.Tiled;

namespace Nabunassar.Entities.Map
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
