namespace Rogue.Entites.Items
{
    using Rogue.Items;

    /// <summary>
    /// структуру - переделать, явный пиздец
    /// </summary>
    public class HotPanel
    {
        public Item[] ToArray() => new Item[] { HotItem1, HotItem2, HotItem3, HotItem4, HotItem5, HotItem6 };

        public Item HotItem1 { get; set; }

        public Item HotItem2 { get; set; }

        public Item HotItem3 { get; set; }

        public Item HotItem4 { get; set; }

        public Item HotItem5 { get; set; }

        public Item HotItem6 { get; set; }
    }
}