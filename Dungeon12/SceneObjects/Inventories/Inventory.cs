namespace Dungeon12.Drawing.SceneObjects.Inventories
{
    using Dungeon;
    using Dungeon.Classes;
    using Dungeon.Control;
    using Dungeon.Control.Keys;
    using Dungeon.Drawing;
    using Dungeon.Drawing.Impl;
    using Dungeon.Drawing.SceneObjects.Map;
    using Dungeon.Drawing.SceneObjects.UI;
    using Dungeon.Inventory;
    using Dungeon.SceneObjects;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using Dungeon12.Drawing.SceneObjects.Dialogs.Shop;
    using Dungeon12.Drawing.SceneObjects.Main.CharacterInfo;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Чёт я проебал почему так, поэтому если надо выбрасывать на землю, после <see cref="Inventory"/> обязательно добавлять <see cref="InventoryDropItemMask"/>
    /// </summary>
    public class Inventory : DropableControl<InventoryItem>
    {
        /// <summary>
        /// С целью дебага
        /// </summary>
        public ISceneObject AP => this.Parent;

        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        readonly Backpack backpack;

        private PlayerSceneObject playerSceneObject;

        private Character @char => playerSceneObject.Avatar.Character;

        private readonly Dungeon.Merchants.Merchant merchant;

        public Inventory(PlayerSceneObject playerSceneObject, Backpack backpack, Dungeon.Merchants.Merchant merchant = null)
        {
            this.merchant = merchant;
            this.playerSceneObject = playerSceneObject;

            this.backpack = backpack;

            this.Height = backpack.Height;
            this.Width = backpack.Width;

            var back = new InventoryBackBatch(backpack.Width, backpack.Height);
            this.AddChild(back);
        }

        private List<InventoryItem> inventoryItems = new List<InventoryItem>();

        public ItemWear[] ItemWears { get; set; }

        private Inventory Pair = null;

        public void Refresh(Inventory another = null)
        {
            if (another != null)
            {
                Pair = another;
            }

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

                if (merchant == null)
                {
                    invItem.OnBeforeClick = Sell(Pair);
                }
                else
                {
                    invItem.Cursor = "shop";
                    invItem.OnBeforeClick = Buy(Pair);
                }
            }
        }

        public Func<InventoryItem, bool> Buy(Inventory anotherInventory, bool force = false)
        {
            return inventoryItem =>
            {
                if (sellMode || force && anotherInventory != null)
                {
                    var res = @char.Buy(inventoryItem.Item, merchant);
                    if (res)
                    {
                        var backpack = force
                            ? (inventoryItem.Parent as Inventory).backpack
                            : this.backpack;

                        //Вот конкретно тут "точка расширения" на то что предмет не пропадает у продавца
                        backpack.Remove(inventoryItem.Item);
                    }
                    Trade(res, inventoryItem, anotherInventory);
                    return false;
                }

                return true;
            };
        }

        public Func<InventoryItem, bool> Sell(Inventory anotherInventory, bool force=false)
        {
            return inventoryItem =>
            {
                if (sellMode || force && anotherInventory !=null)
                {
                    if (anotherInventory.Parent is ShopTabContent shopCategoryTab)
                    {
                        var res = @char.Sell(inventoryItem.Item, shopCategoryTab.Merchant);
                        if(res)
                        {
                            anotherInventory.backpack.Add(inventoryItem.Item, owner:
                                playerSceneObject.Component.Entity.Backpack == anotherInventory.backpack
                                ? playerSceneObject.Component.Entity
                                : default);
                        }

                        Trade(res, inventoryItem, anotherInventory);
                    }

                    return false;
                }

                return true;
            };
        }

        private void Trade(Result<string> res, InventoryItem inventoryItem, Inventory another)
        {
            if (!res)
            {
                var pos = new Point(inventoryItem.ComputedPosition.X, inventoryItem.ComputedPosition.Y);
                var msg = new PopupString(res.Value.AsDrawText().InColor(DrawColor.Yellow), pos)
                {
                    Layer = 2000,
                    AbsolutePosition = true
                };
                this.ShowEffects(msg.InList<ISceneObject>());

            }

            another.Refresh();
            this.Refresh();
        }

        protected override ControlEventType[] Handles => new ControlEventType[] { ControlEventType.Key, ControlEventType.ClickRelease };

        protected override Key[] KeyHandles => new Key[]
        {
            Key.LeftShift,
            Key.RightShift
        };

        private bool sellMode = false;

        public override void KeyUp(Key key, KeyModifiers modifier) => sellMode = false;

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold) => sellMode = true;

        protected override void OnDrop(InventoryItem source)
        {
            if (source.Parent != this && source.Parent is Inventory shopInventory)
            {
                this.Buy(shopInventory, true)(source);
                source.Destroy?.Invoke();
            }
            else
            {
                var x = Math.Ceiling(source.Left);
                var y = Math.Ceiling(source.Top);

                if (this.backpack.Add(source.Item, new Dungeon.Types.Point(x, y),moving:true))
                {
                    if (source.Parent is Inventory inventoryParent)
                    {
                        inventoryParent.backpack.Remove(source.Item,
                            playerSceneObject.Component.Entity.Backpack == inventoryParent.backpack
                            ? playerSceneObject.Component.Entity
                            : default, moving: true);
                        inventoryParent.Refresh();
                    }
                }

                Global.DrawClient.Drop();
            }

            this.Refresh();

            base.OnDrop(source);
        }

        private class InventoryBackBatch : EmptySceneObject
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
                            AbsolutePosition = true,
                            CacheAvailable = false
                        });
                    }
                }
            }
            protected override void CallOnEvent(dynamic obj)
            {
                OnEvent(obj);
            }
        }

        private class InventoryCell : EmptySceneObject
        {

            public InventoryCell()
            {
                this.Height = 1;
                this.Width = 1;
                this.AddChild(new InventoryCellBorder()
                {
                    AbsolutePosition = true,
                    CacheAvailable = false
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
                            PathPredefined = Dungeon.View.Enums.PathPredefined.Rectangle,
                            Region = this.Position,
                            Radius = 5f
                        };
                    }

                    return drawablePath;
                }
            }

            private class InventoryCellBorder : EmptySceneObject
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
                                PathPredefined = Dungeon.View.Enums.PathPredefined.Rectangle,
                                Region = this.Position,
                                Radius = 5f
                            };
                        }

                        return drawablePath;
                    }
                }
                protected override void CallOnEvent(dynamic obj)
                {
                    OnEvent(obj);
                }
            }
            protected override void CallOnEvent(dynamic obj)
            {
                OnEvent(obj);
            }
        }
    }
}