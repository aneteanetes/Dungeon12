namespace Dungeon
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public static class AllExtensions
    {
        public static IEnumerable<T> All<T>(this Type enumType)
        {
            return Enum.GetValues(enumType).Cast<T>();
        }

        private static readonly Dictionary<ValueType, string> DisplayCache = new Dictionary<ValueType, string>();

        /// <summary>
        /// enum To display
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDisplay<T>(this T value) where T: struct
        {
            if (DisplayCache.TryGetValue(value, out string display))
                return display;

            return AddToDisplayCache(value);
        }

        public static string Display<T>(this T value)
            where T : MemberInfo
        {
            var displayAttr = Attribute.GetCustomAttributes(value, typeof(DisplayAttribute)).FirstOrDefault().As<DisplayAttribute>();
            if (displayAttr != default)
                return displayAttr.Name;

            return value.Value<ValueAttribute, string>();
        }

        private static string AddToDisplayCache<T>(this T value) where T : struct
        {
            var memInfo = typeof(T).GetMember(value.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);

            string description = "";
            if (attributes.Length > 0)
            {
                description = ((DisplayAttribute)attributes[0]).Name;
            }
            else
            {
                attributes = memInfo[0].GetCustomAttributes(typeof(TitleAttribute), false);
                if (attributes.Length > 0)
                {
                    description = ((TitleAttribute)attributes[0]).Value.As<string>();
                }
            }

            DisplayCache.Add(value, description);

            return description;
        }

        private static readonly Dictionary<object, object> ValueCache = new Dictionary<object, object>();

        public static T ToValue<T>(this object value)
        {
            if (ValueCache.TryGetValue(value, out object val))
                return (T)val;

            return AddToValueCache<T>(value);
        }


        private static T AddToValueCache<T>(this object value)
        {
            var memInfo = value.GetType().GetMember(value.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(ValueAttribute), false);
            if (attributes.Length != 0)
            {
                var val = ((ValueAttribute)attributes[0]).Value;

                ValueCache.Add(value, val);

                return (T)val;
            }
            else
            {
                var val = memInfo[0].GetCustomAttributes(false);
                var valueAttr = val.FirstOrDefault(x => x is T);

                if (valueAttr != default)
                {
                    ValueCache.Add(value, valueAttr);
                    return (T)valueAttr;
                }
            }

            return default;
        }
    }
}
