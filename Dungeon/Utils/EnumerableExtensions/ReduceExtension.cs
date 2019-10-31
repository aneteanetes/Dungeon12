namespace Dungeon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ReduceExtension
    {
        public static TVal Reduce<T, TVal>(this IEnumerable<T> @enum, TVal @in, Func<T, TVal, TVal> func)
        {
            foreach (var item in @enum)
            {
                @in = func(item, @in);
            }

            return @in;
        }
    }
}