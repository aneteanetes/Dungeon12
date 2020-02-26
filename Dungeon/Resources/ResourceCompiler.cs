using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Dungeon.Resources
{
    public class ResourceCompiler
    {
        public ResourceManifest LastBuild { get; private set; }

        public ResourceManifest CurrentBuild { get; private set; }

        public ResourceCompiler()
        {
            LastBuild = GetLastResourceManifestBuild();
            CurrentBuild = new ResourceManifest();
        }

        private LiteCollection<Resource> db;

        public void Compile(bool rebuild=false)
        {
            IEnumerable<string> resDirectories = Directory.GetDirectories(Store.ProjectDirectory, "Resources", SearchOption.AllDirectories);

            var path = $@"{MainPath}\Data\{Assembly.GetExecutingAssembly().GetName().Name}.dtr";
            if (rebuild && File.Exists(path))
            {
                File.Delete(path);
            }

            using var litedb = new LiteDatabase(path);
            
            db = litedb.GetCollection<Resource>();
            db.EnsureIndex("Path");

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
                ProcessFile(file, dir.Parent.Name);
            }
        }

        private void ProcessFile(string file, string projectName)
        {
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
                Path = file.Substring(file.IndexOf(projectName + "\\")).Replace("\\", "."),
                LastWriteTime = lastTime,
                Data = File.ReadAllBytes(file)
            };
            db.Insert(newResource);
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