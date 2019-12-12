using Dungeon;
using Dungeon.Data;
using Dungeon.Resources;
using Dungeon12.Database.QuestsKill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon12.Entities.Quests
{
    public static class QuestLoader
    {
        public static IQuest Load(string identifyName)
        {
            var kill = Store.Entity<KillQuest>(x => x.IdentifyName == identifyName).FirstOrDefault();
            if (kill != default)
                return kill;

            var collect = CollectQuest.Load(identifyName);
            if (collect != default)
                return collect;
            
            var achive = AchiveQuest.Load(identifyName);
            if (achive != default)
                return achive;

            return default;
        }
    }
}
