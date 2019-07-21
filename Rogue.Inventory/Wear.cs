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
                    if (OnPutOn(item, this.Weapon))
                    {
                        wasItem = this.Weapon;
                        this.Weapon = item;
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                    break;
                case ItemKind.Helm:
                    if (OnPutOn(item, this.Helm))
                    {
                        wasItem = this.Helm;
                        this.Helm = item;
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                    break;
                case ItemKind.Armor:
                    if (OnPutOn(item, this.Armor))
                    {
                        wasItem = this.Armor;
                        this.Armor = item;
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                    break;
                case ItemKind.Boots:
                    if (OnPutOn(item, this.Boots))
                    {
                        wasItem = this.Boots;
                        this.Boots = item;
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                    break;
                case ItemKind.OffHand:
                    if (OnPutOn(item, this.OffHand))
                    {
                        wasItem = this.OffHand;
                        this.OffHand = item;
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                    break;
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
                    if (OnPutOff(this.Weapon))
                    {
                        this.Weapon = null;
                        success = true;
                    }
                    break;
                case ItemKind.Helm:
                    if (OnPutOff(this.Helm))
                    {
                        this.Helm = null;
                        success = true;
                    }
                    break;
                case ItemKind.Armor:
                    if (OnPutOff(this.Armor))
                    {
                        this.Armor = null;
                        success = true;
                    }
                    break;
                case ItemKind.Boots:
                    if (OnPutOff(this.Boots))
                    {
                        this.Boots = null;
                        success = true;
                    }
                    break;
                case ItemKind.OffHand:
                    if (OnPutOff(this.OffHand))
                    {
                        this.OffHand = null;
                        success = true;
                    }
                    break;
                default:
                    break;
            }

            return (success, item);
        }

        public Func<Item, Item, bool> OnPutOn = (x, y) => true;

        public Func<Item, bool> OnPutOff = x => true;
    }
}