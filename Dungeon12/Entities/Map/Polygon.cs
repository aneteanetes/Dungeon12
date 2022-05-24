using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.Entities.Map
{
    internal class Polygon
    {
        public string ObjectId { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        public string ObjectImage { get; set; }

        public string Function { get; set; }

        public bool IsNotEmpty => !string.IsNullOrWhiteSpace(ObjectId) || !string.IsNullOrWhiteSpace(Function);

        public void Load(Polygon polygon)
        {
            this.Name = polygon.Name;
            this.Icon = polygon.Icon;
            this.Function = polygon.Function;
        }

        public void Init()
        {
            if (P0 == null)
                P0 = new Polygon();
            if (P1 == null)
                P1 = new Polygon();
            if (P2 == null)
                P2 = new Polygon();
            if (P3 == null)
                P3 = new Polygon();
            if (P4 == null)
                P4 = new Polygon();
            if (P5 == null)
                P5 = new Polygon();
        }

        public Polygon P0 { get; set; }

        public Polygon P1 { get; set; }

        public Polygon P2 { get; set; }

        public Polygon P3 { get; set; }

        public Polygon P4 { get; set; }

        public Polygon P5 { get; set; }
    }
}
