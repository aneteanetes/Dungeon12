using Dungeon;
using Dungeon.Data.Attributes;
using Dungeon.Entities;
using Dungeon.Map;
using Dungeon12.CardGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.CardGame.Entities
{
    [DataClass(typeof(string))]
    public class Card : Entity
    {
        public string Description { get; set; }

        public string PublishTriggerName { get; set; }

        public virtual void OnPublish(CardGamePlayer enemy, CardGamePlayer player)
        {
            if (PublishTriggerName != default)
            {
                PublishTriggerName.GetInstanceFromAssembly<IAbilityCardTrigger>(Assembly).Activate(this,enemy, player);
            }
        }

        public string TurnTriggerName { get; set; }

        public void OnTurn(CardGamePlayer enemy, CardGamePlayer player)
        {
            if (TurnTriggerName != default)
            {
                TurnTriggerName.GetInstanceFromAssembly<IAbilityCardTrigger>(Assembly).Activate(this, enemy, player);
            }
        }

        public string DieTriggerName { get; set; }

        public void OnDie(CardGamePlayer enemy, CardGamePlayer player)
        {
            if (DieTriggerName != default)
            {
                DieTriggerName.GetInstanceFromAssembly<IAbilityCardTrigger>(Assembly).Activate(this, enemy, player);
            }
        }
    }
}
