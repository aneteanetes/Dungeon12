using Dungeon.Data;
using Dungeon12.Data.Region;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Database.ClickTriggers
{
    public class ClickTriggerData : RegionPart
    {
        public string TriggerName { get; set; }

        public object[] TriggerArguments { get; set; }

        public string Name { get; set; }
    }
}
