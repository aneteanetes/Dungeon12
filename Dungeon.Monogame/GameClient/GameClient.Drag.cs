namespace Dungeon.Monogame
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Dungeon.View.Interfaces;
    using Dungeon.Resources;

    public partial class GameClient : Game, IGameClient
    {
        private bool dragging = false;

        public void Drag(ISceneObject @object, ISceneObject area = null)
        {
            dragging = true;
            var texture = ImageLoader.LoadTexture2D(Scene.Resources, @object.Image);
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

        private string cursor = null;

        public void SetCursor(ResourceTable resources, string textureSrc)
        {
            cursor = textureSrc;
            var texture = ImageLoader.LoadTexture2D(resources, textureSrc);
            if (texture == default)
                return;
            CurrentCursor = MouseCursor.FromTexture2D(texture, 0, 0);
            if (!dragging)
            {
                Mouse.SetCursor(CurrentCursor);
            }
        }

        public string GetCursor() => cursor;
    }
}