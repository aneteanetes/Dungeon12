using Dungeon;
using Dungeon12.CardGame.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.CardGame.Engine
{
    public class CardGame
    {
        private CardGameSettings _cardGameSettings;
        private Deck areaDeck;

        public CardGame(CardGameSettings cardGameSettings)
        {
            _cardGameSettings = cardGameSettings;
            areaDeck = new Deck()
            {
                Cards = new Queue<Card>(new AreaCard()
                {
                    Rounds = 5
                }.InEnumerable())
            };
        }

        private CardGamePlayer Player1;
        private CardGamePlayer Player2;

        public (CardGamePlayer Player1, CardGamePlayer Player2) Start(Deck enemyDeck, Deck playerDeck)
        {
            Player1 = new CardGamePlayer()
            {
                Influence=this._cardGameSettings.Influence,
                Hits=this._cardGameSettings.Hits,
                Resources=this._cardGameSettings.Resources,
                Deck=enemyDeck
            };

            Player2 = new CardGamePlayer()
            {
                Influence = this._cardGameSettings.Influence,
                Hits = this._cardGameSettings.Hits,
                Resources = this._cardGameSettings.Resources,
                Deck = playerDeck
            };

            return (Player1, Player2);
        }

        private AreaCard currentArea;
        public bool Turn()
        {
            currentArea.Rounds--;
            if (currentArea.Rounds == 0)
            {
                currentArea = areaDeck.Cards.Dequeue().As<AreaCard>();
            }

            if (areaDeck.Cards.Count == 0)
            {
                return true;
            }

            return false;
        }

        public bool PlayCard(Card card, CardGamePlayer player)
        {
            var enemy = player == Player1 ? Player2 : Player1;
            switch (card)
            {
                case AbilityCard ability:
                    {
                        ability.Activate(enemy, player);
                        break;
                    }
                case GuardCard guard:
                    {
                        player.Guards.Add(guard);
                        break;
                    }
                case Card _:
                    {
                        player.Resources++;
                        break;
                    }
                default:
                    break;
            }

            return true;
        }
    }
}