using Dungeon.View.Interfaces;

namespace Dungeon12.Functions.ObjectFunctions
{
    public class SelectOriginFunction : IFunction
    {
        public string Name => nameof(SelectOriginFunction);

        public bool Call(ISceneLayer layer)
        {
            //layer.Scene.GetLayer("ui").AddObjectCenter(new NameEnterSceneObject(Global.Game.Party.Hero1));
            return true;
        }
    }
}
