using Dungeon;
using Dungeon.Data;
using Dungeon.Types;
using LiteDB;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.Loot
{
    public class LootTable : Persist
    {
        private LootTable() { }

        public string[] LootDropIds { get; set; } = new string[0];

        private List<LootDrop> _lootDrops;

        [BsonIgnore]
        public List<LootDrop> LootDrops
        {
            get
            {
                if (_lootDrops == default)
                {
                    _lootDrops = Dungeon.Store.Entity<LootDrop>(x => LootDropIds.Contains(x.IdentifyName)).ToList();
                }

                return _lootDrops;
            }
        }

        public LootContainer Generate(int goldAmount = 0)
        {
            var lvl = Global.GameState.Character.Level;

            var container = new LootContainer()
            {
                Gold = RandomDungeon.Range(lvl, lvl * 100)
            };

            foreach (var lootDrop in LootDrops)
            {
                if (RandomDungeon.Chance(lootDrop.Chance))
                {
                    var loot = lootDrop.Generator.Generate();
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
                value = Dungeon.Store.Entity<LootTable>(x => x.IdentifyName == identify).FirstOrDefault();
                ___GetLootTableCache.Add(identify, value);
            }

            return value;
        }
        private static readonly Dictionary<string, LootTable> ___GetLootTableCache = new Dictionary<string, LootTable>();
    }
}