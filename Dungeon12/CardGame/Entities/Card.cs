using Dungeon;
using Dungeon.Data.Attributes;
using Dungeon.Entities;
using Dungeon.Map;
using Dungeon12.CardGame.Interfaces;
using Dungeon12.Database.CardGameCard;
using Dungeon12.Database.CardGameDeck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Dungeon12.CardGame.Entities
{
    public class Card : DataEntity<Card, CardGameCardData>
    {
        public string Description { get; set; }

        public string PublishTriggerName { get; set; }

        public virtual void OnPublish(CardGamePlayer enemy, CardGamePlayer player, AreaCard areaCard)
        {
            if (PublishTriggerName != default)
            {
                PublishTriggerName.GetInstanceFromAssembly<IAbilityCardTrigger>(Assembly).Activate(this, enemy, player,areaCard);
            }
        }

        public string TurnTriggerName { get; set; }

        public void OnTurn(CardGamePlayer enemy, CardGamePlayer player, AreaCard areaCard)
        {
            if (TurnTriggerName != default)
            {
                TurnTriggerName.GetInstanceFromAssembly<IAbilityCardTrigger>(Assembly).Activate(this, enemy, player,areaCard);
            }
        }

        public string DieTriggerName { get; set; }

        public void OnDie(CardGamePlayer enemy, CardGamePlayer player, AreaCard areaCard)
        {
            if (DieTriggerName != default)
            {
                DieTriggerName.GetInstanceFromAssembly<IAbilityCardTrigger>(Assembly).Activate(this, enemy, player,areaCard);
            }
        }

        public CardType CardType { get; set; }

        public new static List<Card> Load(Expression<Func<CardGameCardData, bool>> filterOne, object cacheObject=default)
        {
            List<Card> cards = new List<Card>();
            var dataClasses = Dungeon.Data.Database.Entity(filterOne);

            foreach (var dataClass in dataClasses)
            {
                Card card = default;
                if (dataClass != default)
                {
                    switch (dataClass.CardType)
                    {
                        case CardType.Resource: card = dataClass.Card; break;
                        case CardType.Guardian: card = dataClass.Card.As<GuardCard>(); break;
                        case CardType.Ability: card = dataClass.Card.As<AbilityCard>(); break;
                        case CardType.Region: card = dataClass.Card.As<AreaCard>(); break;
                    }
                }

                if (card != default)
                {
                    card.CardType = dataClass.CardType;
                    card.Assembly = dataClass.Assembly;
                    card.Name = dataClass.Name;
                    cards.Add(card);
                }
            }          

            return cards;
        }
    }
}