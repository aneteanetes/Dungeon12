using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon
{
    public static class ShuffleExtensions
    {
        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = RandomGlobal.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }

        public static T[] Shuffle<T>(this T[] list)
        {
            int n = list.Length;
            while (n > 1)
            {
                n--;
                int k = RandomGlobal.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> @enum)
        {
            var list = @enum.ToArray();
            return list.Shuffle();
        }
    }
}
