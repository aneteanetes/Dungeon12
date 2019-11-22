
using Dungeon.Data;
using LiteDB;

namespace Dungeon.Loot
{
    public class LootDrop : Persist
    {
        private LootGenerator _generator;

        [BsonIgnore]
        public LootGenerator Generator
        {
            get
            {
                if (_generator == default)
                {
                    _generator = ItemGeneratorTrigger.Trigger<LootGenerator>();
                    _generator.SetArguments(ItemGeneratorArguments);
                }

                return _generator;
            }
        }

        public string ItemGeneratorTrigger { get; set; }

        public string[] ItemGeneratorArguments { get; set; } = new string[0];

        public int Chance { get; set; }

        /// <summary>
        /// Это поле используется для того что бы знать в какую таблицу надо добавлять дроп
        /// </summary>
        public string LootTableIdentify { get; set; }
    }
}