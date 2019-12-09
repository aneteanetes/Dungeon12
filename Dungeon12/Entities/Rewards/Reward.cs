using Dungeon;
using Dungeon.Entities;
using Dungeon12.Entities.Alive;
using Dungeon12.Game;
using Dungeon12.Items;
using Dungeon12.Loot;
using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.Database.Rewards;
using Dungeon12.Entities.Rewards.Triggers;
using Dungeon12.SceneObjects.UI;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.Entities.Quests
{
    public class Reward : DataEntity<Reward, RewardData>
    {
        public List<LootGenerator> ItemGenerators { get; set; } = new List<LootGenerator>();

        public List<Perk> Perks { get; set; } = new List<Perk>();

        public int Exp { get; set; }

        public int Gold { get; set; }

        public IRewardTrigger GiveReward { get; set; }

        protected override void Init(RewardData dataClass)
        {
            this.Name = dataClass.RewardText;
            this.ForegroundColor = dataClass.RewardColor;
            this.Exp = dataClass.Exp;
            this.Gold = dataClass.Gold;

            this.ItemGenerators = LootDrop.Load<LootDrop>(x => dataClass.LootDropIds.Contains(x.IdentifyName))
                .Select(x => x.Generator)
                .ToList();

            if (dataClass.PerksId != default)
            {
                this.Perks = Perk.LoadAll(x => dataClass.PerksId.Contains(x.ObjectId)).ToList();
            }

            GiveReward = dataClass.TriggerName.Trigger<IRewardTrigger>();
        }

        public override ISceneObject Visual()
        {
            return new RewardSceneObject(this);
        }
    }
}