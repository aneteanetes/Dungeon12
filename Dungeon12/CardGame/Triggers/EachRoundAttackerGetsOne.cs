using Dungeon12.CardGame.Entities;
using Dungeon12.CardGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.CardGame.Triggers
{
    public class EachRoundAttackerGetsOne : IAbilityCardTrigger
    {
        public string Description => "Наносит 1 урон каждый ход";

        public void Activate(Card card, CardGamePlayer enemy, CardGamePlayer player, AreaCard areaCard)
        {
            enemy.Damage(player, 1, areaCard);
        }
    }
}
