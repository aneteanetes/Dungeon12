using Dungeon12.CardGame.Entities;
using Dungeon12.CardGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.CardGame.Triggers
{
    public class AddInfluence : IAbilityCardTrigger
    {
        public string Description => "Добавляет 1*ресурсы влияния.";

        public void Activate(Card card, CardGamePlayer enemy, CardGamePlayer player, AreaCard areaCard)
        {
            player.Influence += 1 * player.Resources;
        }
    }
}