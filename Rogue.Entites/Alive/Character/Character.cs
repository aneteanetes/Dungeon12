namespace Rogue.Entites.Alive
{
    using Rogue.Entites.Alive.Enums;
    using Rogue.Items;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Абстрактный класс персонажа
    /// </summary>
    public abstract class Character : Moveable
    {
        public Race Race { get; set; }

        public Origins Origin { get; set; }

        public long EXP { get; set; }

        public long MaxExp => 100;

        public long Gold { get; set; } = 100;

        public virtual string ClassName { get; }

        public virtual string Resource => "";

        public virtual string ResourceName => "Мана";

        public virtual void AddToResource(double value) { }

        public virtual void RemoveToResource(double value) { }

        public virtual ConsoleColor ClassColor { get; }

        public virtual void AddClassPerk() { }

        public virtual string Avatar => "Rogue.Resources.Images.ui.player.noone.png";

        /// <summary>
        /// это пиздец, выпили это нахуй
        /// </summary>
        /// <returns></returns>
        public virtual ConsoleColor ResourceColor => ConsoleColor.Blue;

        public virtual IEnumerable<ClassStat> ClassStats => new ClassStat[0];

        public List<Item> Backpack { get; set; } = new List<Item>();
    }
}