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

        private static string repositorypath = null;

        public static string RepositoryPath
        {
            get
            {
#if DEBUG
                if (repositorypath == null)
                {
                    DirectoryInfo path = new DirectoryInfo(DungeonGlobal.BuildLocation);
                    while (!Directory.Exists(Path.Combine(path.FullName, ".git")))
                    {
                        path = path.Parent;
                    }

                    return path.FullName;
                }
#endif

                return repositorypath;
            }
        }

        private static bool log = false;
        private static bool _logOnlyNewUpdate = false;

        public ResourceCompiler(bool logging = false, bool logOnlyNewUpdate=false)
        {
            _logOnlyNewUpdate=logOnlyNewUpdate;
            log = logging;
            LastBuild = GetLastResourceManifestBuild();
            CurrentBuild = new ResourceManifest();
        }

        private ILiteCollection<Resource> db;

        private void CopyPreCompiled(string path)
        {
            File.Copy(PreCompiledPath, path);
        }

        public void Compile(bool rebuild = false)
        {
            var caller = DungeonGlobal.GameAssemblyName;
            var dir = Path.Combine(MainPath, "Data");
            if (!Directory.Exists(dir))
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
            var manifest = JsonConvert.SerializeObject(CurrentBuild, Formatting.Indented);

            File.WriteAllText(ManifestPath, manifest);
        }

        private void ProcessProjectResources(bool rebuild)
        {
            string[] currentResourcePathsInDb = db.Query().Select(x => x.Path).ToArray();
            string[] filePaths = Directory.GetFiles(Path.Combine(DungeonGlobal.ProjectPath, "Resources"), "*.*",
                SearchOption.AllDirectories);
            string[] formattedFilePaths = filePaths.Select(path => FormatPathForDB(path)).ToArray();

            // Delete all resources from DB that are not present in Resource folder
            foreach (var resPathInDb in currentResourcePathsInDb)
            {
                if (!formattedFilePaths.Contains(resPathInDb))
                {
                    db.DeleteMany(x => x.Path == resPathInDb);
                }
            }

            foreach (string filePath in filePaths)
            {
                try
                {
                    ProcessFile(filePath);
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
            var formattedPath = FormatPathForDB(filePath);
            var lastTime = File.GetLastWriteTime(filePath);
            var res = LastBuild.Resources.FirstOrDefault(x => x.Path == formattedPath);

            CurrentBuild.Resources.Add(new Resource() { Path = formattedPath, LastWriteTime = lastTime });

            if (log)
                Console.WriteLine($"file {filePath} {(res == default ? "not" : "")} exists");

            if (res == default)
            {
                if (log || _logOnlyNewUpdate)
                    Console.WriteLine($"Add file: {filePath}");
                CompileNewResource(filePath, db, lastTime);
            }
            else
            {
                if (log)
                    Console.WriteLine($"check update {filePath}");
                CheckUpdateNeeded(filePath, db, lastTime, res);
            }
        }

        private static void CheckUpdateNeeded(string file, ILiteCollection<Resource> db, DateTime lastTime, Resource res)
        {
            if (res.LastWriteTime.ToString() != lastTime.ToString())
            {
                if (log || _logOnlyNewUpdate)
                    Console.WriteLine($"Update file: {file}");
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
        
        // We use manifest file to optimize querying resource metadata without loading content in memory.
        // LiteDB doesn't provide a convenient way to only load certain fields in memory.
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