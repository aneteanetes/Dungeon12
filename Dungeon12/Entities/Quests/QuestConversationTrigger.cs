using Dungeon;
using Dungeon12.Conversations;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.Map;
using Dungeon.View.Interfaces;
using System.Linq;

namespace Dungeon12.Entities.Quests
{
    public class QuestConversationTrigger : ConversationTrigger
    {
        public override bool Storable => true;

        protected override IDrawText Trigger(PlayerSceneObject arg1, GameMap arg2, string[] arg3)
        {
            var @class = arg1.Component.Entity;
            var quest = QuestLoader.Load(arg3[0]);
            quest.Bind(@class,arg2);

            if (arg3.ElementAtOrDefault(1) != default)
            {
                try
                {
                    arg3[1].Trigger<ITrigger<bool, string[]>>().Trigger(arg3);
                }
                catch { }
            }

            return default;
        }
    }
}
