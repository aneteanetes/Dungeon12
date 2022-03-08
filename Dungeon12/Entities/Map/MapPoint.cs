using Dungeon12.Entities.Enums;

namespace Dungeon12.Entities.Map
{
    public class MapPoint
    {
        public int Id { get; set; }

        public Fraction Fraction { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public string Name { get; set; }
    }
}
