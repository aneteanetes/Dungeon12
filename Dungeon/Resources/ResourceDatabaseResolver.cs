using LiteDB;

namespace Dungeon.Resources
{
    public abstract class ResourceDatabaseResolver
    {
        public abstract LiteDatabase Resolve();
    }
}
