namespace Rogue.Merchants
{
    using Rogue.Items.Enums;
    using Rogue.Items.Types;
    using System.Collections.Generic;
    using System.Linq;

    public class Merchant
    {
        public List<MerchantCategory> Categories { get; set; } = MerchantCategory.CommonCategories;

        /// <summary>
        /// Заполнить товары
        /// </summary>
        public void FillBackpacks()
        {
            foreach (var category in Categories)
            {
                category.Goods = new List<Items.Backpack>()
                {
                    new Items.Backpack(12,15)
                };

                foreach (var item in Enumerable.Range(0, 10))
                {
                    category.Goods[0].Add(new Weapon()
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
                        Rare = Rarity.Rare,
                        Cost=50
                    });
                }
            }
        }
    }
}