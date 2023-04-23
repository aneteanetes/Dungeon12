namespace Dungeon.SceneObjects.Grouping
{
    public class ObjectGroupItem
    {
        public ObjectGroupProperty Property { get; set; }

        public bool IsActive => Property.Value;
    }
}
