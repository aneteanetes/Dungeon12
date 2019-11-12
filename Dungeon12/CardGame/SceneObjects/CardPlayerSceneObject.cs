using Dungeon;
using Dungeon.SceneObjects;
using Dungeon12.CardGame.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.CardGame.SceneObjects
{
    public class CardPlayerSceneObject : SceneObject<CardGamePlayer>
    {
        private TextControl influence;
        private TextControl hits;

        public override bool AbsolutePosition => true;
        public override bool CacheAvailable => false;

        public CardPlayerSceneObject(CardGamePlayer component, bool bindView = true) : base(component, bindView)
        {
            this.Height = 22.5;
            this.Width = 7.5;

            influence = this.AddTextCenter($" ".AsDrawText().InSize(30).Triforce(), false, false);
            influence.Top = 1;

            var m = MeasureText(influence.Text).Y/32;
            
            hits = this.AddTextCenter($" ".AsDrawText().InSize(30).Triforce(), false, false);
            hits.Top = 1.5 + m;
        }

        public override void Update()
        {
            influence.Text.SetText($"Влияние: {Component.Influence}");
            hits.Text.SetText($"Жизни: {Component.Hits}");
        }
    }
}
