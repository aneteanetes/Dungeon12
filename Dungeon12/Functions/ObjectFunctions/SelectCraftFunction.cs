using Dungeon.View.Interfaces;
using Dungeon12.SceneObjects.UserInterface.CraftSelect;

namespace Dungeon12.Functions.ObjectFunctions
{
    public class SelectCraftFunction : IFunction
    {
        public string Name => nameof(SelectCraftFunction);

        public bool Call(ISceneLayer layer, string objectId)
        {
            layer.Scene.GetLayer("ui").AddObjectCenter(new CraftSelectSceneObject(Global.Game.Party.Hero1));
            return true;
        }
    }
}
