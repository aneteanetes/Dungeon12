using Dungeon;
using Dungeon12.Items;
using Dungeon12.Loot;
using Dungeon12.Entities.Items;
using System.Linq;

namespace Dungeon12.Loot
{
    public class SpecialItemsGenerator : LootGenerator
    {
        public override Item Generate()
        {
            return Dungeon.Store.EntitySingle<Item>(typeof(Item).AssemblyQualifiedName, Arguments[0]);
        }
    }
}