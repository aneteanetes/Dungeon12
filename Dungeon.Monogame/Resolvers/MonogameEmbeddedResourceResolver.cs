﻿using Dungeon.Resources;
using Dungeon.Resources.Resolvers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Dungeon.Monogame.Resolvers
{
    internal class MonogameEmbeddedResourceResolver : ResourceResolver
    {
        private Assembly Assembly;

        public MonogameEmbeddedResourceResolver()
        {
            Assembly = Assembly.GetExecutingAssembly();
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

            return new Resource()
            {
                Path = contentPath,
                Data = ms.ToArray()
            };
        }
    }
}
