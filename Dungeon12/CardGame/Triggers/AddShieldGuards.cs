using Dungeon12.CardGame.Entities;
using Dungeon12.CardGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.CardGame.Triggers
{
    public class AddShieldGuards : AbilityCardTrigger
    {
        public override string Description => "Увеличивает щиты стражей на 1*ресурсы.";

        public override void Activate(Card card, CardGamePlayer enemy, CardGamePlayer player, AreaCard areaCard)
        {
            player.Guards.ForEach(g =>
            {
                g.Shield += 1 * player.Resources;
            });
        }
    }
}
