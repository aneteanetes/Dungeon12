namespace Rogue
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class GlobalTime
    {
        private System.Timers.Timer Timer;
        private int _hours = 0;
        private int _minutes = 0;

        public GlobalTime()
        {
            Timer = new System.Timers.Timer
            {
                Interval = 1
            };
            Timer.Elapsed += Time;
            Timer.Start();
        }

        public int Hours => _hours;

        public int Minutes => _minutes;

        private void Time(object sender, System.Timers.ElapsedEventArgs e)
        {
            this._minutes += 1;
            if (this._minutes > 59)
            {
                this._minutes = 0;
                this._hours += 1;
                if (_hours > 23)
                {
                    this._hours = 0;
                }
            }
            OnMinute?.Invoke();
        }

        public Action OnMinute { get; set; }

        public static implicit operator string(GlobalTime globalTime) => globalTime.ToString();

        public override string ToString()
        {
            return $"{Hours:00}:{Minutes:00}";
        }
    }
}