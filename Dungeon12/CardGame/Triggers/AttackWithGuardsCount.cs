using Dungeon12.CardGame.Entities;
using Dungeon12.CardGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.CardGame.Triggers
{
    public class AttackWithGuardsCount : IAbilityCardTrigger
    {
        public string Description => "Наносит урон 2*ресурсы плюс кол-во стражей.";

        public void Activate(Card card, CardGamePlayer enemy, CardGamePlayer player, AreaCard areaCard)
        {
            enemy.Damage(player, (2 * player.Resources) + player.Guards.Count, areaCard);
        }
    }
}
