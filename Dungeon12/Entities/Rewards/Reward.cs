using Dungeon;
using Dungeon.Entities;
using Dungeon.Entities.Alive;
using Dungeon.Game;
using Dungeon.Items;
using Dungeon.Loot;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.Database.Rewards;
using Dungeon12.Entities.Rewards.Triggers;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.Entities.Quests
{
    public class Reward : DataEntity<Reward, RewardData>
    {
        public List<LootGenerator> ItemGenerators { get; set; }

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

            this.ItemGenerators = LootDrop.Load<LootDrop>(x => dataClass.LootDropIds.Contains(x.IdentifyName))
                .Select(x => x.Generator)
                .ToList();

            this.Perks = Perk.LoadAll(x => dataClass.PerksId.Contains(x.ObjectId)).ToList();

            GiveReward = dataClass.TriggerName.Trigger<IRewardTrigger>();
        }

        public override ISceneObject Visual(GameState gameState)
        {
            return new TextControl(("Опыт: " + Exp.ToString()).AsDrawText().InColor(this.ForegroundColor).Montserrat());
        }
    }
}