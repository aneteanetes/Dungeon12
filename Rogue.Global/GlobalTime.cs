namespace Rogue
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class GlobalTime
    {
        private System.Timers.Timer Timer;

        public GlobalTime()
        {
            Timer = new System.Timers.Timer
            {
                Interval = 1
            };
            Timer.Elapsed += Time;
            Timer.Start();
        }

        /// <summary>
        /// Остановить время
        /// </summary>
        internal void Pause() => this.Timer.Stop();

        /// <summary>
        /// Продолжить время
        /// </summary>
        internal void Resume() => this.Timer.Start();

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

    public class TimeTrigger
    {
        Func<int> hoursSource;

        public TimeTrigger Auto()
        {
            Global.Time.OnMinute += Trigger;
            return this;
        }

        public TimeTrigger(int hours) => BindHours(hours);

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