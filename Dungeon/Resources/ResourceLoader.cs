using Dungeon.Resources.Processing;
using Dungeon.Resources.Resolvers;
using Dungeon.View.Interfaces;
using LiteDB;
using MoreLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

#pragma warning disable CS0618 // Type or member is obsolete

namespace Dungeon.Resources
{
    public static class ResourceLoader
    {
        public static ResourceLoaderSettings Settings => DungeonGlobal.Configuration.ResourceLoader;

        public static List<ResourceResolver> ResourceResolvers { get; set; } = new List<ResourceResolver>();

        public static List<ResourceProcessor> ResourceProcessors { get; set; } = new List<ResourceProcessor>();

        private static LiteDatabase resourceDatabase;
        private static LiteDatabase ResourceDatabase
        {
            get
            {
                try
                {
                    if (resourceDatabase == default)
                    {

                        if (Store.MainPath == null)
                            return null;

                        var caller = DungeonGlobal.GameAssemblyName;
                        var dir = Path.Combine(Store.MainPath, "Data");
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        var path = Path.Combine(dir, DungeonGlobal.Configuration.DbAssetsFileName);
                        Console.WriteLine(path);
                        resourceDatabase = new LiteDatabase(path);
                        DungeonGlobal.OnExit += () => resourceDatabase.Dispose();
                    }
                    return resourceDatabase;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
            }
        }

        private static LiteDatabase dataDb;
        private static LiteDatabase DataDb
        {
            get
            {
                try
                {
                    if (dataDb == default)
                    {

                        if (Store.MainPath == null)
                            return null;

                        var caller = DungeonGlobal.GameAssemblyName;
                        var dir = Path.Combine(Store.MainPath, DungeonGlobal.Configuration.DbDataFileName);
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        var path = Path.Combine(dir, $"Data.dtr");
                        Console.WriteLine(path);
                        dataDb = new LiteDatabase(path);
                        DungeonGlobal.OnExit += () => dataDb.Dispose();
                    }
                    return dataDb;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
            }
        }

        private static LiteDatabase LocaleDatabase(string locale)
        {
            var dir = Path.Combine(Store.MainPath, DungeonGlobal.Configuration.LocaleDirectory);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var path = Path.Combine(dir, $"{locale}.dtr");
            var liteDatabase = new LiteDatabase(path);
            return liteDatabase;
        }

        private static Dictionary<string, Dictionary<string,string>> LoadedLocales = new();

        public static Dictionary<string, string> LoadLocale(string lang)
        {
            if(LoadedLocales.ContainsKey(lang))
                return LoadedLocales[lang];

            using var db = LocaleDatabase(lang);

            var collection = db?.GetCollection<Resource>();
            if (collection != null)
            {
                var locale = new Dictionary<string, string>();

                try
                {
                    var files = collection.FindAll().ToList();
                    foreach (var file in files)
                    {
                        var localeFile = JsonConvert.DeserializeObject<Dictionary<string, string>>(file.Stream.AsString());

                        var segments = file.Path.Split(".");
                        var fileName = segments[segments.Length - 2].ToLowerInvariant();
                        if (fileName == lang){
                            fileName = string.Empty;
                        }
                        else { fileName += "."; }

                        foreach (var pair in localeFile)
                        {
                            var key = fileName + pair.Key;
                            locale.Add(key.ToLowerInvariant(), pair.Value.Replace("\\r\\n",Environment.NewLine));
                        }
                    }
                }
                catch { }

                LoadedLocales.Add(lang, locale);
                return locale;
            }

            return null;
        }

        public static T LoadData<T>(ResourceTable table,ISceneObject sceneObject, string resource, bool @throw = true)
        {
            var res = LoadResource(resource,sceneObject, table, DataDb);
            if (res == default)
                return default;

            return JsonConvert.DeserializeObject<T>(res.Stream.AsString());
        }

        public static Resource Load(ResourceTable table,ISceneObject sceneObject, string resource, bool @throw = true)
        {
            if (!resource.Contains(".Resources."))
            {
                resource = Assembly.GetEntryAssembly().GetName().Name + ".Resources." + resource.Embedded();
            }

            var res = LoadResource(resource, sceneObject,table, ResourceDatabase);

            if (res == default)
            {
                var resResolution = resource.IndexOf("@");
                if (resResolution > 0)
                {
                    var fileName = resource.Substring(0, resResolution);
                    var fileExt = Path.GetExtension(resource);
                    return Load(table, sceneObject, $"{fileName}{fileExt}", @throw);
                }
            }

            table.Add(resource, res);

            return res;
        }

        private static Resource LoadResource(string resource, ISceneObject sceneObject, ResourceTable table, LiteDatabase liteDb)
        {
            if (table.ContainsKey(resource))
            {
                if (table.TryGetValue(resource,sceneObject, out var loadedRes))
                    return loadedRes;
            }

            Resource res = default;

            var db = liteDb?.GetCollection<Resource>();
            if (db != null)
            {
                try
                {
                    res = db.Find(x => x.Path == resource).FirstOrDefault();
                }
                catch { }
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
            else
            {
                foreach (var rp in ResourceProcessors)
                {
                    if(rp.IsCanProcess(resource))
                        rp.Process(resource, res);
                }
            }

            return res;
        }

        public static IEnumerable<Resource> LoadResourceFolder(string folder, ResourceTable table)
        {
            if (table.IsFolderLoaded(folder))
            {
                return table.GetFolder(folder);
            }

            var db = ResourceDatabase?.GetCollection<Resource>();
            if (db != null)
            {
                try
                {
                    var resources = db.Find(x => x.Path.StartsWith(folder)).ToArray();
                    table.AddFolder(folder, resources);

                    foreach (var res in resources)
                    {
                        foreach (var rp in ResourceProcessors)
                        {
                            if (rp.IsCanProcess(res.Path))
                                rp.Process(res.Path, res);
                        }
                    }

                    return resources;
                }
                catch { }
            }

            return Enumerable.Empty<Resource>();
        }

        public static IEnumerable<Type> LoadTypes<TAssignableFrom>()
            => DungeonGlobal.Assemblies.Concat([DungeonGlobal.GameAssembly])
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

            if (className.Contains("`"))
            {
                var openGeneric = className.IndexOf("[");
                var genericClassName = className.Substring(0, openGeneric);
                var genericClass = LoadTypeInternal(genericClassName, true);

                var argumentTypes = className
                    .Substring(openGeneric)
                    .Split("]", StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Replace("[", "").Replace("]", ""))
                    .Select(s => LoadTypeInternal(s, true))
                    .ToArray();

                return genericClass.MakeGenericType(argumentTypes);
            }
            else return LoadTypeInternal(className);
        }

        private static Type LoadTypeInternal(string className, bool domainForce = false)
        {
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

            if (domainForce)
            {
                var domain = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var domainAsm in domain)
                {
                    type = TryGetFromAssembly(className, domainAsm);
                    if (type != default)
                    {
                        break;
                    }
                }
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
            if (type == default && className.Contains("Culture"))
            {
                var comma = className.IndexOf(",");
                className = className.Substring(0, comma).Trim();
                type = assembly.GetType(className);
            }

            if (type == default)
            {
                type = assembly.GetTypesSafe().FirstOrDefault(x => x.Name == className);
            }

            return type;
        }
    }
}

#pragma warning restore CS0618 // Type or member is obsolete