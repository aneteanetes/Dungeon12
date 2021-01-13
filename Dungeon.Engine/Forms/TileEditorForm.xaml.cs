using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Engine.Controls;
using Dungeon.Engine.Editable.Structures;
using Dungeon.Engine.Editable.TileMap;
using Dungeon.Engine.Host;
using Dungeon.Engine.Projects;
using Dungeon.SceneObjects;
using Dungeon.Scenes.Manager;
using Dungeon.View.Interfaces;
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
        private StructureTilemap structureTilemap;

        public TileEditorForm()
        {
            InitializeComponent();

            XnaHost.MouseDown += HostPressed;
            XnaHost.MouseUp += HostRelease;
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

        private void InitStructure(StructureTilemap structure)
        {
            if (structure == default)
                return;

            this.LayerCollection.CollectionName.Content = "Слои";
            this.LayerCollection.Init<Editable.TileMap.TilemapLayer>(new EngineCollectionEditorSettings<Editable.TileMap.TilemapLayer>(structure.Layers, this.OnLayerSelect, this.OnLayerRemove, this.OnLayerAdd));

            this.ImageCollection.CollectionName.Content = "Тайлсеты";
            this.ImageCollection.Init<Editable.TileMap.TilemapSourceImage>(new EngineCollectionEditorSettings<TilemapSourceImage>(structure.Sources, this.OnImageSourceSelect, this.OnImageSourceRemove, this.OnImageSourceAdd));

            if (structure.Layers.Count == 0)
            {
                structureTilemap.Unlock(nameof(StructureTilemap.Width));
                structureTilemap.Unlock(nameof(StructureTilemap.Height));
            }

            XnaHost.Width = structure.Width * structure.CellWidth;
            XnaHost.Height = structure.Height * structure.CellHeight;

            TileMapSize.Content = $"{XnaHost.Width}x{XnaHost.Height}";
        }

        public new void Show(StructureTilemap structure)
        {
            Project = App.Container.Resolve<EngineProject>();
            this.structureTilemap = structure;

            if (structureTilemap.Height <= 0 || structureTilemap.Width <= 0)
            {
                Message.Show("Must set map height & width!");
                return;
            }

            InitStructure(structure);
            if (XnaHost.IsLoaded)
            {
                ResetScene();
            }

            base.Show();
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

#warning host moving
        private void HostMoving(object sender, MouseEventArgs e)
        {
            if (draw && SelectedLayer != default && !SelectedLayer.BorderMode)
            {
                var mPos = Mouse.GetPosition(XnaHost);
                var pos = new Point(Math.Floor(mPos.X / 32), Math.Floor(mPos.Y / 32));
                var offsetX = (int)tilePos.X * structureTilemap.CellWidth;
                var offsetY = (int)tilePos.Y * structureTilemap.CellHeight;

                var existedTile = SelectedLayer.SceneObject.Children.FirstOrDefault(x => x.Left == pos.X && x.Top == pos.Y);
                var tileSource = new Types.Rectangle()
                {
                    Width = structureTilemap.CellWidth,
                    Height = structureTilemap.CellHeight,
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
                    SelectedLayer.SceneObject.AddChild(new TilemapCell(tile, cellSize, SelectedLayer));
                    SelectedLayer.SceneObject.Expired = true;
                }
            }
        }

        private void ResetScene()
        {
            var width = XnaHost.Width;
            var height = XnaHost.Height;

            SceneManager.Change<EasyScene>();
            var layer = SceneManager.Current.AddLayer("Main");
            layer.Width = 1920;
            layer.Height = 1080;
            layer.AddObject(new EngineTileMap(structureTilemap.Width, structureTilemap.Height, structureTilemap.CellWidth, structureTilemap.CellHeight));
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

            //if (cellSize != structureTilemap.CellSize)
            //{
            //    cellSize = structureTilemap.CellSize;
            //    if (TiledMapImageCells.Source != default)
            //    {
            //        if (ImageCollection.Selected != default)
            //        {
            //            TiledMapImageCells.Source = ToImage(ImageCollection.Selected.As<TilemapSourceImage>().GetData(structureTilemap.CellSize));
            //        }
            //    }
            //}
            ResetScene();
            PublishLayers();

            TileMapSize.Content = $"{structureTilemap.Width}x{structureTilemap.Height}";
            this.LayerCollection.CollectionView.Items.Refresh();
            this.ImageCollection.CollectionView.Items.Refresh();
        }

        private void PublishLayers()
        {
            foreach (var layer in structureTilemap.Layers)
            {
                if (layer.SceneObject == default)
                {
                    layer.SceneObject = new TilemapLayer(layer, structureTilemap.Width,structureTilemap.Height);
                    SceneManager.Current.AddObject(layer.SceneObject);
                    foreach (var tile in layer.Tiles)
                    {
                        layer.SceneObject.AddChild(new TilemapCell(tile, cellSize, layer));
                    }
                }
            }
        }

        private Editable.TileMap.TilemapLayer SelectedLayer => LayerCollection.Selected.As<Editable.TileMap.TilemapLayer>();

        private TilemapSourceImage SelectedSourceImage => ImageCollection.Selected.As<TilemapSourceImage>();

        private void OnImageSourceSelect(TilemapSourceImage source)
        {
            TiledMapImageCells.Source = ToImage(source.GetData(structureTilemap.CellWidth,structureTilemap.CellHeight));
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
                structureTilemap.Sources.Add(new TilemapSourceImage() { Name = input.Text });
            }
        }

        private void OnImageSourceRemove(TilemapSourceImage source)
        {
            structureTilemap.Sources.Remove(source);
        }

        private void OnLayerSelect(Editable.TileMap.TilemapLayer layer)
        {
            DisableBorderModeOnPreviouslyLayer?.Invoke();
            PropGrid.Fill(new Events.PropGridFillEvent(layer));
            if (layer.SceneObject == default)
            {
                layer.SceneObject = new TilemapLayer(layer, structureTilemap.Width, structureTilemap.Height);
                SceneManager.Current.AddObject(layer.SceneObject);
            }
        }

        private void OnLayerAdd(Editable.TileMap.TilemapLayer layer)
        {
            layer = new Editable.TileMap.TilemapLayer();
            structureTilemap.Layers.Add(layer);
            structureTilemap.Lock(nameof(StructureTilemap.Width));
            structureTilemap.Lock(nameof(StructureTilemap.Height));
            OnLayerSelect(layer);
        }

        private void OnLayerRemove(Editable.TileMap.TilemapLayer layer)
        {
            structureTilemap.Layers.Remove(layer);
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
            this.Hide();
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
                    this.tilePos = new Point(Math.Floor(pos.X / structureTilemap.CellWidth), Math.Floor(pos.Y / structureTilemap.CellHeight));
                    TilePeviewImage.Source = new CroppedBitmap(new BitmapImage(new Uri(ImageCollection.Selected.As<TilemapSourceImage>().Name)),
                        new Int32Rect((int)this.tilePos.X * structureTilemap.CellWidth, (int)this.tilePos.Y * structureTilemap.CellHeight, structureTilemap.CellWidth, structureTilemap.CellHeight));
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
            public override bool IsBatch => true;

            public override bool CacheAvailable => false;

            private double w, h;

            public TilemapLayer(Editable.TileMap.TilemapLayer component, double width, double height) : base(component, true)
            {
                w = width;
                h = height;
            }

            public override double Width => w;

            public override double Height => h;
        }

        public class EngineTileMap : EmptySceneObject
        {
            public override bool Interface => true;

            public override bool IsBatch => true;

            public EngineTileMap(int width, int height, int cellWidth, int cellHeight)
            {
                this.Width = width * cellWidth;
                this.Height = height * cellHeight;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        this.AddChild(new ImageControl("Images.isometric_cell.png".AsmRes())
                        {
                            Top = y * (y % 2 == 66 ? 0 : 33),
                            Left = (x * 128) + (y % 2 == 0 ? 0 : 64),
                            Width = cellWidth,
                            Height = cellHeight,
                            Opacity = 1,
                        });
                    }
                }
            }

            public override bool Visible => ShowCells;

            private T Montserrat<T>(T drawText) where T : IDrawText
            {
                drawText.FontName = "Montserrat";
                drawText.FontAssembly = "Dungeon12";
                //drawText.FontPath = "Dungeon.Resources.Fonts.Mont.otf";

                return drawText;
            }
        }
    }
}