using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Engine.Controls;
using Dungeon.Engine.Editable.TileMap;
using Dungeon.Engine.Host;
using Dungeon.Engine.Projects;
using Dungeon.SceneObjects;
using Dungeon.Scenes.Manager;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Dungeon.Engine.Forms
{
    public partial class TileEditorForm : Window
    {
        private DungeonEngineProject Project;
        private SceneManager SceneManager;

        public TileEditorForm(DungeonEngineProject dungeonEngineProject)
        {
            Project = dungeonEngineProject;

            InitializeComponent();

            this.MapCollection.CollectionName.Content = "Карты";
            this.MapCollection.Init<DungeonEngineTilemap>(new EngineCollectionEditorSettings<DungeonEngineTilemap>(Project.Maps,OnMapSelect,OnMapRemove, OnMapAdd,"Карты"));
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
            if (draw && SelectedLayer != default)
            {
                var mPos = Mouse.GetPosition(XnaHost);
                var pos = new Point(Math.Floor(mPos.X / 32), Math.Floor(mPos.Y / 32));

                var existedTile = SceneManager.Current.Objects.FirstOrDefault(x => x.Left == pos.X && x.Top == pos.Y && x.IsNot<EngineTileMap>());
                if (existedTile != default)
                {
                    SelectedLayer.Tiles.Remove(existedTile.As<TilemapCell>().Component);
                    existedTile.Destroy();
                }

                if (!clearing && tilePos != default)
                {
                    var cellSize = SelectedTiliemap.CellSize;
                    var tile = new DungeonEngineTilemapTile()
                    {
                        SourceImage = SelectedSourceImage.Name,
                        OffsetX = (int)tilePos.X* cellSize,
                        OffsetY = (int)tilePos.Y* cellSize,
                        XPos = (int)pos.X,
                        YPos = (int)pos.Y
                    };
                    SelectedLayer.Tiles.Add(tile);
                    SceneManager.Current.AddObject(new TilemapCell(tile, cellSize));
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
                        TiledMapImageCells.Source = ToImage(ImageCollection.Selected.As<DungeonEngineTilemapSourceImage>().GetData(SelectedTiliemap.CellSize));
                    }
                }
            }

            if (SelectedTiliemap.Width != XnaHost.Width || SelectedTiliemap.Height != XnaHost.Height)
            {
                XnaHost.Width = SelectedTiliemap.Width;
                XnaHost.Height = SelectedTiliemap.Height;
            }
            ResetScene();

            TileMapSize.Content = $"{SelectedTiliemap.Width}x{SelectedTiliemap.Height}";
            this.MapCollection.CollectionView.Items.Refresh();
            this.LayerCollection.CollectionView.Items.Refresh();
            this.ImageCollection.CollectionView.Items.Refresh();
        }

        private void OnMapSelect(DungeonEngineTilemap map)
        {
            cellSize = map.CellSize;
            PropGrid.FillPropGrid(new Events.PropGridFillEvent(map));
            ResetScene();
            this.LayerCollection.Init<DungeonEngineTilemapLayer>(new EngineCollectionEditorSettings<DungeonEngineTilemapLayer>(map.Layers, OnLayerSelect, OnLayerRemove, OnLayerAdd));
            this.ImageCollection.Init<DungeonEngineTilemapSourceImage>(new EngineCollectionEditorSettings<DungeonEngineTilemapSourceImage>(map.Sources, OnImageSourceSelect, add: OnImageSourceAdd));
        }

        private void OnMapRemove(DungeonEngineTilemap map)
        {
            Project.Maps.Remove(map);
        }

        private void OnMapAdd(DungeonEngineTilemap map)
        {
            map = new DungeonEngineTilemap();
            Project.Maps.Add(map);
            OnMapSelect(map);
        }

        private DungeonEngineTilemap SelectedTiliemap => MapCollection.Selected.As<DungeonEngineTilemap>();

        private DungeonEngineTilemapLayer SelectedLayer => LayerCollection.Selected.As<DungeonEngineTilemapLayer>();

        private DungeonEngineTilemapSourceImage SelectedSourceImage => ImageCollection.Selected.As<DungeonEngineTilemapSourceImage>();

        private void OnImageSourceSelect(DungeonEngineTilemapSourceImage source)
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

        private void OnImageSourceAdd(DungeonEngineTilemapSourceImage source)
        {
            var input = new AddNamedForm("Добавить тайлсет", true);
            input.ShowDialog();
            if (!string.IsNullOrWhiteSpace(input.Text))
            {
                SelectedTiliemap.Sources.Add(new DungeonEngineTilemapSourceImage() { Name = input.Text });
            }
        }

        private void OnLayerSelect(DungeonEngineTilemapLayer layer)
        {
            PropGrid.FillPropGrid(new Events.PropGridFillEvent(layer));
        }

        private void OnLayerAdd(DungeonEngineTilemapLayer layer)
        {
            layer = new DungeonEngineTilemapLayer();
            SelectedTiliemap.Layers.Add(layer);
            OnLayerSelect(layer);
        }

        private void OnLayerRemove(DungeonEngineTilemapLayer layer)
        {
            SelectedTiliemap.Layers.Remove(layer);
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

        static bool editMode = false;

        private void EditMode(object sender, RoutedEventArgs e)
        {
            ModeName.Content = "[Редактирование]";
            editMode = false;
        }

        private void BorderMode(object sender, RoutedEventArgs e)
        {
            ModeName.Content = "[Границы]";
            editMode = true;
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
                    TilePeviewImage.Source = new CroppedBitmap(new BitmapImage(new Uri(ImageCollection.Selected.As<DungeonEngineTilemapSourceImage>().Name)),
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



        private class TilemapCell : SceneObject<DungeonEngineTilemapTile>
        {
            public TilemapCell(DungeonEngineTilemapTile component, int fieldCellSize) : base(component, false)
            {
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

                this.AddChild(new BorderInfo(component, DungeonEgineTilemapTileBoundsType.Left)
                {
                    Top = 13
                });

                this.AddChild(new BorderInfo(component, DungeonEgineTilemapTileBoundsType.Full)
                {
                    Top = 13,
                    Left = 13
                });

                this.AddChild(new BorderInfo(component, DungeonEgineTilemapTileBoundsType.Right)
                {
                    Left = 22
                });

                this.AddChild(new BorderInfo(component, DungeonEgineTilemapTileBoundsType.Bottom)
                {
                    Top = 22
                });
            }

            private class BorderInfo : HandleSceneControl<DungeonEngineTilemapTile>
            {
                private DungeonEgineTilemapTileBoundsType boundsType;

                public BorderInfo(DungeonEngineTilemapTile tile, DungeonEgineTilemapTileBoundsType boundsType) : base(tile, false)
                {
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

                public override bool Visible => editMode;

                public override void Click(PointerArgs args)
                {
                    Moveable = !Moveable;
                    Component.SetPropertyExpr<bool>(boundsType.ToString(), Moveable);
                }
            }
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
        }
    }
}
