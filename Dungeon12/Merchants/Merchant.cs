namespace Dungeon12.Merchants
{
    using Dungeon.Inventory;
    using Dungeon.Items.Enums;
    using Dungeon.Items.Types;
    using Dungeon.Loot;
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
                category.Goods = new List<Backpack>()
                {
                    new Backpack(12,15)
                };

                foreach (var item in Enumerable.Range(0, 10))
                {
                    category.Goods[0].Add(LootGenerator.GenerateWeapon());
                }
            }
        }
    }
}