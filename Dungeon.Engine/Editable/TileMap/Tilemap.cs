using Dungeon.Drawing.SceneObjects;
using Dungeon.Engine.Projects;
using Dungeon.Utils;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Dungeon.Engine.Editable.TileMap
{
    public class Tilemap
    {
        public string Name { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public int CellSize { get; set; } = 32;

        [Hidden]
        public ObservableCollection<TilemapLayer> Layers { get; set; } = new ObservableCollection<TilemapLayer>();

        [Hidden]
        public ObservableCollection<TilemapSourceImage> Sources { get; set; } = new ObservableCollection<TilemapSourceImage>();
    }

    public class TilemapSourceImage
    {
        public string Name { get; set; }

        public byte[] Data { get; set; }

        public byte[] GetData(int cellWidth,int cellHeight, bool forceRefresh=false)
        {
            if (Data == default || forceRefresh)
            {
                var sceneObject = new ImageControl(Name);                
                var size = DungeonGlobal.DrawClient.MeasureImage(Name);
                sceneObject.Width = size.X;
                sceneObject.Height = size.Y;

                for (int i = 0; i < size.Y / cellHeight; i++)//for each line
                {
                    sceneObject.AddChild(new DarkRectangle()
                    {
                        Top = i * cellHeight,
                        Left = 0,
                        Width = size.X,
                        Height = 1,
                        Opacity = 1
                    });
                }

                for (int i = 0; i < size.X / cellWidth; i++)//for each line
                {
                    sceneObject.AddChild(new DarkRectangle()
                    {
                        Top = 0,
                        Left = i * cellWidth,
                        Width = 1,
                        Height = size.Y,
                        Opacity = 1
                    });
                }

                var projPath = App.Container.Resolve<EngineProject>().Path;
                var tilesetsPath = Path.Combine(projPath, "Tilesets");
                if (!Directory.Exists(tilesetsPath))
                    Directory.CreateDirectory(tilesetsPath);

                DungeonGlobal.DrawClient.SaveObject(sceneObject, Path.Combine(tilesetsPath, Name));

                Data = File.ReadAllBytes(Name);
            }

            return Data;
        }
    }
}