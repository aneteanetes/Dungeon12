using Dungeon;
using Dungeon.GameObjects;
using Dungeon12.CardGame.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.CardGame.Engine
{
    public class CardGame : GameComponent
    {
        private CardGameSettings _cardGameSettings;
        private Queue<Card> areaDeck;

        public CardGame(CardGameSettings cardGameSettings)
        {
            _cardGameSettings = cardGameSettings;
            areaDeck = Card.Load(x => x.Type == Interfaces.CardType.Region).Shuffle().AsQueue();
        }

        private CardGamePlayer Player1;
        private CardGamePlayer Player2;
        private CardGamePlayer Winner;

        public (CardGamePlayer Player1, CardGamePlayer Player2) Start(Deck player1deck, Deck player2deck)
        {
            Player1 = new CardGamePlayer()
            {
                Hits = this._cardGameSettings.Hits,
                Resources = this._cardGameSettings.Resources,
                Deck = player1deck,
            };
            Player1.Cards = Player1.Deck.Cards.Shuffle().AsQueue();

            Player2 = new CardGamePlayer()
            {
                Hits = this._cardGameSettings.Hits,
                Resources = this._cardGameSettings.Resources,
                Deck = player2deck
            };
            Player2.Cards = Player2.Deck.Cards.Shuffle().AsQueue();

            return (Player1, Player2);
        }

        private AreaCard currentArea;

        public bool Turn()
        {
            Winner = CheckWinner();
            if (Winner != null)
            {
                return true;
            }

            currentArea.Rounds--;
            if (currentArea.Rounds == 0)
            {
                currentArea = areaDeck.Dequeue().As<AreaCard>();
                if (currentArea == default)
                {
                    return true;
                }
            }

            Player1.Guards.ForEach(g => g.OnTurn(Player2, Player1, currentArea));
            Player2.Guards.ForEach(g => g.OnTurn(Player2, Player1, currentArea));

            return false;
        }

        public CardGamePlayer CheckWinner()
        {
            if (Player1.Hits <= 0)
            {
                return Player2;
            }
            if (Player2.Hits <= 0)
            {
                return Player1;
            }

            if (Player1.Influence >= this._cardGameSettings.Influence)
            {
                return Player1;
            }

            if (Player2.Influence >= this._cardGameSettings.Influence)
            {
                return Player2;
            }

            return default;
        }

        public bool PlayerTurn(CardGamePlayer player)
        {
            return player.AddInHand();
        }

        public bool PlayCard(Card card, CardGamePlayer player)
        {
            var enemy = player == Player1 ? Player2 : Player1;
            card.OnPublish(enemy, player,currentArea);
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