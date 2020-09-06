using LiteDB;
using MoreLinq;
using System.Collections.ObjectModel;
using System.Linq;

namespace Dungeon.Engine.Projects
{
    public class DungeonEngineResourcesGraph
    {
        public string Display
        {
            get
            {
                if (Name != default)
                {
                    var parts = Name.Split(".", System.StringSplitOptions.RemoveEmptyEntries).ToList();

                    var last = parts.Last();
                    if (System.IO.Path.GetExtension(Name) == "."+last)
                    {
                        return parts.ElementAt(parts.Count - 2) + "." + last;
                    }
                    else
                    {
                        return last;
                    }
                }
                return "Resources";
            }
        }

        public string Name { get; set; }

        public string GetFullPath()
        {
            if (Display == "Resources")
                return "Resources";

            if (Parent == default)
                return Name;

            return Parent.GetFullPath() + "." + Name;
        }

        public DungeonEngineResourceType Type { get; set; }

        [BsonIgnore]
        public DungeonEngineResourcesGraph Parent { get; set; }

        public void Load()
        {
            if (this.Type != DungeonEngineResourceType.Folder && this.Display != "Resources")
                return;

            foreach (var node in Nodes)
            {
                node.Parent = this;
                node.Load();
            }
        }

        public ObservableCollection<DungeonEngineResourcesGraph> Nodes { get; set; } = new ObservableCollection<DungeonEngineResourcesGraph>();
    }
}
