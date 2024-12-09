﻿using Dungeon.Scenes;
using System;
using System.Collections.Generic;

namespace Dungeon
{
    public class TimerTrigger : IDisposable
    {
        System.Timers.Timer timer;

        private readonly string name;

        private static object aliveTimeslock = new object();

        private static readonly HashSet<string> aliveTimers = new HashSet<string>();

        private Scene ActiveScene;

        internal TimerTrigger(string name)
        {
            ActiveScene = DungeonGlobal.SceneManager?.Current;
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
                if (!sceneIndependent)
                {
                    if (ActiveScene?.Destroyed ?? false)
                    {
                        this.Dispose();
                        ActiveScene = default;
                        return;
                    }
                }

                action?.Invoke();
                if (!timer.AutoReset && !this.recoverable)
                {
                    this.Dispose();
                }
            };

            return this;
        }

        private bool sceneIndependent = false;

        /// <summary>
        /// таймер не будет уничтожен после уничтожения сцены
        /// </summary>
        /// <returns></returns>
        public TimerTrigger SceneFree()
        {
            sceneIndependent = true;
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

        private bool recoverable = false;

        /// <summary>
        /// Таймер будет запускаться несколько раз
        /// </summary>
        /// <returns></returns>
        public TimerTrigger Recoverable()
        {
            recoverable = true;
            return this;
        }

        /// <summary>
        /// Запускать автоматически
        /// </summary>
        /// <returns></returns>
        public TimerTrigger Auto()
        {
            lock (aliveTimeslock)
                aliveTimers.Add(name);

            if (!disposed)
            {
                timer.Start(); //обдумать ещё
            }
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

        protected bool disposed = false;

        /// <summary>
        /// Остановить, уничтожить, и освободить таймер
        /// </summary>
        public void Dispose()
        {
            timer?.Dispose();
            disposed = true;
            lock (aliveTimeslock)
                aliveTimers.Remove(this.name);
        }
    }
}