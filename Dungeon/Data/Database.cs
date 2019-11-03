﻿namespace Dungeon.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq.Expressions;
    using LiteDB;

    public static partial class Database
    {
        private static string MainPath = $@"{AppDomain.CurrentDomain.BaseDirectory}";

        public static IEnumerable<T> Entity<T>(Expression<Func<T, bool>> predicate = null)
        {
            if (!Directory.Exists(MainPath))
                Directory.CreateDirectory(MainPath);

            using (var db = new LiteDatabase($@"{MainPath}\Data.db"))
            {
                var collection = db.GetCollection<T>();
                
                if (predicate != null)
                    return collection.Find(predicate);

                return collection.FindAll();
            }
        }
    }
}