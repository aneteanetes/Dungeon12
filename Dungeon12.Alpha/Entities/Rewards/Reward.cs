using Dungeon;
using Dungeon.Entities;
using Dungeon.View.Interfaces;
using Dungeon12.Database.Rewards;
using Dungeon12.Entities.Alive;
using Dungeon12.Entities.Rewards.Triggers;
using Dungeon12.Loot;
using Dungeon12.SceneObjects.UI;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.Entities.Quests
{
    public class Reward : DataEntity<Reward, RewardData>
    {
        public List<LootGenerator> ItemGenerators { get; set; } = new List<LootGenerator>();

        public Dictionary<string, string> TalantMap { get; set; } = new Dictionary<string, string>();

        public List<Perk> Perks { get; set; } = new List<Perk>();

        public int Exp { get; set; }

        public int Gold { get; set; }

        public string Varible { get; set; }

        public string[] LootTables { get; set; }

        public string[] LootDrops { get; set; }

        public IRewardTrigger GiveReward { get; set; }

        protected override void Init(RewardData dataClass)
        {
            this.Name = dataClass.RewardText;
            this.ForegroundColor = dataClass.RewardColor;
            this.Exp = dataClass.Exp;
            this.Gold = dataClass.Gold;
            this.TalantMap = dataClass.PossibleTalants;
            this.ItemGenerators = LootDrop.Load<LootDrop>(x => dataClass.LootDropIds.Contains(x.IdentifyName))
                .Select(x => x.Generator)
                .ToList();

            if (dataClass.PerksId != default)
            {
                this.Perks = Perk.LoadAll(x => dataClass.PerksId.Contains(x.ObjectId)).ToList();
            }

            GiveReward = dataClass.TriggerName.Trigger<IRewardTrigger>();
            Varible = dataClass.Variable;
        }

        public override ISceneObject Visual()
        {
            return new RewardSceneObject(this);
        }
    }
}