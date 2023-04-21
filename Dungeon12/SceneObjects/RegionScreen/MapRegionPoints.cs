using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing.Impl;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities.Map;
using Dungeon12.SceneObjects.Location_;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.SceneObjects.RegionScreen
{
    internal class MapRegionPoints : SceneControl<MapRegion>
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

        public MapRegionPoints(MapRegion component) : base(component)
        {
            this.Width = Global.Resolution.Width;
            this.Height = Global.Resolution.Height;

            foreach (var point in component.Points)
            {
                this.AddChild(new MapRegionPoint(point,component));
            }
        }

        private class MapRegionPoint : SceneControl<MapPoint>, ITooltipedDrawText
        {
            private MapRegion _region;
            private ImageObject _pointer;
            private EmptySceneObject _illumination;

            public MapRegionPoint(MapPoint component, MapRegion region) : base(component)
            {
                _region = region;
                _pointer = this.AddChild(new ImageObject("UI/layout/partypointer.png")
                {
                    Left = -8,
                    Top = -43,
                    VisibleFunction = () => Global.Game.State.PointId == component.Id
                });

                _illumination = this.AddChild(new EmptySceneObject()
                {
                    Width = 25,
                    Height = 25,
                    Left = 13,
                    Top = 13,
                    ParticleEffects = new List<IEffectParticle>()
                    {
                        new ParticleEffect()
                        {
                            Name="PointBlind",
                            Scale = .7,
                        }
                    },
                    VisibleFunction = () => component.Illuminate
                });

                this.Width = 25;
                this.Height = 25;
                this.Left = component.X;
                this.Top = component.Y;

                string frac = "";

                switch (component.Fraction)
                {
                    case Entities.Enums.Fraction.Vanguard: frac = "_v"; break;
                    case Entities.Enums.Fraction.MageGuild: frac = "_m"; break;
                    case Entities.Enums.Fraction.Mercenary: frac = "_r"; break;
                    case Entities.Enums.Fraction.Exarch: frac = "_e"; break;
                    case Entities.Enums.Fraction.Cult: frac = "_c"; break;
                    //case Entities.Enums.Fraction.Friendly: frac = "_p"; break;
                    //case Entities.Enums.Fraction.Neutral:
                    default:
                        break;
                }

                Image = $"UI/layout/point{frac}.png".AsmImg();

                TooltipText = component.Name.AsDrawText().Gabriela();
            }

            public IDrawText TooltipText { get; set; }

            public bool ShowTooltip => true;

            public override void Focus()
            {
                _region.Points.ForEach(p => p.Illuminate = false);

                var shortestPath = _region.Graph.ShortestPathFunction(Global.Game.State.PointId);

                _region.Points
                    .Join(shortestPath(Component.Id), x => x.Id, i => i, (x, i) => x)
                    .ForEach(p => p.Illuminate = true);

                _region.PointMap[Global.Game.State.PointId].Illuminate = false;

                base.Focus();
            }

            public override void Unfocus()
            {
                _region.Points.ForEach(p => p.Illuminate = false);

                base.Unfocus();
            }

            public override void Click(PointerArgs args)
            {
                var shortestPath = _region.Graph.ShortestPathFunction(Global.Game.State.PointId);

                Global.Game.State.PointId = Component.Id;

                this.Layer.AddObject(new LocationWindow(Component));

                base.Click(args);
            }
        }
    }
}
