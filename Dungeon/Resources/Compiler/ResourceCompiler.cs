using Dungeon.Resources.Internal;
using LiteDB;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Dungeon.Resources.Compiler
{
    public class ResourceCompiler
    {
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

        public ResourceCompiler(bool logging = false, bool logOnlyNewUpdate = false)
        {
            _logOnlyNewUpdate = logOnlyNewUpdate;
            log = logging;
            LastBuild = GetLastResourceManifestBuild();
            CurrentBuild = new ResourceManifest();
        }

        private string DataDir = null;
        private ILiteCollection<Resource> DataDb = null;

        private string ResDir = null;
        private ILiteCollection<Resource> ResourceDb = null;

        private string LocaleDir = null;
        private ILiteCollection<Resource> LocaleDb = null;

        private void CopyPreCompiled(string path)
        {
            File.Copy(PreCompiledPath, path);
        }

        public void Compile()
        {
            var caller = DungeonGlobal.GameAssemblyName;

            DataDir = ResDir = Path.Combine(MainPath, DungeonGlobal.Configuration.DataDirectory);
            LocaleDir = Path.Combine(MainPath, DungeonGlobal.Configuration.LocaleDirectory);

            CreateDirectories();

            var dataPath = Path.Combine(ResDir, DungeonGlobal.Configuration.DbDataFileName);
            var resPath = Path.Combine(ResDir, DungeonGlobal.Configuration.DbAssetsFileName);
            var localePath = Path.Combine(LocaleDir, $"{DungeonGlobal.Configuration.TwoLetterISOLanguageName}.dtr");

            using var resLiteDb = new LiteDatabase(resPath);
            ResourceDb = resLiteDb.GetCollection<Resource>();
            ResourceDb.EnsureIndex("Path");

            using var dataLiteDb = new LiteDatabase(dataPath);
            DataDb = dataLiteDb.GetCollection<Resource>();
            DataDb.EnsureIndex("Path");


            using var localeLiteDb = new LiteDatabase(localePath);
            LocaleDb = localeLiteDb.GetCollection<Resource>();
            LocaleDb.EnsureIndex("Path");

            ProcessProjectResources();
            WriteCurrentBuild();
        }

        private void CreateDirectories()
        {
            if (!Directory.Exists(ResDir))
            {
                Directory.CreateDirectory(ResDir);
            }

            if (!Directory.Exists(LocaleDir))
            {
                Directory.CreateDirectory(LocaleDir);
            }
        }

        private void WriteCurrentBuild()
        {
            var manifest = JsonConvert.SerializeObject(CurrentBuild, Formatting.Indented);

            File.WriteAllText(ManifestPath, manifest);
        }

        private void ProcessProjectResources()
        {
            var filePaths = Directory.GetFiles(Path.Combine(DungeonGlobal.ProjectPath, "Resources"), "*.*", SearchOption.AllDirectories);

            var localePath = Path.Combine(Path.Combine(DungeonGlobal.ProjectPath, "Resources", DungeonGlobal.Configuration.LocaleDirectory));

            var localesPaths = filePaths.Where(x => x.StartsWith(localePath)).ToArray();
            ProcessDatabase(localesPaths, LocaleDb);

            var dataPath = Path.Combine(Path.Combine(DungeonGlobal.ProjectPath, "Resources", "Data"));
            var dataPaths = filePaths.Where(x => x.StartsWith(dataPath)).ToArray();
            ProcessDatabase(dataPaths, DataDb);


            filePaths = filePaths.Except(localesPaths.Concat(dataPaths)).ToArray();
            ProcessDatabase(filePaths, ResourceDb);
        }

        private void ProcessDatabase(string[] filePaths, ILiteCollection<Resource> db)
        {
            string[] currentResourcePathsInDb = db.Query().Select(x => x.Path).ToArray();

            string[] formattedFilePaths = filePaths.Select(FormatPathForDB).ToArray();


            // Delete all resources from DB that are not present in Resource folder
            foreach (var resPathInDb in currentResourcePathsInDb)
            {
                if (!formattedFilePaths.Contains(resPathInDb))
                {
                    Console.WriteLine($"res {resPathInDb} deleted!");
                    db.DeleteMany(x => x.Path == resPathInDb);
                }
            }

            foreach (string filePath in filePaths)
            {
                try
                {
                    ProcessFile(filePath, db);
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                    throw;
                }
            }
        }

        private void ProcessFile(string filePath, ILiteCollection<Resource> db)
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