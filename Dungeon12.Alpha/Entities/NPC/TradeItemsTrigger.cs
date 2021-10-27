using Dungeon;
using Dungeon.Drawing;
using Dungeon.View.Interfaces;
using Dungeon12.Conversations;
using Dungeon12.Drawing.SceneObjects.Map;
using Dungeon12.Items;
using Dungeon12.Map;
using Force.DeepCloner;
using System.Linq;

namespace Dungeon12.Entities
{
    public class TradeItemsTrigger : ConversationTrigger
    {
        protected override IDrawText Trigger(PlayerSceneObject arg1, GameMap arg2, string[] arg3)
        {
            var getItemId= arg3.ElementAtOrDefault(0);
            var getItemAmount = arg3.ElementAtOrDefault(2);
            
            var putItemId = arg3.ElementAtOrDefault(3);
            var putItemAmount = arg3.ElementAtOrDefault(4);

            if (getItemId != default && int.TryParse(getItemAmount, out var getAmount)
                && putItemId != default && int.TryParse(putItemAmount, out var putAmount))
            {
                var @char = Global.GameState.Character;
                var neededItems = @char.Backpack.GetItems().Where(x => x.IdentifyName == getItemId).ToList();

                foreach (var needItem in neededItems)
                {
                    if (needItem.Stackable)
                    {
                        @char.Backpack.Remove(neededItems.FirstOrDefault(), quantity: getAmount);
                        break;
                    }
                    @char.Backpack.Remove(neededItems.FirstOrDefault());
                }

                var putItem = Store.EntitySingle<Item>(putItemId);

                for (int i = 0; i < putAmount; i++)
                {
                    @char.Backpack.Add(putItem.DeepClone());
                }

                return new DrawText("Отличная сделка!");
            }

            return new DrawText("У вас недостаточно нужных предметов!");
        }
    }
}
