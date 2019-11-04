using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Data
{
    public class Instance
    {
        public Instance(string typeName)
        {
            value = GetInstanceFromAssemblyExtensions.GetType(typeName);
        }

        private readonly object value;

        public T Value<T>() => value.As<T>();
    }
}
