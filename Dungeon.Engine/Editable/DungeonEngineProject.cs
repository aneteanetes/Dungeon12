using Dungeon.Data;
using LiteDB;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Controls;

namespace Dungeon.Engine.Projects
{
    public class DungeonEngineProject : Persist
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public DungeonEngineProjectType Type { get; set; }

        public List<string> References { get; set; }

        public ObservableCollection<DungeonEngineScene> Scenes { get; set; } = new ObservableCollection<DungeonEngineScene>();

        public ObservableCollection<DungeonEngineResourcesGraph> Resources { get; set; }

        public DungeonEngineProjectSettings CompileSettings { get; set; } = new DungeonEngineProjectSettings();

        public bool DataBaseExists => File.Exists(DbFilePath);

        public string DbFilePath => System.IO.Path.Combine(Path,Name, $"{Name}.deproj");

        public void Save()
        {
            using var db = new LiteDatabase(DbFilePath);
            var updated = db.GetCollection<DungeonEngineProject>().Update(new BsonValue(this.Id),this);
        }

        public void Load()
        {
            foreach (var scene in Scenes)
            {
                foreach (var obj in scene.SceneObjects)
                {
                    obj.InitTable();
                }
            }
        }
    }
}