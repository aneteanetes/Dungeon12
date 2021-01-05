using System;

namespace Dungeon.Utils.ReflectionExtensions
{
    public static class IsEnumerableExtension
    {
        public static bool IsEnumerable(this Type type)=> typeof(System.Collections.IEnumerable).IsAssignableFrom(type) && type != typeof(string);
    }
}
