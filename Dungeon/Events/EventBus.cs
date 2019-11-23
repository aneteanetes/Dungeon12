using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dungeon.Events
{
    public class EventBus
    {
        private readonly Dictionary<string, Delegate> events = new Dictionary<string, Delegate>();
        private List<Action<object>> allsubscribers = new List<Action<object>>();
        //private List<Action<object>> allsubscribers = new List<Action<object>>();

        private readonly Dictionary<string, Action<object, string[]>> typesubscribers = new Dictionary<string, Action<object, string[]>>();

        /// <summary>
        /// Подписаться вообще на все события
        /// </summary>
        public void Subscribe(Action<object> action) => allsubscribers.Add(action);

        /// <summary>
        /// Отписаться от всех событий
        /// </summary>
        /// <param name="action"></param>
        public void Unsubscribe(Action<object> action) => allsubscribers.Remove(action);

        /// <summary>
        /// Подписаться на события всех таких типов
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        public void Subscribe<TEvent, TSubscriber>(Action<object, string[]> action, object from = null)
        {
            var key = typeof(TEvent).FullName;
            if (!typesubscribers.TryGetValue(key, out var sub))
            {
                Action<object,string[]> empty = (x,y) => { };
                typesubscribers.Add(key, empty);
            }
            typesubscribers[key] += action;
        }

        public void Subscribe<TEvent>(Action<TEvent> action, bool autoUnsubscribe = true, params string[] args) where TEvent : IEvent
        {
            var ev = Get<TEvent>(args);
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
                string @event = EventTypeName<TEvent>();
                if (args != default)
                {
                    @event += string.Join("`", args);
                }
                events[@event] = ev += action;
            }
        }

        public static string EventTypeName<TEvent>()
        {
            string name = typeof(TEvent).FullName;
            int index = name.IndexOf('`');
            return index == -1 ? name : name.Substring(0, index);
        }

        private Action<TEvent> Get<TEvent>(params string[] args)
        {
            string @event = EventTypeName<TEvent>();

            if (args != default)
            {
                @event += string.Join("`", args);
            }

            if (!events.ContainsKey(@event))
            {
                Action<TEvent> action = x => {};
                events.Add(@event, action);
            }

            if (events[@event] is Action<TEvent> tAction)
            {
                return tAction;
            }

            Console.WriteLine("ВНИМАНИЕ! СОБЫТИЕ КОТОРОЕ НЕ ЗАРЕГИСТРИРОВАННО!");
            return x => { };
        }

        public void Raise<TEvent>(TEvent @event, params string[] args) where TEvent : IEvent
        {
            Get<TEvent>(args)?.DynamicInvoke(@event);
            allsubscribers.ForEach(x =>
            {
                x.Invoke(@event);
            });
            typesubscribers.Where(ts => @event.GetType().FullName.Contains(ts.Key)).ToList().ForEach(ts =>
            {
                ts.Value?.Invoke(@event, args);
            });
        }
    }
}