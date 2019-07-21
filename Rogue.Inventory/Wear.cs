namespace Rogue.Inventory
{
    using Rogue.Items;
    using Rogue.Items.Enums;
    using System;

    public class Wear
    {
        public Item Helm { get; set; }

        public Item Armor { get; set; }

        public Item Boots { get; set; }

        public Item Weapon { get; set; }

        public Item OffHand { get; set; }

        public (bool success, Item oldItem) PutOn(Item item)
        {
            bool success = false;
            Item wasItem = null;

            switch (item.Kind)
            {
                case ItemKind.Weapon:
                case ItemKind.Helm:
                case ItemKind.Armor:
                case ItemKind.Boots:
                case ItemKind.OffHand:
                    {
                        var kind = item.Kind.ToString();
                        var itm = this.GetProperty<Item>(kind);

                        if (OnPutOn(item, itm))
                        {
                            wasItem = itm;
                            this.SetProperty(kind, item);
                            success = true;
                        }
                        else
                        {
                            success = false;
                        }
                        break;
                    }
                default:
                    success = false;
                    break;
            }

            return (success, wasItem);
        }

        public (bool success, Item putOffItem) PutOff(ItemKind kind)
        {
            bool success = false;
            Item item = null;

            switch (kind)
            {
                case ItemKind.Weapon:
                case ItemKind.Helm:
                case ItemKind.Armor:
                case ItemKind.Boots:
                case ItemKind.OffHand:
                    {
                        var itm = this.GetProperty<Item>(kind.ToString());

                        if (OnPutOff(itm))
                        {
                            this.SetProperty<Wear, Item>(kind.ToString(), default);
                            success = true;
                        }
                        break;
                    }
                default:
                    break;
            }

            return (success, item);
        }

        public Func<Item, Item, bool> OnPutOn = (x, y) => true;

        public Func<Item, bool> OnPutOff = x => true;
    }
}