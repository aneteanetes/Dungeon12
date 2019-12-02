using Dungeon.Conversations;
using Dungeon.Entities.Alive;
using Dungeon.Loot;
using Dungeon.Types;
using LiteDB;

namespace Dungeon12.Entities
{
    public class NPC : Moveable, ILootable
    {
        public bool Aggressive { get; set; }

        public string DieImage { get; set; }

        public Rectangle DieImagePosition { get; set; }

        public string LootTableName { get; set; }

        public Point AttackRange { get; set; }

        public Conversation Conversation { get; set; }

        public bool ChasingPlayers { get; set; }

        public bool ChasingEnemies { get; set; }

        [BsonIgnore]
        public LootTable LootTable => LootTable.GetLootTable(this.LootTableName ?? this.IdentifyName ?? this.Name);
    }
}
