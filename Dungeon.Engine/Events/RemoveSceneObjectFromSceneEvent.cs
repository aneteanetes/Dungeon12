using Dungeon.Engine.Editable;
using Dungeon.Engine.Projects;
using Dungeon.Events;

namespace Dungeon.Engine.Events
{
    public class RemoveSceneObjectFromSceneEvent : IEvent
    {
        public SceneObject RootedObject { get; set; }

        public RemoveSceneObjectFromSceneEvent(SceneObject rootedObj) => RootedObject = rootedObj;
    }
}
