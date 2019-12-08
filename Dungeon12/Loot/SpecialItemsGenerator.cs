using Dungeon;
using Dungeon.Items;
using Dungeon.Loot;
using Dungeon12.Entities.Items;
using System.Linq;

namespace Dungeon12.Loot
{
    public class SpecialItemsGenerator : LootGenerator
    {
        public override Item Generate()
        {
            return Dungeon.Data.Database.EntitySingle<Item>(typeof(Item).AssemblyQualifiedName, Arguments[0]);
        }
    }
}