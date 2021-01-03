using Dungeon.Engine.Editable;
using Dungeon.Events;

namespace Dungeon.Engine.Events
{
    public class AddStructObjectEvent : IEvent
    {
        public StructureObject StructureObject { get; set; }

        public AddStructObjectEvent(StructureObject structureObject)
        {
            StructureObject = structureObject;
        }
    }
}