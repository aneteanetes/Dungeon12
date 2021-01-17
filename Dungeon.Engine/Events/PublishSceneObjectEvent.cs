using Dungeon.Engine.Editable.Structures;
using Dungeon.Engine.Projects;
using Dungeon.Events;

namespace Dungeon.Engine.Events
{
    public class PublishSceneObjectEvent : IEvent
    {
        public SceneObject SceneObject { get; set; }

        public SceneObject Parent { get; set; }

        public StructureLayer Layer { get; set; }

        public PublishSceneObjectEvent(SceneObject sceneObject, SceneObject parent, StructureLayer structureLayer)
        {
            Parent = parent;
            Layer=structureLayer;
            this.SceneObject = sceneObject;
        }
    }
}
