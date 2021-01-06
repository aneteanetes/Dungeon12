﻿using Dungeon.Engine.Editable.TileMap;
using Dungeon.Engine.Forms;
using Dungeon.Engine.Projects;
using Dungeon.Utils;
using LiteDB;
using System.Collections.ObjectModel;

namespace Dungeon.Engine.Editable.Structures
{
    public class StructureTilemap : StructureObject
    {
        public override StructureObjectType StructureType => StructureObjectType.TileMap;

        public StructureTilemap()
        {
            this.BindEmbeddedIcon("DataViewHH_16x");
            if (Mode == default)
                Mode = TilemapMode.Enum;
        }

        [Visible]
        [Title("Редактировать")]
        public void Edit()
        {
            new TileEditorForm(this).Show();
        }

        [Title("Высота")]
        public int Height { get; set; }

        [Title("Ширина")]
        public int Width { get; set; }

        [Title("Размер ячейки")]
        public int CellSize { get; set; } = 32;

        [Hidden]
        public string CompiledImagePath { get; set; }

        [Title("Режим")]
        [BsonIgnore]
        public ObservableCollection<TilemapMode> Mode { get; set; } 

        [Hidden]
        [BsonIgnore]
        public ObservableCollection<TilemapLayer> Layers { get; set; } = new ObservableCollection<TilemapLayer>();

        [Hidden]
        [BsonIgnore]
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