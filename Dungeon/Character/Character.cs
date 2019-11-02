namespace Dungeon.Classes
{
    using Dungeon.Entites.Alive;
    using Dungeon.Entites.Alive.Enums;
    using Dungeon.Inventory;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Абстрактный класс персонажа
    /// </summary>
    public abstract partial class Character : Moveable
    {
        //private List<Equipment> Equipment = new List<Equipment>();

        public Character()
        {
            this.Clothes.OnPutOn += PutOnItem;
            this.Clothes.OnPutOff += PutOffItem;
        }

        public Race Race { get; set; }

        public Origins Origin { get; set; }

        public long EXP { get; set; }

        public long MaxExp => 100;

        public void Exp(long amount)
        {
            EXP += amount;
        }

        public long Gold { get; set; } = 100;

        public virtual string ClassName { get; }

        public virtual string Resource => "";

        public virtual string ResourceName => "Мана";

        public virtual void AddToResource(double value) { }

        public virtual void RemoveToResource(double value) { }

        public virtual ConsoleColor ClassColor { get; }

        public virtual void AddClassPerk() { }

        public virtual string Avatar => $"{Global.GameAssemblyName}.Resources.Images.{this.GetType().Name}.png";
         
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

        /// <summary>
        /// Пересчитывает все характеристики
        /// </summary>
        public void Recalculate()
        {

        }
    }
}