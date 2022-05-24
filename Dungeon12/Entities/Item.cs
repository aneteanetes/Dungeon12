using Dungeon;
using Dungeon.GameObjects;
using Dungeon12.Entities.Enums;

namespace Dungeon12.Entities
{
    internal class Item : GameComponent
    {
        public string Id { get; set; }

        public ItemType Type { get; set; }

        public ItemMaterial Material { get; set; }

        public WeaponType AttackType { get; set; }

        public Rarity Rarity { get; set; }
    }
}
