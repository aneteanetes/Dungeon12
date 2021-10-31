using LiteDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Dungeon.Resources
{
    public class ResourceCompiler
    {
        public bool PreCompiled { get; set; }

        public string PreCompiledPath { get; set; }

        public ResourceManifest LastBuild { get; private set; }

        public ResourceManifest CurrentBuild { get; private set; }

        private static bool log = false;

        public ResourceCompiler(bool logging=false)
        {
            log = logging;
            LastBuild = GetLastResourceManifestBuild();
            CurrentBuild = new ResourceManifest();
        }

        private ILiteCollection<Resource> db;

        private void CopyPreCompiled(string path)
        {
            File.Copy(PreCompiledPath, path);
        }

        public void Compile(bool rebuild=false)
        {
            var caller = Assembly.GetCallingAssembly().GetName().Name;
            var dir = Path.Combine(MainPath, "Data");
            if(!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var path = Path.Combine(dir, $"{caller}.dtr");
            if (rebuild && File.Exists(path))
            {
                File.Delete(path);
            }

            if (PreCompiled)
            {
                CopyPreCompiled(path);
                return;
            }

            using var litedb = new LiteDatabase(path);
            
            db = litedb.GetCollection<Resource>();
            db.EnsureIndex("Path");
            
            ProcessProjectResources(rebuild);
            WriteCurrentBuild();
        }

        private void WriteCurrentBuild()
        {
            var manifest =  JsonConvert.SerializeObject(CurrentBuild, Formatting.Indented);

            File.WriteAllText(ManifestPath, manifest);
        }

        private void ProcessProjectResources(bool rebuild)
        {
            var path = Path.Combine(DungeonGlobal.ProjectPath, "Resources");
            var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                try
                {
                    ProcessFile(file);
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                    throw;
                }
            }
        }

        private void ProcessFile(string filePath)
        {
            var relativePath = FormatPathForDB(filePath);
            var lastTime = File.GetLastWriteTime(filePath);
            var res = LastBuild.Resources.FirstOrDefault(x => x.Path == relativePath);

            CurrentBuild.Resources.Add(new Resource() { Path = relativePath, LastWriteTime = lastTime });

            if(log)
            Console.WriteLine($"file {filePath} {(res==default ? "not" : "")} exists");

            if (res == default)
            {
                if (log)
                    Console.WriteLine($"compiling {filePath}");
                CompileNewResource(filePath, db, lastTime);
            }
            else
            {
                if (log)
                    Console.WriteLine($"check update {filePath}");
                CheckUpdateNeeded(filePath, db, lastTime, res);
                LastBuild.Resources.Remove(res);
            }
        }

        private static void CheckUpdateNeeded(string file, ILiteCollection<Resource> db, DateTime lastTime, Resource res)
        {
            if (res.LastWriteTime.ToString() != lastTime.ToString())
            {
                if (log)
                    Console.WriteLine($"Compile file: {file}");
                CompileExistedResource(file, db, res.Path, lastTime);
            }
        }

        private static void CompileExistedResource(string file, ILiteCollection<Resource> db, string path, DateTime lastTime)
        {
            var dataResource = db.Find(x => x.Path == path).FirstOrDefault();
            dataResource.Data = File.ReadAllBytes(file);
            dataResource.LastWriteTime = lastTime;
            db.Update(dataResource);
        }

        private static void CompileNewResource(string filePath, ILiteCollection<Resource> db, DateTime lastTime)
        {
            var newResource = new Resource()
            {
                Path = FormatPathForDB(filePath),
                LastWriteTime = lastTime,
                Data = File.ReadAllBytes(filePath)
            };
            db.Insert(newResource);
        }

        public static string FormatPathForDB(string filePath)
        {
            return Path.GetRelativePath(Directory.GetParent(DungeonGlobal.ProjectPath).ToString(), filePath).Replace(Path.DirectorySeparatorChar, '.');
        }

        public static string MainPath => Store.MainPath;

        public static string CompilePath => Path.Combine(MainPath, "Data", $"{DungeonGlobal.GameAssembly.GetName().Name}.dtr");

        public static string ManifestPath = Path.Combine(MainPath, "ResourceManifest.dtr");
        
        private ResourceManifest GetLastResourceManifestBuild()
        {
            if (File.Exists(ManifestPath))
            {
                return JsonConvert.DeserializeObject<ResourceManifest>(File.ReadAllText(ManifestPath));
            }

            return new ResourceManifest();
        }
    }
}