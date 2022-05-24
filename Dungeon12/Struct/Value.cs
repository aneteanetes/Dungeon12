using System.Collections.Generic;
using System.Linq;

namespace Dungeon12
{
    internal class Value
    {
        public Value(double value = 0)
        {
            Set(value);
        }

        private const string ValueInitialConst = "__$_$_VALUE_&CONST&INI_@";

        public double FlatValue => Values.Sum(x => x.Value);

        private Dictionary<string, double> Values { get; set; } = new Dictionary<string, double>();

        public bool Set(double value) => Replace(ValueInitialConst, value);

        public bool Add(string name, double value) => Values.TryAdd(name, value);

        public bool Clear()
        {
            Values.TryGetValue(ValueInitialConst, out var initial);
            Values.Clear();
            return Set(initial);
        }

        public bool Remove(string name) => Values.Remove(name);

        public bool Replace(string name, double value)
        {
            if (Values.ContainsKey(name))
            {
                Values[name] = value;
                return true;
            }
            else Add(name, value);

            return false;
        }

        public override string ToString()=>FlatValue.ToString();
    }
}
