namespace Dungeon12.Entities
{
    internal class Party : Quad<Hero>
    {
        public RangeValue Movements { get; set; } = new();

        public Food Food { get; set; } = new Food();

        public Fame Fame { get; set; } = new Fame();

        public Prayers Prayers { get; set; } = new Prayers();

        public int Gold { get; set; }
    }
}