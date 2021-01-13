using Dungeon.Engine.Editable.ObjectTreeList;
using Dungeon.Types;
using Dungeon.Utils;
using Dungeon.Utils.ReflectionExtensions;
using LiteDB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Dungeon.Engine.Editable.PropertyTable
{
    public abstract class SimplePropertyTable : IPropertyTable
    {
        [Hidden]
        public List<PropertyTableRow> PropertyTable { get; set; } = new List<PropertyTableRow>();

        public void InitTable()
        {
            if (PropertyTable == default || PropertyTable.Count == 0)
            {
                PropertyTable = InitializePropertyTable();
                IsInitialized = true;
            }
        }

        [BsonIgnore]
        public IEnumerable<PropertyTableRow> Properties => PropertyTable;

        [Hidden]
        public bool IsInitialized { get; set; }

        [Hidden]
        [BsonIgnore]
        public Action<IEnumerable, object> CollectionValueChanged { get; set; }

        public PropertyTableRow Get(string key) => PropertyTable.FirstOrDefault(x => x.Name == key);

        public void Lock(string key)
        {
            var row = Get(key);
            if (row != default)
                row.Locked = true;
        }

        public void Set(string key, object value, Type type)
        {
            var row = Get(key);
            if (row.Locked)
                return;

            row.Value = value;
            row.Type = type;
        }

        public void Set(string key, object value, Type type, int index)
        {
            var row = PropertyTable.ElementAtOrDefault(index);
            if (row != default)
            {
                if (row.Locked)
                    return;

                row.Value = value;
                row.Type = type;
            }
        }

        protected virtual List<PropertyTableRow> InitializePropertyTable()
        {
            var type = this.GetType();
            var bodyProps = type.GetProperties().Where(prop =>
            {
                var hidden = Attribute.GetCustomAttributes(prop)
                       .FirstOrDefault(x => x.GetType() == typeof(HiddenAttribute)) != default;
                if (hidden)
                    return false;

                if (!prop.CanWrite)
                    return false;

                return true;
            })
            .Select(x => {
                var row = new PropertyTableRow(x.Name, this.GetPropertyExprRaw(x.Name), x.PropertyType);

                row.Display = x.Value<TitleAttribute, string>() ?? x.Display();

                if (typeof(System.Collections.IEnumerable).IsAssignableFrom(x.PropertyType) && x.PropertyType != typeof(string))
                {
                    row.Value = null;
                }

                return row;
            })
            .ToList();

            return bodyProps;
        }

        public virtual void Commit() { }

        public virtual void InitRuntime() { }
    }
}
