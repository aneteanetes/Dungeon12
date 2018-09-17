using System;

namespace Rogue.Entites.Alive.Character.Attributes
{
    /// <summary>
    /// Помеченый класс является игровым классом
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class ClassAttribute : Attribute
    {
        public string Display { get; set; }

        /// <summary>
        /// Помеченый класс является игровым классом
        /// </summary>
        /// <param name="display">Наименование класса</param>
        public ClassAttribute(string display)
        {
            this.Display = display;
        }
    }
}