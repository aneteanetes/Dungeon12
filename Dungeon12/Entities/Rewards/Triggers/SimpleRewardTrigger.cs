using Dungeon;
using Dungeon.Map;
using Dungeon.View.Interfaces;
using Dungeon12.Entities.Quests;

namespace Dungeon12.Entities.Rewards.Triggers
{
    public class SimpleRewardTrigger : IRewardTrigger
    {
        public IDrawText Trigger(Reward arg1, Dungeon12Class arg2, GameMap arg3)
        {
            arg2.Exp(arg1.Exp);
            return " ".AsDrawText();
        }
    }
}
