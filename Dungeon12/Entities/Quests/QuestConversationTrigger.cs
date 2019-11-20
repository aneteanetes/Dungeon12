using Dungeon;
using Dungeon.Conversations;
using Dungeon.Drawing.SceneObjects.Map;
using Dungeon.Map;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entities.Quests
{
    public class QuestConversationTrigger : IConversationTrigger
    {
        public bool Storable => true;

        public IDrawText Trigger(PlayerSceneObject arg1, GameMap arg2, string[] arg3)
        {
            var @class = arg1.Component.Entity.As<Dungeon12Class>();
            var quest = QuestLoader.Load(arg3[0]);
            quest.Bind(@class,arg2);

            return default;
        }
    }
}
