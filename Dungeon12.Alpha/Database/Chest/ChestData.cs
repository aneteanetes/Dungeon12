using Dungeon.View;
using Dungeon12.Data.Region;

namespace Dungeon12.Database.Chest
{
    public class ChestData : RegionPart
    {
        public string Name { get; set; }

        public string LootTable { get; set; }

        public Animation Animation { get; set; }
    }
}