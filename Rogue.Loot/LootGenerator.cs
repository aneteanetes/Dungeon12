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
                Items = new System.Collections.Generic.List<Item>()
                {
                    new Weapon()
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
                        Rare =  Rarity.Rare
                    },
                    new Boots()
                    {
                        Tileset = "Rogue.Resources.Images.Items.Boots.Leather.VampireBoots.png",
                        TileSetRegion = new Types.Rectangle()
                        {
                            X = 0,
                            Y = 0,
                            Width = 64,
                            Height = 64
                        },
                        Name = "Вампирские сапоги",
                        InventorySize = new Types.Point(2, 2),
                        Rare = Rarity.Epic
                    },
                    new Helm()
                    {
                        Tileset = "Rogue.Resources.Images.Items.Helms.Plate.DragonHelm.png",
                        TileSetRegion = new Types.Rectangle()
                        {
                            X = 0,
                            Y = 0,
                            Width = 64,
                            Height = 64
                        },
                        Name = "Драконий шлем",
                        InventorySize = new Types.Point(2, 2),
                        Rare = Rarity.Uncommon
                    },
                    new Armor()
                    {
                        Tileset = "Rogue.Resources.Images.Items.Chest.Mail.Nordic.png",
                        TileSetRegion = new Types.Rectangle()
                        {
                            X = 0,
                            Y = 0,
                            Width = 64,
                            Height = 64
                        },
                        Name = "Нордический доспех",
                        InventorySize = new Types.Point(2, 2),
                        Rare = Rarity.Watered
                    },
                    new OffHand()
                    {
                        Tileset = "Rogue.Resources.Images.Items.Offhands.Shields.Tall.DragonShield.png",
                        TileSetRegion = new Types.Rectangle()
                        {
                            X = 0,
                            Y = 0,
                            Width = 64,
                            Height = 64
                        },
                        Name = "Щит дракона",
                        InventorySize = new Types.Point(2, 4),
                        Rare = Rarity.Artefact
                    }
                }
            };
        }
    }
}