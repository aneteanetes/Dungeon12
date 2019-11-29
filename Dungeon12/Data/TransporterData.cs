using Dungeon.Data.Region;
using Dungeon.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Data
{
    public class TransporterData : RegionPart
    {
        public string UnderlevelIdentify { get; set; }

        public string RegionIdentify { get; set; }

        public Point Destination { get; set; }
    }
}
