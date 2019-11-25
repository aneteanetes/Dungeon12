using Dungeon.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Dungeon.Data
{
    public class Persist : GameComponent, IPersist
    {
        /// <summary>
        /// Внутреннее свойство для LiteDb
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// числовой Id который можно использовать в коде
        /// </summary>
        public int ObjectId { get; set; }

        public string IdentifyName { get; set; }

        public string Assembly { get; set; }

        public static IEnumerable<T> Load<T>(Expression<Func<T, bool>> predicate = null, object cacheObject = default)
            where T : IPersist
            => Database.Entity<T>(predicate, cacheObject);
    }
}