namespace Dungeon
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class GameTime : Time
    {
        private System.Timers.Timer internalTimer;

        public GameTime()
        {
            internalTimer = new System.Timers.Timer
            {
                Interval = 2000
            };
            internalTimer.Elapsed += Time;            
        }

        /// <summary>
        /// Остановить время
        /// </summary>
        public void Pause() => this.internalTimer.Stop();

        /// <summary>
        /// Продолжить время
        /// </summary>
        public void Resume() => this.internalTimer.Start();

        /// <summary>
        /// Запустить время
        /// </summary>
        public void Start() => Resume();


        private void Time(object sender, System.Timers.ElapsedEventArgs e)
        {
            AddMinute();
            OnMinute?.Invoke();
        }

        [Newtonsoft.Json.JsonIgnore]
        public Action<Time,Time> OnTimeSet { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public Action OnMinute { get; set; }

        public static implicit operator string(GameTime globalTime) => globalTime.ToString();

        /// <summary>
        /// Таймер
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TimerTrigger Timer(string name) => new TimerTrigger(name);

        /// <summary>
        /// Таймер
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TimerTrigger Timer() => Timer(Guid.NewGuid().ToString());

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

        public GameTime Set(Time time)
        {
            this.OnTimeSet?.Invoke(new Time(Hours, Minutes, Days, Years), time);

            this.Years = time.Years;
            this.Days = time.Days;
            this.Hours = time.Hours;
            this.Minutes = time.Minutes;

            return this;
        }

        public override string ToString()
        {
            return $"{Hours:00}:{Minutes:00}";
        }
    }
}