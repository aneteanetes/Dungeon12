using Rogue.Events.Events;
using Rogue.Map.Objects;
using System.Collections.Generic;

namespace Rogue.Map
{
    public partial class MapObject
    {
        public MapObject()
        {
            Global.Events.Subscribe<TotemArrivedEvent, MapObject>((@event, args) =>
			{
				this.Dispatch((so, arg) => so.OnEvent(arg), @event);
			});
        }

        public virtual void OnEvent(TotemArrivedEvent @event)
        {
            var totem = @event.Totem.As<Totem>();

            var timer = Global.Time.Timer(this.Uid + totem.Uid)
                .After(500)
                .Do(()=>CheckTotem(totem))
                .Repeat();

            totem.Destroy += () =>
            {
                timer.StopDestroy();
                this.RemoveState(totem.ApplicableEffect);
            };

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
            var totemActive = this.IntersectsWith(totem.Range);
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
    }
}
