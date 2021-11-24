using Dungeon12.Entities;
using Dungeon12.Entities.Map;
using Dungeon12.Entities.MapRelated;

namespace Dungeon12
{
    public class Game
    {
        public Party Party { get; set; }

        public Region Region { get; set; }

        public Location Location { get; set; }

        public Polygon Polygon { get; set; }
    }
}
