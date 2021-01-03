using Dungeon.Engine.Editable.ObjectTreeList;

namespace Dungeon.Engine.Editable
{
    public class DungeonEngineStructureObject : ObjectTreeListItem<DungeonEngineStructureObject>
    {
        public DungeonEngineStructureObjectType StructureType { get; set; }
    }
}
