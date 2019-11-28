namespace Dungeon.Inventory
{
    using Force.DeepCloner;
    using Dungeon.Items;
    using Dungeon.Physics;
    using Dungeon.Types;
    using System.Linq;
    using Dungeon.Entities.Alive;
    using MoreLinq;
    using System.Collections.Generic;

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

        public List<BackpackItem> BackpackSaveItems
        {
            get => Container.Nodes;
            set
            {
                foreach (var item in value)
                {
                    Add(item.Item, item.Position.ToPoint(), null, item.Item.Quantity, true);
                }
            }
        }

        /// <summary>
        /// Добавляет предмет
        /// <para>
        /// События: <see cref="ItemPickedUpEvent"/>
        /// </para>
        /// </summary>
        /// <param name="itemSource"></param>
        /// <param name="location"></param>
        /// <param name="owner"></param>
        /// <param name="moving">этот параметр используется для ПЕРЕМЕЩЕНИЯ стакуемых вещей</param>
        /// <returns></returns>
        public bool Add(Item itemSource, Point location = null, Alive owner=default, int quantity = 1, bool moving=false)
        {
            if (itemSource.Stackable && !moving)
            {
                if (AddStackable(itemSource, quantity,owner, out var overflow))
                {
                    if (overflow == 0)
                    {
                        AddStackableEvent(itemSource, owner, quantity);
                        // если удалось добавить в стакаемую вещь, тогда не надо её добавлять в инвентарь,
                        // и наоборот, если вещь умеет стакаться, но её ещё нет, надо добавить физический экземпляр
                        // если при добавлении стак переполнился, значит надо добавить новый экземпляр
                        return true;
                    }
                    else
                    {
                        // если стак переполнился, значит надо добавить столько сколько переполнилось
                        quantity = overflow;
                    }
                }
            }
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

            if (item.Stackable && quantity > 1)
            {
                AddStackableEvent(itemSource, owner, quantity-1);
                backpackItem.Item.QuantityAdd(quantity - 1);
            }

            Container.Add(backpackItem);
            backpackItem.Size.Width += .2;
            backpackItem.Size.Height += .2;
            backpackItem.Position.X -= .1;
            backpackItem.Position.Y -= .1;

            if (!moving)
            {
                Global.Events.Raise(new ItemPickedUpEvent() { Item = item, Owner = owner });
            }

            return true;
        }

        private static void AddStackableEvent(Item itemSource, Alive owner, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                Global.Events.Raise(new ItemPickedUpEvent() { Item = itemSource, Owner = owner });
            }
        }

        private static void RemoveStackableEvent(Item itemSource, Alive owner, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                Global.Events.Raise(new ItemDropOffEvent() { Item = itemSource, Owner = owner });
            }
        }

        private bool AddStackable(Item itemSource, int quantity,Alive owner, out int overflow)
        {
            overflow = 0;
            var stackableItem = Container.Nodes
                .FirstOrDefault(n => n.Item.IdentifyName == itemSource.IdentifyName
                && !n.Item.StackFull);

            if (stackableItem == default)
            {
                return false;
            }

            overflow = stackableItem.Item.QuantityAdd(quantity);
            return overflow >= 0;
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

        /// <summary>
        /// Удаляет предмет
        /// <para>
        /// События: <see cref="ItemDropOffEvent"/>
        /// </para>
        /// </summary>
        /// <param name="item"></param>
        /// <param name="owner"></param>
        /// <param name="moving">этот параметр используется для ПЕРЕМЕЩЕНИЯ стакуемых вещей</param>
        /// <returns></returns>
        public bool Remove(Item item, Alive owner = default, int quantity = 1, bool moving=false)
        {
            if(item.Stackable && !moving)
            {
                if (RemoveStackable(item, owner, quantity))
                {
                    RemoveStackableEvent(item, owner, quantity);
                    // если удалось удалить стаки из одного и он остался
                    // не надо удалять предмет
                    return true;
                    // другая ситуация которая может породить true:
                    // когда мы удаляем кол-во вещей в разных стаках
                    // тогда в методе RemoveStackable будет вызываться
                    // Remove с признаком не stackable если стак удалился
                }
            }

            var backpackItem = Container.Nodes.FirstOrDefault(n => n.Item == item);
            if (item != null)
            {
                Container.Nodes.Remove(backpackItem);
                if (!moving)
                {
                    Global.Events.Raise(new ItemDropOffEvent() { Item = item, Owner = owner });
                }
                return true;
            }

            return false;
        }

        private bool RemoveStackable(Item item, Alive owner, int quantity)
        {
            var stackableItem = Container.Nodes
                .Where(n => n.Item.IdentifyName == item.IdentifyName)
                .MaxBy(n => n.Item.Quantity)
                .FirstOrDefault();

            var overflow = stackableItem.Item.QuantityRemove(quantity);
            if (overflow < 0)
            {
                item.Stackable = false;
                Remove(item, owner);
                item.Stackable = true;
                Remove(item, owner, overflow);
                return true;
            }
            else if (overflow == 0)
            {
                return false;
            }
            else return true;
        }

        public class BackpackItem : PhysicalObject<BackpackItem>
        {
            public Item Item { get; set; }

            protected override BackpackItem Self => this;

            protected override bool Containable => true;
        }

        public class BackpackContainer : BackpackItem
        {
            protected override bool Containable => true;
        }
    }
}