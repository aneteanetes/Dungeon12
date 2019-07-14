namespace Rogue.Drawing.SceneObjects.Inventories
{
    using Rogue.Control.Pointer;
    using Rogue.Drawing.SceneObjects.Main.CharacterInfo;
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.Items;
    using System.Linq;
    
    public class InventoryItem : DraggableControl<InventoryItem>
    {
        public override bool TextureDragging => true;

        public Item Item { get; set; }

        private ItemWear[] itemWears;

        public InventoryItem(ItemWear[] itemWears, Item item)
        {
            this.itemWears = itemWears;
            this.Item = item;
            this.Image = item.Tileset;
            this.Width = item.InventorySize.X;
            this.Height = item.InventorySize.Y;

            CacheAvailable = false;
            AbsolutePosition = true;
            Left = item.InventoryPosition.X;
            Top = item.InventoryPosition.Y;
        }

        public override void Click(PointerArgs args)
        {
            if (args.MouseButton == MouseButton.Right)
            {
                var itemWear = itemWears.FirstOrDefault(x => x.ItemKind == this.Item.Kind);
                itemWear.WearItem(this);
            }
            else
            {
                base.Click(args);
            }
        }
    }
}