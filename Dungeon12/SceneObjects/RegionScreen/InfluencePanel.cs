﻿using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Nabunassar.ECS.Components;
using Nabunassar.Entities;
using Nabunassar.Entities.Enums;
using Nabunassar.Entities.Map;
using System.Linq;

namespace Nabunassar.SceneObjects.RegionScreen
{
    internal class InfluencePanel : SceneControl<MapRegion>
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

        public InfluencePanel(MapRegion component) : base(component)
        {
            this.Width = 195;
            this.Height = 220;

            //this.AddChild(new InfluencePlate(component, Fraction.Neutral, true)
            //{
            //    Left = 150
            //});

            //this.AddChild(new InfluencePlate(component, Fraction.Friendly, true)
            //{
            //    Left = 50
            //});

            var top = 65;

            foreach (var frac in new Fraction[] { Fraction.Vanguard, Fraction.MageCircle, Fraction.ShadowGuild, Fraction.Exarchate, Fraction.DeathCult })
            {
                this.AddChild(new InfluencePlate(component, frac, false)
                {
                    Left = 165,
                    Top = top
                });

                top += 42;
            }
        }

        private class InfluencePlate : SceneControl<MapRegion>, ITooltipedDrawText
        {
            Fraction _fraction;

            public InfluencePlate(MapRegion region, Fraction fraction, bool isBig) :base(region)
            {
                _fraction = fraction;

                this.Width = isBig ? 45 : 30;
                this.Height = isBig ? 45 : 30;

                var postfix = "_p";

                switch (fraction)
                {
                    //case Fraction.Neutral: postfix = "_n"; break;
                    case Fraction.Vanguard: postfix = "_v"; break;
                    case Fraction.MageCircle: postfix = "_m"; break;
                    case Fraction.ShadowGuild: postfix = "_r"; break;
                    case Fraction.Exarchate: postfix = "_e"; break;
                    case Fraction.DeathCult: postfix = "_c"; break;
                    //case Fraction.Friendly:
                    default:
                        break;
                }

                this.Image = $"UI/layout/influence{postfix}.png".AsmImg();

                TooltipText=Global.Strings[fraction.ToString()]
                    .AsDrawText()
                    .Gabriela();

                var txt = this.AddTextCenter(FractionCounter.AsDrawText().Gabriela().InColor(Global.CommonColor).InSize(isBig ? 26 : 14));
                txt.Left -= isBig ? 50 : 32;
            }

            public override bool Visible => Component.Points.Any(x => x.Fraction == _fraction);

            private string FractionCounter => Component.Points.Where(p => p.Fraction == _fraction).Count().ToString();

            public IDrawText TooltipText { get; set; }

            public bool ShowTooltip => true;
        }
    }
}