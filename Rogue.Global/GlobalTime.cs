namespace Rogue
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class GlobalTime
    {
        private System.Timers.Timer internalTimer;

        public GlobalTime()
        {
            internalTimer = new System.Timers.Timer
            {
                Interval = 1
            };
            internalTimer.Elapsed += Time;
            internalTimer.Start();
        }

        /// <summary>
        /// Остановить время
        /// </summary>
        public void Pause() => this.internalTimer.Stop();

        /// <summary>
        /// Продолжить время
        /// </summary>
        public void Resume() => this.internalTimer.Start();

        public int Hours { get; private set; } = 0;

        public int Minutes { get; private set; } = 0;

        public int Days { get; private set; } = 128;

        public int Years { get; private set; } = 600;
        
        private void Time(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Minutes += 1;
            if (this.Minutes > 59)
            {
                this.Minutes = 0;
                this.Hours += 1;
                if (Hours > 23)
                {
                    this.Hours = 0;
                    this.Days += 1;

                    if (this.Days >= 365)
                    {
                        this.Days = 0;
                        this.Years += 1;
                    }
                }
            }
            OnMinute?.Invoke();
        }

        public Action OnMinute { get; set; }

        public static implicit operator string(GlobalTime globalTime) => globalTime.ToString();

        /// <summary>
        /// Таймер
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TimerTrigger Timer(string name) => new TimerTrigger(name);

        /// <summary>
        /// Действие после времени
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        public TimeTrigger After(int hours) => new TimeTrigger(hours);

        public Time Create(int hours = 0, int minutes = 0, int days = 0, int years = 0)
        {
            return new Time(hours, minutes, days, years);
        }

        public void Add(Time time)
        {
            this.Years = time.Years;
            this.Days = time.Days;
            this.Hours = time.Hours;
            this.Minutes = time.Minutes;

            this.OnMinute?.Invoke();
        }

        public override string ToString()
        {
            return $"{Hours:00}:{Minutes:00}";
        }
    }

    public class Time
    {
        public int Hours { get; private set; } = 0;

        public int Minutes { get; private set; } = 0;

        public int Days { get; private set; } = 0;

        public int Years { get; private set; } = 0;

        internal Time(int hours = 0, int minutes = 0, int days = 0, int years = 0)
        {
            Hours = hours;
            Minutes = minutes;
            Days = days;
            Years = years;
        }
    }

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
        /// Каждые N времени
        /// </summary>
        /// <param name="intervalMs"></param>
        /// <returns></returns>
        public TimerTrigger Each(double intervalMs)
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

    public class TimeTrigger
    {
        Func<int> hoursSource;

        /// <summary>
        /// Запускать автоматически
        /// </summary>
        /// <returns></returns>
        public TimeTrigger Auto()
        {
            Global.Time.OnMinute += Trigger;
            return this;
        }

        internal TimeTrigger(int hours) => BindHours(hours);

        public TimeTrigger After(int hours)
        {
            BindHours(hours);
            return this;
        }

        private void BindHours(int hours)=> hoursSource = () => hours;

        private List<(Func<bool> check, Action action)> Bindings = new List<(Func<bool> check, Action action)>();

        public TimeTrigger Do(Action action)
        {
            var day = 0;
            var h = hoursSource();
            Bindings.Add((() =>
            {
                var calculate = Global.Time.Hours >= h;

                if (day != 0 && Global.Time.Days > day)
                {
                    day = 0;
                }

                if (calculate && day == 0)
                {
                    day = Global.Time.Days;
                    return true;
                }
                else if (day > 0 || !calculate)
                {
                    return false;
                }

                day = 0;
                return true;
            }, () => action?.Invoke()));

            return this;
        }

        /// <summary>
        /// Запустить
        /// </summary>
        public void Trigger()
        {
            foreach (var binding in Bindings)
            {
                if (binding.check?.Invoke() == true)
                {
                    binding.action?.Invoke();
                }
            }
        }
    }
}