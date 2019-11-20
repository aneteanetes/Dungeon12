using Dungeon;
using Dungeon.Entities;
using Dungeon.Entities.Alive;
using Dungeon.Items;
using Dungeon12.Database.Rewards;
using Dungeon12.Entities.Rewards.Triggers;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.Entities.Quests
{
    public class Reward : DataEntity<Reward, RewardData>
    {
        public List<Item> Items { get; set; }

        public List<Perk> Perks { get; set; }

        public int Exp { get; set; }

        public int Gold { get; set; }

        public IRewardTrigger GiveReward { get; set; }

        protected override void Init(RewardData dataClass)
        {
            this.Name = dataClass.RewardText;
            this.ForegroundColor = dataClass.RewardColor;
            this.Exp = dataClass.Exp;
            this.Gold = dataClass.Gold;

            this.Items = Item.LoadAll(x => dataClass.ItemsId.Contains(x.ObjectId)).ToList();
            this.Perks = Perk.LoadAll(x => dataClass.PerksId.Contains(x.ObjectId)).ToList();

            GiveReward = dataClass.TriggerName.Trigger<IRewardTrigger>();
        }
    }
}