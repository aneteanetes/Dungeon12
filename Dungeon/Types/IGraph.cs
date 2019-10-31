using System.Collections.Generic;

namespace Dungeon.Types
{
    public interface IGraph<T>
    {
        T This { get; }

        IEnumerable<T> Nodes { get; }
    }
}