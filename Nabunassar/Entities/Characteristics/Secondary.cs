namespace Nabunassar.Entities.Characteristics
{
    internal class Secondary
    {
        public RangeValue Will { get; set; } = new RangeValue(1, 1);

        public RangeValue Actions { get; set; } = new RangeValue(1, 1);

        public RangeValue Endurance { get; set; } = new RangeValue(100, 100);
    }
}