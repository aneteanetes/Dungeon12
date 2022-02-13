using Dungeon.View.Interfaces;
using Dungeon12.SceneObjects.UserInterface.FractionSelect;

namespace Dungeon12.Functions.ObjectFunctions
{
    public class SelectFractionFunction : IFunction
    {
        public string Name => nameof(SelectFractionFunction);

        public bool Call(ISceneLayer layer, string objectId)
        {
            layer.Scene.GetLayer("ui").AddObjectCenter(new FractionSelectSceneObject(Global.Game.Party.Hero1));
            return true;
        }
    }
}
