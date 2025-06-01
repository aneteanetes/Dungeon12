using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using System.Diagnostics;

namespace Dungeon.Scenes
{
    [DebuggerDisplay("This is scene ({scene}) or scenelayer ({sceneLayer}) object.")]
    internal class SceneLayerSceneObject : EmptySceneObject
    {
        ISceneLayer sceneLayer;
        IScene scene;

        public SceneLayerSceneObject(ISceneLayer sceneLayer=null, IScene scene=null)
        {
            this.scene = scene;
            this.sceneLayer = sceneLayer;
        }

        public override string ToString()
        {
            return $"This is scene ({scene}) or scenelayer ({sceneLayer}) object.";
        }
    }
}
