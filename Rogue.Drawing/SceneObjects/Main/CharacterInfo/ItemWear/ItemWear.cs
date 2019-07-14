namespace Rogue.Drawing.SceneObjects.Main.CharacterInfo
{
    using System;
    using System.Collections.Generic;
    using Force.DeepCloner;
    using Rogue.Control.Events;
    using Rogue.Control.Pointer;
    using Rogue.Drawing.SceneObjects.Inventories;
    using Rogue.Drawing.SceneObjects.Map;
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.Entites.Alive;
    using Rogue.Items;
    using Rogue.Items.Enums;
    using Rogue.View.Interfaces;

    public class ItemWear : DropableControl<InventoryItem>
    {
        protected override ControlEventType[] Handles => new ControlEventType[]
        {
             ControlEventType.ClickRelease,
             ControlEventType.Click,
              ControlEventType.Focus
        };

        private string borderImage = string.Empty;

        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        private Character character;
        public ItemKind ItemKind;
        private Item item;
        private Inventory inventory;

        public ItemWear(Inventory inventory, Character character, ItemKind itemKind)
        {
            this.inventory = inventory;
            this.ItemKind = itemKind;
            this.character = character;
            var tall = itemKind == ItemKind.Weapon || itemKind == ItemKind.OffHand;

            this.borderImage = tall
                ? "Rogue.Resources.Images.ui.squareWeapon"
                : "Rogue.Resources.Images.ui.square";

            this.Width = 2;
            this.Height = tall
                ? 4
                : 2;

            this.Image = SquareTexture();

            switch (itemKind)
            {
                case ItemKind.Weapon:
                    if (character.Clothes.Weapon != null)
                    {
                        this.dressItemControl = new DressedItem(character.Clothes.Weapon)
                        {
                            CacheAvailable = this.CacheAvailable,
                            AbsolutePosition = this.AbsolutePosition
                        };
                    }
                    break;
                case ItemKind.Helm:
                    if (character.Clothes.Helm != null)
                    {
                        this.dressItemControl = new DressedItem(character.Clothes.Helm)
                        {
                            CacheAvailable = this.CacheAvailable,
                            AbsolutePosition = this.AbsolutePosition
                        };
                    }
                    break;
                case ItemKind.Armor:
                    if (character.Clothes.Armor != null)
                    {
                        this.dressItemControl = new DressedItem(character.Clothes.Armor)
                        {
                            CacheAvailable = this.CacheAvailable,
                            AbsolutePosition = this.AbsolutePosition
                        };
                    }
                    break;
                case ItemKind.Boots:
                    if (character.Clothes.Boots != null)
                    {
                        this.dressItemControl = new DressedItem(character.Clothes.Boots)
                        {
                            CacheAvailable = this.CacheAvailable,
                            AbsolutePosition = this.AbsolutePosition
                        };
                    }
                    break;
                case ItemKind.OffHand:
                    if (character.Clothes.OffHand != null)
                    {
                        this.dressItemControl = new DressedItem(character.Clothes.OffHand)
                        {
                            CacheAvailable = this.CacheAvailable,
                            AbsolutePosition = this.AbsolutePosition
                        };
                    }
                    break;
                default:
                    break;
            }

            DressUpItem();
        }

        private string SquareTexture(bool focus = false)
        {
            var f = focus
                ? "_f"
                : "";

            return $"{borderImage}{f}.png";
        }

        public override void Focus()
        {
            if (DragAndDropSceneControls.IsDragging)
            {
                base.Focus();
                if (this.DropAvailable)
                {
                    this.Image = SquareTexture(true);
                }
            }
            else
            {
                this.Image = SquareTexture(true);                
            }
        }

        protected override bool CheckDropAvailable(InventoryItem source)
        {
            return source.Item.Kind == this.ItemKind;
        }

        public override void Unfocus()
        {
            this.Image = SquareTexture();
            base.Unfocus();
        }

        protected override void OnDrop(InventoryItem source)
        {
            if (source.Item.Kind == this.ItemKind)
            {
                WearItem(source,true);
            }
        }

        public void WearItem(InventoryItem source, bool dropping=false)
        {
            var putOn = character.Clothes.PutOn(source.Item);

            if (putOn.success)
            {
                if (putOn.oldItem != null)
                {
                    DressOffItemToInventory(putOn.oldItem);
                }

                if (dropping)
                {
                    Global.DrawClient.Drop();
                }

                this.dressItemControl = new DressedItem(source.Item)
                {
                    CacheAvailable = this.CacheAvailable,
                    AbsolutePosition = this.AbsolutePosition
                };
                character.Backpack.Remove(source.Item);
                inventory.Refresh();
                DressUpItem();

                source.Destroy?.Invoke();
            }
        }

        private DressedItem dressItemControl;

        private void DressUpItem()
        {
            if (dressItemControl != null)
            {
                dressItemControl?.Destroy?.Invoke();
                this.AddChild(dressItemControl);
            }
        }

        private void DressOffItem()
        {
            if (dressItemControl != null)
            {
                if (character.Clothes.PutOff(this.ItemKind).success)
                {
                    DressOffItemToInventory(dressItemControl.item);
                }
            }
        }

        private void DressOffItemToInventory(Item item)
        {
            character.Backpack.Add(item);
            inventory.Refresh();
            dressItemControl?.Destroy?.Invoke();
            this.RemoveChild(dressItemControl);
            dressItemControl = null;
        }

        public override void Click(PointerArgs args)
        {
            if (args.MouseButton == MouseButton.Right)
            {
                DressOffItem();
            }
        }        

        private class DressedItem : TooltipedSceneObject
        {
            public Item item;

            public DressedItem(Item item) : base(item.Description, null)
            {
                this.item = item;
                this.Image = item.Tileset;
                this.ImageRegion = item.TileSetRegion;

                var tall = item.Kind == ItemKind.Weapon || item.Kind == ItemKind.OffHand;

                this.Height = tall ? 4 : 2;
                this.Width = 2;
            }
        }
    }
}