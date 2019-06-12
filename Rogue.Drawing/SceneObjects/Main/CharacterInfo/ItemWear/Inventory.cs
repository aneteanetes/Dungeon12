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

    public class Inventory : DraggableControl
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        private Character character;

        private ICollection<Item> items;

        /// <summary>
        /// БЛЯДЬ! это невозможно, надо ОБЯЗАТЕЛЬНО разделить на инвентарь и лутбокс
        /// </summary>
        /// <param name="zIndex"></param>
        /// <param name="size"></param>
        /// <param name="items"></param>
        /// <param name="ownBack"></param>
        /// <param name="character"></param>
        public Inventory(int zIndex, Rogue.Types.Point size, ICollection<Item> items,bool ownBack=false, Character character=null)
        {
            this.items = items;
            this.character = character;

            var xPoint = 1;
            var yPoint = 1;

            double offsetX = 0;
            double offsetY = 0;

            if (ownBack)
            {
                this.Image = "Rogue.Resources.Images.ui.inventory(6_5x6_5).png";
                this.Height = 6+1;
                this.Width = 6+1;

                offsetX += 0.5;
                offsetY += 0.5;

                this.AddChild(new CloseButton(()=>this.Destroy())
                {
                    Left = this.Width - 1.5,
                    Top = 0.5
                });
            }

            for (int y = 0; y < size.Y; y++)
            {
                for (int x = 0; x < size.X; x++)
                {
                    this.AddChild(new InventoryCell()
                    {
                        Left = x * xPoint + offsetX,
                        Top = y * yPoint + offsetY,
                        ZIndex= zIndex
                    });
                }
            }

            if (ownBack)
            {
                foreach (var item in items)
                {
                    var randomX = RandomRogue.Next(0, 5);
                    var randomY = RandomRogue.Next(0, 3);

                    this.AddChild(new SimpleInventoryItem(item, TakeItem)
                    {
                        Top = randomX+0.5,
                        Left = randomX+0.5
                    });
                }
            }
            else
            {
                var xdouble = 0;
                foreach (var item in items)
                {
                    this.AddChild(new ImageControl(item.Tileset)
                    {
                        CacheAvailable=false,
                        AbsolutePosition=true,
                        Left = xdouble
                    });
                    xdouble =+ 1;
                }
            }
        }

        private void TakeItem(Item item)
        {
            items.Remove(item);
            character.Backpack.Add(item); //if can

            if (items.Count() == 0)
            {
                this.Destroy?.Invoke();
            }

        }


        private class SimpleInventoryItem : TooltipedSceneObject
        {
            public override bool CacheAvailable => false;

            public override bool AbsolutePosition => true;

            Action<Item> take;
            Item item;

            public SimpleInventoryItem(Item item, Action<Item> take) :base(item.Name,null)
            {
                this.item = item;
                this.take = take;

                this.Width = item.InventorySize.X;
                this.Height = item.InventorySize.Y;

                this.Image = item.Tileset;
                this.ImageRegion = item.TileSetRegion;
            }

            public override void Click(PointerArgs args)
            {
                take(item);
                this.Destroy?.Invoke();
            }
        }

        private class InventoryCell : DarkRectangle
        {
            protected override ControlEventType[] Handles => new ControlEventType[0];

            public override bool CacheAvailable => false;

            public override bool AbsolutePosition => true;

            public InventoryCell()
            {
                this.AddChild(new DarkRectangle() { Opacity = 1, Fill = false, Color= ConsoleColor.Black, Height=1, Width=1 });

                Opacity = 0.7;

                this.Height =1;
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

        private class CloseButton : TooltipedSceneObject
        {
            public override bool CacheAvailable => false;
            public override bool AbsolutePosition => true;

            private readonly Action close;

            public CloseButton(Action close) : base("Закрыть",null)
            {
                this.close = close;

                this.Height = 1;
                this.Width = 1;

                this.AddChild(new ImageControl("Rogue.Resources.Images.ui.cross.png")
                {
                    AbsolutePosition = true,
                    CacheAvailable = false,
                    Height = 1,
                    Width = 1,
                });

                this.Image = SquareTexture(false);
            }

            private string SquareTexture(bool focus)
            {
                var f = focus
                    ? "_f"
                    : "";

                return $"Rogue.Resources.Images.ui.square{f}.png";
            }

            public override void Focus()
            {
                this.Image = SquareTexture(true);
                base.Focus();
            }

            public override void Unfocus()
            {
                this.Image = SquareTexture(false);
                base.Focus();
            }

            protected override Key[] KeyHandles => new Key[]
            {
                Key.Escape
            };

            public override void KeyDown(Key key, KeyModifiers modifier, bool hold) => close?.Invoke();

            public override void Click(PointerArgs args) => close?.Invoke();
        }
    }
}
