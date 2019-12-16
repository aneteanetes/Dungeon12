using Dungeon12.Items;
using Dungeon12.Items.Enums;
using Dungeon.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entities.Items
{
    public class QuestItem : Item
    {
        public override ItemKind Kind => ItemKind.Quest;

        public override Point InventorySize => new Point(1, 1);
    }
}