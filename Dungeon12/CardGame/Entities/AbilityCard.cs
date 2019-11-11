using Dungeon;
using Dungeon12.CardGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.CardGame.Entities
{
    public class AbilityCard : Card
    {
        public string ActivateTriggerName { get; set; }

        public void Activate(CardGamePlayer enemy, CardGamePlayer player)
        {
            if (ActivateTriggerName != default)
            {
                ActivateTriggerName.GetInstanceFromAssembly<IAbilityCardTrigger>("Dungeon12").Activate(enemy, player);
            }
        }
    }
}
