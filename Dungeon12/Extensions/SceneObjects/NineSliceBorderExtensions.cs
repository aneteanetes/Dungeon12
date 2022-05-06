using Dungeon.View.Interfaces;
using Dungeon12.SceneObjects.Base;

namespace Dungeon12
{
    static internal class NineSliceBorderExtensions
    {
        public static void AddBorder(this ISceneObject sceneObject)
        {
            sceneObject.AddChild(new NineSliceBorder(sceneObject.Width, sceneObject.Height));
        }
    }
}
