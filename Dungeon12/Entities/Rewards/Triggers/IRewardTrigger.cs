using Dungeon;
using Dungeon.Map;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entities.Rewards.Triggers
{
    public interface IRewardTrigger : ITrigger<IDrawText, Dungeon12Class, GameMap>
    {
    }
}
