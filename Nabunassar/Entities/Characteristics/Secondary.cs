namespace Nabunassar.Entities.Characteristics
{
    internal class Secondary
    {
        /// <summary>
        /// Воля
        /// </summary>
        public RangeValue Will { get; set; } = new RangeValue(1, 1);

        /// <summary>
        /// Действия
        /// </summary>
        public RangeValue Actions { get; set; } = new RangeValue(1, 1);

        /// <summary>
        /// Выносливость
        /// </summary>
        public RangeValue Endurance { get; set; } = new RangeValue(100, 100);

        /// <summary>
        /// Передвижение
        /// </summary>
        public RangeValue Movement { get; set; } = new RangeValue(100, 100);

        /// <summary>
        /// Скорость
        /// </summary>
        public RangeValue Speed { get; set; }
    }
}