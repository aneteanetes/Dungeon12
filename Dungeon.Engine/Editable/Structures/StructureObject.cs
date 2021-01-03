using Dungeon.Engine.Editable.ObjectTreeList;
using Dungeon.Utils;

namespace Dungeon.Engine.Editable
{
    public class StructureObject<T> : ObjectTreeListItem<T>
        where T: StructureObject<T>
    {
        [Hidden]
        public virtual StructureObjectType StructureType { get; set; }
    }

    public class StructureObject : StructureObject<StructureObject>
    {

    }
}
