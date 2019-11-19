using Dungeon;
using Dungeon12.CardGame.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.CardGame.Interfaces
{
    public interface IAbilityCardTrigger : ITrigger<bool,Card,CardGamePlayer,CardGamePlayer,AreaCard>
    {
        string Description { get; }
    }

    public abstract class AbilityCardTrigger : IAbilityCardTrigger
    {
        public abstract string Description { get; }

        public abstract void Activate(Card card, CardGamePlayer enemy, CardGamePlayer player, AreaCard areaCard);

        public bool Trigger(Card arg1, CardGamePlayer arg2, CardGamePlayer arg3, AreaCard arg4)
        {
            Activate(arg1, arg2, arg3, arg4);
            return true;
        }
    }
}
