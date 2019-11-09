using Dungeon.Data;
using Dungeon.Data.Region;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Database.Barrels
{
    public class BarrelData : RegionPart
    {
        public ConsoleColor Color { get; set; }

        public string Name { get; set; }
    }
}
