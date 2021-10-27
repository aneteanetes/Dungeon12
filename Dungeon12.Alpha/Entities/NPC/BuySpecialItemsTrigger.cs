using Dungeon.Drawing;
using Dungeon.View.Interfaces;
using Dungeon12.Conversations;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.Map;
using System.Linq;

namespace Dungeon12.Entities
{
    public class BuySpecialItemsTrigger : ConversationTrigger
    {
        protected override IDrawText Trigger(PlayerSceneObject arg1, GameMap arg2, string[] arg3)
        {
            var specialItemId = arg3.ElementAtOrDefault(0);
            int.TryParse(arg3.ElementAtOrDefault(1), out var gold);
            if (specialItemId != default)
            {
                var @char = Global.GameState.Character;
                var neededResources = @char.Backpack.GetItems().Where(x => x.IdentifyName == specialItemId).ToList();
                foreach (var neededResource in neededResources)
                {
                    @char.Backpack.Remove(neededResource);
                    @char.Gold += gold;
                }
            }

            return new DrawText("Отличная сделка!");
        }
    }
}
