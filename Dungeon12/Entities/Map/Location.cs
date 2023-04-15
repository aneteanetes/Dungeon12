using Dungeon.Types;

namespace Dungeon12.Entities.Map
{
    internal class Location : CoordDictionary<Polygon>
    {
        public string UId => Region.MapId + Index;

        public Region Region { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string BackgroundImage { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public string ObjectImage { get; set; }

        public bool IsCurrent { get; set; }

        public string ObjectId { get; set; }

        public List<Polygon> Polygons { get; set; } = new List<Polygon>();

        public int Index { get; set; }

        public int[] IndexLinks { get; set; }

        public List<Location> Links { get; set; }

        public List<LocationTransition> Transitions { get; set; } = new();

        public Dot Size { get; set; }

        public Dot Position { get; set; }

        public bool IsOdd { get; set; }

        public bool IsOpen { get; set; }

        public bool IsActivable { get; set; } = true;

        public void Init()
        {
            this.Polygons.ForEach(x =>
            {
                this.Add(x.X, x.Y, x);
            });
        }
    }
}
