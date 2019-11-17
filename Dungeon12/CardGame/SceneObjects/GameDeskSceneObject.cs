using Dungeon;
using Dungeon.Drawing.SceneObjects.UI;
using Dungeon12.CardGame.Entities;
using System;
using System.Linq;

namespace Dungeon12.CardGame.SceneObjects
{
    public class GameDeskSceneObject : DropableControl<CardSceneObject>
    {
        private Engine.CardGame _cardGame;

        public GameDeskSceneObject(Engine.CardGame cardGame)
        {
            _cardGame = cardGame;

            this.Width = 35;
            this.Height = 3.5 * 1.5;

            double left = 0;
            for (int i = 0; i < 8; i++)
            {
                left += 4;
            }
        }

        private long playerCards = 0;

        private void AddCard(Card card, CardGamePlayer cardGamePlayer)
        {
            var cardObj = this.AddChildCenter(new CardSceneObject(card, cardGamePlayer), false, false)
                .Minimize();

            card.OnDieEvent += () =>
            {
                playerCards--;
                cardObj.Destroy?.Invoke();
                this.GetChildren<CardSceneObject>().Where(x => x.Player == cardGamePlayer).ForEach(c =>
                {
                    c.Left += 4;
                });
            };

            cardObj.Left = 12 - (playerCards * 4);
            playerCards++;
        }

        private long enemyCards = 0;
        public void AddCardEnemy(Card card, CardGamePlayer cardGamePlayer)
        {
            var cardObj = this.AddChildCenter(new CardSceneObject(card, cardGamePlayer), false, false)
                .Minimize();

            card.OnDieEvent += () =>
            {
                enemyCards--;
                cardObj.Destroy?.Invoke();
                this.GetChildren<CardSceneObject>().Where(x => x.Player == cardGamePlayer).ForEach(c =>
                {
                    c.Left -= 4;
                });
            };

            cardObj.Left = 16 + (enemyCards * 4);
            enemyCards++;
        }

        public Action<GameDeskSceneObject> AfterHandPlayed { get; set; }

        protected override void OnDrop(CardSceneObject source)
        {
            var card = source.Card;
            var wasGuards = _cardGame.Player1.Guards.Count;
            _cardGame.PlayCard(card, _cardGame.Player1);
            var nowGuards = _cardGame.Player1.Guards.Count;

            switch (card.CardType)
            {
                case Interfaces.CardType.Guardian:
                    if (wasGuards < nowGuards)
                    {
                        AddCard(card, source.Player);
                    }
                    break;
                default:
                    break;
            }

            source.Destroy?.Invoke();

            AfterHandPlayed?.Invoke(this);
        }
    }
}
