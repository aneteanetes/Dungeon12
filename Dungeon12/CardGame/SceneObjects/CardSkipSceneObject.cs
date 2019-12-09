using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Dungeon;
using Dungeon.Control;
using Dungeon12.CardGame.Entities;
using Dungeon12.Drawing.SceneObjects.Map;

namespace Dungeon12.CardGame.SceneObjects
{
    public class CardSkipSceneObject : EmptyTooltipedSceneObject
    {
        protected override ControlEventType[] Handles => new ControlEventType[]
        {
            ControlEventType.ClickRelease
        };

        public Action Skip { get; set; }

        public CardSkipSceneObject():base("Пропустить ход")
        {
            this.Image = "Cards/Guardian/skip.png".AsmImgRes();
            this.Width = 4.65625/2;
            this.Height = 7;
        }

        public override void ClickRelease(PointerArgs args)
        {
            Skip?.Invoke();
        }
    }
}
