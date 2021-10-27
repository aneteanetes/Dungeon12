namespace Dungeon.Types
{
    public class Range
    {
        public Range() { }

        public Range(int from, int to, int value = 0)
        {
            this.From = from;
            this.To = to;
            this.Value = value;
        }

        public int From { get; set; }

        public int To { get; set; }

        private int _value;

        public int Value
        {
            get => _value;
            set
            {
                var v = value;
                if (v > To)
                    v = To;
                if (v < From)
                    v = From;
                _value = v;
            }
        }

        public int Random() => Dungeon.Random.Next(From, To);

        public int Mid() => (From + To) / 2;
    }
}