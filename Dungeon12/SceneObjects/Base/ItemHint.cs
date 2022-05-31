using Dungeon;
using Dungeon.Drawing;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.Entities;

namespace Dungeon12.SceneObjects.Base
{
    internal class ItemHint : EmptySceneObject, ISceneObjectHosted
    {
        public ISceneObject Host { get; set; }

        public ItemHint(Item item)
        {
            this.Width = 355;

            this.AddBorder();

            this.AddText(item.Name
                .SegoeUIBold()
                .InSize(14)
                .WithWordWrap()
                .InColor(item.Rarity.Colour()), topOffset, leftOffset);

            topOffset+=20;

            this.AddText(Global.Strings[item.Type]
                .SegoeUIBold()
                .InSize(12)
                .WithWordWrap()
                .InColor(new DrawColor(255, 209, 0)), topOffset, leftOffset);

            topOffset+=20;

            this.AddText(Global.Strings[item.Slot]
                .SegoeUIBold()
                .InSize(12)
                .WithWordWrap(), topOffset, leftOffset);

            this.AddText(Global.Strings[item.Material]
                .SegoeUIBold()
                .InSize(12)
                .WithWordWrap()
                .InColor(item.Material.Colour()), this.Width-leftOffset, topOffset);

            topOffset+=20;

            if (item.Type == Entities.Enums.ItemType.Armor || item.Type== Entities.Enums.ItemType.Shield)
            {
                var armor = Global.Strings["ArmorDefence"];
                this.AddText($"{armor}:{item.Armor}"
                    .SegoeUIBold()
                    .InSize(12), leftOffset, topOffset);
            }
            else if (item.Type == Entities.Enums.ItemType.Weapon)
            {
                this.AddText(Global.Strings[item.AttackType]
                    .SegoeUIBold()
                    .InSize(12)
                    .InColor(new DrawColor(197, 199, 196)), leftOffset, topOffset);
            }

            topOffset+=20;

            var durability = Global.Strings["ArmorDurability"];

            this.AddText($"{durability}: {item.Durability}/{item.MaxDurability}"
                .SegoeUIBold()
                .InSize(12)
                .WithWordWrap(), this.Width-leftOffset, topOffset);
        }

        private double leftOffset = 10;
        private double topOffset = 10;
    }
}