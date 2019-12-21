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

        private readonly Type value;

        public T Value<T>(params object[] args) => value.NewAs<T>(args);
    }
}
