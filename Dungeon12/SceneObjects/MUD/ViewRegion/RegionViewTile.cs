using Dungeon;
using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities.Map;

namespace Dungeon12.SceneObjects.MUD.ViewRegion
{
    internal class RegionViewTile : SceneControl<Location>, ITooltipedDrawText
    {
        public RegionViewTile(Location component) : base(component)
        {
            this.Width=25;
            this.Height=25;

            var rect = this.AddChild(new DarkRectangle()
            {
                Color = new DrawColor(53,149,83),
                Width=this.Width,
                Height=this.Height,
                Opacity=1
            });

            this.AddChild(new DarkRectangle()
            {
                Color = new DrawColor(ConsoleColor.DarkGray),
                Width=this.Width,
                Height=this.Height,
                Depth=2,
                Opacity=1,
                Fill=false
            });
        }

        public IDrawText TooltipText => Component.Name.AsDrawText();

        public bool ShowTooltip => true;
    }
}
