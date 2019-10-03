using System;
using System.Collections.Generic;

namespace Rogue.Events
{
    public class EventBus
    {
        private readonly Dictionary<string, object> events = new Dictionary<string, object>();
        private List<Action<object>> allsubscribers = new List<Action<object>>();

        public void Subscribe<TEvent>(Action<TEvent> action, bool autoUnsubscribe = true) where TEvent : IEvent
        {
            var ev = Get<TEvent>();
            if (autoUnsubscribe)
            {
                void subs(TEvent @event)
                {
                    action?.Invoke(@event);
                    ev -= subs;
                }

                ev += subs;
            }
            else
            {
                ev += action;
            }
        }

        /// <summary>
        /// Подписаться вообще на все события
        /// </summary>
        public void Subscribe(Action<object> action) => allsubscribers.Add(action);

        private Action<TEvent> Get<TEvent>()
        {
            string @event = typeof(TEvent).FullName;
            if (!events.ContainsKey(@event))
            {
                Action<TEvent> action = x => { };
                allsubscribers.ForEach(s =>
                {
                    action += x => s(x);
                });

                events.Add(@event, action);
            }

            if(events[@event] is Action<TEvent> tAction)
            {
                return tAction;
            }

            Console.WriteLine("ВНИМАНИЕ! СОБЫТИЕ КОТОРОЕ НЕ ЗАРЕГИСТРИРОВАННО!");
            return x => { };
        }

        public void Raise<TEvent>(TEvent @event) where TEvent : IEvent
        {
            Get<TEvent>()?.Invoke(@event);
        }
    }
}