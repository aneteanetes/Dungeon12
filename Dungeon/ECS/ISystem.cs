using Dungeon.Control;
using Dungeon.Utils.AttributesForInformation;
using Dungeon.View.Interfaces;

namespace Dungeon.ECS
{
    public interface ISystem
    {
        bool IsApplicable(ISceneObject sceneObject);

        void ProcessFocus(ISceneObject sceneObject);

        void ProcessUnfocus(ISceneObject sceneObject);

        void ProcessClick(PointerArgs pointerArgs, ISceneObject sceneObject);

        void ProcessGlobalClickRelease(PointerArgs pointerArgs);
    }
}