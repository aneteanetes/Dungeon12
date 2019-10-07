using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Abilities
{
    public class Cooldown : ICooldownChain
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

        private Cooldown Next = null;
        private Cooldown Parent = null;

        public string Name { get; }

        public double Milliseconds { get; }

        internal System.Timers.Timer Timer { get; set; }

        internal bool Available { get; set; } = true;

        /// <summary>
        /// Проверка что нету кулдауна у способности
        /// </summary>
        /// <returns></returns>
        public bool Check()
        {
            var cooldownResult = cooldowns[Name].Available;

            if (Next != default)
            {
                return cooldownResult && cooldowns[Next.Name].Check();
            }

            return cooldownResult;
        }

        /// <summary>
        /// Указывает что способность используется что бы нельзя было её вызвать
        /// </summary>
        public void Cast()
        {
            cooldowns[Name].Available = false;
            cooldowns[Name].Timer.Start();
            StartChain();
        }

        private void StartChain()
        {
            if (Next != default)
            {
                Next.Cast();
            }
        }

        public ICooldownChain Chain(double milliseconds, string name = null)
        {
            Next = new Cooldown(milliseconds, name);
            Next.Parent = this;
            return Next;
        }

        public Cooldown Build()
        {
            if (this.Parent != default)
            {
                return this.Parent.Build();
            }

            return this;
        }
    }

    public interface ICooldownChain
    {
        ICooldownChain Chain(double milliseconds, string name = null);

        Cooldown Build();
    }
}