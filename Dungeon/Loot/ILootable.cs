namespace Dungeon.Loot
{
    public interface ILootable
    {
        string LootTableName { get; set; }

        LootTable LootTable { get; }
    }
}