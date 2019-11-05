namespace Dungeon.Classes
{
    using Dungeon.Entites.Alive;
    using Dungeon.Entites.Alive.Enums;
    using Dungeon.Inventory;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Абстрактный класс персонажа
    /// </summary>
    public abstract partial class Character : Moveable
    {   
        private void BindClassStats()
        {
            var props = this.GetType().GetProperties()
                .Select(p => new { Property = p, Attribute = Attribute.GetCustomAttribute(p, typeof(ClassStatAttribute)) })
                .Where(x => x.Attribute != null)
                .Select(x => new { Property = x.Property, Attribute = x.Attribute as ClassStatAttribute });

            var grouped = props.GroupBy(p => p.Attribute.Title);

            foreach (var group in grouped)
            {
                var first = group.First();

                var @this = Expression.Constant(this);

                var propertyAccessors = group.Select(x => Expression.Property(@this, x.Property)).ToArray();

                var func = Expression.Lambda<Func<long[]>>(Expression.NewArrayInit(typeof(long), propertyAccessors)).Compile();

                var value = StatValues.Function(func);

                var stat = new ClassStat(group.Key, group.Select(x => x.Property.Name), value, first.Attribute.Color)
                {
                    Group = first.Attribute.Group
                };

                this.ClassStats.Add(stat);
            }
        }
    }
}