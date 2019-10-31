namespace Dungeon12.Drawing.SceneObjects.Inventories
{
    using Dungeon.Control.Pointer;
    using Dungeon.Drawing.GUI;
    using Dungeon.Drawing.Impl;
    using Dungeon.Drawing.SceneObjects.Main.CharacterInfo;
    using Dungeon.Drawing.SceneObjects.UI;
    using Dungeon.Items;
    using Dungeon.Items.Enums;
    using Dungeon.Types;
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

            base.OnDrag += PreDrag;
        }

        protected override bool ProvidesTooltip => true;

        protected override Tooltip ProvideTooltip(Point position)
        {
            return new ItemTooltip(Item, position);
        }

        private bool showTooltip = true;

        private void PreDrag()
        {
            showTooltip = false;
        }

        public override void GlobalClickRelease(PointerArgs args)
        {
            showTooltip = true;
            base.GlobalClickRelease(args);
        }

        public override void Focus()
        {
            if (showTooltip)
                base.Focus();
        }

        public Func<InventoryItem, bool> OnBeforeClick { get; set; }

        public override void Click(PointerArgs args)
        {
            if (OnBeforeClick?.Invoke(this) == false)
                return;

            if (args.MouseButton == MouseButton.Right)
            {
                if (itemWears != null)
                {
                    var itemWear = itemWears.FirstOrDefault(x => x.ItemKind == this.Item.Kind);
                    itemWear.WearItem(this);
                }
            }
            else
            {
                base.Click(args);
            }
        }
        protected override void CallOnEvent(dynamic obj)
        {
            OnEvent(obj);
        }
    }
}