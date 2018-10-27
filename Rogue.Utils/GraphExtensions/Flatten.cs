using System;
using System.Collections.Generic;
using System.Linq;
using Rogue.Types;

namespace Rogue
{
    public static class Flatten
    {
        public static IEnumerable<T> Flat<T>(this IGraph<T> source) where T : IGraph<T>
        {
            return source.Nodes.SelectMany(s => s.Nodes.Any()
                ? s.Nodes.Concat(s.Flat())
                : new T[] { s.This })
            .Distinct();
        }

        public static IEnumerable<T> FlatThis<T>(this IGraph<T> source) where T : IGraph<T>
        {
            return new T[] { source.This }.Concat(source.Nodes.SelectMany(s => s.Nodes.Any()
                 ? s.Nodes.Concat(s.Flat())
                 : new T[] { s.This })
            .Distinct());
        }

        public static float FlatSum<T>(this IGraph<T> source, Func<T, float> selector) where T : IGraph<T>
        {
            var flatted = source.FlatThis();

            float result = default(float);

            foreach (var flatItem in flatted)
            {
                result = result + selector(flatItem);
            }

            return result;
        }
    }
}