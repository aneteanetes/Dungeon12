using Dungeon.SceneObjects;
using Dungeon12.CardGame.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.CardGame.SceneObjects
{
    public class CardInHands : HandleSceneControl<CardGamePlayer>
    {
        public CardInHands(CardGamePlayer component) : base(component, false)
        {
            this.Width = 40;
            this.Height = 5;
            var left = 0d;

            component.HandCards.ForEach(hc =>
            {
                this.AddChild(new CardSceneObject(hc)
                {
                    Left = left,
                    AbsolutePosition = true,
                    CacheAvailable = false
                });
                left += 5.5;
            });
        }
    }
}
