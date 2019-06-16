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
                var invItem = new InventoryItem(item);
                this.AddChild(invItem);
                this.inventoryItems.Add(invItem);
            }
        }

        protected override void OnDrop(InventoryItem source)
        {
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
        public Item Item { get; set; }

        public InventoryItem(Item item)
        {
            this.Item = item;
            this.Image = item.Tileset;
            this.Width = item.InventorySize.X;
            this.Height = item.InventorySize.Y;

            CacheAvailable = false;
            AbsolutePosition = true;
            Left = item.InventoryPosition.X;
            Top = item.InventoryPosition.Y;
        }
    }
}