using Dungeon.Entities.Alive;
using Dungeon.Events;
using Dungeon.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Inventory
{
    public class ItemPickedUpEvent : IEvent
    {
        public Alive Owner { get; set; }

        public Item Item { get; set; }
    }
}
