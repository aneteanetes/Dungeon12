using Dungeon.View.Interfaces;
using Dungeon12.SceneObjects.UserInterface.FractionSelect;
using Dungeon12.SceneObjects.UserInterface.SpecSelect;

namespace Dungeon12.Functions.ObjectFunctions
{
    public class SelectSpecFunction : IFunction
    {
        public string Name => nameof(SelectSpecFunction);

        public bool Call(ISceneLayer layer, string objectId)
        {
            layer.Scene.GetLayer("ui").AddObjectCenter(new SpecSelectSceneObject(Global.Game.Party.Hero1));
            return true;
        }
    }
}
