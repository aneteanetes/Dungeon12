using Dungeon.Engine.Projects;
using Dungeon.Events;

namespace Dungeon.Engine.Events
{
    public class PublishSceneObjectEvent : IEvent
    {
        public SceneObject SceneObject { get; set; }

        public PublishSceneObjectEvent(SceneObject sceneObject)
        {
            this.SceneObject = sceneObject;
        }
    }
}
