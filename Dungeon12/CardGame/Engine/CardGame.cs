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
            areaDeck = Card.Load(x => x.CardType == Interfaces.CardType.Region).Shuffle().AsQueue();
        }

        public CardGamePlayer Player1 { get; private set; }
        public CardGamePlayer Player2 { get; private set; }
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

        public AreaCard CurrentArea { get; private set; }

        public bool AreaEnded = false;

        public bool Turn()
        {
            Winner = CheckWinner();
            if (Winner != null)
            {
                OnafterTurn?.Invoke();
                return true;
            }

            bool nextRound = CurrentArea == default || CurrentArea.Rounds == 0;
            if (nextRound)
            {
                CurrentArea = areaDeck.Dequeue().As<AreaCard>();
                if (CurrentArea == default)
                {
                    AreaEnded = true;
                    OnafterTurn?.Invoke();
                    return true;
                }
            }
            CurrentArea.Rounds--;

            Player1.Guards.ForEach(g => g.OnTurn(Player2, Player1, CurrentArea));
            Player2.Guards.ForEach(g => g.OnTurn(Player2, Player1, CurrentArea));

            OnafterTurn?.Invoke();
            return false;
        }

        public Action OnafterTurn { get; set; }

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
            card.OnPublish(enemy, player,CurrentArea);
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
                        player.AddResource();
                        break;
                    }
                default:
                    break;
            }

            return true;
        }
    }
}