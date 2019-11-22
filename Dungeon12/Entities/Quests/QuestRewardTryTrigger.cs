using Dungeon;
using Dungeon.Conversations;
using Dungeon.Drawing.SceneObjects.Map;
using Dungeon.Map;
using Dungeon.View.Interfaces;
using System;
using System.Linq;

namespace Dungeon12.Entities.Quests
{
    public class QuestRewardTryTrigger : IConversationTrigger
    {
        public bool Storable => false;

        public IDrawText Trigger(PlayerSceneObject arg1, GameMap arg2, string[] arg3, Replica arg4)
        {
            var questName = arg3[0];
            var variableSuccessName = arg3[1];
            var replicaFailure = arg3[2];
            var replicaSuccess = arg3[3];
            var player = arg1.Component.Entity.As<Dungeon12Class>();

            var quest = player.ActiveQuests.FirstOrDefault(q => q.IdentifyName == questName);
            if (quest.IsCompleted())
            {
                arg4.Conversation.Variables.FirstOrDefault(v => v.Name == variableSuccessName)?.Trigger(arg4.Tag);
                quest.Complete();
                return replicaSuccess.AsDrawText();
            }

            return replicaFailure.AsDrawText();
        }
    }
}