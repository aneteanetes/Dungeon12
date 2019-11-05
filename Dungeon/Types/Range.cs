namespace Dungeon.Types
{
    public class Range
    {
        public Range() { }

        public Range(int from, int to)
        {
            this.From = from;
            this.To = to;
        }

        public int From { get; set; }

        public int To { get; set; }

        public int Random() => RandomDungeon.Next(From, To);

        public int Mid() => (From + To) / 2;
    }
}