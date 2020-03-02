using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Linq;
using Dungeon.Scenes.Manager;
using LiteDB;

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

        public static bool Exists(string resource) => LoadResource(resource) != default;

        private static Lazy<LiteDatabase> LiteDatabase;

        static ResourceLoader()
        {
            LiteDatabase = new Lazy<LiteDatabase>(() =>
            {
                var litedb = new LiteDatabase(ResourceCompiler.CompilePath);
                DungeonGlobal.Exit += () => litedb.Dispose();

                return litedb;
            });
        }

        private static Resource LoadResource(string resource)
        {
            if (RuntimeCache.ContainsKey(resource))
            {
                return new Resource()
                {
                    Path = resource,
                    Data = RuntimeCache[resource]
                };
            }

            var db = LiteDatabase.Value.GetCollection<Resource>();

            var res = db.Find(x => x.Path == resource).FirstOrDefault();

            if (res != default)
            {
                var bytes = new byte[res.Data.Length];

                var mem = new Memory<byte>(bytes);
                res.Data.CopyTo(mem);

                RuntimeCache.Add(resource, mem.Span.ToArray());
            }

            return res;
        }

        public static Resource Load(string resource, bool caching = false)
        {
            var res = LoadResource(resource);

            if (res == default)
            {
                throw new KeyNotFoundException($"Ресурс {resource} не найден!");
            }

            bool addToScene = !caching;
            if (NotDisposingResources)
            {
                addToScene = !SceneManager.Preapering?.Resources.Any(r => r.Path == res.Path) ?? false;
            }

            if (addToScene)
            {
                SceneManager.Preapering?.Resources.Add(res);
            }

            return res;
        }

        private static Dictionary<string, byte[]> RuntimeCache = new Dictionary<string, byte[]>();
        public static void SaveStream(byte[] bytes, string image)
        {
            RuntimeCache[image] = bytes;
        }

        public static Type LoadType(string className)
        {
            if (string.IsNullOrWhiteSpace(className))
                return default;

            var type = TryGetFromAssembly(className, DungeonGlobal.GameAssembly);
            if (type == default)
            {
                foreach (var asm in DungeonGlobal.Assemblies)
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