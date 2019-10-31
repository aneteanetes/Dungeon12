namespace Dungeon.Monogame
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Dungeon.View.Interfaces;

    public partial class XNADrawClient : Game, IDrawClient
    {
        private bool dragging = false;

        public void Drag(ISceneObject @object, ISceneObject area = null)
        {
            dragging = true;
            var texture = TileSetByName(@object.Image);
            if (texture == default)
                return;
#if Core
            var cur = MouseCursor.FromTexture2D(texture, 0, 0);
            Mouse.SetCursor(cur);
#endif
        }

        public void Drop()
        {
            dragging = false;
            #if Core
            Mouse.SetCursor(CurrentCursor);
#endif
        }
        #if Core
        private MouseCursor CurrentCursor = null;
#endif
        public void SetCursor(string textureSrc)
        {
            var texture = TileSetByName(textureSrc);
            if (texture == default)
                return;
            #if Core
            CurrentCursor = MouseCursor.FromTexture2D(texture, 0, 0);
            if (!dragging)
            {
                Mouse.SetCursor(CurrentCursor);
            }
#endif
        }
    }
}