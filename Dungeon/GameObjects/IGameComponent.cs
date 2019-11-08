using Dungeon.Entities.Alive;
using Dungeon.Map;

namespace Dungeon.View.Interfaces
{
    public interface IGameComponent
    {
        ISceneObject SceneObject { get; set; }

        void SetView(ISceneObject sceneObject);
    }
}
