using Dungeon.Data;
using Dungeon12.Data.Region;
using Dungeon.Physics;
using System.Collections.Generic;
using Dungeon;

namespace Dungeon12.Database.Respawn
{
    public class RespawnData : RegionPart
    {
        public string VariableCondition { get; set; }

        public RespawnPointData[] Respawns { get; set; }

        public PhysicalObject Zone { get; set; }

        public int RespawnSeconds { get; set; }

        public string LocationIdentify { get; set; }
    }

    public class RespawnPointData
    {
        public string Identify { get; set; }

        public string Icon { get; set; }

        public int Amount { get; set; }
    }
}