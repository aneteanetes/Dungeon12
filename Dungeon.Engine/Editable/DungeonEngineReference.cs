namespace Dungeon.Engine.Editable
{
    public class DungeonEngineReference
    {
        public string Title { get; set; }

        public string Path { get; set; }

        public string DbPath { get; set; }

        public DungeonEngineReferenceKind Kind { get; set; }
    }

    public enum DungeonEngineReferenceKind
    {
        Embedded = 0,
        Loadable = 1
    }
}
