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

        public Merchant Merchant { get; }

        public Inventory Inventory {get; private set;}

        public ShopCategoryTab(Merchant merchant, MerchantCategory merchantCategory, double left, PlayerSceneObject playerSceneObject, Inventory another)
        {
            Inventory = new Inventory(playerSceneObject, merchantCategory.Goods.First(), merchant)
            {
                Top = 2,
                Left=-left
            };
            this.Merchant = merchant;
            Inventory.Refresh(another);
            this.AddChild(Inventory);
        }
    }
}