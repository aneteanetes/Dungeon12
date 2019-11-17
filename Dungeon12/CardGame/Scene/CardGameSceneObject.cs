using Dungeon;
using Dungeon.Drawing.SceneObjects.UI;
using Dungeon.SceneObjects;
using Dungeon12.CardGame.Engine;
using Dungeon12.CardGame.SceneObjects;
using System;

namespace Dungeon12.CardGame.Scene
{
    public class CardGameSceneObject : HandleSceneControl<Engine.CardGame>
    {
        public CardGameSceneObject(Engine.CardGame component, Deck enemyDeck, Deck playerDeck, CardDropMask cardDropMask) : base(component, true)
        {
            this.Width = 40;
            this.Height = 22.5;

            this.Image = "ui/horizontal(40x225).png".AsmImgRes();

            var (Player1, Player2) = component.Start(playerDeck, enemyDeck);

            var pLeft = new CardPlayerSceneObject(Player1)
            {
                Left = 1,
                Top = 1
            };
            var pRight = new CardPlayerSceneObject(Player2, true);
            pRight.Top = 1;
            pRight.Left = 40 - pRight.Width;

            this.AddChild(pLeft);
            this.AddChild(pRight);

            this.AddChild(new LandCardSceneObject(component)
            {
                Left = 16,
                Top = 1
            });

            component.Turn();
            component.PlayerTurn(Player1);

            var gameDesk = this.AddChildCenter(new GameDeskSceneObject(component));
            gameDesk.Left += 1.7;
            gameDesk.Top -= 1;
            gameDesk.AfterHandPlayed = EnemyTurn;

            var cardInHands = new CardInHands(Player1, () => EnemyTurn(gameDesk))
            {
                Left = 7.5,
                Top = 14,
                AbsolutePosition = true,
                CacheAvailable = false
            };
            this.AddChild(cardInHands);

            cardDropMask.OnDropInMask += cardInHands.Redraw;
        }

        private void EnemyTurn(GameDeskSceneObject gameDeskSceneObject)
        {
            Component.PlayerTurn(Component.Player2);

            var card = Component.Player2.Auto(Component);
            if (card != default)
            {
                var wasGuards = Component.Player2.Guards.Count;
                Component.PlayCard(card, Component.Player2);
                var nowGuards = Component.Player2.Guards.Count;

                switch (card.CardType)
                {
                    case Interfaces.CardType.Guardian:
                        if (wasGuards < nowGuards)
                        {
                            gameDeskSceneObject.AddCardEnemy(card, Component.Player2);
                        }
                        break;
                    default:
                        break;
                }
            }

            Component.Turn();
            Component.PlayerTurn(Component.Player1);
        }
    }
}