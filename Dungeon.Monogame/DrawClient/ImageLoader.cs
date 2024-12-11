using Dungeon.Resources;
using Dungeon.Resources.Processing;
using Dungeon.View.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dungeon.Monogame
{
    public class ImageLoader : ResourceProcessor
    {
        internal static readonly Dictionary<string, Texture2D> tilesetsCache = new Dictionary<string, Texture2D>();
        private GraphicsDevice _graphicsDevice;

        public ImageLoader(GraphicsDevice graphicsDevice) => _graphicsDevice= graphicsDevice;

        private string[] imageFormats = { ".jpg", ".png",".tga" };

        public override bool IsCanProcess(string path) => imageFormats.Contains(Path.GetExtension(path));

        public Texture2D LoadTexture2D(ResourceTable resource, string imageFullPath, ISceneObject sceneObject = default)
        {
            if (!tilesetsCache.TryGetValue(imageFullPath, out var bitmap))
            {
                Resource res = null;

                if (res == null)
                {
                    res = resource.Get(imageFullPath);
                }

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

        public override void Process(string imageFullPath, Resource resource)
        {
            if (!tilesetsCache.TryGetValue(imageFullPath, out var bitmap))
            {
                bitmap = Texture2D.FromStream(_graphicsDevice, resource.Stream);

                tilesetsCache.TryAdd(imageFullPath, bitmap);

                resource.OnDispose += () =>
                {
                    tilesetsCache.Remove(imageFullPath);
                    bitmap.Dispose();
                };
            }
        }
    }
}
