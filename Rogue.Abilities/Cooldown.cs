using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Abilities
{
    public class Cooldown
    {
        private static Dictionary<string, Cooldown> cooldowns = new Dictionary<string, Cooldown>();

        public Cooldown(double milliseconds, string name = null)
        {
            Milliseconds = milliseconds;
            Name = name ?? Guid.NewGuid().ToString();

            if (!cooldowns.ContainsKey(name))
            {
                this.Timer = new System.Timers.Timer(milliseconds);
                this.Timer.AutoReset = false;
                this.Timer.Elapsed += (x, y) =>
                {
                    cooldowns[name].Available = true;
                };

                cooldowns.Add(name, this);
            }
        }

        public string Name { get; }

        public double Milliseconds { get; }

        internal System.Timers.Timer Timer { get; set; }

        internal bool Available { get; private set; } = true;

        /// <summary>
        /// Проверка что нету кулдауна у способности
        /// </summary>
        /// <returns></returns>
        public bool Check() => cooldowns[Name].Available;

        /// <summary>
        /// Указывает что способность используется что бы нельзя было её вызвать
        /// </summary>
        public void Cast()
        {
            cooldowns[Name].Available = false;
            cooldowns[Name].Timer.Start();
        }
    }
}
