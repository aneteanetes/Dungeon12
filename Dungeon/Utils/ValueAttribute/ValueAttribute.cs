namespace Dungeon
{
    using FastMember;
    using System;
    using System.Linq;
    using System.Reflection;

    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class ValueAttribute : Attribute
    {
        public object Value { get; set; }

        public ValueAttribute(object value) => Value = value;
    }

    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class IconAttribute : ValueAttribute
    {
        public IconAttribute(object value) : base(value)
        {
        }
    }

    public static class ValueAttributeExtensions
    {
        public static object ValueAttribute<T>(this Assembly obj)
            where T : ValueAttribute
        {
            return GetAttribute<T>(Attribute.GetCustomAttributes(obj, typeof(T)));
        }

        public static object ValueAttribute<T>(this MemberInfo obj)
            where T : ValueAttribute
        {
            return GetAttribute<T>(Attribute.GetCustomAttributes(obj, typeof(T)));
        }

        public static object ValueAttribute<T>(this Module obj)
            where T : ValueAttribute
        {
            return GetAttribute<T>(Attribute.GetCustomAttributes(obj, typeof(T)));
        }

        public static object ValueAttribute<T>(this ParameterInfo obj)
            where T : ValueAttribute
        {
            return GetAttribute<T>(Attribute.GetCustomAttributes(obj, typeof(T)));
        }

        public static object ValueAttribute<T>(this Member obj)
            where T : ValueAttribute
        {
            var m = typeof(Member)
                .GetField("member", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(obj);

            return GetAttribute<T>(Attribute.GetCustomAttributes(m as MemberInfo, typeof(T)));
        }

        private static object GetAttribute<T>(Attribute[] attributes) where T : ValueAttribute => (attributes.FirstOrDefault(x => x is T) as T).Value;

        public static object ValueAttribute(this Assembly asm) => asm.ValueAttribute<ValueAttribute>();

        public static object ValueAttribute(this Member asm) => asm.ValueAttribute<ValueAttribute>();

        public static object ValueAttribute(this ParameterInfo asm) => asm.ValueAttribute<ValueAttribute>();

        public static object ValueAttribute(this Module asm) => asm.ValueAttribute<ValueAttribute>();

        public static object ValueAttribute(this MemberInfo asm) => asm.ValueAttribute<ValueAttribute>();
    }
}