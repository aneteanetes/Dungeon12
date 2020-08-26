using Dungeon.Data;
using Dungeon.Engine.Editable;
using Dungeon.Engine.Events;
using Dungeon.Utils;
using LiteDB;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Dungeon.Engine.Projects
{
    public class DungeonEngineScene : Persist, IEditable
    {
        public string Name { get; set; }

        [BsonIgnore]
        public string Text => Name;

        [Hidden]
        public ObservableCollection<DungeonEngineSceneObject> SceneObjects { get; set; } = new ObservableCollection<DungeonEngineSceneObject>();

        public bool StartScene { get; set; }

        [Display(Name="Ширина",Description ="Ширина сцены, может быть больше экрана, но по умолчанию равна ей")]
        public int Width { get; set; }

        [Display(Name = "Высота", Description = "Высота сцены, может быть больше экрана, но по умолчанию равна ей")]
        public int Height { get; set; }

        public void Commit()
        {
            DungeonGlobal.Events.Raise(new SceneResolutionChangedEvent(this.Width, this.Height));
        }
    }
}
