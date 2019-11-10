﻿using Dungeon.Entities;
using Dungeon.Events.Events;
using Dungeon.Map.Objects;
using Dungeon.View.Interfaces;
using System.Collections.Generic;

namespace Dungeon.Map
{
    public class EntityMapObject<TEntity> : MapObject
        where TEntity : Entity
    {
        public EntityMapObject(TEntity entity)
        {
            ReEntity(entity);
            Global.Events.Subscribe<TotemArrivedEvent, MapObject>((@event, args) =>
            {
                this.Dispatch((so, arg) => so.OnEvent(arg), @event);
            }, this is Avatar ? this : null);
        }

        public void ReEntity(TEntity entity)
        {
            entity.MapObject = this;
            Entity = entity;
        }

        public virtual void OnEvent(TotemArrivedEvent @event)
        {
            var totem = @event.Totem.As<Totem>();

            if (!totem.CanAffect(this))
                return;

            var timer = Dungeon.Global.Time.Timer(this.Uid + totem.Uid)
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

        public virtual void OnEvent(object @object)
        {
            CallOnEvent(@object as dynamic);
        }

        /// <summary>
        /// Method must call this.Discard(obj); for runtime dynamic binding 
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void CallOnEvent(dynamic obj) => OnEvent(obj);

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

        public TEntity Entity { get; private set; }

        public override void SetView(ISceneObject sceneObject)
        {
            base.SetView(sceneObject);
            Entity.SceneObject = sceneObject;
        }
    }
}