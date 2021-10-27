using Dungeon12.Entities.Alive;
using Dungeon.Events;
using Dungeon12.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Inventory
{
    public class ItemPickedUpEvent : IEvent
    {
        public Alive Owner { get; set; }

        public Item Item { get; set; }
    }
}
