using System.Collections.Generic;
using System.Linq;
using Rogue.Types;

namespace Rogue
{
    public static class Flatten
    {
        public static IEnumerable<T> Flat<T>(this IGraph<T> source) where T : IGraph<T>
        {
            return source.Nodes.SelectMany(
              s => s.Nodes.Any()
                ? s.Nodes.Concat(s.Flat())
                : new T[] { s.This });
        }
    }
}