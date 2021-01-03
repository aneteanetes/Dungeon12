using Dungeon.Data;
using Dungeon.Engine.Editable;
using Dungeon.Engine.Editable.Structures;
using Dungeon.Engine.Editable.TileMap;
using Dungeon.Engine.Engine;
using Dungeon.Resources;
using LiteDB;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Windows.Controls;

namespace Dungeon.Engine.Projects
{
    public class EngineProject : Persist
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public ProjectType Type { get; set; }

        public List<Reference> References { get; set; }

        public ObservableCollection<Scene> Scenes { get; set; } = new ObservableCollection<Scene>();

        public ObservableCollection<Tilemap> Maps { get; set; } = new ObservableCollection<Tilemap>();

        public ObservableCollection<ResourcesGraph> Resources { get; set; }

        public ProjectSettings CompileSettings { get; set; } = new ProjectSettings();

        public bool DataBaseExists => File.Exists(DbFilePath);

        public string DbFilePath => System.IO.Path.Combine(Path,Name, $"{Name}.deproj");

        public void Save()
        {
            using var db = new LiteDatabase(DbFilePath);
            var updated = db.GetCollection<EngineProject>().Update(new BsonValue(this.Id),this);
        }

        public void Load()
        {
            foreach (var res in Resources)
            {
                res.Load();
            }

            if (References == default)
            {
                References = new List<Reference>
                {
                    new Reference()
                    {
                        Kind = ReferenceKind.Embedded,
                        Title = "Dungeon",
                        Path = "Embedded"
                    }
                };
            }

            foreach (var refasm in References)
            {
                if (refasm.Kind != ReferenceKind.Embedded)
                {
                    ResourceLoader.LoadAssemblyUnloadable(refasm.Path);
                    if (!string.IsNullOrWhiteSpace(refasm.DbPath))
                    {
                        ResourceLoader.ResourceDatabaseResolvers.Add(new EngineResourceDatabaseResolver(refasm.DbPath));
                    }
                }
            }

            foreach (var scene in Scenes)
            {
                scene.Load();
                foreach (var layer in scene.StructObjects)
                {
                    if (layer is StructureSceneObject structSceneObj)
                    {
                        structSceneObj?.SceneObject?.Load();
                        structSceneObj?.SceneObject?.InitTable();
                    }
                }
            }
        }

        public void Close()
        {
            ResourceLoader.UnloadAssemblies().GetAwaiter().GetResult();
        }
    }
}