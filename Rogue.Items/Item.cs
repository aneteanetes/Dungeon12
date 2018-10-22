namespace Rogue.Items
{
    using System.Collections.Generic;
    using Rogue.Items.Enums;
    using Rogue.Transactions;
    using Rogue.Types;
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
        public int Level { get; set; }

        /// <summary>
        /// Редкость вещи
        /// </summary>
        public Rarity Rare { get; set; }

        /// <summary>
        /// очередное дохуя спорное решение во имя запускаемости
        /// </summary>
        public static Item Empty => new EmptyItem();

        public string Tileset => "";

        public Rectangle TileSetRegion => default;

        public Rectangle Region { get; set; }

        private class EmptyItem : Item
        {
            public override Stats AvailableStats => Stats.None;
        }
    }
}