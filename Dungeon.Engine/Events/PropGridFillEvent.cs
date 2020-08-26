using Dungeon.Events;
using System;

namespace Dungeon.Engine.Events
{
    public class PropGridFillEvent : IEvent
    {
        public object Target { get; set; }

        public Type TargetType { get; set; }

        public PropGridFillEvent(object target) => Target = target;
    }
}
