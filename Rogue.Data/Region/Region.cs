namespace Rogue.Data.Region
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Region : Persist
    {
        public List<RegionPart> Objects { get; set; }
    }
}
