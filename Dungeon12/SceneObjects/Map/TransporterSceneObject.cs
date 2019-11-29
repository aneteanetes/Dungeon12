using Dungeon.Control.Pointer;
using Dungeon.Drawing.SceneObjects.Map;
using Dungeon12.Map;

namespace Dungeon12.SceneObjects.Map
{
    public class TransporterSceneObject : ClickActionSceneObject<Transporter>
    {
        public TransporterSceneObject(PlayerSceneObject playerSceneObject, Transporter @object) : base(playerSceneObject, @object, @object.Name)
        {
            this.Image = @object.Image;
            this.AutoSizeImage();
        }

        protected override void Action(MouseButton mouseButton) => this.Component.Interact(playerSceneObject.Component);
    }
}