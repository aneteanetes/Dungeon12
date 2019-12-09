namespace Dungeon12.Classes
{
    using Dungeon;
    using Dungeon12.Entities.Alive;
    using Dungeon12.Entities.Alive.Enums;
    using Dungeon12.Inventory;
    using Dungeon12.Items;
    using Force.DeepCloner;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Абстрактный класс персонажа
    /// </summary>
    public abstract partial class Character
    {
        private void BindClassStats()
        {
            var props = this.GetType().GetProperties()
                .Select(p => new { Property = p, Attribute = Attribute.GetCustomAttribute(p, typeof(ClassStatAttribute)) })
                .Where(x => x.Attribute != null)
                .Select(x => new { x.Property, Attribute = x.Attribute as ClassStatAttribute });

            var grouped = props.GroupBy(p => p.Attribute.Title);

            foreach (var group in grouped)
            {
                var first = group.First();

                var stat = new ClassStat(group.Key, first.Property.Name, group.Select(x => x.Property.Name), first.Attribute.Color)
                {
                    Group = first.Attribute.Group,
                    Description = group?.FirstOrDefault(x => x.Attribute.Description != null).Attribute.Description,
                    Image = $"{Global.GameAssemblyName}/{this.GetType().Name}/Resources/Images/Stats/{first.Property.Name}.png".Embedded()
                };

                Global.GameState.Equipment.AddEquip(stat.DeepClone());

                var @this = Expression.Constant(this);
                var propertyAccessors = group.Select(x => Expression.Property(@this, x.Property)).ToArray();
                var func = Expression.Lambda<Func<long[]>>(Expression.NewArrayInit(typeof(long), propertyAccessors)).Compile();
                stat.StatValues = func().ToList();

                this.ClassStats.Add(stat);
            }
        }
    }
}