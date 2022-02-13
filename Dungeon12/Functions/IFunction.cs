using Dungeon.View.Interfaces;

namespace Dungeon12.Functions
{
    public interface IFunction
    {
        string Name { get; }

        bool Call(ISceneLayer layer, string objectId);
    }
}
