using LiteDB;

namespace Dungeon.Resources.Resolvers
{
    public abstract class ResourceDatabaseResolver
    {
        public abstract LiteDatabase Resolve();
    }
}
