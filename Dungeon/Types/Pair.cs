using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Types
{
    public struct Pair<TKey, TValue>
    {
        public Pair(TKey first, TValue second)
        {
            First = first;
            Second = second;
        }

        public TKey First { get; set; }

        public TValue Second { get; set; }

        public override bool Equals(object obj)
        {
            var internalValue = obj.GetPropertyExpr<string>(nameof(InternalValue));
            return internalValue == this.InternalValue;
        }

        private string InternalValue => First.ToString();

        public override int GetHashCode() => InternalValue.GetHashCode();
    }
}