namespace Rogue.Items
{
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
                case Enums.ItemKind.Weapon:
                    if (OnPutOn(item, this.Weapon))
                    {
                        this.Weapon = item;
                        wasItem = this.Weapon;
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                    break;
                case Enums.ItemKind.Helm:
                    if (OnPutOn(item, this.Helm))
                    {
                        this.Helm = item;
                        wasItem = this.Helm;
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                    break;
                case Enums.ItemKind.Armor:
                    if (OnPutOn(item, this.Armor))
                    {
                        this.Armor = item;
                        wasItem = this.Armor;
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                    break;
                case Enums.ItemKind.Boots:
                    if (OnPutOn(item, this.Boots))
                    {
                        this.Boots = item;
                        wasItem = this.Boots;
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                    break;
                case Enums.ItemKind.OffHand:
                    if (OnPutOn(item, this.OffHand))
                    {
                        this.OffHand = item;
                        wasItem = this.OffHand;
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