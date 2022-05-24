using Dungeon.View.Interfaces;
using Dungeon12.SceneObjects.Base;

namespace Dungeon12.ECS.Components
{
    internal interface IMouseHint
    {
        ISceneObjectHosted CreateMouseHint();
    }
}
