namespace Rogue
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Rogue.View.Interfaces;

    public partial class XNADrawClient : Game, IDrawClient
    {
        private bool dragging = false;

        public void Drag(ISceneObject @object, ISceneObject area = null)
        {
            dragging = true;
            var texture = TileSetByName(@object.Image);            
            var cur = MouseCursor.FromTexture2D(texture, 0, 0);
            Mouse.SetCursor(cur);
        }

        public void Drop()
        {
            dragging = false;
            Mouse.SetCursor(CurrentCursor);
        }

        private MouseCursor CurrentCursor = null;

        public void SetCursor(string textureSrc)
        {
            var texture = TileSetByName(textureSrc);
            CurrentCursor = MouseCursor.FromTexture2D(texture, 0, 0);
            if (!dragging)
            {
                Mouse.SetCursor(CurrentCursor);
            }
        }
    }
}