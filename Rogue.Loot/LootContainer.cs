namespace Rogue.Loot
{
    using Rogue.Items;
    using System.Collections.Generic;

    public class LootContainer
    {
        public int Gold { get; set; }

        public List<Item> Items { get; set; }
    }
}