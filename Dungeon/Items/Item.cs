namespace Dungeon.Items
{
    using System.Collections.Generic;
    using Dungeon.Data;
    using Dungeon.Entities;
    using Dungeon.GameObjects;
    using Dungeon.Items.Enums;
    using Dungeon.Transactions;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;

    /// <summary>
    /// вещи могут быть сетами -не забыть
    /// //Вес = (УровеньПредмета — КачествоПредмета) * МультипликаторКачества * МультипликаторВидаПредмета;
    /// формирование цен бладжад
    /// </summary>
    public abstract partial class Item : DataEntity<Item, ItemData>, IDrawable, IPersist
    {
        public string Description { get; set; }

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

        public virtual ItemKind Kind { get; set; }

        public int Cost { get; set; }

        public Point InventoryPosition { get; set; }

        public Point InventorySize { get; set; }
               
        private class EmptyItem : Item
        {
            public override Stats AvailableStats => Stats.None;
        }


        /// <summary>
        /// Внутреннее свойство для LiteDb
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// числовой Id который можно использовать в коде
        /// </summary>
        public int ObjectId { get; set; }

        public string IdentifyName { get; set; }
    }
}