using Dungeon.GameObjects;
using Dungeon12.World.Map;

namespace Dungeon12.World
{
    public class GameWorld : GameComponent
    {
        public GameWorld()
        {

        }

        public WorldMap WorldMap { get; set; }

        public WorldMap CreateWorldMap(string regionName)
        {
            return WorldMap = new WorldMap(regionName);            
        }
    }
}