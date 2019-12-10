using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
using Dungeon12.CardGame.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Dungeon;
using Dungeon12.Drawing.SceneObjects.Map;

namespace Dungeon12.CardGame.SceneObjects
{
    public class CardPlayerSceneObject : Dungeon12.SceneObjects.HandleSceneControl<CardGamePlayer>
    {
        private TextControl influence;
        private TextControl hits;

        public override bool AbsolutePosition => true;
        public override bool CacheAvailable => false;

        private bool _reverse;

        public CardPlayerSceneObject(CardGamePlayer component, bool reverse=false, bool bindView = true) : base(component, bindView)
        {
            _reverse = reverse;
            this.Height = 22.5;
            this.Width = 7.5;

            void AddBox(string box,Func<int> source, double left=0,string tooltip="")
            {
                this.AddChild(new EmptyTooltipedSceneObject(tooltip)
                {
                    Image= $"Cards/UI/{box}.png".AsmImgRes(),
                    AbsolutePosition = true,
                    CacheAvailable = false,
                    Left = left
                });

                var num = new CardNum(source, 1.8, 1.8, .9)
                {
                    Top = .75,
                    Left = left - .2,
                };
                num.SlideNeed = () =>
                {
                    var needSlide = num.Value < 100;
                    if (needSlide)
                    {
                        num.SlideOffsetLeft = num.Value.ToString().Length == 2 ? .5 : 1;
                    }
                    return needSlide;
                };
                this.AddChild(num);
            }

            AddBox("hits", () => component.Hits, reverse ? 3.5 : 0,"Очки жизней");
            AddBox("influence", () => component.Influence, reverse ? 0 : 3.5,"Влияние");

            for (int i = 0; i < 5; i++)
            {
                this.AddChild(new ResBox(component, i)
                {
                    AbsolutePosition = true,
                    CacheAvailable = false,
                    Top = topRes,
                    Left = _reverse ? 3.4 : 0
                });

                topRes += 3;
            }
        }
        private double topRes = 4;

        private class ResBox : EmptyTooltipedSceneObject
        {
            public override bool AbsolutePosition => true;

            public override bool CacheAvailable => false;

            private int _res;
            private CardGamePlayer _component;
            public ResBox(CardGamePlayer component, int res) : base("Ресурс")
            {
                Image = "Cards/Guardian/ressquare.png".AsmImgRes();
                _res = res;
                _component = component;
            }

            public override string Image => $"Cards/Guardian/ressquare{(_component.Resources > _res ? "" : "0")}.png".AsmImgRes();                
        }
    }
}
