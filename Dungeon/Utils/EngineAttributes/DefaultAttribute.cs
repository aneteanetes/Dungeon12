using System;

namespace Dungeon.Utils
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class DefaultAttribute : ValueAttribute
    {
        public DefaultAttribute(object value) : base(value)
        {
        }
    }
}
