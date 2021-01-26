#if Core
using Dungeon.GameObjects;
using Dungeon.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
#endif

namespace Dungeon.Data
{
    public class Persist
#if Core
        : GameComponent, IPersist
#endif
    {
        /// <summary>
        /// Внутреннее свойство для LiteDb
        /// </summary>
#if Core
        [Hidden]
#endif
        public int Id { get; set; }

        /// <summary>
        /// числовой Id который можно использовать в коде
        /// </summary>
        public int ObjectId { get; set; }

        public string IdentifyName { get; set; }

        public string Assembly { get; set; }

#if Core
        public static IEnumerable<T> Load<T>(Expression<Func<T, bool>> predicate = null, object cacheObject = default)
            where T : IPersist
            => Store.Entity<T>(predicate, cacheObject);

        public static T LoadOne<T>(Expression<Func<T, bool>> predicate = null, object cacheObject = default)
            where T : IPersist
            => Store.Entity<T>(predicate, cacheObject).FirstOrDefault();

        public static T LoadById<T>(string id, object cacheObject = default)
            where T : IPersist
            => Store.EntitySingle<T>(id, cacheObject);
#endif
    }
}