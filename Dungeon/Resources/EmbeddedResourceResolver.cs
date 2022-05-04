using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Dungeon.Resources
{
    public class EmbeddedResourceResolver : ResourceResolver
    {
        private Assembly Assembly;

        public EmbeddedResourceResolver(Assembly assembly)
        {
            Assembly = assembly;
        }

        /// <summary>
        /// Получить ресурс из embedded
        /// <para>
        /// [Кэшируемый]
        /// </para>
        /// </summary>
        public override Resource Resolve(string contentPath)
        {
            if (!___ResolveCache.TryGetValue(contentPath, out var value))
            {
                value = InternalResolve(contentPath);
                ___ResolveCache.Add(contentPath, value);
            }

            return value;
        }
        private readonly Dictionary<string, Resource> ___ResolveCache = new Dictionary<string, Resource>();

        internal Resource InternalResolve(string contentPath)
        {
            if (contentPath.Contains(' '))
            {
                var split = contentPath.Split(".");
                var filename = split[^2]+"."+split[^1];
                var originalPath = contentPath.Replace(filename, "").Replace(" ", "_");
                contentPath = $"{originalPath}{filename}";
            }

            MemoryStream ms = new MemoryStream();
            using (Stream stream = Assembly.GetManifestResourceStream(contentPath))
            {
                if (stream == default)
                    return default;

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

            var res = new Resource()
            {
                Path = contentPath,
                Data = ms.ToArray()
            };
            res.OnDispose += () =>
            {
                ___ResolveCache.Remove(contentPath);
            };

            return res;
        }
    }
}
