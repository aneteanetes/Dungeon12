namespace Rogue.Data.Region
{
    using System.Collections.Generic;

    public class Region : Persist
    {
        public List<RegionPart> Objects { get; set; }

        public List<RegionNPC> NPCs { get; set; }

        public string Name { get; set; }
    }
}
