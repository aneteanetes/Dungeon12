using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
using Dungeon12.CardGame.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.CardGame.SceneObjects
{
    public class CardInHands : Dungeon12.SceneObjects.HandleSceneControl<CardGamePlayer>
    {
        public CardInHands(CardGamePlayer component, Action enemyTurn) : base(component, false)
        {
            this.Width = 40;
            this.Height = 5;

            this.AddChild(new CardSkipSceneObject()
            {
                Skip = enemyTurn,
                Left =-3
            });

            Redraw();
            component.HandChanged += Redraw;
        }

        public void Redraw()
        {
            this.RemoveChild<CardSceneObject>();
            var left = 0d;
            Component.HandCards.ForEach(hc =>
            {
                this.AddChild(new CardSceneObject(hc,this.Component)
                {
                    Left = left,
                    AbsolutePosition = true,
                    CacheAvailable = false
                });
                left += 5;
            });
        }
    }
}
