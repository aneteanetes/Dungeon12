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

            var respData = Dungeon.Store.EntitySingle<RespawnData>(respawnId);

            var resp = MapObject.Create(respData);
            (resp as Respawn).Spawn();

            return true;
        }
    }
}
