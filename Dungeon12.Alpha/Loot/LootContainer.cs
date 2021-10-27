namespace Dungeon12.Loot
{
    using Dungeon12.Items;
    using System.Collections.Generic;

    public class LootContainer
    {
        public int Gold { get; set; }

        public List<Item> Items { get; set; } = new List<Item>();
    }
}