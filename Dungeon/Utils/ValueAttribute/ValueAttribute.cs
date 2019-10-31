namespace Dungeon
{
    using System;

    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class ValueAttribute : Attribute
    {
        public object Value { get; set; }

        public ValueAttribute(object value) => Value = value;
    }
}
