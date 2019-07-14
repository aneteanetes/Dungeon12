namespace Rogue.Drawing.SceneObjects.Dialogs.Shop
{
    using Rogue.Drawing.SceneObjects.Inventories;
    using Rogue.Drawing.SceneObjects.Map;
    using Rogue.Merchants;
    using System.Linq;

    public class ShopCategoryTab : HandleSceneControl
    {
        public override bool AbsolutePosition => true;

        public override bool CacheAvailable => false;

        public ShopCategoryTab(MerchantCategory merchantCategory, double left, PlayerSceneObject playerSceneObject)
        {
            var inventory = new Inventory(playerSceneObject, merchantCategory.Goods.First())
            {
                Top = 2,
                Left=-left
            };            
            inventory.Refresh();
            this.AddChild(inventory);
        }
    }
}