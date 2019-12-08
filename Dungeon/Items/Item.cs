namespace Dungeon.Items
{
    using System.Collections.Generic;
    using Dungeon.Data;
    using Dungeon.Entities;
    using Dungeon.Entities.Alive;
    using Dungeon.Entities.Alive.Proxies;
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
    public partial class Item : Drawable, IPersist, ILootable
    {
        public virtual bool Stackable { get; set; }

        public bool StackFull => Quantity == QuantityMax;

        /// <summary>
        /// [Лимит]
        /// </summary>
        [Proxied(typeof(Limit))]
        public virtual int Quantity { get; set; } = 1;

        public virtual int QuantityMax { get; set; } = 20;

        public virtual int QuantityRemove(int quantity)
        {
            var overflow = Quantity - quantity;
            if(overflow<0)
            {
                Quantity = 0;
                return overflow;
            }
            else if (overflow==0)
            {
                return 0;
            }

            Quantity -= quantity;
            return 1;
        }

        public virtual int QuantityAdd(int quantity)
        {
            var max = (Quantity + quantity) - QuantityMax;
            if (max > 0)
            {
                Quantity = QuantityMax;
                return max;
            }
            Quantity += quantity;
            return 0;
        }

        public string Description { get; set; }

        public virtual Stats AvailableStats { get; }

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