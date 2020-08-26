using System.Collections.ObjectModel;
using System.IO;

namespace Dungeon.Engine.Projects
{
    public class DungeonEngineResourcesGraph
    {
        public string Name
        {
            get
            {
                if (Path != default)
                {
                    return System.IO.Path.GetFileName(Path);
                }
                return "Resources";
            }
        }

        public string Path { get; set; }

        public DungeonEngineResourceType Type { get; set; }

        public ObservableCollection<DungeonEngineResourcesGraph> Nodes { get; set; } = new ObservableCollection<DungeonEngineResourcesGraph>();
    }
}
