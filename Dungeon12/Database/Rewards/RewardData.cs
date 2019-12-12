using Dungeon.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Database.Rewards
{
    public class RewardData : Persist
    {
        public PersistColor RewardColor { get; set; }

        public string RewardText { get; set; }

        public string[] LootDropIds { get; set; } = new string[0];

        public int[] PerksId { get; set; }

        public int Exp { get; set; }

        public int Gold { get; set; }

        public string TriggerName { get; set; }

        public Dictionary<string,string> PossibleTalants { get; set; }
    }
}