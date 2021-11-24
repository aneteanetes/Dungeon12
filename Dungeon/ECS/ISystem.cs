using Dungeon.Utils.AttributesForInformation;
using Dungeon.View.Interfaces;

namespace Dungeon.ECS
{
    public interface ISystem
    {
        [Injected]
        ISceneLayer SceneLayer { get; set; }

        bool IsApplicable(ISceneObject sceneObject);

        void ProcessFocus(ISceneObject sceneObject);

        void ProcessUnfocus(ISceneObject sceneObject);
    }
}
