using System;
using System.Collections.Generic;

namespace Rogue
{
    public class TimeTrigger
    {
        public static Func<GameTime> GlobalTimeSource = () => new GameTime();
        private static GameTime GlobalTime => GlobalTimeSource();

        Func<int> hoursSource;

        /// <summary>
        /// Запускать автоматически
        /// </summary>
        /// <returns></returns>
        public TimeTrigger Auto()
        {
            GlobalTime.OnMinute += Trigger;
            return this;
        }

        internal TimeTrigger(int hours) => BindHours(hours);

        public TimeTrigger After(int hours)
        {
            BindHours(hours);
            return this;
        }

        private void BindHours(int hours) => hoursSource = () => hours;

        private List<(Func<bool> check, Action action)> Bindings = new List<(Func<bool> check, Action action)>();

        public TimeTrigger Do(Action action)
        {
            var day = 0;
            var h = hoursSource();
            Bindings.Add((() =>
            {
                var calculate = GlobalTime.Hours >= h;

                if (day != 0 && GlobalTime.Days > day)
                {
                    day = 0;
                }

                if (calculate && day == 0)
                {
                    day = GlobalTime.Days;
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