namespace Rogue.Items
{
    using Force.DeepCloner;
    using Rogue.Physics;
    using Rogue.Types;
    using System.Linq;

    public class Backpack
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Backpack(int height, int width)
        {
            Width = width;
            Height = height;
            Container = new BackpackContainer()
            {
                Size = new PhysicalSize
                {
                    Width = width,
                    Height = height
                },
                Position = new PhysicalPosition()
            };
        }

        private readonly BackpackContainer Container;

        public Item[] GetItems() => Container.Nodes.Select(x => x.Item).ToArray();

        public bool Add(Item itemSource, Point location = null)
        {
            var item = itemSource.DeepClone();
            var backpackItem = new BackpackItem()
            {
                Item = item,
                Size = new PhysicalSize()
                {
                    Width = item.InventorySize.X,
                    Height = item.InventorySize.Y,
                }
            };

            if (location != null)
            {
                location.X += 0.1;
                location.Y += 0.1;

                backpackItem.Position = new PhysicalPosition()
                {
                    X = location.X,
                    Y = location.Y
                };

                if (Container.Query(backpackItem) != Container)
                {
                    return false;
                }
            }

            backpackItem.Size.Width -= .2;
            backpackItem.Size.Height -= .2;

            var locationSetted = TrySetLocation(backpackItem, location ?? new Point(0.1, 0.1));
            if (!locationSetted)
            {
                return false;
            }

            Container.Add(backpackItem);
            backpackItem.Size.Width += .2;
            backpackItem.Size.Height += .2;
            backpackItem.Position.X -= .1;
            backpackItem.Position.Y -= .1;

            return true;
        }

        private bool TrySetLocation(BackpackItem item, Point location)
        {
            item.Position = new PhysicalPosition()
            {
                X = location.X,
                Y = location.Y
            };

            //надо проверить чё будет если вещь выходит за пределы инвентаря

            if (Container.Query(item) != Container)
            {
                if (location.Y < Height)
                {
                    location.Y++;
                }
                else
                {
                    location.Y = 0.1;
                    location.X++;
                }

                if (location.X > Width)
                {
                    return false;
                }

                return TrySetLocation(item, location);
            }
            else
            {
                bool xOut = item.MaxPosition.X > Container.MaxPosition.X+0.1;
                bool yOut = item.MaxPosition.Y > Container.MaxPosition.Y+0.1;

                if (xOut)
                {
                    return false;
                }
                if (yOut)
                {
                    location.Y = 0.1;
                    location.X++;
                    TrySetLocation(item, location);
                }
            }

            item.Item.InventoryPosition = new Point(item.Position.X-0.1, item.Position.Y-0.1);

            return true;
        }

        public bool Remove(Item item)
        {
            var backpackItem = Container.Nodes.FirstOrDefault(n => n.Item == item);
            if (item != null)
            {
                Container.Nodes.Remove(backpackItem);
                return true;
            }

            return false;
        }

        private class BackpackItem : PhysicalObject<BackpackItem>
        {
            public Item Item { get; set; }

            protected override BackpackItem Self => this;

            protected override bool Containable => true;
        }

        private class BackpackContainer : BackpackItem
        {
            protected override bool Containable => true;
        }
    }
}