using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;
using Dungeon12.ECS.Components;
using Dungeon12.Entities;
using Dungeon12.Entities.Map;

namespace Dungeon12.SceneObjects.MUD.Locations
{
    internal class PolygonView : SceneControl<Polygon>, ITooltiped, ITooltipedPositionByComponent
    {
        public override bool PerPixelCollision => true;

        private ImageObject focus = null;
        private ImageObject selector = null;

        public PolygonView(Polygon component, double left, double top, double width, double height) : base(component)
        {
            Left= left;
            Top= top;
            Width= width;
            Height= height;

            focus = this.AddChild(new ImageObject("Tiles/empty_invert.png")
            {
                Width = width,
                Height = height,
            });
            focus.Visible = false;

            selector = this.AddChild(new ImageObject("Tiles/empty_invert.png")
            {
                Width = width,
                Height = height,
                VisibleFunction = () => component.Object?.IsSelected ?? false
            });
            selector.Visible = false;
        }

        public override string Image
        {
            get => Component.Object?.GameObject?.Chip ?? $"Tiles/{Component.Icon}.png".AsmImg();
        }

        public string TooltipText => Component.Object?.Name ?? Component.Object?.GameObject?.Name;

        public override void Click(PointerArgs args)
        {
            Component.Object?.Select();
            base.Click(args);
        }

        public override void Focus()
        {
            if (Component.Object==null)
                return;

            focus.Visible=true;
            base.Focus();
        }

        public override void Unfocus()
        {
            if (Component.Object==null)
                return;

            focus.Visible=false;
            base.Unfocus();
        }
    }
}
