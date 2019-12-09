namespace Dungeon12.Map.Infrastructure
{
    using Dungeon.View.Interfaces;

    public class MapObjectColor : IDrawColor
    {
        public byte R { get; set; }

        public byte G { get; set; }

        public byte B { get; set; }

        public byte A { get; set; }

        public double Opacity { get; set; }
    }
}