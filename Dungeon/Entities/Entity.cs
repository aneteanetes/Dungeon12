using Dungeon.Data;
using Dungeon.Map;
using Dungeon.Network;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Dungeon.Entities
{
    public class Entity : VisualObject
    {
        public string IdentifyName { get; set; }

        public MapObject MapObject { get; set; }

        public string Assembly { get; set; }
    }

    public class DataEntity<TEntity,TPersist> : Entity
        where TEntity : DataEntity<TEntity, TPersist>
        where TPersist : Persist
    {
        protected virtual void Init(TPersist dataClass)
        {
        }

        public static TEntity Load(string id)
        {
            var entity = typeof(TEntity).New<TEntity>();
            if (entity == default)
                return default;

            var dataClass = Database.Entity<TPersist>(x => x.IdentifyName == id, id).FirstOrDefault();
            if (dataClass != default)
            {
                entity.Init(dataClass);
            }

            return entity;
        }

        public static TEntity Load(Expression<Func<TPersist, bool>> filterOne, object cacheObject=default)
        {
            var entity = typeof(TEntity).New<TEntity>();
            var dataClass = Database.Entity(filterOne, cacheObject).FirstOrDefault();
            if (dataClass != default)
            {
                entity.Init(dataClass);
            }

            return entity;
        }

        public static IEnumerable<TEntity> LoadAll(Expression<Func<TPersist, bool>> filter = null, object cacheObject = default)
        {
            List<TEntity> entities = new List<TEntity>();

            var dataClasses = Database.Entity(filter, cacheObject).ToList();
            foreach (var dataClass in dataClasses)
            {
                var entity = typeof(TEntity).New<TEntity>();
                entity.Init(dataClass);
                entities.Add(entity);
            }

            return entities;
        }
    }
}
