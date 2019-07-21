namespace Rogue.Classes
{
    using Rogue.View.Interfaces;

    public class ClassStat
    {
        public ClassStat(string title,string value, IDrawColor color)
        {
            Title = title;
            Value = value;
            Color = color;
        }

        public string Title { get; set; }

        public string Value { get; set; }

        public IDrawColor Color { get; set; }

        public int Group { get; set; }

    }
}
