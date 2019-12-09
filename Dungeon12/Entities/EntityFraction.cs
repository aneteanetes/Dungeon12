using Dungeon;
using Dungeon.Data;
using Dungeon.Entities;
using Dungeon12.Entities.Fractions;
using Dungeon12.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Dungeon12.Entities
{
    public class EntityFraction : Entity
    {
        public Fraction Fraction { get; set; }


        [Newtonsoft.Json.JsonIgnore]
        public MapObject MapObject { get; set; }

        public bool IsEnemy(EntityFraction another)
        {
            if (this.Is<Fraction>() || another.Is<Fraction>())
                return false;

            var thisHate = this.Fraction?.EnemiesIdentities.Any(x => x == another.Fraction?.IdentifyName) ?? false;
            if (!thisHate)
            {
                return another.IsEnemyInternal(this);
            }

            return true;
        }

        private bool IsEnemyInternal(EntityFraction another)
        {
            return this.Fraction?.EnemiesIdentities.Any(x => x == another.Fraction?.IdentifyName) ?? false;
        }
    }

    public class DataEntityFraction<TEntity, TPersist> : EntityFraction
        where TEntity : DataEntityFraction<TEntity, TPersist>
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

            var dataClass = Store.Entity<TPersist>(x => x.IdentifyName == id).FirstOrDefault();
            if (dataClass != default)
            {
                entity.Init(dataClass);
            }

            return entity;
        }

        protected void LoadFraction(string id)
        {
            Fraction = FractionView.Load(id).ToFraction();
        }

        public static TEntity Load(Expression<Func<TPersist, bool>> filterOne, object cacheObject = default)
        {
            var entity = typeof(TEntity).New<TEntity>();
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
                var entity = typeof(TEntity).New<TEntity>();
                entity.Init(dataClass);
                entities.Add(entity);
            }

            return entities;
        }
    }
}
