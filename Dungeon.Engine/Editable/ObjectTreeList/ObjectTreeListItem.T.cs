using LiteDB;
using System.Collections.ObjectModel;

namespace Dungeon.Engine.Editable.ObjectTreeList
{
    public class ObjectTreeListItem<T> : ObjectTreeListItem
        where T : ObjectTreeListItem<T>

    {
        [BsonIgnore]
        public T ParentT { get; set; }
    }
}
