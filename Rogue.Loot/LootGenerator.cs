namespace Rogue.Loot
{
    public static class LootGenerator
    {
        public static Loot Generate()
        {
            return new Loot()
            {
                Gold = 5,
                Items = new System.Collections.Generic.List<Items.Item>()
            };
        }
    }
}