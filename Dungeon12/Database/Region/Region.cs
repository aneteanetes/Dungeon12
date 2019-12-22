namespace Dungeon12.Data.Region
{
    using Dungeon.Physics;
    using Dungeon.Types;
    using Dungeon.Data;
    using System.Collections.Generic;
    using Dungeon12.Database.Respawn;

    public class Region : Persist
    {
        public string Tile { get; set; }

        public string TileBack { get; set; }

        public Point TileBackOffset { get; set; }

        public Point Offset { get; set; }

        public List<RegionPart> Objects { get; set; }

        public List<PhysicalObject> SafeZones { get; set; }

        public List<RespawnData> RandomObjects { get; set; } = new List<RespawnData>();

        public string Name { get; set; }

        public string Display { get; set; }

        public bool IsUnderLevel { get; set; }

        public string RegionMusic { get; set; }
    }
}
