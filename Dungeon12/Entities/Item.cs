using Dungeon12.Entities.Enums;

namespace Dungeon12.Entities
{
    internal class Item
    {
        public ItemType Type { get; set; }

        public ItemMaterial Material { get; set; }

        public WeaponType AttackType { get; set; }

        public string Image { get; set; }
    }
}
