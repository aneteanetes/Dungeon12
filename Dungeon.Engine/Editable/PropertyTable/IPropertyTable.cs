using System;
using System.Collections;

namespace Dungeon.Engine.Editable.PropertyTable
{
    public interface IPropertyTable : IEditable
    {
        PropertyTableRow Get(string key);

        void Set(string key, object value, Type type);

        void Set(string key, object value, Type type, int index);

        System.Collections.Generic.IEnumerable<PropertyTableRow> Properties { get; }

        public bool IsInitialized { get; set; }

        Action<IEnumerable, object> CollectionValueChanged { get; set; }

        void InitRuntime();
    }
}