namespace Rogue
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class ObjectToCollection
    {
        public static IEnumerable<T> InEnumerable<T>(this T obj) { yield return obj; }

        public static List<T> InList<T>(this T obj) => new List<T>() { obj };
    }
}