using Dungeon12.CardGame.Entities;
using Dungeon12.CardGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon12.CardGame.Triggers
{
    public class TakeGuardCardAbility : IAbilityCardTrigger
    {
        public string Description => "Забирает вражеского стража если они есть.";

        public void Activate(Card card, CardGamePlayer enemy, CardGamePlayer player, AreaCard areaCard)
        {
            var enemyGuard = enemy.Guards.FirstOrDefault();
            if(enemyGuard!=null)
            {
                enemy.Guards.Remove(enemyGuard);

                if (player.Guards.Count < areaCard.Size)
                {
                    player.Guards.Add(enemyGuard);
                }
            }
        }
    }
}
