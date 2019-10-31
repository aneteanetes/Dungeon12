namespace Dungeon12.Drawing.SceneObjects.Main.CharacterInfo
{
    using System;
    using System.Collections.Generic;
    using Force.DeepCloner;
    using Dungeon.Classes;
    using Dungeon.Control.Events;
    using Dungeon.Control.Pointer;
    using Dungeon.Drawing.GUI;
    using Dungeon.Drawing.SceneObjects.Inventories;
    using Dungeon.Drawing.SceneObjects.Map;
    using Dungeon.Drawing.SceneObjects.UI;
    using Dungeon.Entites.Alive;
    using Dungeon.Items;
    using Dungeon.Items.Enums;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;

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
        private Inventory inventory;
        
        private DressedItem dressItemControl;

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

            this.dressItemControl = new DressedItem(null);
            this.AddChild(this.dressItemControl);

            DressUpCurrent(character, itemKind);
        }

        private void DressUpCurrent(Character character, ItemKind itemKind)
        {
            switch (itemKind)
            {
                case ItemKind.Weapon:
                case ItemKind.Helm:
                case ItemKind.Armor:
                case ItemKind.Boots:
                case ItemKind.OffHand:
                    {
                        var cloth = character.Clothes.GetProperty<Item>(itemKind.ToString());
                        if (cloth != null)
                        {
                            this.dressItemControl.Dress(cloth);
                        }
                        break;
                    }
                default:
                    break;
            }
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

            ShowTooltip();
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
            if (source.Parent is Inventory sourceInventory) // получить таргет (бллядь, это уже второй такой хак, надо переработать к хуям)
            {
                if (source.Item.Kind == this.ItemKind && sourceInventory.Parent is CharacterInfoWindow)
                {
                    WearItem(source, true);
                }
                else
                {
                    source.Destroy?.Invoke();
                    sourceInventory.Refresh();
                }
            }
        }

        public void WearItem(InventoryItem source, bool dropping = false)
        {
            var (success, oldItem) = character.Clothes.PutOn(source.Item);

            if (success)
            {
                if (oldItem != null)
                {
                    character.Backpack.Add(this.dressItemControl.item);
                    inventory.Refresh();
                }

                if (dropping)
                {
                    Global.DrawClient.Drop();
                }

                character.Backpack.Remove(source.Item);
                inventory.Refresh();

                this.dressItemControl.Dress(source.Item);

                source.Destroy?.Invoke();
            }
        }
        
        public override void Click(PointerArgs args)
        {
            if (args.MouseButton == MouseButton.Right)
            {
                DressOffItem();
            }
        }

        private void DressOffItem()
        {
            if (character.Clothes.PutOff(this.ItemKind).success)
            {
                this.character.Backpack.Add(this.dressItemControl.item);
                inventory.Refresh();
                this.dressItemControl.Undress();
            }
        }

        protected override bool ProvidesTooltip => dressItemControl.item != null;

        protected override Tooltip ProvideTooltip(Point position)
        {
            return new ItemTooltip(dressItemControl.item, position);
        }
        protected override void CallOnEvent(dynamic obj)
        {
            OnEvent(obj);
        }

        private class DressedItem : TooltipedSceneObject
        {
            public override bool CacheAvailable => false;

            public override bool AbsolutePosition => true;

            public Item item;

            public DressedItem(Item item) : base(item?.Description, null) => Dress(item);

            public void Dress(Item itemSource)
            {
                if (itemSource != null)
                {
                    var item = itemSource.DeepClone();

                    this.TooltipText = item.Description;
                    this.item = item;
                    this.Image = item.Tileset;
                    this.ImageRegion = item.TileSetRegion;

                    var tall = item.Kind == ItemKind.Weapon || item.Kind == ItemKind.OffHand;

                    this.Height = tall ? 4 : 2;
                    this.Width = 2;
                }
            }

            public void Undress()
            {
                this.TooltipText = string.Empty;
                this.item = null;
                this.Image = string.Empty;
                this.ImageRegion = new Types.Rectangle(1, 1, 1, 1);
            }

            public bool IsEmpty => item == null;
            protected override void CallOnEvent(dynamic obj)
            {
                OnEvent(obj);
            }
        }
    }
}