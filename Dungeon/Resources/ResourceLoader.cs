using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Linq;
using Dungeon.Scenes.Manager;
using LiteDB;
using System.Runtime.CompilerServices;
using MoreLinq;
using System.Diagnostics;
using System.Threading.Tasks;

#pragma warning disable CS0618 // Type or member is obsolete

namespace Dungeon.Resources
{
    public static class ResourceLoader
    {
        public static ResourceLoaderSettings Settings { get; set; } = new ResourceLoaderSettings();

        /// <summary>
        /// Флаг позволяющий не освобождать ресуры
        /// полезно для дебага
        /// </summary>
        public static bool NotDisposingResources = false;

        /// <summary>
        /// Флаг кэширования изображений и их масок
        /// </summary>
        public static bool CacheImagesAndMasks = true;

        public static List<ResourceDatabaseResolver> ResourceDatabaseResolvers { get; set; } = new List<ResourceDatabaseResolver>();

        public static List<ResourceResolver> ResourceResolvers { get; set; } = new List<ResourceResolver>();

        public static bool Exists(string resource) => LoadResource(resource) != default;

        private static LiteDatabase liteDatabase;
        private static LiteDatabase LiteDatabase
        {
            get
            {
                if (liteDatabase == default)
                {
                    liteDatabase = new LiteDatabase(ResourceCompiler.CompilePath);
                    DungeonGlobal.Exit += () => liteDatabase.Dispose();
                }
                return liteDatabase;
            }
        }

        private static Resource LoadResource(string resource)
        {
            if (RuntimeCache.ContainsKey(resource))
            {
                return RuntimeCache[resource];
            }

            var db = LiteDatabase.GetCollection<Resource>();

            var res = db.Find(x => x.Path == resource).FirstOrDefault();
            if (res == default)
            {
                foreach (var rdb in ResourceDatabaseResolvers)
                {
                    res = rdb.Resolve()
                        .GetCollection<Resource>()
                        .Find(x => x.Path == resource)
                        .FirstOrDefault();

                    if (res != default)
                        break;
                }
            }

            if (res == default)
            {
                foreach (var rr in ResourceResolvers)
                {
                    res = rr.Resolve(resource);

                    if (res != default)
                        break;
                }
            }

            if (res != default)
            {
                res.OnDispose += () => RuntimeCache.Remove(resource);
                RuntimeCache.Add(resource, res);
            }

            if (res == default)
            {
                RuntimeCache.Add(resource, null);
            }

            return res;
        }

        public static Resource Load(string resource, bool caching = false, bool @throw=true, SceneManager sceneManager=default)
        {
            var res = LoadResource(resource);

            if (res == default)
            {
                Settings.NotFoundAction?.Invoke(resource);
                if ((Settings?.ThrowIfNotFound ?? true) && @throw)
                {
                    throw new KeyNotFoundException($"Ресурс {resource} не найден!");
                }
                else return default;
            }

            bool addToScene = !caching;
            if (NotDisposingResources)
            {
                addToScene = !(sceneManager ?? DungeonGlobal.SceneManager).Preapering?.Resources.Any(r => r.Path == res.Path) ?? false;
            }

            if (addToScene)
            {
                (sceneManager ?? DungeonGlobal.SceneManager).Preapering?.Resources.Add(res);
            }

            return res;
        }

        private static Dictionary<string, Resource> RuntimeCache = new Dictionary<string, Resource>();
        public static void SaveStream(byte[] bytes, string image)
        {
            throw new NotImplementedException("А нефиг оставлять TODOшки");
            //RuntimeCache[image] = bytes;
        }

        private static bool allAssembliesLoaded = false;
        public static void LoadAllAssembliesInFolder()
        {
            if (allAssembliesLoaded)
                return;

            var assemblies = new List<Assembly>();
            var asms = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            foreach (var asm in asms)
            {
                try
                {
                    assemblies.Add(LoadAssemblyFromContext(AssemblyLoadContext.Default,asm));
                }
                catch
                {
                }
            }
            DungeonGlobal.StaticAssemblies = assemblies;
            allAssembliesLoaded = true;
        }

        public static bool LoadAssembly(string asmPath)
        {
            try
            {
                DungeonGlobal.StaticAssemblies.Add(LoadAssemblyFromContext(AssemblyLoadContext.Default,asmPath));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static Assembly LoadAssemblyFromContext(AssemblyLoadContext assemblyLoadContext, string asmPath)
            => assemblyLoadContext.LoadFromAssemblyPath(asmPath);

        public static bool LoadAssemblyUnloadable(string asmPath)
        {
            if (DungeonGlobal.DungeonAssemblyContext == default)
            {
                DungeonGlobal.DungeonAssemblyContext = new Global.DungeonAssemblyContext();
            }

            try
            {
                LoadAssemblyFromContext(DungeonGlobal.DungeonAssemblyContext, asmPath);
                return true;
            }
            catch { }

            return false;
        }

        public static Task<bool> UnloadAssemblies()
        {
            return Task.Run(() =>
            {
                try
                {
                    var ctx = DungeonGlobal.DungeonAssemblyContext;
                    if (ctx == default)
                    {
                        return true;
                    }

                    var weakRef = new WeakReference(ctx);
                    DungeonGlobal.DungeonAssemblyContext = default;
                    ctx.Unload();

                    for (int i = 0; weakRef.IsAlive && (i < 10); i++)
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    DungeonGlobal.Logger.Log(ex.ToString());
                    return false;
                }
            });
        }

        public static IEnumerable<Type> LoadTypes<TAssignableFrom>()
            => DungeonGlobal.Assemblies.Concat(DungeonGlobal.GameAssembly)
                .SelectMany(x => x.GetTypesSafe().Where(t => typeof(TAssignableFrom).IsAssignableFrom(t)));


        /// <summary>
        /// 
        /// <para>
        /// [Кэшируемый]
        /// </para>
        /// </summary>
        public static Type LoadType(string className, bool force = false)
        {
            if (!___LoadTypeCache.TryGetValue(className, out var value) && !force)
            {
                value = LoadTypeImpl(className);
                ___LoadTypeCache.Add(className, value);
            }

            return value;
        }
        private static readonly Dictionary<string, Type> ___LoadTypeCache = new Dictionary<string, Type>();


        private static Type LoadTypeImpl(string className)
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
                type = assembly.GetTypesSafe().FirstOrDefault(x => x.Name == className);
            }

            return type;
        }
    }
}

#pragma warning restore CS0618 // Type or member is obsolete