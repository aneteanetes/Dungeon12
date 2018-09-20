using System;
using Rogue.Entites.Enums;

namespace Rogue.Entites.Alive.Character
{
    /// <summary>
    /// Абстрактный класс персонажа
    /// </summary>
    public class Player : Modified
    {
        public Race Race { get; set; }

        public long EXP { get; set; }

        public long MaxExp => EXP * 2;

        public int Gold { get; set; }

        public virtual string Resource() { return $"Мана: {HitPoints}/{MaxHitPoints}"; }

        /// <summary>
        /// это пиздец, выпили это нахуй
        /// </summary>
        /// <returns></returns>
        public virtual ConsoleColor ResourceColor() => ConsoleColor.Blue;
    }
}