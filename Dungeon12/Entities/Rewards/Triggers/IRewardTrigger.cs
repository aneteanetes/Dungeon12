using Dungeon;
using Dungeon.Map;
using Dungeon.View.Interfaces;
using Dungeon12.Entities.Quests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entities.Rewards.Triggers
{
    public interface IRewardTrigger : ITrigger<IDrawText, Reward, Dungeon12Class, GameMap>
    {
    }
}
