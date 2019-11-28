using Dungeon.Data;
using Dungeon.Physics;
using System.Collections.Generic;

namespace Dungeon12.Database.Respawn
{
    public class RespawnData : Persist
    {
        public Dictionary<string, int> MobsNameAmount { get; set; }

        public PhysicalObject Zone { get; set; }
    }
}