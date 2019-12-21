using Dungeon.Scenes.Manager;
using Dungeon.View.Interfaces;
using Dungeon12.Conversations;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.SceneObjects.NPC;
using Dungeon12.Scenes.Menus;

namespace Dungeon12.Map.Triggers
{
    public class EndTrigger : ConversationTrigger
    {
        protected override IDrawText Trigger(PlayerSceneObject arg1, GameMap arg2, string[] arg3)
        {
            NPCDialogue.ForceExit();

            SceneManager.Switch<EndGame>();

            return default;
        }
    }
}