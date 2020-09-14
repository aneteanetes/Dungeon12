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

        private LiteDatabase db;

        public override LiteDatabase Resolve()
        {
            if (db == default)
            {
                db = new LiteDatabase(PathToDb);
            }

            return db;
        }
    }
}