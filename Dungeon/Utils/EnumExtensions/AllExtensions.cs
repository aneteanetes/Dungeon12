namespace Dungeon
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;

    public static class AllExtensions
    {
        public static IEnumerable<T> All<T>(this Type enumType)
        {
            return Enum.GetValues(enumType).Cast<T>();
        }

        private static readonly Dictionary<ValueType, string> DisplayCache = new Dictionary<ValueType, string>();

        public static string ToDisplay<T>(this T value) where T: struct
        {
            if (DisplayCache.TryGetValue(value, out string display))
                return display;

            return AddToDisplayCache(value);
        }

        private static string AddToDisplayCache<T>(this T value) where T : struct
        {
            var memInfo = typeof(T).GetMember(value.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
            var description = ((DisplayAttribute)attributes[0]).Name;

            DisplayCache.Add(value, description);

            return description;
        }

        private static readonly Dictionary<object, object> ValueCache = new Dictionary<object, object>();

        public static T ToValue<T>(this object value)
        {
            if (ValueCache.TryGetValue(value, out object val))
                return (T)val;

            return (T)AddToValueCache(value);
        }


        private static object AddToValueCache(this object value)
        {
            var memInfo = value.GetType().GetMember(value.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(ValueAttribute), false);
            var val = ((ValueAttribute)attributes[0]).Value;

            ValueCache.Add(value, val);

            return val;
        }
    }
}
