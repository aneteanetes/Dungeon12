using Dungeon.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Engine.Editable.PropertyTable
{
    public interface IPropertyTable : IEditable
    {
        PropertyTableRow Get(string key);

        void Set(string key, object value, Type type);

        void Set(string key, object value, Type type, int index);

        IEnumerable<PropertyTableRow> Properties { get; }

        public bool IsInitialized { get; set; }
    }
}