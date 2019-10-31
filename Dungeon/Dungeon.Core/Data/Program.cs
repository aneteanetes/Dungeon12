using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using LiteDB;
using Newtonsoft.Json;
using Dungeon.Data.Perks;
//using Dungeon.DataAccess.Perk;

namespace Dungeon.Data
{
    /// <summary>
    /// Лучше конечно прикрутить Nuke
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Компилирует БД из json файлов, перед использованием - стирает нахой data
        /// <see cref="Main(string[])"/> что бы отдельно консолькой можно было запускать
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            if (!Directory.Exists(MainPath))
                Directory.CreateDirectory(MainPath);

            if (File.Exists(Path.Combine(MainPath, "Data.db")))
                File.Delete(Path.Combine(MainPath, "Data.db"));

            LoadRogueDataAssembly();
            CompileDatabase();

            //Test();
        }

        private static void Test()
        {
            using (var db = new LiteDatabase($@"{MainPath}\Data.db"))
            {
                var collection = db.GetCollection<ValuePerk>();

                foreach (var item in collection.FindAll())
                {
                    Console.WriteLine(JsonConvert.SerializeObject(item));
                }
            }
        }

        private static void CompileDatabase()
        {
            foreach (var data in JsonFiles())
            {
                using (var db = new LiteDatabase($@"{MainPath}\Data.db"))
                {
                    var collection = GetGenericLiteCollection(data.Type, db);

                    var list = GenericList(data.Type);

                    foreach (var item in data.JsonFiles)
                    {
                        AddToGenericList(list, JsonConvert.DeserializeObject(File.ReadAllText(item,Encoding.UTF8), GetDataType(data.Type)));
                    }

                    Insert(collection, list);
                }
            }
        }

        private static object GenericList(string typeName)
        {
            var t = GetDataType(typeName);
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(t);

            return Activator.CreateInstance(constructedListType);
        }

        private static void AddToGenericList(object genericList,object value)
        {
            ListAddMethod(genericList)
                .Invoke(genericList,new object[] { value });
        }

        private static MethodInfo ListAddMethod(object list) => list.GetType().GetMethods().Where(x => x.Name == "Add").First();

        private static Assembly RogueDataAssembly;
        private static void LoadRogueDataAssembly()
        {
            RogueDataAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dungeon.Data.dll"));
        }

        private static Type GetDataType(string typeName)
        {
            return RogueDataAssembly.GetType(typeName);
        }

        private static object GetGenericLiteCollection(string typeName, LiteDatabase db)
        {
            return GetCollectionMethod.MakeGenericMethod(GetDataType(typeName)).Invoke(db, new object[0]);
        }

        private static void Insert(object collection, object obj)
        {
            InsertMethod(collection)
                .Invoke(collection, new object[] { obj });
        }

        //int Insert(IEnumerable<T> docs)
        private static MethodInfo InsertMethod(object collection) => collection.GetType().GetMethods().Where(x => x.Name == "Insert" && x.ReturnType == typeof(int)).First();

        private static MethodInfo GetCollectionMethod => typeof(LiteDatabase).GetMethods().Where(x => x.IsGenericMethod && x.Name == "GetCollection").Last();

        private static readonly string MainPath = $@"{AppDomain.CurrentDomain.BaseDirectory}\Data";

        private static readonly string DataDirectory = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.ToString(), "Dungeon.Data");

        private static IEnumerable<DataInfo> JsonFiles()
        {
            IEnumerable<string> dataDirectories = Directory.GetDirectories(DataDirectory);

            dataDirectories = dataDirectories
                .Where(x =>
                    x != Path.Combine(DataDirectory, "bin")
                    && x != Path.Combine(DataDirectory, "obj"));

            return dataDirectories.Select(dataDirectory =>
            {
                var csFileDeclaringType = Directory.GetFiles(dataDirectory, "*.cs").FirstOrDefault();
                if (string.IsNullOrEmpty(csFileDeclaringType))
                {
                    throw new Exception($"Директория {dataDirectory} не содержит *.cs файла описывающего тип данных");
                }

                return new DataInfo
                {
                    Type = $"Dungeon.Data.{new DirectoryInfo(dataDirectory).Name}.{Path.GetFileNameWithoutExtension(csFileDeclaringType)}",
                    JsonFiles = Directory.GetFiles(Path.Combine(dataDirectory, "Data"), "*.json", SearchOption.AllDirectories)
                };
            });
        }
    }

    internal class DataInfo
    {
        public string Type { get; set; }

        public IEnumerable<string> JsonFiles { get; set; }
    }
}