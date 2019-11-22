
using Dungeon.Data;
using Dungeon.Types;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon.Loot
{
    public class LootDrop : Persist
    {
        public string ItemType { get; set; }

        public string ItemIdentify { get; set; }

        public int Chance { get; set; }

        public string LootTableIdentify { get; set; }
    }
}