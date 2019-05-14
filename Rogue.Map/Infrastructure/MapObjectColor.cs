namespace Rogue.Map.Infrastructure
{
    using Rogue.View.Interfaces;

    public class MapObjectColor : IDrawColor
    {
        public byte R { get; set; }

        public byte G { get; set; }

        public byte B { get; set; }

        public byte A { get; set; }

        public double Opacity { get; set; }
    }
}