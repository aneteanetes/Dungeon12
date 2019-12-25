using Force.DeepCloner;
using System;
using System.Diagnostics;

namespace Dungeon
{
    public class Time
    {
        public int Hours { get; set; } = 0;

        public int Minutes { get; set; } = 0;

        public int Months { get; set; } = 5;        

        public int Days { get; set; } = 150;

        public int Years { get; set; } = 600;

        public static Time GameStart { get; } = new Time(12, 0, 150, 600);

        public Time() { }

        internal Time(int hours = 0, int minutes = 0, int days = 0, int years = 0)
        {
            Hours = hours;
            Minutes = minutes;
            Days = days;
            Years = years;
        }

        public Time Clone()
        {
            return this.DeepClone();
        }

        public void AddMinute()
        {
            this.Minutes += 1;
            if (this.Minutes > 59)
            {
                this.Minutes = 0;
                AddHourse(1);
            }
        }

        public void AddHourse(int h)
        {
            for (int i = 0; i < h; i++)
            {
                this.Hours += 1;
                if (Hours > 23)
                {
                    this.Hours = 0;
                    this.Days += 1;

                    if (this.Days > 30)
                    {
                        this.Days = 0;
                        this.Months++;
                    }

                    if (this.Months > 10)
                    {
                        this.Months = 0;
                        this.Years += 1;
                    }
                }
            }
        }
    }
}
