using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Types
{
    public struct Pair<TKey, TValue>
    {
        public Pair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public TKey Key { get; set; }

        public TValue Value { get; set; }

        public override bool Equals(object obj)
        {
            var internalValue = obj.GetPropertyExpr<string>(nameof(InternalValue));
            return internalValue == this.InternalValue;
        }

        private string InternalValue => Key.ToString();

        public override int GetHashCode() => InternalValue.GetHashCode();
    }
}