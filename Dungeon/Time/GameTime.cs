namespace Dungeon
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class GameTime
    {
        private System.Timers.Timer internalTimer;

        public GameTime()
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

        public static implicit operator string(GameTime globalTime) => globalTime.ToString();

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
}