namespace Dungeon.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using Dungeon.Types;
    using LiteDB;

    public static partial class Database
    {
        private static string MainPath = $@"{AppDomain.CurrentDomain.BaseDirectory}";
        
        /// <summary>
        /// 
        /// <para>
        /// [Кэшируемый]
        /// </para>
        /// </summary>
        public static IEnumerable<T> Entity<T>(Expression<Func<T,bool>> predicate=null, object cacheObject = default)
        {
            var key = new CompositeKey<object>()
            {
                Owner=typeof(T),
                Value=predicate ?? cacheObject
            };

            if (!___EntityCache.TryGetValue(key, out var value))
            {
                value = EntityQuery(predicate).ToArray();
                ___EntityCache.Add(key, value);
            }

            return value.As<IEnumerable<T>>();
        }
        private static readonly Dictionary<CompositeKey<object>, object> ___EntityCache = new Dictionary<CompositeKey<object>, object>();


        public static IEnumerable<T> EntityQuery<T>(Expression<Func<T, bool>> predicate = null)
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