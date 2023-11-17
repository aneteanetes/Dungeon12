using System;

namespace Dungeon.Varying
{
    public class Variable
    {
        public Variable(string name)
        {
            Name = name;
        }

        public string Name { get; set; }        

        public object Value { get => Get(); set => Set(value); }

        public virtual void Set(string value) { }

        public virtual void Set<T>(T value) { }

        public virtual T Get<T>() => default;

        public virtual object Get() => default;

        public Action OnChange;
    }
}