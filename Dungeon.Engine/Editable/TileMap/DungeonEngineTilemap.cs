using Dungeon.Drawing.SceneObjects;
using Dungeon.Utils;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Dungeon.Engine.Editable.TileMap
{
    public class DungeonEngineTilemap
    {
        public string Name { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public int CellSize { get; set; } = 32;

        [Hidden]
        public ObservableCollection<DungeonEngineTilemapLayer> Layers { get; set; } = new ObservableCollection<DungeonEngineTilemapLayer>();

        [Hidden]
        public ObservableCollection<DungeonEngineTilemapSourceImage> Sources { get; set; } = new ObservableCollection<DungeonEngineTilemapSourceImage>();
    }

    public class DungeonEngineTilemapSourceImage
    {
        public string Name { get; set; }

        public byte[] Data { get; set; }

        public byte[] GetData(int cellSize, bool forceRefresh=false)
        {
            if (Data == default || forceRefresh)
            {
                var sceneObject = new ImageControl(Name);                
                var size = DungeonGlobal.DrawClient.MeasureImage(Name);
                sceneObject.Width = size.X;
                sceneObject.Height = size.Y;

                for (int i = 0; i < size.Y / cellSize; i++)//for each line
                {
                    sceneObject.AddChild(new DarkRectangle()
                    {
                        Top = i * cellSize,
                        Left = 0,
                        Width = size.X,
                        Height = 1,
                        Opacity = 1
                    });
                }

                for (int i = 0; i < size.X / cellSize; i++)//for each line
                {
                    sceneObject.AddChild(new DarkRectangle()
                    {
                        Top = 0,
                        Left = i * cellSize,
                        Width = 1,
                        Height = size.Y,
                        Opacity = 1
                    });
                }

                DungeonGlobal.DrawClient.SaveObject(sceneObject, Name);

                Data = File.ReadAllBytes(Name);
            }

            return Data;
        }
    }
}