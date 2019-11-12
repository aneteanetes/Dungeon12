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
                Influence = this._cardGameSettings.Influence,
                Hits = this._cardGameSettings.Hits,
                Resources = this._cardGameSettings.Resources,
                Deck = enemyDeck
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
                if (currentArea == default)
                {
                    return true;
                }
            }

            Player1.Guards.ForEach(g => g.OnTurn(Player2, Player1));
            Player2.Guards.ForEach(g => g.OnTurn(Player2, Player1));

            return false;
        }

        public bool PlayCard(Card card, CardGamePlayer player)
        {
            var enemy = player == Player1 ? Player2 : Player1;
            card.OnPublish(enemy, player);
            switch (card)
            {
                case GuardCard guard:
                    {
                        player.Influence += 5;
                        player.Guards.Add(guard);
                        break;
                    }
                case Card _:
                    {
                        player.Influence += 1;
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