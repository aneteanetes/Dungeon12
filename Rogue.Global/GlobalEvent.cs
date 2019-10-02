using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue
{
    public class GlobalEvent
    {
        private Dictionary<string, Action> events = new Dictionary<string, Action>();

        public void Subscribe(string @event, Action action,bool autoUnsubscribe = true)
        {
            if (!events.ContainsKey(@event))
            {
                events.Add(@event, () => { });
            }

            if (autoUnsubscribe)
            {
                void subs()
                {
                    action?.Invoke();
                    events[@event] -= subs;
                }

                events[@event] += subs;
            }
            else
            {
                events[@event] += action;
            }
        }

        public void Raise(string @event)
        {
            events[@event]?.Invoke();
            events.Remove(@event);
        }

        public const string ClassChange = nameof(ClassChange);
    }
}
