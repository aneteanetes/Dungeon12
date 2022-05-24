using Dungeon12.Entities.Enums;
using Dungeon12.Entities.Objects;
using System.Collections.Generic;

namespace Dungeon12.Entities.Map
{
    internal class MapPoint
    {
        public int Id { get; set; }

        public Fraction Fraction { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Illuminate { get; set; }

        public int[] Joins { get; set; } = new int[0];

        public bool Closeable { get; set; } = true;

        public List<MapObject> Objects { get; set; } = new List<MapObject>();
    }
}
