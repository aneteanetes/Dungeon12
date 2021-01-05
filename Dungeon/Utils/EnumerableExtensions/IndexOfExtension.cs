using System;

namespace Dungeon.Utils.EnumerableExtensions
{
    public static class IndexOfExtension
    {
        public static int IndexOf(this System.Collections.IEnumerable enumerable, object @ref)
        {
            if (enumerable == default)
                return -2;

            var i = 0;
            var @enum = enumerable.GetEnumerator();
            while (@enum.MoveNext())
            {
                var current = @enum.Current;
                if (current.Equality(@ref))
                    return i;

                i++;
            }

            return -1;
        }
    }
}