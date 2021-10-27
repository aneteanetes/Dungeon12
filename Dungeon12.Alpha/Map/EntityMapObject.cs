using Dungeon.Entities;
using Dungeon12.Entities.Alive;
using Dungeon12.Events.Events;
using Dungeon12.Map.Objects;
using Dungeon.View.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Dungeon;
using Dungeon12.Entities;

namespace Dungeon12.Map
{
    public class EntityMapObject<TEntity> : Сonversational
        where TEntity : EntityFraction
    {
        public override EntityFraction BindedEntity => Entity;

        public EntityMapObject(TEntity entity)
        {
            ReEntity(entity);
            Global.Events.Subscribe<TotemArrivedEvent, MapObject>((@event, args) =>
            {
                this.Dispatch((so, arg) => so.OnEvent(arg), @event);
            }, this is Avatar ? this : null);
        }

        public override void Reload() => ReEntity(Entity);

        public void ReEntity(TEntity entity)
        {
            if (entity != default) // случай загрузки
            {
                entity.MapObject = this;
                Entity = entity;

                if (Entity.SceneObject == default)
                    Entity.SceneObject = this.SceneObject;

                if(entity is Alive aliveEntity)
                {
                    aliveEntity.OnDie += this.Die;
                }
            }
        }

        public virtual void OnEvent(TotemArrivedEvent @event)
        {
            var totem = @event.Totem.As<Totem>();

            if (!totem.CanAffect(this))
                return;

            var timer = Dungeon12.Global.Time.Timer(this.Uid + totem.Uid)
                .After(500)
                .Do(() => CheckTotem(totem))
                .Repeat();

            totem.Destroy += () =>
            {
                timer.StopDestroy();
                if (TotemEnableState[totem.Uid])
                {
                    this.RemoveState(totem.ApplicableEffect);
                }
            };

            TotemEnableState.Add(totem.Uid, false);
            CheckTotem(totem);

            timer.Trigger();
        }

        public override void OnEvent(object @object)
        {
            CallOnEvent(@object as dynamic);
        }

        /// <summary>
        /// Method must call this.Discard(obj); for runtime dynamic binding 
        /// </summary>
        /// <param name="obj"></param>
        protected override void CallOnEvent(dynamic obj) => OnEvent(obj);

        private readonly Dictionary<string, bool> TotemEnableState = new Dictionary<string, bool>();

        private void CheckTotem(Totem totem)
        {
            if (this.Location == default)
            {
                return;
            }

            var totemActive = this.IntersectsWithOrContains(totem.Range);
            if (totemActive != TotemEnableState[totem.Uid])
            {
                var effect = totem.ApplicableEffect;
                if (totemActive)
                {
                    this.AddState(effect);
                }
                else
                {
                    this.RemoveState(effect);
                }
                TotemEnableState[totem.Uid] = totemActive;
            }
        }

        /// <summary>
        /// сеттер не приватный потому что сериализация
        /// </summary>
        public TEntity Entity
        {
            get;
            /*private*/            
            set;
        }

        public override void SetView(ISceneObject sceneObject)
        {
            base.SetView(sceneObject);
            Entity.SceneObject = sceneObject;
        }
    }
}