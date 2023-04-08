using Dungeon;
using Dungeon12.ECS.Components;
using Dungeon12.Entities.Map;

namespace Dungeon12.SceneObjects.MUD.Locations
{
    internal class PolygonView : SceneControl<Polygon>, ITooltiped
    {
        public override bool PerPixelCollision => true;

        public PolygonView(Polygon component) : base(component)
        {
            this.Image = $"Tiles/{component.Icon}.png";
        }

        public string TooltipText => $"X:{Component.X} Y:{Component.Y}";
    }
}
