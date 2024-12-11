namespace Dungeon.Resources.Processing
{
    public abstract class ResourceProcessor
    {
        public abstract bool IsCanProcess(string path);

        public abstract void Process(string path, Resource resource);
    }
}
