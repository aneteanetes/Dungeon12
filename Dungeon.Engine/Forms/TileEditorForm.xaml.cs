using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Engine.Controls;
using Dungeon.Engine.Editable.Structures;
using Dungeon.Engine.Editable.TileMap;
using Dungeon.Engine.Host;
using Dungeon.Engine.Projects;
using Dungeon.SceneObjects;
using Dungeon.Scenes.Manager;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Dungeon.Engine.Forms
{
    public partial class TileEditorForm : Window
    {
        private EngineProject Project;
        private SceneManager SceneManager;

        public TileEditorForm(StructureTilemap dungeonEngineProject)
        {
#warning wrong type assignment
            Project = App.Container.Resolve<EngineProject>();

            InitializeComponent();

            this.MapCollection.CollectionName.Content = "Карты";
            this.MapCollection.Init<Tilemap>(new EngineCollectionEditorSettings<Tilemap>(Project.Maps, OnMapSelect, OnMapRemove, OnMapAdd, "Карты"));
            this.LayerCollection.CollectionName.Content = "Слои";
            this.ImageCollection.CollectionName.Content = "Тайлсеты";

            XnaHost.MouseUp += HostRelease;
            XnaHost.MouseDown += HostPressed;
            XnaHost.MouseMove += HostMoving;
            XnaHost.Loaded += (x, y) =>
            {
                SceneManager = new SceneManager()
                {
                    DrawClient = XnaHost
                };
                DungeonGlobal.SceneManager = SceneManager;
                XnaHost.BindSceneManager(SceneManager);
                SceneManager.Start();
                ResetScene();
            };

            CompositionTarget.Rendering += (x, y) => this.FPSView.Content = $"FPS:{XnaHost.FPS}";
        }

        private void HostRelease(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && tilePos != default)
            {
                draw = false;
            }

            if (e.ChangedButton == MouseButton.Right)
            {
                draw = false;
                clearing = false;
            }
        }

        bool draw = false;
        bool clearing = false;

        private void HostPressed(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && tilePos != default)
            {
                draw = true;
            }

            if (e.ChangedButton == MouseButton.Right)
            {
                draw = true;
                clearing = true;
            }
        }

        private void HostMoving(object sender, MouseEventArgs e)
        {
            if (draw && SelectedLayer != default && !SelectedLayer.BorderMode)
            {
                var mPos = Mouse.GetPosition(XnaHost);
                var pos = new Point(Math.Floor(mPos.X / 32), Math.Floor(mPos.Y / 32));
                var cellSize = SelectedTiliemap.CellSize;
                var offsetX = (int)tilePos.X * cellSize;
                var offsetY = (int)tilePos.Y * cellSize;

                var existedTile = SelectedLayer.SceneObject.Children.FirstOrDefault(x => x.Left == pos.X && x.Top == pos.Y);
                var tileSource = new Types.Rectangle()
                {
                    Width = cellSize,
                    Height = cellSize,
                    X = offsetX,
                    Y = offsetY
                };

                if (existedTile != default && existedTile.Image != SelectedSourceImage.Name && !existedTile.ImageRegion.Equals(tileSource))
                {
                    SelectedLayer.Tiles.Remove(existedTile.As<TilemapCell>().Component);
                    existedTile.Destroy();
                    SelectedLayer.SceneObject.Expired = true;
                }

                if (!clearing && tilePos != default)
                {
                    var tile = new TilemapTile()
                    {
                        SourceImage = SelectedSourceImage.Name,
                        OffsetX = offsetX,
                        OffsetY = offsetY,
                        XPos = (int)pos.X,
                        YPos = (int)pos.Y
                    };
                    SelectedLayer.Tiles.Add(tile);
                    SelectedLayer.SceneObject.AddChild(new TilemapCell(tile, cellSize,SelectedLayer));
                    SelectedLayer.SceneObject.Expired = true;
                }
            }
        }

        private void ResetScene()
        {
            var cellSize = SelectedTiliemap?.CellSize ?? 64;
            var width = XnaHost.Width;
            var height = XnaHost.Height;

            SceneManager.Change<EasyScene>();
            SceneManager.Current.AddObject(new EngineTileMap((int)width, (int)height, cellSize));
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.S && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                Save();

            base.OnKeyDown(e);
        }

        int cellSize = 64;

        private void Save()
        {
            Project.Save();
            PropGrid.Save();

            if (cellSize != SelectedTiliemap.CellSize)
            {
                cellSize = SelectedTiliemap.CellSize;
                if (TiledMapImageCells.Source != default)
                {
                    if (ImageCollection.Selected != default)
                    {
                        TiledMapImageCells.Source = ToImage(ImageCollection.Selected.As<TilemapSourceImage>().GetData(SelectedTiliemap.CellSize));
                    }
                }
            }

            if (SelectedTiliemap.Width != XnaHost.Width || SelectedTiliemap.Height != XnaHost.Height)
            {
                XnaHost.Width = SelectedTiliemap.Width;
                XnaHost.Height = SelectedTiliemap.Height;
            }
            ResetScene();
            PublishLayers(SelectedTiliemap);

            TileMapSize.Content = $"{SelectedTiliemap.Width}x{SelectedTiliemap.Height}";
            this.MapCollection.CollectionView.Items.Refresh();
            this.LayerCollection.CollectionView.Items.Refresh();
            this.ImageCollection.CollectionView.Items.Refresh();
        }

        private void OnMapSelect(Tilemap map)
        {
            XnaHost.Width = map.Width;
            XnaHost.Height = map.Height;
            cellSize = map.CellSize;
            PropGrid.Fill(new Events.PropGridFillEvent(map));
            ResetScene();
            this.LayerCollection.Init<Editable.TileMap.TilemapLayer>(new EngineCollectionEditorSettings<Editable.TileMap.TilemapLayer>(map.Layers, this.OnLayerSelect, this.OnLayerRemove, this.OnLayerAdd));
            this.ImageCollection.Init<TilemapSourceImage>(new EngineCollectionEditorSettings<TilemapSourceImage>(map.Sources, OnImageSourceSelect, add: OnImageSourceAdd));
            PublishLayers(map);
        }

        private void PublishLayers(Tilemap map)
        {
            foreach (var layer in map.Layers)
            {
                if (layer.SceneObject == default)
                {
                    layer.SceneObject = new TilemapLayer(layer, SelectedTiliemap);
                    SceneManager.Current.AddObject(layer.SceneObject);
                    foreach (var tile in layer.Tiles)
                    {
                        layer.SceneObject.AddChild(new TilemapCell(tile, cellSize, layer));
                    }
                }
            }
        }

        private void OnMapRemove(Tilemap map)
        {
            Project.Maps.Remove(map);
        }

        private void OnMapAdd(Tilemap map)
        {
            map = new Tilemap();
            Project.Maps.Add(map);
            OnMapSelect(map);
        }

        private Tilemap SelectedTiliemap => MapCollection.Selected.As<Tilemap>();

        private Editable.TileMap.TilemapLayer SelectedLayer => LayerCollection.Selected.As<Editable.TileMap.TilemapLayer>();

        private TilemapSourceImage SelectedSourceImage => ImageCollection.Selected.As<TilemapSourceImage>();

        private void OnImageSourceSelect(TilemapSourceImage source)
        {
            TiledMapImageCells.Source = ToImage(source.GetData(SelectedTiliemap.CellSize));
        }

        public BitmapImage ToImage(byte[] array)
        {
            using var ms = new System.IO.MemoryStream(array);
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad; // here
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        private void OnImageSourceAdd(TilemapSourceImage source)
        {
            var input = new AddNamedForm("Добавить тайлсет", true);
            input.ShowDialog();
            if (!string.IsNullOrWhiteSpace(input.Text))
            {
                SelectedTiliemap.Sources.Add(new TilemapSourceImage() { Name = input.Text });
            }
        }

        private void OnLayerSelect(Editable.TileMap.TilemapLayer layer)
        {
            DisableBorderModeOnPreviouslyLayer?.Invoke();
            PropGrid.Fill(new Events.PropGridFillEvent(layer));
            if (layer.SceneObject == default)
            {
                layer.SceneObject = new TilemapLayer(layer, SelectedTiliemap);
                SceneManager.Current.AddObject(layer.SceneObject);
            }
        }

        private void OnLayerAdd(Editable.TileMap.TilemapLayer layer)
        {
            layer = new Editable.TileMap.TilemapLayer();
            SelectedTiliemap.Layers.Add(layer);
            OnLayerSelect(layer);
        }

        private void OnLayerRemove(Editable.TileMap.TilemapLayer layer)
        {
            SelectedTiliemap.Layers.Remove(layer);
            layer.SceneObject?.Destroy?.Invoke();
        }

        private void OnMaximizeRestoreButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void EditMode(object sender, RoutedEventArgs e)
        {
            if (SelectedLayer == default)
                return;

            ModeName.Content = "[Редактирование]";
            SelectedLayer.BorderMode = false;
        }

        private Action DisableBorderModeOnPreviouslyLayer;
        private void BorderMode(object sender, RoutedEventArgs e)
        {
            if (SelectedLayer == default)
                return;

            ModeName.Content = "[Границы]";
            SelectedLayer.BorderMode = true;
            var link = SelectedLayer;
            DisableBorderModeOnPreviouslyLayer = () => link.BorderMode = false;
        }

        Point tilePos;

        private void TiledMapImageCellsClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (ImageCollection.Selected != default)
                {
                    var pos = Mouse.GetPosition(TiledMapImageCells);
                    this.tilePos = new Point(Math.Floor(pos.X / 32), Math.Floor(pos.Y / 32));
                    TilePeviewImage.Source = new CroppedBitmap(new BitmapImage(new Uri(ImageCollection.Selected.As<TilemapSourceImage>().Name)),
                        new Int32Rect((int)this.tilePos.X * cellSize, (int)this.tilePos.Y * cellSize, SelectedTiliemap.CellSize, SelectedTiliemap.CellSize));
                    TilePreview.Visibility = Visibility.Visible;
                }
            }
        }

        private void HideTilePreview(object sender, RoutedEventArgs e)
        {
            TilePreview.Visibility = Visibility.Hidden;
            tilePos = default;
        }

        private static bool ShowCells = true;

        private void HideShowCells(object sender, RoutedEventArgs e)
        {
            ShowCells = !ShowCells;
        }

        private class TilemapCell : SceneObject<TilemapTile>
        {
            private Editable.TileMap.TilemapLayer layer;

            public TilemapCell(TilemapTile component, int fieldCellSize, Editable.TileMap.TilemapLayer layer) : base(component, false)
            {
                this.layer = layer;

                this.Width = fieldCellSize;
                this.Height = fieldCellSize;

                this.Image = component.SourceImage;
                this.ImageRegion = new Types.Rectangle()
                {
                    Width = fieldCellSize,
                    Height = fieldCellSize,
                    X = component.OffsetX,
                    Y = component.OffsetY
                };
                this.Top = component.YPos * fieldCellSize;
                this.Left = component.XPos * fieldCellSize;

                this.AddChild(new BorderInfo(component, DungeonEgineTilemapTileBoundsType.Left, layer)
                {
                    Top = 13
                });

                this.AddChild(new BorderInfo(component, DungeonEgineTilemapTileBoundsType.Full, layer)
                {
                    Top = 13,
                    Left = 13
                });

                this.AddChild(new BorderInfo(component, DungeonEgineTilemapTileBoundsType.Right, layer)
                {
                    Left = 22
                });

                this.AddChild(new BorderInfo(component, DungeonEgineTilemapTileBoundsType.Bottom, layer)
                {
                    Top = 22
                });

                this.AddChild(new BorderInfo(component, DungeonEgineTilemapTileBoundsType.Top, layer)
                {
                    Left = 13
                });
            }

            private class BorderInfo : ControlSceneObject<TilemapTile>
            {
                private DungeonEgineTilemapTileBoundsType boundsType;
                private Editable.TileMap.TilemapLayer layer;

                public BorderInfo(TilemapTile tile, DungeonEgineTilemapTileBoundsType boundsType, Editable.TileMap.TilemapLayer layer) : base(tile, false)
                {
                    this.layer = layer;
                    this.boundsType = boundsType;
                }

                bool Moveable = false;

                public override double Angle
                {
                    get
                    {
                        switch (boundsType)
                        {
                            case DungeonEgineTilemapTileBoundsType.Left: return -90;
                            case DungeonEgineTilemapTileBoundsType.Right: return 90;
                            case DungeonEgineTilemapTileBoundsType.Bottom: return 180;
                            default: return 0;
                        }
                    }
                }

                public override string Image => $"Icons\\{(boundsType == DungeonEgineTilemapTileBoundsType.Full ? "circle" : "triangle")}{(!Moveable ? "" : "black")}10.png";

                public override bool Visible => layer.BorderMode;

                public override void Click(PointerArgs args)
                {
                    Moveable = !Moveable;
                    Component.SetPropertyExpr<bool>(boundsType.ToString(), Moveable);
                }
            }
        }

        private class TilemapLayer : SceneObject<Editable.TileMap.TilemapLayer>
        {
            private readonly Tilemap map;

            public override bool IsBatch => true;

            public override bool CacheAvailable => false;

            public TilemapLayer(Editable.TileMap.TilemapLayer component, Tilemap map) : base(component, true)
            {
                this.map = map;
            }

            public override double Width => map.Width;

            public override double Height => map.Height;
        }

        private class EngineTileMap : EmptySceneObject
        {
            public override bool Interface { get => true; set => base.Interface = value; }

            public EngineTileMap(int width, int height, int cellSize)
            {
                for (int i = 0; i < height / cellSize; i++)//for each line
                {
                    this.AddChild(new DarkRectangle()
                    {
                        Top = i * cellSize,
                        Left = 0,
                        Width = width,
                        Height = 1,
                        Opacity = 1
                    });
                }

                for (int i = 0; i < width / cellSize; i++)//for each line
                {
                    this.AddChild(new DarkRectangle()
                    {
                        Top = 0,
                        Left = i * cellSize,
                        Width = 1,
                        Height = height,
                        Opacity = 1
                    });
                }
            }

            public override bool Visible => ShowCells;
        }
    }
}