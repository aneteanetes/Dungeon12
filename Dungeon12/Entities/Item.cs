using Dungeon;
using Dungeon.GameObjects;
using Dungeon12.Entities.Enums;

namespace Dungeon12.Entities
{
    internal class Item : GameComponent
    {
        public string Id { get; set; }

        public ItemSlot Slot { get; set; }

        public ItemType Type { get; set; }

        public ItemMaterial Material { get; set; }

        public AttackType AttackType { get; set; }

        public Rarity Rarity { get; set; }

        public int Armor { get; set; } = 0;

        public int Durability { get; set; }

        public int MaxDurability => Material.Durability();
    }
}
