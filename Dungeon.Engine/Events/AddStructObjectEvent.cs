using Dungeon.Engine.Editable;
using Dungeon.Events;

namespace Dungeon.Engine.Events
{
    public class AddStructObjectEvent : IEvent
    {
        public DungeonEngineStructureObject StructureObject { get; set; }

        public AddStructObjectEvent(DungeonEngineStructureObject structureObject)
        {
            StructureObject = structureObject;
        }
    }
}