using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using LiteDB;
using Newtonsoft.Json;
using System.Linq;
using System.Runtime.Loader;
using Dungeon.Resources;
using Dungeon.Data;
using System.Linq.Expressions;
using Dungeon.Localization;

namespace Dungeon
{
    public static partial class Store
    {
        private static JsonSerializerSettings _jsonSerializerSettings;

        /// <summary>
        /// Компилирует БД из json файлов, перед использованием - стирает нахой data
        /// <see cref="CompileData(string[])"/> что бы отдельно консолькой можно было запускать
        /// </summary>
        /// <param name="args"></param>
        public static void Init(JsonSerializerSettings jsonSerializerSettings)
        {
            _jsonSerializerSettings = jsonSerializerSettings;
            ResourceLoader.LoadAllAssembliesInFolder();

            if (!Directory.Exists(MainPath))
                Directory.CreateDirectory(MainPath);

            LastBuild = GetLastDataManifestBuild();
            CurrentBuild = new ResourceManifest();

            var strings = CompileDatabase();

            var global = DungeonGlobal.GetBindedGlobal();
            if (global == default)
                return;

            var localizationSettings = global.GetStringsClass();
            localizationSettings.DynamicStrings = strings;
            localizationSettings.___Save(localizationSettings.___DefaultLanguageCode);
        }

        private static List<LocalizedString> CompileDatabase()
        {
            var strings = new List<LocalizedString>();
            using (var db = new LiteDatabase(Path.Combine(MainPath, "Data.db")))
            {
                foreach (var data in JsonFiles())
                {
                    var collection = GetGenericLiteCollection(data.Type, db);
                    EnsureIndex(collection);

                    var list = GenericList(data.Type);

                    foreach (var item in data.JsonFiles)
                    {
                        var res = LastBuild.Resources.FirstOrDefault(x => x.Path == item.path);

                        CurrentBuild.Resources.Add(new Resource()
                        {
                            Path = item.path,
                            LastWriteTime = item.modifydate
                        });

                        if (res == default)
                        {
                            strings.AddRange(StoreData(data, collection, item.path));
                        }
                        else if (res.LastWriteTime.ToString() != File.GetLastWriteTime(item.path).ToString())
                        {
                            strings.AddRange(StoreData(data, collection, item.path));
                            LastBuild.Resources.Remove(res);
                        }
                    }
                }
            }

            using (var buildDb = new LiteDatabase(Path.Combine(MainPath, "DataManifest.dtr")))
            {
                buildDb
                  .GetCollection<ResourceManifest>()
                  .Insert(CurrentBuild);
            }

            return strings;
        }

        private static List<LocalizedString> StoreData(DataInfo data, object collection, string path)
        {
            var strings = new List<LocalizedString>();
            var obj = JsonConvert.DeserializeObject(File.ReadAllText(path, Encoding.UTF8), data.Type, _jsonSerializerSettings);
            if (obj is IPersist persist)
            {
                var id = Path.GetFileNameWithoutExtension(path);
                persist.Assembly = data.Assembly;
                if (persist.IdentifyName == default)
                {
                    persist.IdentifyName = id;
                }

                foreach (var stringProp in data.LocalizedStringsProps)
                {
                    var localized = obj.GetPropertyExpr<LocalizedString>(stringProp);
                    strings.Add(new LocalizedString()
                    {
                        Code = localized.Code,
                        Lang = localized.Lang,
                        Value = localized.Value
                    });
                }

                Insert(collection, id, obj);
            }

            return strings;
        }

        private static object GenericList(Type type)
        {
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(type);

            return Activator.CreateInstance(constructedListType);
        }

        private static void AddToGenericList(object genericList, object value)
        {
            ListAddMethod(genericList)
                .Invoke(genericList, new object[] { value });
        }

        private static MethodInfo ListAddMethod(object list) => list.GetType().GetMethods().Where(x => x.Name == "Add").First();

        private static Assembly RogueDataAssembly;
        private static void LoadRogueDataAssembly()
        {
            RogueDataAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dungeon.Data.dll"));
        }

        private static object GetGenericLiteCollection(Type type, LiteDatabase db)
        {
            return GetCollectionMethod.MakeGenericMethod(type).Invoke(db, new object[0]);
        }

        private static void EnsureIndex(object collection)
        {
            EnsureIndexMethod(collection)
                .Invoke(collection, new object[] { "IdentifyIndex", false });
        }

