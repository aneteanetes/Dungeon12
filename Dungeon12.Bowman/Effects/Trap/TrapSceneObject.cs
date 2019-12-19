using Dungeon12.Drawing.SceneObjects.Map;

namespace Dungeon12.Bowman.Effects.Trap
{
    public class TrapSceneObject : TooltipedSceneObject<TrapObject>
    {
        public TrapSceneObject(TrapObject component) : base(component, "Ловушка", true)
        {
            this.Width = 1;
            this.Height = 1;

            this.Image = "trapimage";

            this.Left = component.Location.X;
            this.Top = component.Location.Y;
        }
    }
}
