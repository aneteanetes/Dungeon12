using Dungeon.Items;
using Dungeon.Items.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entities.Items
{
    public class QuestItem : Item
    {
        public override Stats AvailableStats => Stats.None;

        public override ItemKind Kind => ItemKind.Quest;
    }
}