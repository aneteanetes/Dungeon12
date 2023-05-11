namespace Dungeon12
{
    internal class RangeValue
    {
        public RangeValue(double current = 0, double max = 0)
        {
            Set(current, max);
        }

        public double Current { get; set; }

        public Value Max { get; set; } = new Value();

        public void Add(double value)
        {
            if (Current + value > Max.FlatValue)
                Current=Max.FlatValue;
            else
                Current+=value;
        }

        public void Set(double current, double max)
        {
            Current=current;
            Max.Set(max);
        }

        public bool ValuesEquals()=>Current==Max.FlatValue;

        public string ToString(string delimiter = "/") => $"{Current}{delimiter}{Max.FlatValue}";

        public override string ToString() => ToString("/");
    }
}
