using Dungeon;
using System;
using System.ComponentModel.DataAnnotations;

namespace Dungeon12.Entities
{
    public class Calendar
    {
        public TimeSpan Time { get; set; } = TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(56));

        public int HoursSave { get; set; }

        public int MinutesSave { get; set; }

        public int Week { get; set; } = 4;

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
            var month = ((MonthYear)Month).ToDisplay();
            return $"{Time.Hours:00}:{Time.Minutes:00} {dayofweek}, {month}, {Year}г.";
        }
        // 8:15 Пн, Месяц Капли, 1201г.

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
        [Display(Name = "Месяц Внешних")]
        Outers =1,
        [Display(Name = "Месяц Тьмы")]
        Darks=2,
        [Display(Name = "Месяц Света")]
        Lights = 3,
        [Display(Name = "Месяц Гроз")]
        Thunders = 4,
        [Display(Name = "Месяц Туманов")]
        Fogs = 5,
        [Display(Name = "Месяц Спокойствия")]
        Relaxs = 6,
        [Display(Name = "Месяц Огня")]
        Fires = 7,
        [Display(Name = "Месяц Пепла")]
        Dusts = 8,
        [Display(Name = "Месяц Капли")]
        Water = 9,
        [Display(Name = "Месяц Страха")]
        Fears = 10,
        [Display(Name = "Месяц Пустоты")]
        Voids = 11,
        [Display(Name = "Месяц Духов")]
        Spirits = 12,
    }
}
