using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Engine.Editable.PropertyTable
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class PropertyFilterAttribute : ValueAttribute
    {
        public PropertyFilterAttribute(object value) : base(value)
        {
        }
    }
}
