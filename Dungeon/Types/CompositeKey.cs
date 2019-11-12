using System;

namespace Dungeon.Types
{
    public struct CompositeKey<T>
    {
        public CompositeKey(Type owner, T value)
        {
            this.Value = value;
            this.Owner = owner;
        }

        public T Value { get; set; }

        public Type Owner { get; set; }

        public override bool Equals(object obj)
        {
            if (typeof(CompositeKey<>).IsAssignableFrom(obj.GetType()))
            {
                return obj.GetProperty<string>(nameof(InternalValue)).Equals(this.InternalValue);
            }

            return false;
        }

        private string InternalValue => Value.ToString() + Owner?.AssemblyQualifiedName ?? "";

        public override int GetHashCode() => InternalValue.GetHashCode();
    }
}
