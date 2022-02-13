using Dungeon.View.Interfaces;
using Dungeon12.SceneObjects.UserInterface;

namespace Dungeon12.Functions.ObjectFunctions
{
    public class NameEnterWindowFunction : IFunction
    {
        public string Name => "NameEnterWindow";

        public bool Call(ISceneLayer layer, string objectId)
        {
            layer.Scene.GetLayer("ui").AddObjectCenter(new NameEnterSceneObject(Global.Game.Party.Hero1));
            return true;
        }
    }
}
