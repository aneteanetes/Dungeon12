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
            LoadAllAssemblies();

            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var dataFile = Path.Combine(path, "Data.db");

            if (!Directory.Exists(MainPath))
                Directory.CreateDirectory(MainPath);

            if (File.Exists(Path.Combine(MainPath, "Data.db")))
                File.Delete(Path.Combine(MainPath, "Data.db"));

            CompileDatabase();
        }

        public static void CompileResources()
        {
            IEnumerable<string> resDirectories = Directory.GetDirectories(ProjectDirectory, "Resources", SearchOption.AllDirectories);
            foreach (var resDir in resDirectories)
            {
                var dir = new DirectoryInfo(resDir);

                var projectName = dir.Parent.Name;

                using (var litedb = new LiteDatabase($@"{MainPath}\Data\{projectName}.dtr"))
                {
                    var db = litedb.GetCollection<DatabaseResource>();

                    foreach (var file in Directory.GetFiles(dir.FullName, "*.*", SearchOption.AllDirectories))
                    {
                        var res = new DatabaseResource()
                        {
                            Path = file.Substring(file.IndexOf(projectName+"\\")).Replace("\\", "."),
                            Size = new FileInfo(file).Length
                        };

                        var dataResource = db.Find(x => x.Path == res.Path).FirstOrDefault();
                        if (dataResource == default)
                        {
                            res.Data = File.ReadAllBytes(file);
                            db.Insert(res);
                        }
                        else if (dataResource.Size!=res.Size)
                        {
                            res.Data = File.ReadAllBytes(file);
                            db.Update(res);
                        }
                    }
                }
            }
        }

        private class DatabaseResource : Persist
        {
            public string Path { get; set; }

            public byte[] Data { get; set; }

            public long Size { get; set; }
        }

        public static void LoadAllAssemblies()
        {
            var assemblies = new List<Assembly>();
            var asms = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            foreach (var asm in asms)
            {
                try
                {
                    assemblies.Add(AssemblyLoadContext.Default.LoadFromAssemblyPath(asm));
                }
                catch
                {
                }
            }
            DungeonGlobal.Assemblies = assemblies;
        }

        private static void CompileDatabase()
        {
            foreach (var data in JsonFiles())
            {
                Console.WriteLine($"Compiling:{data.Type.Name}");
                using (var db = new LiteDatabase($@"{MainPath}\Data.db"))
                {
                    var collection = GetGenericLiteCollection(data.Type, db);

                    var list = GenericList(data.Type);

                    foreach (var item in data.JsonFiles)
                    {
                        var obj = JsonConvert.DeserializeObject(File.ReadAllText(item, Encoding.UTF8), data.Type, _jsonSerializerSettings);
                        if (obj is IPersist persist)
                        {
                            persist.Assembly = data.Assembly;
                            if (persist.IdentifyName == default)
                            {
                                persist.IdentifyName = Path.GetFileNameWithoutExtension(item);
                            }
                        }
                        AddToGenericList(list, obj);
                    }

                    Insert(collection, list);
                }
            }
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

        private static void Insert(object collection, object obj)
        {
            InsertMethod(collection)
                .Invoke(collection, new object[] { obj });
        }

        //int Insert(IEnumerable<T> docs)
        private static MethodInfo InsertMethod(object collection) => collection.GetType().GetMethods().Where(x => x.Name == "Insert" && x.ReturnType == typeof(int)).First();

        private static MethodInfo GetCollectionMethod => typeof(LiteDatabase).GetMethods().Where(x => x.IsGenericMethod && x.Name == "GetCollection").Last();

        private static string ProjectDirectory
        {
            get
            {
                try
                {
                    return Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.ToString();
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        private static IEnumerable<DataInfo> JsonFiles()
        {
            IEnumerable<string> databaseDirectories = Directory.GetDirectories(ProjectDirectory,"Database", SearchOption.AllDirectories);

            databaseDirectories = databaseDirectories
                .Where(x =>
                    x != Path.Combine(ProjectDirectory, "bin")
                    && x != Path.Combine(ProjectDirectory, "obj"));

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
                        JsonFiles = Directory.GetFiles(dataPath, "*.json", SearchOption.AllDirectories)
                    };
                });
            }).Where(v => v != null);
        }
    }

    internal class DataInfo
    {
        public string Assembly { get; set; }

        public Type Type { get; set; }

        public IEnumerable<string> JsonFiles { get; set; }
    }
}