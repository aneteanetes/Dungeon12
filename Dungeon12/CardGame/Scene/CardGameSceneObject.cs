using Dungeon;
using Dungeon.SceneObjects;
using Dungeon12.CardGame.Engine;
using Dungeon12.CardGame.SceneObjects;

namespace Dungeon12.CardGame.Scene
{
    public class CardGameSceneObject : HandleSceneControl<Engine.CardGame>
    {
        public CardGameSceneObject(Engine.CardGame component, Deck enemyDeck, Deck playerDeck) : base(component, true)
        {
            this.Width = 40;
            this.Height = 22.5;

            this.Image = "ui/horizontal(40x225).png".AsmImgRes();

            var (Player1, Player2) = component.Start(playerDeck, enemyDeck);

            var pLeft = new CardPlayerSceneObject(Player1)
            {
                Left = 2
            };
            var pRight = new CardPlayerSceneObject(Player2);
            pRight.Left = 40 - pRight.Width;

            this.AddChild(pLeft);
            this.AddChild(pRight);

            component.Turn();
            component.PlayerTurn(Player1);

            this.AddChild(new CardInHands(Player1)
            {
                Left = 7.5,
                Top = 14,
                AbsolutePosition = true,
                CacheAvailable = false,
                AfterHandPlayed = EnemyTurn
            });
        }

        private void EnemyTurn()
        {
            Component.PlayerTurn(Component.Player2);
            Component.Turn();
        }
    }
}