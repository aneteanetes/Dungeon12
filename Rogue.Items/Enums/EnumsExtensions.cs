namespace Rogue.Items.Enums
{
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;

    public static class EnumsExtensions
    {
        private static Dictionary<ValueType, IDrawColor> Cache = new Dictionary<ValueType, IDrawColor>();

        public static IDrawColor Color<T>(this T value) where T : struct
        {
            if (Cache.TryGetValue(value, out var color))
                return color;

            return AddToCache(value);
        }

        private static IDrawColor AddToCache<T>(this T value) where T : struct
        {
            var memInfo = typeof(T).GetMember(value.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(ColorAttribute), false);
            var description = ((ColorAttribute)attributes[0]).DrawColor;

            Cache.Add(value, description);

            return description;
        }
    }
}
