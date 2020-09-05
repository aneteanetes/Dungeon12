using Dungeon.Resources;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Engine.Engine
{
    public class DungeonEngineResourceDatabaseResolver : ResourceDatabaseResolver
    {
        public string PathToDb { get; private set; }

        public DungeonEngineResourceDatabaseResolver(string pathToDb) => PathToDb = pathToDb;

        public override LiteDatabase Resolve()
        {
            return new LiteDatabase(PathToDb);
        }
    }
}