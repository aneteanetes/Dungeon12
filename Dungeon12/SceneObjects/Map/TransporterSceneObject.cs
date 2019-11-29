using Dungeon.Control.Pointer;
using Dungeon.Drawing.SceneObjects.Map;
using Dungeon12.Map;

namespace Dungeon12.SceneObjects.Map
{
    public class TransporterSceneObject : ClickActionSceneObject<Transporter>
    {
        public override string Cursor => "home";

        public TransporterSceneObject(PlayerSceneObject playerSceneObject, Transporter @object) : base(playerSceneObject, @object, @object.Name)
        {
            this.Image = @object.Image;
            this.AutoSizeImage();
            this.Left = @object.Location.X;
            this.Top = @object.Location.Y;
        }

        protected override void Action(MouseButton mouseButton) => this.Component.Interact(playerSceneObject.Component);
    }
}