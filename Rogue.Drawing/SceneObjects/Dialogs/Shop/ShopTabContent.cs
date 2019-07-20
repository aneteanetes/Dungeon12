namespace Rogue.Drawing.SceneObjects.Dialogs.Shop
{
    using Rogue.Drawing.SceneObjects.Inventories;
    using Rogue.Drawing.SceneObjects.Map;
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.Merchants;
    using System.Linq;

    public class ShopTabContent : DropableControl<InventoryItem>
    {
        public override bool AbsolutePosition => true;

        public override bool CacheAvailable => false;

        public Merchant Merchant { get; }

        public Inventory Inventory { get; private set; }

        public ShopTabContent(Merchant merchant, MerchantCategory merchantCategory, double left, PlayerSceneObject playerSceneObject, Inventory another)
        {
            this.Top = 2;
            Inventory = new Inventory(playerSceneObject, merchantCategory.Goods.First(), merchant)
            {
                Left = -left
            };

            //TODO: Сделать страницы если потом понадобится

            this.Merchant = merchant;
            Inventory.Refresh(another);
            this.AddChild(Inventory);

            this.Width = Inventory.Width;
            this.Height = Inventory.Height;
        }

        protected override void OnDrop(InventoryItem source)
        {
            if (source.Parent != this.Inventory)
            {
                this.Inventory.Sell(this.Inventory, true)(source);
            }
            else
            {
                this.Inventory.Refresh();
            }

            source.Destroy?.Invoke();
        }
    }
}