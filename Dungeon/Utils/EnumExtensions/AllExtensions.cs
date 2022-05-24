namespace Dungeon
{
    using Dungeon.Drawing;
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
        private static readonly Dictionary<ValueType, DrawColor> EnumColourCache = new Dictionary<ValueType, DrawColor>();

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

        public static DrawColor Colour<T>(this T value) where T : struct
        {
            if (EnumColourCache.TryGetValue(value, out DrawColor colour))
                return colour;

            return AddToEnumColourCache(value);
        }

        public static string ToDisplay<T>(this T? value) where T : struct
        {
            if (!value.HasValue)
                return null;

            return value.Value.ToDisplay<T>();
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

            if (description == null)
            {
                var global = DungeonGlobal.GetBindedGlobal();
                if (global != null)
                {
                    var strings = global.GetStringsClass();
                    description = strings[value.ToString()];
                }
            }

            if (description == null)
                description=$"ENUM-DISPLAY-STRING-NOT-FOUND (EVEN LOCALIZED): {value.GetType().FullName}.{value}";

            DisplayCache.Add(value, description);

            return description;
        }

        private static DrawColor AddToEnumColourCache<T>(this T value) where T : struct
        {
            var memInfo = typeof(T).GetMember(value.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DrawColourAttribute), false);

            DrawColor color = new();
            if (attributes.Length > 0)
            {
                color = ((DrawColourAttribute)attributes[0]).Colour;
            }

            EnumColourCache.Add(value, color);

            return color;
        }

        private static readonly Dictionary<object, object> ValueCache = new Dictionary<object, object>();

        public static T ToValue<T>(this object value)
        {
            if (ValueCache.TryGetValue(value, out object val))
                return (T)val;

            return AddToValueCache<T>(value);
        }

        public static T ToValue<TAttr,T>(this object value)
            where TAttr : ValueAttribute
        {
            var memInfo = value.GetType().GetMember(value.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(TAttr), false);
            if (attributes.Length != 0)
            {
                return ((TAttr)attributes[0]).Value.As<T>();
            }

            return default;
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
