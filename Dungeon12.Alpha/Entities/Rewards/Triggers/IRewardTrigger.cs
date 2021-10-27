using Dungeon;
using Dungeon12.Map;
using Dungeon.View.Interfaces;
using Dungeon12.Entities.Quests;
using System;
using System.Collections.Generic;
using System.Text;
using Dungeon12.Classes;

namespace Dungeon12.Entities.Rewards.Triggers
{
    public interface IRewardTrigger : ITrigger<IDrawText, Reward, Character, GameMap>
    {
    }
}
