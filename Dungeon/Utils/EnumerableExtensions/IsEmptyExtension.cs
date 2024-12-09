namespace Dungeon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class IsEmptyExtension
    {

        public static void ForEach<T>(this IEnumerable<T> @enum, Action<T> action)
        {
            foreach (var item in @enum)
            {
                action(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> @enum, Action<T,int> action)
        {
            for (int i = 0; i < @enum.Count(); i++)
            {
                var item = @enum.ElementAtOrDefault(i);
                action(item, i);
            }
        }
    }
}