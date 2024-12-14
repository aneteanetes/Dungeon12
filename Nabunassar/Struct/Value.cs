namespace Nabunassar
{
    internal class Value
    {
        public bool IsEmpty => Values.Count == 0;

        public double FlatValue => Values.Sum(x => x.Value);

        private Dictionary<string, double> Values { get; set; } = new Dictionary<string, double>();

        public bool Add(string name, double value) => Values.TryAdd(name, value);

        public bool Remove(string name) => Values.Remove(name);

        public bool Clear()
        {
            Values.Clear();
            return true;
        }

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
    }
}
