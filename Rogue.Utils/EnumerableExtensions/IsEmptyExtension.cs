namespace Rogue
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class IsEmptyExtension
    {
        public static bool IsNotEmpty<T>(this IEnumerable<T> @enum)
        {
            return @enum != null && @enum.Count() > 0;
        }

        public static void ForEach<T>(this IEnumerable<T> @enum, Action<T> action)
        {
            foreach (var item in @enum)
            {
                action(item);
            }
        }
    }
}