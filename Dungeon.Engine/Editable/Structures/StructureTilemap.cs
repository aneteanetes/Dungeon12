using Dungeon.Engine.Editable.ObjectTreeList;
using Dungeon.Engine.Editable.PropertyTable;
using Dungeon.Engine.Editable.TileMap;
using Dungeon.Engine.Forms;
using Dungeon.Engine.Projects;
using Dungeon.Utils;
using System.Collections.ObjectModel;
using System.Linq;

namespace Dungeon.Engine.Editable.Structures
{
    public class StructureTilemap : StructureObject
    {
        public override StructureObjectType StructureType => StructureObjectType.TileMap;

        public StructureTilemap()
        {
            this.BindEmbeddedIcon("DataViewHH_16x");
        }

        [Visible]
        [Title("Редактировать")]
        [PropertyFilter("Settings")]
        public void Edit()
        {

        }

        public int Height { get; set; }

        public int Width { get; set; }

        public int CellSize { get; set; } = 32;

        public string CompiledImage { get; set; }

        [Hidden]
        public ObservableCollection<TilemapLayer> Layers { get; set; } = new ObservableCollection<TilemapLayer>();

        [Hidden]
        public ObservableCollection<TilemapSourceImage> Sources { get; set; } = new ObservableCollection<TilemapSourceImage>();

        public override void Remove()
        {
            App.Container.Resolve<EngineProject>().Scenes.ForEach(s =>
            {
                s.StructObjects.Remove(this);
            });
        }

        public override void CopyInParent()
        {
            Message.Show("Tilemap cannot be copied!");
        }
    }
}