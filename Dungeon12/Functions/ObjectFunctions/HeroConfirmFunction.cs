using Dungeon.View.Interfaces;
using Dungeon12.SceneObjects.UserInterface.FractionSelect;
using Dungeon12.SceneObjects.UserInterface.SpecSelect;

namespace Dungeon12.Functions.ObjectFunctions
{
    public class HeroConfirmFunction : IFunction
    {
        public string Name => nameof(HeroConfirmFunction);

        public bool Call(ISceneLayer layer)
        {
            System.Console.WriteLine("Hero created");
            layer.Scene.GetLayer("ui").AddObjectCenter(new SpecSelectSceneObject(Global.Game.Party.Hero1));
            return true;
        }
    }
}
