namespace Dungeon.Items
{
    using System.Collections.Generic;
    using Dungeon.Data;
    using Dungeon.Entities;
    using Dungeon.Entities.Alive;
    using Dungeon.GameObjects;
    using Dungeon.Items.Enums;
    using Dungeon.Loot;
    using Dungeon.Transactions;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using LiteDB;

    /// <summary>
    /// вещи могут быть сетами -не забыть
    /// //Вес = (УровеньПредмета — КачествоПредмета) * МультипликаторКачества * МультипликаторВидаПредмета;
    /// формирование цен бладжад
    /// </summary>
    public abstract partial class Item : Drawable, IPersist, ILootable
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

        public virtual Point InventorySize { get; set; }

        public string LootTableName { get; set; }

        [BsonIgnore]
        public LootTable LootTable => LootTable.GetLootTable(this.LootTableName ?? this.IdentifyName);

        public int Id { get; set; }
        public int ObjectId { get; set; }
        public string IdentifyName { get; set; }
        public string Assembly { get; set; }

        private class EmptyItem : Item
        {
            public override Stats AvailableStats => Stats.None;
        }
    }
}