using Dungeon.View.Interfaces;
using Dungeon12.SceneObjects.UserInterface.ConfirmHero;

namespace Dungeon12.Functions.ObjectFunctions
{
    public class HeroConfirmFunction : IFunction
    {
        public string Name => nameof(HeroConfirmFunction);

        public bool Call(ISceneLayer layer)
        {
            layer.Scene.GetLayer("ui").AddObjectCenter(new ConfirmHeroSceneObject(Global.Game.Party.Hero1));
            return true;
        }
    }
}