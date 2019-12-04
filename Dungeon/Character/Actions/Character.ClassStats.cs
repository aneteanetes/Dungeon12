namespace Dungeon.Classes
{
    using Dungeon.Entities.Alive;
    using Dungeon.Entities.Alive.Enums;
    using Dungeon.Inventory;
    using Dungeon.Items;
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

                this.ClassStats.Add(new ClassStat(group.Key,first.Property.Name, group.Select(x => x.Property.Name), first.Attribute.Color)
                {
                    Group = first.Attribute.Group,
                    Description = group?.FirstOrDefault(x => x.Attribute.Description != null).Attribute.Description,
                    Image = $"{Global.GameAssemblyName}/{this.GetType().Name}/Resources/Images/Stats/{first.Property.Name}.png".Embedded()
                });
            }

            this.ClassStats.ForEach(Global.GameState.Equipment.AddEquip);
        }
    }
}