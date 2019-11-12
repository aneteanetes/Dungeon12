using Dungeon.SceneObjects;
using Dungeon12.CardGame.Engine;
using Dungeon12.Drawing.SceneObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Dungeon;
using Dungeon12.CardGame.SceneObjects;
using System.Linq;

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

            var pLeft = new CardPlayerSceneObject(Player1);
            var pRight = new CardPlayerSceneObject(Player2);
            pRight.Left = 40 - pRight.Width;

            this.AddChild(pLeft);
            this.AddChild(pRight);

            var g = Player1.Deck.Cards.First();
            g.CardType = Interfaces.CardType.Guardian;

            Player1.HandCards.Add(g);
            Player1.HandCards.Add(g);
            Player1.HandCards.Add(g);
            Player1.HandCards.Add(g);
            Player1.HandCards.Add(g);

            this.AddChild(new CardInHands(Player1)
            {
                Top=12,
                AbsolutePosition = true,
                CacheAvailable = false
            });
        }
    }
}