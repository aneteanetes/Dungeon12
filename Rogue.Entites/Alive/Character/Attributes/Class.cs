namespace Rogue.Entites.Alive.Character.Attributes
{
    using System;

    /// <summary>
    /// Помеченный тип относится к этому классу
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ClassAttribute : Attribute
    {
        public Type Class { get; set; }

        /// <summary>
        /// Помеченный тип относится к этому классу
        /// </summary>
        /// <param name="class">Класс</param>
        public ClassAttribute(Type @class)
        {
            this.Class = @class;
        }
    }
}