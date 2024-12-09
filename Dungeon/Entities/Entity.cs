﻿using Dungeon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dungeon.Entities
{
    public class Entity : VisualObject, IPersist
    {
        public string IdentifyName { get; set; }

        public string Assembly { get; set; }

        public int Id { get; set; }

        public int ObjectId { get; set; }
    }

    public class DataEntity<TEntity,TPersist> : Entity
        where TEntity : DataEntity<TEntity, TPersist>
        where TPersist : Persist, IPersist
    {
        protected virtual void Init(TPersist dataClass)
        {
        }

        public static TEntity Load(string id)
        {
            var entity = typeof(TEntity).NewAs<TEntity>();
            if (entity == default)
                return default;

            var dataClass = Store.Entity<TPersist>(x => x.IdentifyName == id).FirstOrDefault();
            if (dataClass != default)
            {
                entity.Init(dataClass);
            }

            return entity;
        }

        public static TEntity Load(Expression<Func<TPersist, bool>> filterOne, object cacheObject=default)
        {
            var entity = typeof(TEntity).NewAs<TEntity>();
            var dataClass = Store.Entity(filterOne, cacheObject).FirstOrDefault();
            if (dataClass != default)
            {
                entity.Init(dataClass);
            }

            return entity;
        }

        public static IEnumerable<TEntity> LoadAll(Expression<Func<TPersist, bool>> filter = null, object cacheObject = default)
        {
            List<TEntity> entities = new List<TEntity>();

            var dataClasses = Store.Entity(filter, cacheObject).ToList();
            foreach (var dataClass in dataClasses)
            {
                var entity = typeof(TEntity).NewAs<TEntity>();
                entity.Init(dataClass);
                entities.Add(entity);
            }

            return entities;
        }
    }
}
