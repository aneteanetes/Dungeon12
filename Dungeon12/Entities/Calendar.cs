using Dungeon;
using System.ComponentModel.DataAnnotations;

namespace Dungeon12.Entities
{
    internal class Calendar
    {
        public TimeSpan Time { get; set; } = TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(56));

        public int HoursSave { get; set; }

        public int MinutesSave { get; set; }

        public int Week { get; set; } = 4;

        public DayWeek DayOfWeek => (DayWeek)Week;

        public int Month { get; set; } = 9;

        public int Day { get; set; } = 15;

        public int Year { get; set; } = 1201;

        public string TimeText()
        {
            return $"{Time.Hours:00}:{Time.Minutes:00}";
        }

        public string ClockText()
        {
            var dayofweek = ((DayWeek)Week).ToValue<string>();
            var month = ((MonthYear)Month).AsShimmer();
            return $"{Time.Hours:00}:{Time.Minutes:00} {dayofweek}, {month}, {Year}г.";
        }
        // 8:15 Пн, Месяц Капли, 1201г.

        public string DateText()
        {
            var dayofweek = ((DayWeek)Week).ToValue<string>();
            var month = ((MonthYear)Month).ToDisplay();
            return $"{Day} {dayofweek}, {month}, {Year}г.";
        }
        // 17 Пн, Месяц Капли, 1201г.

        public void Add(int hours, int minutes)
        {
            var time = Time
                .Add(TimeSpan.FromMinutes(minutes))
                .Add(TimeSpan.FromHours(hours));

            if (time.Days > 0)
            {
                time -= TimeSpan.FromDays(time.Days);
                Day++;
                Week++;
                if (Week > 7)
                    Week = 1;

                if (Day > 28)
                {
                    Month++;
                    if (Month > 12)
                    {
                        Month = 1;
                        Year++;
                    }
                }
            }

            Time = time;
        }
    }

    public enum DayWeek
    {
        /// <summary>
        /// пн
        /// </summary>
        [Display(Name = "Понедельник")]
        [Value("Пн")]
        Amonos = 1,
        /// <summary>
        /// вт
        /// </summary>
        [Display(Name = "Вторник")]
        [Value("Вт")]
        Budos = 2,
        /// <summary>
        /// ср
        /// </summary>
        [Display(Name = "Среда")]
        [Value("Ср")]
        Citrus = 3,
        /// <summary>
        /// чт
        /// </summary>
        [Display(Name = "Четверг")]
        [Value("Чт")]
        Dechuts = 4,
        /// <summary>
        /// пт
        /// </summary>
        [Display(Name = "Пятница")]
        [Value("Пт")]
        Eaftus = 5,
        /// <summary>
        /// сб
        /// </summary>
        [Display(Name = "Суббота")]
        [Value("Сб")]
        Fastus = 6,
        //вс
        [Display(Name = "Воскресенье")]
        [Value("Вс")]
        Gasuvs = 7
    }

    public enum MonthYear
    {
        [Display(Name = "Месяц Ангела")]
        january =1,
        [Display(Name = "Месяц Хаоса")]
        february=2,
        [Display(Name = "Месяц Болезни")]
        march = 3,
        [Display(Name = "Месяц Крови")]
        april = 4,
        [Display(Name = "Месяц Нефрита")]
        may = 5,
        [Display(Name = "Месяц Ярости")]
        june = 6,
        [Display(Name = "Месяц Кузнеца")]
        july = 7,
        [Display(Name = "Месяц Моря")]
        august = 8,
        [Display(Name = "Месяц Солнца")]
        september = 9,
        [Display(Name = "Месяц Мудрости")]
        october = 10,
        [Display(Name = "Месяц Луны")]
        november = 11,
        [Display(Name = "Месяц Смерти")]
        december = 12,
    }

    public static class MonthYearExtensions
    {
        public static string AsShimmer(this MonthYear monthYear)
        {
            return Global.Strings[monthYear.ToString()];
        }
    }
}
