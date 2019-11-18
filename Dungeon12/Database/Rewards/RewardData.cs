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

        public string ItemGenerator { get; set; }

        public string EntityUpGenerator { get; set; }
    }
}