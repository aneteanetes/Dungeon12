using LiteDB;
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

        public ResourceCompiler()
        {
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
            var dir = $@"{MainPath}\Data";
            if(!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var path = $@"{dir}\{Assembly.GetExecutingAssembly().GetName().Name}.dtr";
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
            using (var buildDb = new LiteDatabase($@"{MainPath}\ResourceManifest.dtr"))
            {
                buildDb
                  .GetCollection<ResourceManifest>()
                  .Insert(CurrentBuild);
            }
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
            Console.WriteLine($"Compile file: {file}");

            var lastTime = File.GetLastWriteTime(file);
            var res = LastBuild.Resources.FirstOrDefault(x => x.Path == file);

            CurrentBuild.Resources.Add(new Resource() { Path = file, LastWriteTime = lastTime });

            if (res == default)
            {
                CompileNewResource(file, projectName, db, lastTime);
            }
            else
            {
                CheckUpdateNeeded(file, db, lastTime, res);
                LastBuild.Resources.Remove(res);
            }
        }

        private static void CheckUpdateNeeded(string file, LiteCollection<Resource> db, DateTime lastTime, Resource res)
        {
            if (res.LastWriteTime.ToString() != lastTime.ToString())
            {
                CompileExistedResource(file, db, res);
            }
        }

        private static void CompileExistedResource(string file, LiteCollection<Resource> db, Resource res)
        {
            var dataResource = db.Find(x => x.Path == res.Path).FirstOrDefault();
            res.Data = File.ReadAllBytes(file);
            db.Update(res);
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

        public static string CompilePath => $@"{MainPath}\Data\{Assembly.GetExecutingAssembly().GetName().Name}.dtr";

        private ResourceManifest GetLastResourceManifestBuild()
        {
            ResourceManifest lastBuild = new ResourceManifest();

            using (var buildDb = new LiteDatabase($@"{MainPath}\ResourceManifest.dtr"))
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
}