using Dungeon.Events;

namespace Dungeon.Engine.Events
{
    public class PropGridFillEvent : IEvent
    {
        public object Target { get; set; }

        public PropGridFillEvent(object target) => Target = target;
    }
}
