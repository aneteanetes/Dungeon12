using Dungeon.Resources;
using System;
using System.Linq;

namespace Dungeon.Utils.ReflectionExtensions
{
    public static class ExtractGenericCollectionExtension
    {
        public static Type ExtractGenericCollection(this Type type)
        {
            if (!type.IsGenericType)
                return default;

            var className = type.FullName;

            var openGeneric = className.IndexOf("[");
            var genericClassName = className.Substring(0, openGeneric);
            return  ResourceLoader.LoadType(genericClassName);
        }

        public static Type ExtractGenericCollectionItem(this Type type)
        {
            if (!type.IsGenericType)
                return default;

            var className = type.FullName;
            var openGeneric = className.IndexOf("[");

            var generic = className.Substring(openGeneric)
                    .Split("]", StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Replace("[", "").Replace("]", ""))
                    .FirstOrDefault();

            return ResourceLoader.LoadType(generic);
        }
    }
}
