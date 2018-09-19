namespace Rogue.Items
{
    using System.Collections.Generic;
    using Rogue.Items.Enums;
    using Rogue.Transactions;
    using Rogue.View.Interfaces;

    /// <summary>
    /// вещи могут быть сетами -не забыть
    /// //Вес = (УровеньПредмета — КачествоПредмета) * МультипликаторКачества * МультипликаторВидаПредмета;
    /// формирование цен бладжад
    /// </summary>
    public abstract class Item : IDrawable
    {
        public string Icon { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IDrawColor BackgroundColor { get; set; }

        public IDrawColor ForegroundColor { get; set; }

        public abstract Stats AvailableStats { get; }

        public List<Applicable> Modifiers { get; set; } = new List<Applicable>();

        /// <summary>
        /// Уровень вещи
        /// </summary>
        public int Level;

        /// <summary>
        /// Редкость вещи
        /// </summary>
        public Rarity Rare;
    }
}