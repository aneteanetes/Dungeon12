using Dungeon.Engine.Editable;
using Dungeon.Engine.Projects;
using Dungeon.Events;

namespace Dungeon.Engine.Events
{
    public class RemoveSceneObjectFromSceneEvent : IEvent
    {
        public DungeonEngineSceneObject RootedObject { get; set; }

        public RemoveSceneObjectFromSceneEvent(DungeonEngineSceneObject rootedObj) => RootedObject = rootedObj;
    }
}
