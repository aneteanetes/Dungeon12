using Dungeon.Engine.Projects;
using Dungeon.Events;

namespace Dungeon.Engine.Events
{
    public class AddSceneObjectEvent : IEvent
    {
        public SceneObject SceneObject { get; set; }

        public bool Root { get; set; }

        public AddSceneObjectEvent(bool root=true)
        {
            this.Root = root;
        }
    }
}