        private static void Insert(object collection,string identify, object obj)
        {
            var existed = FindMethod(collection)
                .Invoke(collection, new object[] { Query.EQ(nameof(IPersist.IdentifyName), identify) });

            if (existed == default) {
                InsertMethod(collection)
                    .Invoke(collection, new object[] { obj });
            }
            else if (existed is IPersist persistExisted)
            {
                var updMethod = UpdateMethod(collection);

                var r = updMethod.Invoke(collection, new object[] { new BsonValue(persistExisted.Id), obj });
                Console.WriteLine(r);
            }
        }

        private static MethodInfo FindMethod(object collection) => collection.GetType().GetMethods().Where(x => x.Name == "FindOne" && (x.GetParameters().FirstOrDefault()?.Name == "query")).First();

        private static MethodInfo InsertMethod(object collection) => collection.GetType().GetMethods().Where(x => x.Name == "Insert" && x.ReturnType == typeof(BsonValue)).First();

        private static MethodInfo UpdateMethod(object collection) => collection.GetType().GetMethods().Where(x => x.Name == "Update" && x.GetParameters().Length == 2).First();

        private static MethodInfo EnsureIndexMethod(object collection) => collection.GetType().GetMethods().Where(x => x.Name == "EnsureIndex" && !x.IsGenericMethod && x.GetParameters().Length == 2).First();

        private static MethodInfo GetCollectionMethod => typeof(LiteDatabase).GetMethods().Where(x => x.IsGenericMethod && x.Name == "GetCollection").Last();

        private static IEnumerable<DataInfo> JsonFiles()
        {
            IEnumerable<string> databaseDirectories = Directory.GetDirectories(DungeonGlobal.ProjectPath, "Database", SearchOption.AllDirectories);

            databaseDirectories = databaseDirectories
                .Where(x =>
                    x != Path.Combine(DungeonGlobal.ProjectPath, "bin")
                    && x != Path.Combine(DungeonGlobal.ProjectPath, "obj"));

            return databaseDirectories.SelectMany(databaseDirectory =>
            {
                var asm = new DirectoryInfo(databaseDirectory).Parent.Name;

                return Directory.GetDirectories(databaseDirectory).Select(dataDirectory =>
                {
                    var csFileDeclaringType = Directory.GetFiles(dataDirectory, "*.cs").ToArray();
                    if (csFileDeclaringType.Length == 0 || csFileDeclaringType.Length > 1)
                    {
                        return null;
                        //throw new Exception($"Директория {dataDirectory} не содержит *.cs файла описывающего тип данных");
                    }

                    var csFile = csFileDeclaringType.FirstOrDefault();

                    var dataPath = Path.Combine(Path.GetDirectoryName(csFile), "Data");
                    if (!Directory.Exists(dataPath))
                    {
                        return null;
                    }

                    var typeName = Path.GetFileNameWithoutExtension(csFile);
                    var type = ResourceLoader.LoadType(typeName);

                    return new DataInfo
                    {
                        Assembly=asm,
                        Type = type,
                        JsonFiles = Directory.GetFiles(dataPath, "*.json", SearchOption.AllDirectories).Select(x=> (x,File.GetLastWriteTime(x)))
                    };
                });
            }).Where(v => v != null);
        }

        private static ResourceManifest LastBuild { get; set; }

        private static ResourceManifest CurrentBuild { get; set; }

        private static ResourceManifest GetLastDataManifestBuild()
        {
            ResourceManifest lastBuild = new ResourceManifest();

            using (var buildDb = new LiteDatabase(Path.Combine(MainPath, "DataManifest.dtr")))
            {
                lastBuild = buildDb
                    .GetCollection<ResourceManifest>()
                    .FindOne(Query.All(1));
            }

            if (lastBuild == default)
            {
                lastBuild = new ResourceManifest();
            }

            return lastBuild;
        }
    }

    internal class DataInfo
    {
        public string Assembly { get; set; }

        public Type Type { get; set; }

        public List<string> LocalizedStringsProps
        {
            get
            {
                return Type.GetProperties()
                    .Where(x => x.PropertyType == typeof(Localization.LocalizedString))
                    .Select(x => x.Name)
                    .ToList();
            }
        }

        public IEnumerable<(string path,DateTime modifydate)> JsonFiles { get; set; }
    }
}