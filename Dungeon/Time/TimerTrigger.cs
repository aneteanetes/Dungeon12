using System;
using System.Collections.Generic;

namespace Rogue
{
    public class TimerTrigger : IDisposable
    {
        System.Timers.Timer timer;

        private readonly string name;

        private static readonly HashSet<string> aliveTimers = new HashSet<string>();

        internal TimerTrigger(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Существует ли таймер
        /// </summary>
        public bool IsAlive => aliveTimers.Contains(this.name);

        /// <summary>
        /// Через N времени
        /// </summary>
        /// <param name="intervalMs"></param>
        /// <returns></returns>
        public TimerTrigger After(double intervalMs)
        {
            timer = new System.Timers.Timer(intervalMs)
            {
                AutoReset = false
            };

            return this;
        }

        /// <summary>
        /// Действие
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public TimerTrigger Do(Action action)
        {
            timer.Elapsed += (s, e) =>
            {
                action?.Invoke();
                if (!timer.AutoReset)
                {
                    this.Dispose();
                }
            };

            return this;
        }

        /// <summary>
        /// Постоянно повторять
        /// </summary>
        /// <returns></returns>
        public TimerTrigger Repeat()
        {
            timer.AutoReset = true;
            return this;
        }

        /// <summary>
        /// Запускать автоматически
        /// </summary>
        /// <returns></returns>
        public TimerTrigger Auto()
        {
            aliveTimers.Add(name);
            timer.Start(); //обдумать ещё
            return this;
        }

        public void StopDestroy()
        {
            timer.Stop();
            this.Dispose();
        }

        /// <summary>
        /// Запустить сейчас
        /// </summary>
        public void Trigger() => Auto();

        public void Dispose()
        {
            timer?.Dispose();
            aliveTimers.Remove(this.name);
        }
    }
}