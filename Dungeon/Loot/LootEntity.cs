using Dungeon.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Loot
{
    public class LootEntity : Entity
    {
        public string LootTableName { get; set; }

        public LootTable LootTable => LootTable.GetLootTable(this.LootTableName);
    }
}
