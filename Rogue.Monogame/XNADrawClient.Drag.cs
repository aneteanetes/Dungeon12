namespace Rogue
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Rogue.View.Interfaces;

    public partial class XNADrawClient : Game, IDrawClient
    {
        public void Drag(ISceneObject @object, ISceneObject area = null)
        {
            var texture = TileSetByName(@object.Image);            
            var cur = MouseCursor.FromTexture2D(texture, 0, 0);
            Mouse.SetCursor(cur);
        }

        public void Drop()
        {
            Mouse.SetCursor(MouseCursor.Arrow);
        }
    }
}