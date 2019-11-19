using Dungeon12.Database.Quests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Database.QuestsKill
{
    public class QuestKillData : QuestData
    {
        public string[] MobIdentify { get; set; }

        public int[] Amount { get; set; }
    }
}
