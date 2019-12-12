namespace Dungeon12.Items.Types
{
    using Dungeon12.Entities.Rewards.Triggers;
    using Dungeon12.Items.Enums;
    using System.Collections.Generic;

    public class TalantItem : Item
    {
        public override ItemKind Kind => ItemKind.Activable;

        public override Rarity Rare => Rarity.Watered;

        public Dictionary<string, string> TalantMap { get; set; }

        public override void Activate()
        {
            var @char = Global.GameState.Character;

            new TalantRewardTrigger().Trigger(
                new Entities.Quests.Reward() { TalantMap = TalantMap },
                @char,
                Global.GameState.Map);

            @char.Backpack.Remove(this);            
        }
    }
}