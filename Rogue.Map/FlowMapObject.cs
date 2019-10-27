using Rogue.Events.Events;
using Rogue.Map.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Map
{
    public class FlowMapObject : MapObject, IFlowable
    {
        public FlowMapObject()
        {
            Global.Events.Subscribe<TotemArrivedEvent, MapObject>((@event, args) =>
            {
                this.Dispatch((so, arg) => so.OnEvent(arg), @event);
            }, this is Avatar ? this : null);
        }

        public virtual void OnEvent(TotemArrivedEvent @event)
        {
            var totem = @event.Totem.As<Totem>();

            if (!totem.CanAffect(this))
                return;

            var timer = Global.Time.Timer(this.Uid + totem.Uid)
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


        private object flowContext = null;

        public T GetFlowProperty<T>(string property, T @default = default) => flowContext.GetProperty<T>(property, @default);

        public bool SetFlowProperty<T>(string property, T value)
        {
            try
            {
                flowContext.SetProperty(property, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SetFlowContext(object context) => flowContext = context;

        public object GetFlowContext() => flowContext;

        private IFlowable flowparent = null;

        public void SetParentFlow(IFlowable parent) => flowparent = parent;

        public IFlowable GetParentFlow() => flowparent;
        
        [FlowMethod]
        public void AddEffect(bool forward) { }
    }
}
