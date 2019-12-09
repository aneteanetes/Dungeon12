using Dungeon;
using Dungeon12.Items;
using Dungeon12.Loot;
using Dungeon12.Entities.Items;
using System.Linq;

namespace Dungeon12.Loot
{
    public class QuestItemsGenerator : LootGenerator
    {
        public override Item Generate()
        {
            var questItem = Dungeon.Store.EntitySingle<Item>(typeof(QuestItem).AssemblyQualifiedName, Arguments[0]);

            if (int.TryParse(Arguments.ElementAtOrDefault(1), out var limit))
            {
                var now = Global.GameState.Player.Component.Entity[questItem.IdentifyName].As<int>();

                if (now == limit)
                {
                    return default;
                }
                else
                {
                    now++;
                    Global.GameState.Player.Component.Entity[questItem.IdentifyName] = now;
                }
            }

            return questItem;
        }
    }
}