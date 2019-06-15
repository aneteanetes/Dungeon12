using Rogue.Items;
using Rogue.Items.Enums;
using Rogue.Items.Types;
using System.Linq;

namespace Rogue.Loot
{
    public static class LootGenerator
    {
        public static LootContainer Generate()
        {
            return new LootContainer()
            {
                Gold = RandomRogue.Next(0, 20),
                Items = typeof(Rarity).All<Rarity>().Select(rarity =>
                {
                    return new Weapon()
                    {
                        Tileset = "Rogue.Resources.Images.Items.Weapons.OneHand.Swords.TrainerSword.png",
                        TileSetRegion = new Types.Rectangle()
                        {
                            X = 0,
                            Y = 0,
                            Width = 32,
                            Height = 96
                        },
                        Name = "Тренировочный меч",
                        InventorySize = new Types.Point(1, 3),
                        InventoryPosition = Items.Enums.PositionInInventory.Vertical,
                        Rare = rarity
                    };
                })
                .Cast<Item>()
                .ToList()
            };
        }
    }
}