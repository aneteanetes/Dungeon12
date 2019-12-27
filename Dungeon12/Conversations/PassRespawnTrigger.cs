using Dungeon;
using Dungeon12.Map;
using Dungeon12.Database.Respawn;
using System;
using System.Collections.Generic;
using System.Text;
using Dungeon12.Map.Objects;

namespace Dungeon12.Conversations
{
    public class PassRespawnTrigger : ITrigger<bool, string[]>
    {
        public virtual bool Trigger(string[] arg1)
        {
            var respawnId = arg1[2];

            if (respawnId.Contains(","))
            {
                foreach (var respawnIdIn in respawnId.Split(",",StringSplitOptions.RemoveEmptyEntries))
                {
                    Global.GameState.RestorableRespawns.Add(respawnIdIn);
                    PassRespawn(respawnIdIn);
                }
            }
            else
            {
                Global.GameState.RestorableRespawns.Add(respawnId);
                PassRespawn(respawnId);
            }

            return true;
        }

        public static void PassRespawn(string respawnId)
        {
            var respData = Dungeon.Store.EntitySingle<RespawnData>(respawnId);

            if (respData.LocationIdentify != default)
            {
                GameMap.AddMapObjectDeffered(new MapDeferredOptions()
                {
                    MapIdentity = respData.LocationIdentify,
                    Object = MapObject.Create(respData)
                });
            }
            else
            {
                var resp = MapObject.Create(respData);
                (resp as Respawn).Spawn();
            }
        }
    }
}
