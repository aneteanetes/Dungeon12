namespace Dungeon.Data.Region
{
    using Dungeon.Physics;
    using Dungeon.Types;
    using System.Collections.Generic;

    public class Region : Persist
    {
        public string Tile { get; set; }

        public string TileBack { get; set; }

        public Point TileBackOffset { get; set; }

        public List<RegionPart> Objects { get; set; }

        public List<PhysicalObject> SafeZones { get; set; }

        public string Name { get; set; }

        public string Display { get; set; }

        public bool IsUnderLevel { get; set; }
    }
}
