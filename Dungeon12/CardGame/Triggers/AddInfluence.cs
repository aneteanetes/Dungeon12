using Dungeon12.CardGame.Entities;
using Dungeon12.CardGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.CardGame.Triggers
{
    public class AddInfluence : AbilityCardTrigger
    {
        public override string Description => "Добавляет 2*ресурсы влияния.";

        public override void Activate(Card card, CardGamePlayer enemy, CardGamePlayer player, AreaCard areaCard)
        {
            player.Influence += 2 * player.Resources;
        }
    }
}