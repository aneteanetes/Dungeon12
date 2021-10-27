using Dungeon;
using Dungeon.Drawing;
using Dungeon.View.Interfaces;
using Dungeon12.Conversations;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.Items;
using Dungeon12.Map;
using Dungeon12.SceneObjects;
using Force.DeepCloner;
using System.Linq;

namespace Dungeon12.Entities
{
    public class AddIQuestItemTrigger : ConversationTrigger
    {
        protected override IDrawText Trigger(PlayerSceneObject arg1, GameMap arg2, string[] arg3)
        {
            var getItemId= arg3.ElementAtOrDefault(0);
           
            if (getItemId != default)
            {
                var @char = Global.GameState.Character;

                var putItem = Store.EntitySingle<Item>(getItemId);
                @char.Backpack.Add(putItem.DeepClone());
                return new DrawText("Получен предмет");
            }

            return default;
        }
    }
}
