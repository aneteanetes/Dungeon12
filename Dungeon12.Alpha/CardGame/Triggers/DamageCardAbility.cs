using Dungeon12.CardGame.Entities;
using Dungeon12.CardGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.CardGame.Triggers
{
    public class DamageCardAbility : AbilityCardTrigger
    {
        public override string Description => "Наносит 1 плюс ресурсы*уровень карты";

        public override void Activate(Card card, CardGamePlayer enemy, CardGamePlayer player, AreaCard areaCard)
        {
            if(!(card is AbilityCard abilityCard))
            {
                return;
            }

            enemy.Damage(player, 1 + (abilityCard.ResourceMultiplier * player.Resources), areaCard);
        }
    }
}
