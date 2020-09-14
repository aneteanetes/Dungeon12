using Dungeon.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Dungeon.Engine.Engine
{
    internal class PhysicalFileResourceResolver : ResourceResolver
    {
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
            if(!File.Exists(contentPath))
            {
                return default;
            }

            return new Resource()
            {
                Path = contentPath,
                Data = File.ReadAllBytes(contentPath)
            };
        }
    }
}
