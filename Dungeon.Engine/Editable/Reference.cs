namespace Dungeon.Engine.Editable
{
    public class Reference
    {
        public string Title { get; set; }

        public string Path { get; set; }

        public string DbPath { get; set; }

        public ReferenceKind Kind { get; set; }
    }

    public enum ReferenceKind
    {
        Embedded = 0,
        Loadable = 1
    }
}
