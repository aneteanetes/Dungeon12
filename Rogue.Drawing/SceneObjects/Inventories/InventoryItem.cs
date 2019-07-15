namespace Rogue.Drawing.SceneObjects.Inventories
{
    using Rogue.Control.Pointer;
    using Rogue.Drawing.GUI;
    using Rogue.Drawing.SceneObjects.Main.CharacterInfo;
    using Rogue.Drawing.SceneObjects.Map;
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.Items;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System;
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

        public Func<InventoryItem, bool> OnBeforeClick { get; set; }

        public override void Click(PointerArgs args)
        {
            if (OnBeforeClick?.Invoke(this) == false)
                return;

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