using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities.Objects;

namespace Dungeon12.SceneObjects.Location_
{
    internal class LocationObject : SceneControl<MapObject>, ITooltipedDrawText
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

        public LocationObject(MapObject component) : base(component)
        {
            this.Width=72;
            this.Height=72;
            this.Image="UI/Windows/Location/cell_b.png";

            //this.AddChild(new ImageObject($"Icons/{component.Icon}.png") { Width=72, Height=72 });
            this.AddChild(new ImageObject($"UI/Windows/Location/cell_a.png"));

            this.Left=65;
            this.Top=206;

            this.Left+=component.X*76;
            this.Top+=component.Y*76;
        }

        public IDrawText TooltipText => Component.Name.Gabriela();

        public bool ShowTooltip => true;
    }
}