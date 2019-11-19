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
            var kill = KillQuest.Load(identifyName);
            if (kill != default)
                return kill;

            return default;
        }
    }
}
