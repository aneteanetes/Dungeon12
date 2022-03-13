using Dungeon;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities.Map;

namespace Dungeon12.SceneObjects.RegionScreen
{
    public class MapRegionPoints : SceneControl<MapRegion>
    {
        public MapRegionPoints(MapRegion component) : base(component)
        {
            this.Width = Global.Resolution.Width;
            this.Height = Global.Resolution.Height;

            foreach (var point in component.Points)
            {
                this.AddChild(new MapRegionPoint(point));
            }
        }

        private class MapRegionPoint : SceneControl<MapPoint>, ITooltiped
        {
            public MapRegionPoint(MapPoint component) : base(component)
            {
                this.Width = 25;
                this.Height = 25;
                this.Left = component.X;
                this.Top = component.Y;

                string frac = "";

                switch (component.Fraction)
                {
                    case Entities.Enums.Fraction.Friendly: frac = "_p"; break;
                    case Entities.Enums.Fraction.Vanguard: frac = "_v"; break;
                    case Entities.Enums.Fraction.MageGuild: frac = "_m"; break;
                    case Entities.Enums.Fraction.Mercenary: frac = "_r"; break;
                    case Entities.Enums.Fraction.Exarch: frac = "_e"; break;
                    case Entities.Enums.Fraction.Cult: frac = "_c"; break;
                    case Entities.Enums.Fraction.Neutral:
                    default:
                        break;
                }

                Image = $"UI/layout/point{frac}.png".AsmImg();

                TooltipText = component.Name.AsDrawText().Gabriela();
            }

            public IDrawText TooltipText { get; set; }

            public bool ShowTooltip => true;
        }
    }
}
