using Dungeon.Items;
using Dungeon.Loot;
using Dungeon12.Entities.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Loot
{
    public class QuestItemsGenerator : LootGenerator
    {
        public override Item Generate()
        {
            return Dungeon.Data.Database.EntitySingle<Item>(typeof(QuestItem).AssemblyQualifiedName, Arguments[0]);
        }
    }
}
