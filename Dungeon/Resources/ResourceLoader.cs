using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Linq;
using Dungeon.Scenes.Manager;

namespace Dungeon.Resources
{
    public static class ResourceLoader
    {
        /// <summary>
        /// Флаг позволяющий не освобождать ресуры
        /// полезно для дебага
        /// </summary>
        public static bool NotDisposingResources = false;

        /// <summary>
        /// Флаг кэширования изображений и их масок
        /// </summary>
        public static bool CacheImagesAndMasks = true;

        public static bool Exists(string resource) => LoadStream(resource) != default;

        private static Stream LoadStream(string resource)
        {
            if (RuntimeCache.ContainsKey(resource))
            {
                return RuntimeCache[resource];
            }

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = resource;

            var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == default)
            {
                stream = Global.GameAssembly.GetManifestResourceStream(resourceName);
            }

            if (stream == default)
            {
                foreach (var asm in Global.Assemblies)
                {
                    stream = asm.GetManifestResourceStream(resourceName);
                    if (stream != default)
                    {
                        break;
                    }
                }
            }

            return stream;
        }

        public static Resource Load(string resource, bool caching = false)
        {
            var stream = LoadStream(resource);

            if (stream == default)
            {
                throw new KeyNotFoundException($"Ресурс {resource} не найден!");
            }

            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            var res = new Resource()
            {
                Name = resource,
                Stream = stream,
                Dispose = () => stream?.Dispose()
            };

            bool addToScene = !caching;
            if (NotDisposingResources)
            {
                addToScene = !SceneManager.Preapering.Resources.Any(r => r.Name == res.Name);
            }

            if (addToScene)
            {
                SceneManager.Preapering.Resources.Add(res);
            }

            return res;
        }

        private static Dictionary<string, Stream> RuntimeCache = new Dictionary<string, Stream>();
        public static void SaveStream(Stream stream, string image)
        {
            RuntimeCache[image] = stream;
        }

        public static Type LoadType(string className)
        {
            if (string.IsNullOrWhiteSpace(className))
                return default;

            var type = TryGetFromAssembly(className, Global.GameAssembly);
            if (type == default)
            {
                foreach (var asm in Global.Assemblies)
                {
                    type = TryGetFromAssembly(className, asm);
                    if (type != default)
                    {
                        break;
                    }
                }
            }

            if (type == default)
            {
                type = Type.GetType(className);
            }

            if (type == default)
            {
                throw new DllNotFoundException($"Тип {className} не найден ни в одной из загруженных сборок!");
            }

            return type;
        }

        private static Type TryGetFromAssembly(string className, Assembly assembly)
        {
            if (assembly == default)
                return default;

            var type = assembly.GetType(className);
            if (type == default)
            {
                type = assembly.GetTypes().FirstOrDefault(x => x.Name == className);
            }

            return type;
        }
    }
}