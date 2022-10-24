using Dungeon.Resources;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System.Reflection;

namespace Dungeon.Monogame.Resolvers
{
    public class EmbeddedContentResolver : ContentResolver
    {
        private Assembly Assembly;

        public EmbeddedContentResolver()
        {
            Assembly = Assembly.GetExecutingAssembly();
        }

        public override Stream Resolve(string contentPath)
        {
            MemoryStream ms = new MemoryStream();
            var res = $"Dungeon.Monogame.Resources.{contentPath.Embedded()}.xnb";
            using (Stream stream = Assembly.GetManifestResourceStream(res))
            {
                if (stream.CanSeek)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                }
                stream.CopyTo(ms);
            }

            if (ms.CanSeek)
            {
                ms.Seek(0, SeekOrigin.Begin);
            }

            return ms;
        }
    }
}
