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

        private LiteCollection<Resource> db;

        private void CopyPreCompiled(string path)
        {
            File.Copy(PreCompiledPath, path);
        }

        public void Compile(bool rebuild=false)
        {
            var caller = Assembly.GetCallingAssembly().GetName().Name;
            var dir = $@"{MainPath}\Data";
            if(!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var path = $@"{dir}\{caller}.dtr";
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

            IEnumerable<string> resDirectories = Directory.GetDirectories(Store.ProjectDirectory, "Resources", SearchOption.AllDirectories);
            foreach (var resDir in resDirectories)
            {
                ProcessProject(resDir, rebuild);
            }

            WriteCurrentBuild();
        }

        private void WriteCurrentBuild()
        {
            var manifestPath = $@"{MainPath}\ResourceManifest.dtr";
            var manifest =  JsonConvert.SerializeObject(CurrentBuild, Formatting.Indented);

            File.WriteAllText(manifestPath, manifest);
        }

        private void ProcessProject(string projectResDirectory, bool rebuild)
        {
            var dir = new DirectoryInfo(projectResDirectory);
            foreach (var file in Directory.GetFiles(dir.FullName, "*.*", SearchOption.AllDirectories))
            {
                try
                {
                    ProcessFile(file, GetProjectName(dir)?.Replace(".csproj", ""));
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                    throw;
                }
            }
        }

        private string GetProjectName(DirectoryInfo directory)
        {
            var proj = directory.GetFiles("*.csproj").FirstOrDefault();
            if (proj != default)
            {
                return proj.Name;
            }
            else if (directory.Parent != default)
            {
                return GetProjectName(directory.Parent);
            }

            return null;
        }

        private void ProcessFile(string file, string projectName)
        {
            var path = GetPathUntillProjectName(file, projectName);
            var lastTime = File.GetLastWriteTime(file);
            var res = LastBuild.Resources.FirstOrDefault(x => x.Path == path);

            CurrentBuild.Resources.Add(new Resource() { Path = path, LastWriteTime = lastTime });

            if(log)
            Console.WriteLine($"file {file} {(res==default ? "not" : "")} exists");

            if (res == default)
            {
                if (log)
                    Console.WriteLine($"compiling {file}");
                CompileNewResource(file, projectName, db, lastTime);
            }
            else
            {
                if (log)
                    Console.WriteLine($"check update {file}");
                CheckUpdateNeeded(file, db, lastTime, res);
                LastBuild.Resources.Remove(res);
            }
        }

        private static void CheckUpdateNeeded(string file, LiteCollection<Resource> db, DateTime lastTime, Resource res)
        {
            if (res.LastWriteTime.ToString() != lastTime.ToString())
            {
                if (log)
                    Console.WriteLine($"Compile file: {file}");
                CompileExistedResource(file, db, res.Path, lastTime);
            }
        }

        private static void CompileExistedResource(string file, LiteCollection<Resource> db, string path, DateTime lastTime)
        {
            var dataResource = db.Find(x => x.Path == path).FirstOrDefault();
            dataResource.Data = File.ReadAllBytes(file);
            dataResource.LastWriteTime = lastTime;
            db.Update(dataResource);
        }

        private static void CompileNewResource(string file, string projectName, LiteCollection<Resource> db, DateTime lastTime)
        {
            var newResource = new Resource()
            {
                Path = GetPathUntillProjectName(file, projectName),// file.Substring(file.IndexOf(projectName + "\\")).Replace("\\", "."),
                LastWriteTime = lastTime,
                Data = File.ReadAllBytes(file)
            };
            db.Insert(newResource);
        }

        public static string GetPathUntillProjectName(string path, string projName)
        {
            var parts = path.Split("\\", StringSplitOptions.RemoveEmptyEntries).Reverse();
            var partsEnum = parts.GetEnumerator();
            partsEnum.MoveNext();
            string currentPart = partsEnum.Current;
            string newPath = default;
            while(currentPart!=projName)
            {
                newPath = currentPart + (newPath == default ? "" : ".") + newPath;
                partsEnum.MoveNext();
                currentPart = partsEnum.Current;
            }

            return $"{projName}.{newPath}";
        }

        public static string MainPath => Store.MainPath;

        public static string CompilePath => $@"{MainPath}\Data\{DungeonGlobal.GameAssembly.GetName().Name}.dtr";

        private ResourceManifest GetLastResourceManifestBuild()
        {
            var manifestPath = $@"{MainPath}\ResourceManifest.dtr";

            if (File.Exists(manifestPath))
            {
                return JsonConvert.DeserializeObject<ResourceManifest>(File.ReadAllText(manifestPath));
            }

            return new ResourceManifest();
        }
    }
}