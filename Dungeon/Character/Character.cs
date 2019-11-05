namespace Dungeon.Classes
{
    using Dungeon.Entites.Alive;
    using Dungeon.Entites.Alive.Enums;
    using Dungeon.Inventory;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Абстрактный класс персонажа
    /// </summary>
    public abstract partial class Character : Moveable
    {
        //private List<Equipment> Equipment = new List<Equipment>();

        public Character()
        {
            this.BindClassStats();
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

        public virtual IDrawColor ClassColor { get; }

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

        public List<ClassStat> ClassStats { get; } = new List<ClassStat>();

        public Backpack Backpack { get; set; } = new Backpack(6, 11);

        public Wear Clothes { get; set; } = new Wear();

        /// <summary>
        /// Пересчитывает все характеристики
        /// </summary>
        public void Recalculate()
        {

        }

        public ClassStat ClassStat<TClass>(Expression<Func<TClass, long>> accessor, ConsoleColor color, int group=0)
        {
            if (!(this is TClass @class))
            {
                throw new Exception("Нельзя добавить классовый параметр другому классу!");
            }

            var classParam = Expression.Parameter(typeof(TClass));
            var lambda = Expression.Lambda<Func<TClass, long>>(accessor, classParam).Compile();
            if (accessor.Body is MemberExpression memberExpression)
            {
                var member = memberExpression.Member;
                var propertyName = member.Name;

                Func<IEnumerable<long>> prov = () => lambda(@class).InEnumerable();

                var stat = new ClassStat(member.Value<TitleAttribute, string>(), propertyName, StatValues.Function(prov), color);
                stat.Group = group;
            }

            return default;
        }
    }
}