using Dungeon.Resources.Resolvers;
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

        private static LiteDatabase liteDatabase;
        private static LiteDatabase LiteDatabase
        {
            get
            {
                try
                {
                    if (liteDatabase == default)
                    {

                        if (Store.MainPath==null)
                            return null;

                        var caller = DungeonGlobal.GameAssemblyName;
                        var dir = Path.Combine(Store.MainPath, "Data");
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        var path = Path.Combine(dir, $"{caller}.dtr");
                        Console.WriteLine(path);
                        liteDatabase = new LiteDatabase(path);
                        DungeonGlobal.OnExit += () => liteDatabase.Dispose();
                    }
                    return liteDatabase;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
            }
        }

        public static T LoadJson<T>(ResourceTable table, string resource, bool @throw = true)
        {
            var res = Load(table, resource, @throw);
            if (res == default)
                return default;

            return JsonConvert.DeserializeObject<T>(res.Stream.AsString());
        }

        public static Resource Load(ResourceTable table, string resource, bool @throw = true)
        {
            if (!resource.Contains(".Resources."))
            {
                resource = Assembly.GetEntryAssembly().GetName().Name + ".Resources." + resource.Embedded();
            }

            var res = LoadResource(resource,table);

            if (res == default)
            {
                var resResolution = resource.IndexOf("@");
                if (resResolution > 0)
                {
                    var fileName = resource.Substring(0, resResolution);
                    var fileExt = Path.GetExtension(resource);
                    return Load(table, $"{fileName}{fileExt}", @throw);
                }
            }

            table.Add(resource, res);

            return res;
        }



        private static Resource LoadResource(string resource, ResourceTable table)
        {
            if (table.ContainsKey(resource))
            {
                return table[resource];
            }

            Resource res = default;

            var db = LiteDatabase?.GetCollection<Resource>();
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

            return res;
        }

        public static IEnumerable<Resource> LoadResourceFolder(string folder, ResourceTable table)
        {
            if (table.IsFolderLoaded(folder))
            {
                return table.GetFolder(folder);
            }

            var db = LiteDatabase?.GetCollection<Resource>();
            if (db != null)
            {
                try
                {
                    var resources = db.Find(x => x.Path.StartsWith(folder)).ToArray();
                    table.AddFolder(folder, resources);

                    return resources;
                }
                catch { }
            }

            return Enumerable.Empty<Resource>();
        }
    }
}

#pragma warning restore CS0618 // Type or member is obsolete