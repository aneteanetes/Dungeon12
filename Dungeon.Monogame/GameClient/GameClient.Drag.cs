namespace Dungeon.Monogame
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Dungeon.View.Interfaces;

    public partial class GameClient : Game, IGameClient
    {
        private bool dragging = false;

        public void Drag(ISceneObject @object, ISceneObject area = null)
        {
            dragging = true;
            var texture = ImageLoader.LoadTexture2D(@object.Image);
            if (texture == default)
                return;

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
            var texture = ImageLoader.LoadTexture2D(textureSrc);
            if (texture == default)
                return;
            CurrentCursor = MouseCursor.FromTexture2D(texture, 0, 0);
            if (!dragging)
            {
                Mouse.SetCursor(CurrentCursor);
            }
        }
    }
}