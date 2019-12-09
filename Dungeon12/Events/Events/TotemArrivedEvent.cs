using Dungeon.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Events.Events
{
    public class TotemArrivedEvent : IEvent
    {
        public TotemArrivedEvent(object totem) => Totem = totem;

        public object Totem { get; set; }
    }
}
