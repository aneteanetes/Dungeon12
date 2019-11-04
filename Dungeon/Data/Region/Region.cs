namespace Dungeon.Data.Region
{
    using Dungeon.Physics;
    using System.Collections.Generic;

    public class Region : Persist
    {
        public List<RegionPart> Objects { get; set; }

        public List<PhysicalObject> SafeZones { get; set; }

        public string Name { get; set; }
    }
}
