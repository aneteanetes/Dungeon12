using System;

namespace Dungeon.Utils.ReflectionExtensions
{
    public static class GetTypeGenericExtensions
    {
        public static Type GetTypeGeneric(this Type type, int index=0)
        {
            if (!type.IsGenericType)
                return type;

            return type.GetGenericArguments()[index];
        }
    }
}