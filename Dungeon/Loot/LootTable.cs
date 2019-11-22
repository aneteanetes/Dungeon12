using Dungeon.Data;
using Dungeon.Types;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon.Loot
{
    public class LootTable : Persist
    {
        private LootTable() { }

        public Dictionary<Pair<string,string>, int> Items { get; set; }

        public LootContainer Generate(int goldAmount = 0)
        {
            var container = new LootContainer()
            {
                Gold = goldAmount
            };

            foreach (var item in Items)
            {
                if (RandomDungeon.Chance(item.Value))
                {
                    var loot = Database.Entity<Items.Item>(item.Key.First, item.Key.Second);
                    container.Items.Add(loot);
                }
            }

            return container;
        }

        /// <summary>
        /// 
        /// <para>
        /// [Кэшируемый]
        /// </para>
        /// </summary>
        public static LootTable GetLootTable(string identify)
        {
            if (!___GetLootTableCache.TryGetValue(identify, out var value))
            {
                value = Database.Entity<LootTable>(x => x.IdentifyName == identify).FirstOrDefault();
                ___GetLootTableCache.Add(identify, value);
            }

            return value;
        }
        private static readonly Dictionary<string, LootTable> ___GetLootTableCache = new Dictionary<string, LootTable>();
    }
}