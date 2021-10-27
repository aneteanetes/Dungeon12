using Dungeon;
using Dungeon.View.Interfaces;
using Dungeon12.CardGame.Scene;
using Dungeon12.Conversations;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.Map;

namespace Dungeon12
{
    public class CardGameTrigger : ConversationTrigger
    {
        public override bool Storable => false;

        protected override IDrawText Trigger(PlayerSceneObject arg1, GameMap arg2, string[] args)
        {
            Global.SceneManager.Change<CardGameScene>(args);
            return "Игра началась...".AsDrawText().Montserrat();
        }
    }
}
