namespace Rogue
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;

    public static class ToDispay
    {
        private static Dictionary<ValueType, string> Cache = new Dictionary<ValueType, string>();

        public static string ToDisplay<T>(this T value) where T: struct
        {
            if (Cache.TryGetValue(value, out string display))
                return display;

            return AddToCache(value);
        }

        public static IEnumerable<T> All<T>(this Type enumType)
        {
            return Enum.GetValues(enumType).Cast<T>();
        }

        private static string AddToCache<T>(this T value) where T : struct
        {
            var memInfo = typeof(T).GetMember(value.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
            var description = ((DisplayAttribute)attributes[0]).Name;

            Cache.Add(value, description);

            return description;
        }
    }
}
