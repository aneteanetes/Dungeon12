namespace Dungeon12.Drawing.SceneObjects.Dialogs.Shop
{
    using Dungeon12.Drawing.SceneObjects.Map;
    using Dungeon12.Drawing.SceneObjects.UI;
    using Dungeon12.Merchants;
    using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
    using Dungeon.View.Interfaces;
    using Dungeon12.Drawing.SceneObjects.Inventories;
    using System;
    public class ShopTab : TabControl<ShopTabContent, MerchantCategory, ShopTab>
    {
        private PlayerSceneObject playerSceneObject;
        private Merchant merchant;
        private Inventory another;

        public Inventory ShopInventory { get; private set; }

        public ShopTab(ISceneObject parent, Inventory another, Merchant merchant, MerchantCategory merchantCategory, PlayerSceneObject playerSceneObject, bool active = false)
            : base(parent, active, merchantCategory, titleImg: Title(merchantCategory.Name))
        {
            this.merchant = merchant;
            this.another = another;
            this.playerSceneObject = playerSceneObject;
        }

        private static string Title(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
                return null;

            return $"Dungeon12.Resources.Images.Icons.Shop.{categoryName}.png";
        }

        protected override Func<MerchantCategory, double, ShopTabContent> CreateContent => OpenCategoryTab;

        protected override ShopTab Self => this;

        private ShopTabContent OpenCategoryTab(MerchantCategory category, double left)
        {
            var catTab= new ShopTabContent(merchant, category, left, playerSceneObject, another);
            ShopInventory = catTab.Inventory;
            return catTab;
        }
    }
}