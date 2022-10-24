using Dungeon.Resources;
using Dungeon.View.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Dungeon.Monogame
{
    public class ImageLoader
    {
        internal static readonly Dictionary<string, Texture2D> tilesetsCache = new Dictionary<string, Texture2D>();
        private GraphicsDevice _graphicsDevice;

        public ImageLoader(GraphicsDevice graphicsDevice) => _graphicsDevice= graphicsDevice;

        public Texture2D LoadTexture2D(string imageFullPath, ISceneObject sceneObject = default)
        {
            if (!tilesetsCache.TryGetValue(imageFullPath, out var bitmap))
            {
                var res = ResourceLoader.Load(imageFullPath, obj: sceneObject);
                if (res == default)
                    return default;

                bitmap = Texture2D.FromStream(_graphicsDevice, res.Stream);

                tilesetsCache.TryAdd(imageFullPath, bitmap);

                res.OnDispose += () =>
                {
                    tilesetsCache.Remove(imageFullPath);
                    bitmap.Dispose();
                };
            }

            return bitmap;
        }
    }
}
