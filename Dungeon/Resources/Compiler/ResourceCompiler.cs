using Dungeon.Resources.Internal;
using LiteDB;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Dungeon.Resources.Compiler
{
    /// <summary>
    /// Компилятор ресурсов
    /// </summary>
    public class ResourceCompiler
    {
        string manifestPath;

        ResourceCompilerConfiguration configuration;

        /// <summary>
        /// Манифест предыдущего билда
        /// </summary>
        public ResourceManifest LastBuild { get; private set; }

        /// <summary>
        /// Манифест текущего билда
        /// </summary>
        public ResourceManifest CurrentBuild { get; private set; }

        /// <summary>
        /// Файлы БД
        /// </summary>
        private Dictionary<string, ILiteCollection<Resource>> DbFiles = [];

        private ResourceCompiler(ResourceCompilerConfiguration settings)
        {
            this.configuration = settings;
            this.configuration.PathRepository = Directory.GetParent(configuration.PathProject).ToString();
            this.configuration.PathData = Path.Combine(configuration.PathBin, "Data");

            manifestPath = Path.Combine(this.configuration.PathData, "ResourceManifest.dtr");

            CurrentBuild = new ResourceManifest();
        }

        private void LoadLastBuild()
        {
            if (File.Exists(manifestPath))
            {
                LastBuild = JsonConvert.DeserializeObject<ResourceManifest>(File.ReadAllText(manifestPath));
            }
            else
            {
                Console.WriteLine("Resource manifest not found!");
                LastBuild = new ResourceManifest();
            }
        }

        private void WriteCurrentBuild()
        {
            var manifest = JsonConvert.SerializeObject(CurrentBuild, Formatting.Indented);
            File.WriteAllText(manifestPath, manifest);
        }

        internal static void Compile(ResourceCompilerConfiguration cfg)
        {
            var compiler = new ResourceCompiler(cfg);
            compiler.LoadLastBuild();

            var resDir = Path.Combine(compiler.configuration.PathProject, "Resources");
            var folders = Directory.GetDirectories(resDir)
                .Where(x => Directory.GetFiles(x,"*.*",SearchOption.AllDirectories).Length > 0)
                .ToArray();

            if (!Directory.Exists(compiler.configuration.PathData))
                Directory.CreateDirectory(compiler.configuration.PathData);

            foreach (var folder in folders)
            {
                var dir = new DirectoryInfo(folder);
                var resFilePath = Path.Combine(compiler.configuration.PathData, $"{dir.Name}.dtr");
                using var resDb = new LiteDatabase(resFilePath);
                var resources = resDb.GetCollection<Resource>();
                resources.EnsureIndex("Path");

                compiler.ProcessResourcesFolder(folder, resources);
            }

            compiler.WriteCurrentBuild();
        }

        private void ProcessResourcesFolder(string folderPath, ILiteCollection<Resource> db)
        {
            var filePaths = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);

            var formattedFilePaths = filePaths.Select(this.FormatPathForDB);

            var currentRes = db.Query().Select(x => x.Path).ToArray();

            // удалить из БД удалённые ресурсы
            var filesForDelete = currentRes.Except(formattedFilePaths);
            foreach (var fileForDelete in filesForDelete)
            {
                Console.WriteLine($"Resource {fileForDelete} was deleted!");
                db.DeleteMany(x => x.Path == fileForDelete);
            }

            // обрабатываем каждую папку в Resources как файл
            foreach (string filePath in filePaths)
            {
                try
                {
                    ProcessFile(filePath, db);
                }
                catch (Exception)
                {
                    Debugger.Break();
                    throw;
                }
            }
        }

        /// <summary>
        /// Переносит файлы из папки в БД
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="db"></param>
        private void ProcessFile(string filePath, ILiteCollection<Resource> db)
        {
            var formattedPath = FormatPathForDB(filePath);
            var lastTime = File.GetLastWriteTime(filePath);
            var res = LastBuild.Resources.FirstOrDefault(x => x.Path == formattedPath);

            CurrentBuild.Resources.Add(new Resource() { Path = formattedPath, LastWriteTime = lastTime });

            if (res == default)
            {
                var newResource = new Resource()
                {
                    Path = formattedPath,
                    LastWriteTime = lastTime,
                    Data = File.ReadAllBytes(filePath)
                };
                db.Insert(newResource);

                if(configuration.IsLogging)
                    Console.WriteLine($"Resource {formattedPath} was added!");
            }
            else
            {
                if (res.LastWriteTime.ToString() != lastTime.ToString())
                {
                    var dataResource = db.Find(x => x.Path == formattedPath).FirstOrDefault();
                    dataResource.Data = File.ReadAllBytes(filePath);
                    dataResource.LastWriteTime = lastTime;
                    db.Update(dataResource);

                    if (configuration.IsLogging)
                        Console.WriteLine($"Resource {formattedPath} was updated!");
                }
            }
        }

        private string FormatPathForDB(string filePath)
            => Path.GetRelativePath(configuration.PathRepository, filePath).Replace(Path.DirectorySeparatorChar, '.');

    }
}