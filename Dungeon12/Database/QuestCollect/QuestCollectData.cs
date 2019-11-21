using Dungeon.Data;
using Dungeon12.Database.Quests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Database.QuestCollect
{
    public class QuestCollectData : QuestData
    {
        public string[] ItemsIdentify { get; set; }

        public int[] Amount { get; set; }
    }
}
