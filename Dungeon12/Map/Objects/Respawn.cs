using Dungeon.Data;
using Dungeon.Data.Attributes;
using Dungeon.Data.Region;
using Dungeon.Entities;
using Dungeon.Map;
using Dungeon12.Database.Respawn;
using System;
using System.Collections.Generic;
using System.Text;
using Dungeon;
using Dungeon.Physics;
using Dungeon.Types;
using System.Linq;

namespace Dungeon12.Map.Objects
{
    [DataClass(typeof(RespawnData))]
    public class Respawn : MapObject
    {
        /// <summary>
        /// TODO: сделать период респауна в игровом времени
        /// </summary>
        public Dictionary<string, int> MobRespawnData { get; set; }

        public PhysicalObject SpawnArea { get; set; }

        protected override void Load(RegionPart regionPart)
        {
            base.Load(regionPart);
            var data = regionPart.As<RespawnData>();
            MobRespawnData = data.MobsNameAmount;
            SpawnArea = data.Zone;

            foreach (var mobData in MobRespawnData)
            {
                for (int i = 0; i < mobData.Value; i++)
                {
                    var mob = Create(new RegionPart()
                    {
                        Icon = "*",
                        IdentifyName = mobData.Key
                    });
                    SetLocation(20, mob);
                }
            }
        }

        private void SetLocation(int tries, MapObject mob)
        {
            for (int i = 0; i < tries; i++)
            {
                if (TrySetLocation(mob))
                    break;
            }
        }

        private bool TrySetLocation(MapObject mob)
        {
            var pos = SpawnArea.Position;
            var posMax = SpawnArea.MaxPosition;

            var x = RandomDungeon.Range(pos.X, posMax.X);
            var y = RandomDungeon.Range(pos.Y, posMax.Y);

            mob.Location = new Point(x, y);

            var otherObject = Global.GameState.Map.MapObject.Query(mob).Nodes.Any(node => node.IntersectsWith(mob));
            if (otherObject)
                return false;

            if (InSafe(mob))
            {
                return false;
            }

            return true;
        }

        public bool InSafe(MapObject @object) => Global.GameState.Map.SafeZones.Any(safeZone => safeZone.IntersectsWith(@object));
    }
}