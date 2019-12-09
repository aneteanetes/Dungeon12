namespace Dungeon12.Classes
{
    using Dungeon12.Entities.Fractions;
    using System.Collections.Generic;

    /// <summary>
    /// Абстрактный класс персонажа
    /// </summary>
    public abstract partial class Character
    {
        public List<Fraction> Fractions { get; set; } = new List<Fraction>();
    }
}