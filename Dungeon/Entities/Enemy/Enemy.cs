namespace Dungeon.Entities.Enemy
{
    using Dungeon.Entities.Alive;
    using Dungeon.Loot;
    using Dungeon.Types;
    using LiteDB;

    public class Enemy : Moveable, ILootable
    {
        public bool Aggressive { get; set; }

        public string DieImage { get; set; }

        public Rectangle DieImagePosition { get; set; }

        public string LootTableName { get; set; }

        [BsonIgnore]
        public LootTable LootTable => LootTable.GetLootTable(this.LootTableName ?? this.IdentifyName ?? this.Name);
    }
}