using Dungeon.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dungeon.Data
{
    public class Persist : IPersist
    {
        /// <summary>
        /// Внутреннее свойство для LiteDb
        /// </summary>
        [Hidden]
        public int Id { get; set; }

        /// <summary>
        /// числовой Id который можно использовать в коде
        /// </summary>
        public int ObjectId { get; set; }

        public string IdentifyName { get; set; }

        public string Assembly { get; set; }

        public static IEnumerable<T> Load<T>(Expression<Func<T, bool>> predicate = null, object cacheObject = default)
            where T : IPersist
            => Store.Entity<T>(predicate, cacheObject);

        public static T LoadOne<T>(Expression<Func<T, bool>> predicate = null, object cacheObject = default)
            where T : IPersist
            => Store.Entity<T>(predicate, cacheObject).FirstOrDefault();

        public static T LoadById<T>(string id, object cacheObject = default)
            where T : IPersist
            => Store.EntitySingle<T>(id, cacheObject);
    }
}