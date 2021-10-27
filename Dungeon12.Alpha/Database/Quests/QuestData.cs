using Dungeon.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Database.Quests
{
    public class QuestData : Persist
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int MaxProgress { get; set; }

        public string RewardIdentify { get; set; }
    }
}
