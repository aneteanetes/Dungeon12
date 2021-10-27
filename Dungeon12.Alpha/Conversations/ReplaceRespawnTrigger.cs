using Dungeon;
using Dungeon12.Map;
using Dungeon12.Database.Respawn;
using System;
using System.Collections.Generic;
using System.Text;
using Dungeon.View.Interfaces;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.Map.Objects;

namespace Dungeon12.Conversations
{
    public class ReplaceRespawnTrigger : ConversationTrigger
    {
        protected override IDrawText Trigger(PlayerSceneObject arg1, GameMap arg2, string[] arg3)
        {
            //ставим респаун
            var respawnId = arg3[0];
            var respData = Dungeon.Store.EntitySingle<RespawnData>(respawnId);
            var resp = MapObject.Create(respData);

            //уничтожаем это обсуждение
            //выходим из диалога
            var npc = arg2.One<NPCMap>(arg2.MapObject, x => x.Conversations.Contains(this.Replica.Conversation));
            if (npc != default)
            {
                npc.Destroy?.Invoke();
            }

            return default;
        }
    }
}
