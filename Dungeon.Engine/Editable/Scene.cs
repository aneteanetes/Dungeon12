using Dungeon.Data;
using Dungeon.Engine.Editable;
using Dungeon.Engine.Editable.ObjectTreeList;
using Dungeon.Engine.Events;
using Dungeon.Utils;
using LiteDB;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Dungeon.Engine.Projects
{
    public class Scene : Persist, IEditable
    {
        public string Name { get; set; }

        [BsonIgnore]
        public string Text => Name;

        [Hidden]
        public ObservableCollection<ObjectTreeListItem> StructObjects { get; set; } = new ObservableCollection<ObjectTreeListItem>();

        public bool StartScene { get; set; }

        [Display(Name="Ширина",Description ="Ширина сцены, может быть больше экрана, но по умолчанию равна ей")]
        public int Width { get; set; }

        [Display(Name = "Высота", Description = "Высота сцены, может быть больше экрана, но по умолчанию равна ей")]
        public int Height { get; set; }

        public void Commit()
        {
            DungeonGlobal.Events.Raise(new SceneResolutionChangedEvent(this.Width, this.Height));
        }

        public void Load()
        {
            foreach (var @struct in StructObjects)
            {
                RecursiveLoad(@struct);
            }
        }

        private void RecursiveLoad(ObjectTreeListItem obj)
        {
            foreach (var child in obj.Nodes)
            {
                child.Parent = obj;
                RecursiveLoad(child);
            }
        }
    }
}
