namespace Dungeon
{
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
}
