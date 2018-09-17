using System;

namespace Rogue.Entites.Abilities
{
    /// <summary>
    /// Помеченый метод является способностью
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class Ability : Attribute
    {
        public int Position { get; set; }

        /// <summary>
        /// Помеченый метод является способностью
        /// </summary>
        /// <param name="position">Позиция способности от 1 до 4</param>
        /// <param name="name">Наименование навыка</param>
        /// <param name="scale">Скалирование от характеристики</param>
        /// <param name="rate">Рейт скалирования</param>
        public Ability(int position, string name, Scale scale, double rate)
        {
            this.Position = position;
        }
    }
}