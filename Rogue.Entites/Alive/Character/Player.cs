using System;
using Rogue.Entites.Enums;

namespace Rogue.Entites.Alive.Character
{
    /// <summary>
    /// Абстрактный класс персонажа
    /// </summary>
    public abstract class Player : Modified
    {
        public Race Race { get; set; }

        public long EXP { get; set; }

        public long MaxExp => EXP * 2;

        public long Gold { get; set; }

        public virtual string ClassName { get; }

        public virtual string Resource => "";

        public virtual string ResourceName => "Мана";

        public virtual void AddToResource(double value) { }

        public virtual void RemoveToResource(double value) { }

        public virtual ConsoleColor ClassColor { get; }

        /// <summary>
        /// это пиздец, выпили это нахуй
        /// </summary>
        /// <returns></returns>
        public virtual ConsoleColor ResourceColor => ConsoleColor.Blue;
    }
}