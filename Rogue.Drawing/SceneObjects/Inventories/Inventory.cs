namespace Rogue.Drawing.SceneObjects.Inventories
{
    using Rogue.Control.Events;
    using Rogue.Control.Keys;
    using Rogue.Control.Pointer;
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.SceneObjects.Base;
    using Rogue.Drawing.SceneObjects.Main.CharacterInfo;
    using Rogue.Drawing.SceneObjects.Map;
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.Entites.Alive;
    using Rogue.Items;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Чёт я проебал почему так, поэтому если надо выбрасывать на землю, после <see cref="Inventory"/> обязательно добавлять <see cref="InventoryDropItemMask"/>
    /// </summary>
    public class Inventory : DropableControl<InventoryItem>
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        readonly Backpack backpack;

        private PlayerSceneObject playerSceneObject;

        public Inventory(PlayerSceneObject playerSceneObject, Backpack backpack)
        {
            this.playerSceneObject = playerSceneObject;
            this.backpack = backpack;

            this.Height = backpack.Height;
            this.Width = backpack.Width;

            var back = new InventoryBackBatch(backpack.Width, backpack.Height);
            this.AddChild(back);
        }
        
        private List<InventoryItem> inventoryItems = new List<InventoryItem>();

        public ItemWear[] ItemWears { get; set; }

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
            double x = 0;
            double y = 0;

            bool buy = false;

            if (source.Parent == this)
            {
                x = Math.Ceiling(source.Left);
                y = Math.Ceiling(source.Top);
            }
            else
            {
                buy = true;
                x = Math.Ceiling(this.ComputedPosition.X - source.ComputedPosition.X);
                y = Math.Ceiling(this.ComputedPosition.Y - source.ComputedPosition.Y);
            }


            if (this.backpack.Add(source.Item, new Types.Point(x, y)))
            {
                if (source.Parent is Inventory inventoryParent)
                {
                    inventoryParent.backpack.Remove(source.Item);
                    inventoryParent.Refresh();
                }

                if (buy)
                {
                    playerSceneObject.Avatar.Character.Gold -= source.Item.Cost;
                }
            }

            Global.DrawClient.Drop();

            this.Refresh();

            base.OnDrop(source);
        }

        private class InventoryBackBatch : SceneObject
        {
            public override bool AbsolutePosition => true;

            public override bool CacheAvailable => false;

            public override bool IsBatch => true;

            public InventoryBackBatch(int width, int height)
            {
                var xPoint = 1;
                var yPoint = 1;

                double offsetX = 0;
                double offsetY = 0;

                this.Height = height;
                this.Width = width;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        this.AddChild(new InventoryCell()
                        {
                            Left = x * xPoint + offsetX,
                            Top = y * yPoint + offsetY,
                            AbsolutePosition=true,
                            CacheAvailable=false
                        });
                    }
                }
            }
        }

        private class InventoryCell : SceneObject
        {

            public InventoryCell()
            {
                this.Height = 1;
                this.Width = 1;
                this.AddChild(new InventoryCellBorder()
                {
                    AbsolutePosition=true,
                    CacheAvailable=false
                });
            }


            private DrawablePath drawablePath;
            public override IDrawablePath Path
            {
                get
                {
                    if (drawablePath == null)
                    {
                        var color = new DrawColor(ConsoleColor.Black)
                        {
                            Opacity = 0.7,
                            A = 255
                        };

                        drawablePath = new DrawablePath
                        {
                            Fill = true,
                            BackgroundColor = color,
                            Depth = 1,
                            PathPredefined = View.Enums.PathPredefined.Rectangle,
                            Region = this.Position,
                            Radius = 5f
                        };
                    }

                    return drawablePath;
                }
            }

            private class InventoryCellBorder : SceneObject
            {
                public InventoryCellBorder()
                {
                    Height = 1;
                    Width = 1;
                }

                private DrawablePath drawablePath;
                public override IDrawablePath Path
                {
                    get
                    {
                        if (drawablePath == null)
                        {
                            var color = new DrawColor(ConsoleColor.Black)
                            {
                                Opacity = 1,
                                A = 255
                            };

                            drawablePath = new DrawablePath
                            {
                                Fill = false,
                                BackgroundColor = color,
                                Depth = 1,
                                PathPredefined = View.Enums.PathPredefined.Rectangle,
                                Region = this.Position,
                                Radius = 5f
                            };
                        }

                        return drawablePath;
                    }
                }
            }
        }
    }
}