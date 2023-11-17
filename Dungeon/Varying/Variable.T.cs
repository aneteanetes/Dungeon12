using System;

namespace Dungeon.Varying
{
    public class Variable<T> : Variable
    {
        public Variable(string name) : base(name)
        {
        }

        public new T Value { get; set; }

        public override void Set(string value)
        {
            Value = Convert.ChangeType(value, typeof(T)).As<T>();
        }

        public override void Set<Ts>(Ts value)
        {
            Set(value.ToString());
        }

        public override object Get()
        {
            return this.Value;
        }

        public override Ts Get<Ts>()
        {
            return this.Value.As<Ts>();
        }

    }
}
