namespace Dungeon.Classes
{
    using Dungeon.Drawing;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ClassStatAttribute : Attribute
    {
        public string Title { get; }

        public IDrawColor Color { get; }

        public int Group { get; }

        public ClassStatAttribute(string title) => Title = title;

        public ClassStatAttribute(string title, IDrawColor color) : this(title) => Color = color;

        public ClassStatAttribute(string title, ConsoleColor color) : this(title, new DrawColor(color)) { }

        public ClassStatAttribute(string title, int group) : this(title) => Group = group;

        public ClassStatAttribute(string title, IDrawColor color, int group) : this(title, color) => Group = group;

        public ClassStatAttribute(string title, ConsoleColor color, int group) : this(title, new DrawColor(color), group) { }
    }

    public class ClassStat : BaseStatEquip
    {
        public ClassStat(string title, IEnumerable<string> properties, StatValues values, IDrawColor color)
        {
            Image = System.Reflection.Assembly.GetCallingAssembly().GetName().Name + "name.png".ImgRes();

            this.StatName = title;
            this.StatProperties = properties.ToList();
            this.StatValues = values;
            this.Color = color;
        }

        public ClassStat(string title, IEnumerable<string> properties, StatValues values, ConsoleColor color)
        {
            Image = System.Reflection.Assembly.GetCallingAssembly().GetName().Name + "name.png".ImgRes();

            this.StatName = title;
            this.StatProperties = properties.ToList();
            this.StatValues = values;
            this.Color = new DrawColor(color);
        }

        public ClassStat(string title, string property, StatValues values, ConsoleColor color)
        {
            Image = System.Reflection.Assembly.GetCallingAssembly().GetName().Name + "name.png".ImgRes();

            this.StatName = title;
            this.StatProperties = property.InList();
            this.StatValues = values;
            this.Color = new DrawColor(color);
        }

        public int Group { get; set; }
    }
}