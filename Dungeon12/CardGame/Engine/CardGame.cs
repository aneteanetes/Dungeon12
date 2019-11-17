using Dungeon;
using Dungeon.GameObjects;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon12.CardGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon12.CardGame.Engine
{
    public class CardGame : GameComponent
    {
        private CardGameSettings _cardGameSettings;
        private SafeQueue<Card> areaDeck;

        public Action<CardGamePlayer> OnWin { get; set; }

        public CardGame(CardGameSettings cardGameSettings)
        {
            _cardGameSettings = cardGameSettings;
            areaDeck = Card.Load(x => x.CardType == Interfaces.CardType.Region).Shuffle().AsQueue();
        }

        public CardGamePlayer Player1 { get; private set; }
        public CardGamePlayer ActivePlayer { get; private set; }
        public CardGamePlayer Player2 { get; private set; }
        private CardGamePlayer Winner;

        public (CardGamePlayer Player1, CardGamePlayer Player2) Start(Deck player1deck,string name1, Deck player2deck, string name2)
        {
            Player1 = new CardGamePlayer()
            {
                Hits = this._cardGameSettings.Hits,
                MaxResources = this._cardGameSettings.Resources,
                Deck = player1deck,
                Name=name1
            };

            Player1.Cards = PrepeareDeck(Player1.Deck.Cards);

            Player2 = new CardGamePlayer()
            {
                Hits = this._cardGameSettings.Hits,
                MaxResources = this._cardGameSettings.Resources,
                Deck = player2deck,
                Name=name2
            };
            Player2.Cards = PrepeareDeck(Player2.Deck.Cards);

            return (Player1, Player2);
        }

        private SafeQueue<Card> PrepeareDeck(IEnumerable<Card> deckCards)
        {
            var newDeckCards = new List<Card>(deckCards);
            var diff = _cardGameSettings.CardsInDeck - newDeckCards.Count;
            if (diff != 0)
            {
                if (diff > 0)
                {
                    newDeckCards.AddRange(newDeckCards.Shuffle().Take(diff));
                }
                else
                {
                    newDeckCards.RemoveRange(0, Math.Abs(diff));
                }
            }

            return newDeckCards.Shuffle().AsQueue();
        }

        public AreaCard CurrentArea { get; private set; }

        public bool AreaEnded = false;
        private List<(Card, CardGamePlayer)> OnTurnCards = new List<(Card, CardGamePlayer)>();

        public bool Turn()
        {
            Winner = CheckWinner();
            if (Winner != null)
            {
                OnWin?.Invoke(Winner);
            }

            bool nextRound = CurrentArea == default || CurrentArea.Rounds == 0;
            if (nextRound)
            {
                Message("Смена территории");
                OnTurnCards.Clear();
                CurrentArea = areaDeck.Dequeue().As<AreaCard>();
                if (CurrentArea == default)
                {
                    AreaEnded = true;
                    OnafterTurn?.Invoke();
                    return true;
                }

                //if (Player1.Influence > Player2.Influence)
                //{
                //    Message($"{Player1.Name} получает ресурсы");
                //    Player1.MaxResources++;
                //    Player2.MaxResources--;
                //}
                //else if (Player2.Influence > Player1.Influence)
                //{
                //    Message($"{Player2.Name} получает ресурсы");
                //    Player2.MaxResources++;
                //    Player1.MaxResources--;
                //}
            }
            CurrentArea.Rounds--;

            ForEachGuard(Player2, Player1);
            ForEachGuard(Player1, Player2);
            OnTurnCards.ForEach(c =>
            {
                var e = c.Item2 == Player1 ? Player2 : Player1;
                c.Item1.OnTurn(e, c.Item2, CurrentArea);
            });

            OnafterTurn?.Invoke();
            return false;
        }

        private void ForEachGuard(CardGamePlayer enemy, CardGamePlayer player)
        {
            for (int i = 0; i < player.Guards.Count; i++)
            {
                var g = player.Guards.ElementAtOrDefault(i);
                if (g != default)
                {
                    g.OnTurn(enemy, player, CurrentArea);
                }
            }
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

            var player1cardsLost = Player1.Cards.Count == 0 && Player1.HandCards.Count == 0;
            var player2cardsLost = Player2.Cards.Count == 0 && Player2.HandCards.Count == 0;

            if (player1cardsLost || player2cardsLost)
            {
                Message($"У {(player1cardsLost ? Player1.Name : Player2.Name)} кончились карты!");

                if (Player1.Influence > Player2.Influence)
                {
                    return Player1;
                }
                else if (Player2.Influence > Player1.Influence)
                {
                    return Player2;
                }
                else if(Player1.Hits > Player2.Hits)
                {
                    return Player1;
                }
                else if (Player2.Hits > Player1.Hits)
                {
                    return Player2;
                }
                else if (Player1.Guards.Count > Player2.Guards.Count)
                {
                    return Player1;
                }
                else if (Player2.Guards.Count > Player1.Guards.Count)
                {
                    return Player2;
                }
            }

            return default;
        }

        public bool PlayerTurn(CardGamePlayer player)
        {
            player.Resources = player.MaxResources;
            return player.AddInHand();
        }

        public bool PlayCard(Card card, CardGamePlayer player)
        {
            var enemy = player == Player1 ? Player2 : Player1;
            card.OnPublish(enemy, player, CurrentArea);
            switch (card.CardType)
            {
                case Interfaces.CardType.Guardian:
                    {
                        player.Resources--;
                        player.Influence += 5;
                        if (player.Guards.Count < 5)
                        {
                            player.Guards.Add(card.As<GuardCard>());
                        }
                        break;
                    }
                case Interfaces.CardType.Ability:
                    {
                        if (card.TurnTriggerName != default)
                        {
                            OnTurnCards.Add((card, player));
                        }
                        player.Resources = 0;
                        player.Influence += 1;
                        break;
                    }
                default:
                    {
                        player.Influence += 1;
                        player.AddResource();
                    }
                    break;
            }

            player.HandCards.Remove(card);

            return true;
        }

        public void Message(string text)
        {
            MessageBox.Show(text, this.SceneObject.ShowEffects);
        }
    }
}