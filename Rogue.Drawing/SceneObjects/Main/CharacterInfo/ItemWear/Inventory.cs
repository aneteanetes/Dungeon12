namespace Rogue.Drawing.SceneObjects.Main.CharacterInfo
{
    using Rogue.Control.Events;
    using Rogue.Control.Keys;
    using Rogue.Control.Pointer;
    using Rogue.Drawing.SceneObjects.Base;
    using Rogue.Drawing.SceneObjects.Map;
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.Entites.Alive;
    using Rogue.Items;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Inventory : DropableControl<InventoryItem>
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        Backpack backpack;

        private List<InventoryItem> inventoryItems = new List<InventoryItem>();

        public ItemWear[] ItemWears { get; set; }

        public Inventory(int zIndex, Backpack backpack)
        {
            this.backpack = backpack;

            var xPoint = 1;
            var yPoint = 1;

            double offsetX = 0;
            double offsetY = 0;

            this.Height = 6;
            this.Width = 11;

            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 11; x++)
                {
                    this.AddChild(new InventoryCell()
                    {
                        Left = x * xPoint + offsetX,
                        Top = y * yPoint + offsetY,
                        ZIndex = zIndex
                    });
                }
            }

            Refresh();
        }

        public void Refresh()
        {
            foreach (var invItem in inventoryItems)
            {
                invItem.Destroy?.Invoke();
                this.RemoveChild(invItem);
            }
            this.inventoryItems.Clear();

            foreach (var item in backpack.GetItems())
            {
                var invItem = new InventoryItem(this.ItemWears, item);
                this.AddChild(invItem);
                this.inventoryItems.Add(invItem);
            }
        }

        protected override void OnDrop(InventoryItem source)
        {
            var x = Math.Ceiling(source.Left);
            var y = Math.Ceiling(source.Top);

            if (this.backpack.Add(source.Item, new Types.Point(x, y)))
            {
                backpack.Remove(source.Item);
            }

            Global.DrawClient.Drop();

            this.Refresh();

            base.OnDrop(source);
        }

        private class InventoryCell : DarkRectangle
        {
            protected override ControlEventType[] Handles => new ControlEventType[0];

            public override bool CacheAvailable => false;

            public override bool AbsolutePosition => true;

            public InventoryCell()
            {
                this.AddChild(new DarkRectangle() { Opacity = 1, Fill = false, Color = ConsoleColor.Black, Height = 1, Width = 1 });

                Opacity = 0.7;

                this.Height = 1;
                this.Width = 1;
            }

            public override void Focus()
            {
                this.Opacity = 0.2;
                base.Focus();
            }

            public override void Unfocus()
            {
                this.Opacity = 0.7;
                base.Unfocus();
            }
        }
    }

    public class InventoryItem : DraggableControl
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