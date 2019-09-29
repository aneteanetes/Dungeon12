using System.Collections.Generic;

namespace Rogue.Types
{
    public interface IGraph<T>
    {
        T This { get; }

        IEnumerable<T> Nodes { get; }
    }
}