using Dungeon.Engine.Projects;
using Dungeon.Events;

namespace Dungeon.Engine.Events
{
    public class SceneObjectInObjectTreeSelectedEvent : IEvent
    {
        public SceneObject SceneObject { get; set; }
    }
}
