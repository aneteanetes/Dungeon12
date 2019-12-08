using Dungeon;
using Dungeon.Map;
using Dungeon12.Database.Respawn;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Conversations
{
    public class PassRespawnTrigger : ITrigger<bool, string[]>
    {
        public bool Trigger(string[] arg1)
        {
            var respawnId = arg1[2];

            var respData = Dungeon.Data.Database.EntitySingle<RespawnData>(respawnId);

            var resp = MapObject.Create(respData);

            return true;
        }
    }
}
