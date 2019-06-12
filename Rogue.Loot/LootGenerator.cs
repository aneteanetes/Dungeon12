using Rogue.Items.Types;

namespace Rogue.Loot
{
    public static class LootGenerator
    {
        public static Loot Generate()
        {
            return new Loot()
            {
                Gold = RandomRogue.Next(0, 20),
                Items = new System.Collections.Generic.List<Items.Item>()
                {
                    new Weapon()
                    {
                        Tileset="Rogue.Resources.Images.Items.Weapons.OneHand.Swords.TrainerSword.png",
                        TileSetRegion=new Types.Rectangle()
                        {
                            X=0,
                            Y=0,
                            Width=32,
                            Height=96
                        },
                        InventorySize=new Types.Point(1,3),
                        InventoryPosition= Items.Enums.PositionInInventory.Vertical
                    }
                }
            };
        }
    }
}