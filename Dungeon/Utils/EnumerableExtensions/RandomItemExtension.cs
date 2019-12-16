using System.Collections.Generic;
using System.Linq;

namespace Dungeon
{
    public static class RandomItemExtension
    {
        public static T Random<T>(this IEnumerable<T> @enum, int start = 0, int max = -1)
        {
            var maxRange = max == -1 ? @enum.Count() - 1 : max;
            var random = RandomDungeon.Range(start, maxRange);
            return @enum.ElementAtOrDefault(random);
        }
    }
}