namespace Dungeon
{
    public class Time
    {
        public int Hours { get; protected set; } = 0;

        public int Minutes { get; protected set; } = 0;

        public int Days { get; protected set; } = 150;

        public int Years { get; protected set; } = 600;

        public static Time GameStart { get; } = new Time(12, 0, 150, 600);

        internal Time(int hours = 0, int minutes = 0, int days = 0, int years = 0)
        {
            Hours = hours;
            Minutes = minutes;
            Days = days;
            Years = years;
        }

        public void AddMinute()
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

                    if (this.Days >= 300)
                    {
                        this.Days = 0;
                        this.Years += 1;
                    }
                }
            }
        }
    }
}
