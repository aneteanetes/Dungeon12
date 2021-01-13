using System;
using System.Collections;

namespace Dungeon.Engine.Editable.PropertyTable
{
    public interface IPropertyTable : IEditable
    {
        PropertyTableRow Get(string key);

        void Set(string key, object value, Type type);

        /// <summary>
        /// Заблокировать <see cref="Set(string, object, Type)"/> и <see cref="Set(string, object, Type, int)"/>
        /// </summary>
        /// <param name="key"></param>
        void Lock(string key);

        void Set(string key, object value, Type type, int index);

        System.Collections.Generic.IEnumerable<PropertyTableRow> Properties { get; }

        public bool IsInitialized { get; set; }

        Action<IEnumerable, object> CollectionValueChanged { get; set; }

        void InitRuntime();
    }
}