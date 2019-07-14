namespace Rogue.Drawing.SceneObjects.Dialogs.Shop
{
    using Rogue.Drawing.SceneObjects.Map;
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.Merchants;
    using System;

    public class ShopTab : TabControl<ShopCategoryTab, MerchantCategory>
    {
        private PlayerSceneObject playerSceneObject;
        public ShopTab(SceneObject parent, MerchantCategory merchantCategory, PlayerSceneObject playerSceneObject, bool active = false)
            : base(parent, active, merchantCategory, titleImg: Title(merchantCategory.Name))
        {
            this.playerSceneObject = playerSceneObject;
        }

        private static string Title(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
                return null;

            return $"Rogue.Resources.Images.Icons.Shop.{categoryName}.png";
        }

        protected override Func<MerchantCategory, double, ShopCategoryTab> CreateContent => OpenCategoryTab;

        private ShopCategoryTab OpenCategoryTab(MerchantCategory category, double left) => new ShopCategoryTab(category,left,playerSceneObject);
    }
}