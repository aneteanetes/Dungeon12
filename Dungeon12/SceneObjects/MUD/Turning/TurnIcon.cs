using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon12.ECS.Components;
using Dungeon12.Entities.Turning;

namespace Dungeon12.SceneObjects.MUD.Turning
{
    internal class TurnIcon : SceneControl<Turn>, ITooltiped
    {
        public TurnIcon(Turn component) : base(component)
        {
            this.Width=33;
            this.Height=50;

            this.Image = component.Object.Image;

            var border = this.AddChild(new ImageObject(() => $"UI/smallsquare{(component.IsActive ? "_a" : "")}.png".AsmImg()) { Width=33, Height=50 });
        }

        public string TooltipText => Component.Object.Name;

        public override void Focus()
        {
            base.Focus();
        }
    }
}
