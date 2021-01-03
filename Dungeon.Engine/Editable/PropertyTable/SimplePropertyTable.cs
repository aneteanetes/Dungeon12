using Dungeon.Engine.Editable.ObjectTreeList;
using Dungeon.Types;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon.Engine.Editable.PropertyTable
{
    public abstract class SimplePropertyTable : ObjectTreeListItem, IPropertyTable
    {
        public List<PropertyTableRow> PropertyTable { get; set; } = new List<PropertyTableRow>();

        public void InitTable()
        {
            if (PropertyTable == default || PropertyTable.Count==0)
            {
                PropertyTable = InitializePropertyTable();
            }
        }

        [BsonIgnore]
        public IEnumerable<PropertyTableRow> Properties => PropertyTable;

        public PropertyTableRow Get(string key) => PropertyTable.FirstOrDefault(x => x.Name == key);

        public void Set(string key, object value, Type type)
        {
            var row = Get(key);
            row.Value = value;
            row.Type = type;
        }

        public void Set(string key, object value, Type type, int index)
        {
            var row = PropertyTable.ElementAtOrDefault(index);
            if (row != default)
            {
                row.Value = value;
                row.Type = type;
            }
        }

        protected abstract List<PropertyTableRow> InitializePropertyTable();

        public virtual void Commit()
        {
            
        }
    }
}
