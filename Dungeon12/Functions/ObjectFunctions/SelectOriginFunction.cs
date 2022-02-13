using Dungeon.View.Interfaces;
using Dungeon12.SceneObjects.UserInterface;

namespace Dungeon12.Functions.ObjectFunctions
{
    public class SelectOriginFunction : IFunction
    {
        public string Name => nameof(SelectOriginFunction);

        public bool Call(ISceneLayer layer, string objectId)
        {
            layer.Scene.GetLayer("ui").AddObjectCenter(new OriginSelectSceneObject(Global.Game.Party.Hero1));
            return true;
        }
    }
}
