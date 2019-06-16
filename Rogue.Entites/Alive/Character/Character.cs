namespace Rogue.Entites.Alive
{
    using Rogue.Entites.Alive.Enums;
    using Rogue.Entites.Items;
    using Rogue.Items;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Абстрактный класс персонажа
    /// </summary>
    public abstract class Character : Moveable
    {
        private List<Equipment> Equipment = new List<Equipment>();

        public Character()
        {
            this.Clothes.OnPutOn += (@new, old) =>
            {
                if (old != null)
                {
                    var oldEquip = Equipment.FirstOrDefault(e => e.Item == old);
                    oldEquip.Discard(this);
                    Equipment.Remove(oldEquip);
                }

                var newEquip = new Equipment(@new);
                newEquip.Apply(this);
                Equipment.Add(newEquip);

                return true;
            };

            this.Clothes.OnPutOff += item =>
            {
                var oldEquip = Equipment.FirstOrDefault(e => e.Item == item);
                oldEquip.Discard(this);
                Equipment.Remove(oldEquip);

                return true;
            };
        }

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
        /// <para>
        /// А вот и нет, полезная фича оказалась
        /// </para>
        /// </summary>
        /// <returns></returns>
        public virtual ConsoleColor ResourceColor => ConsoleColor.Blue;

        public virtual IEnumerable<ClassStat> ClassStats => new ClassStat[0];

        public Backpack Backpack { get; set; } = new Backpack(6, 11);

        public Wear Clothes { get; set; } = new Wear();
    }
}