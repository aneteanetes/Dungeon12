using Dungeon;
using Dungeon.Control;
using Dungeon.Control.Keys;
using Dungeon.Control.Pointer;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.Tiled;
using Dungeon.View.Interfaces;
using SidusXII.SceneObjects.GUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SidusXII.SceneObjects.Main.Map
{
    public class MapContainer : EmptySceneControl
    {
        public const double TileSize = 205;

        public override bool CacheAvailable => false;

        public int YScroll { get; set; } = 5;

        public MapContainer()
        {
            Image = "GUI/Planes/maphd.png".AsmImg();
            Width = 1234;
            Height = 710;

            this.AddChild(new DarkRectangle()
            {
                Width = 1210,
                Height = 680,
                Opacity = 1,
                Left = 15,
                Top = 15
            });

            //AddChildCenter(new DarkRectangle
            //{
            //    Fill = true,
            //    Width = 750,
            //    Height = 650,
            //    Left = 58.5,
            //    Top = 42,
            //    Opacity = 0.7,
            //    Depth = 2
            //}, true, false);

            RecalculateMap();
        }

        protected override ControlEventType[] Handles => new ControlEventType[]{
            ControlEventType.MouseWheel,
             ControlEventType.Key
            };

        public override bool AllKeysHandle => true;

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if(key== Key.Q)
            {
                Columns[0].ForEach(x => x.Visible = true);
            }

            if(key== Key.W)
            {
                Rows[3].ForEach(x => x.Visible = true);
            }

            base.KeyDown(key, modifier, hold);
        }

        public override void MouseWheel(MouseWheelEnum mouseWheelEnum)
        {
            if (mouseWheelEnum == MouseWheelEnum.Down)
            {
                YScroll++;
            }
            else
            {
                YScroll--;
            }

            base.MouseWheel(mouseWheelEnum);
        }

        List<List<ImageTile>> Rows = new List<List<ImageTile>>();
        List<List<ImageTile>> Columns = new List<List<ImageTile>>();

        private void RecalculateMap()
        {
            var w = 167;///3;
            var h = 192;///3;

            var tiled = TiledMap.Load("Maps/faithisland.tmx".AsmRes());

            var examplelayer = tiled.Layers.FirstOrDefault();

            var tileCount = examplelayer.Tiles.Count;

            var layerWidth = examplelayer.width;
            var layerHeight = examplelayer.height;

            var coefficient = 0;

            var x = 0;
            var y = YScroll;

            bool odd = false;

            //11 rows

            var rows = 11;
            var row = 0;

            List<ImageTile> rowTiles = new List<ImageTile>();

            Columns.AddRange(Enumerable.Range(0, layerWidth).Select(x => new List<ImageTile>()));
            Rows.AddRange(Enumerable.Range(0, layerHeight).Select(x => new List<ImageTile>()));

            for (int i = 0; i < tileCount; i++)
            {
                var imgtile = new ImageTile()
                {
                    Width = TileSize,
                    Height = TileSize,
                    Left = x + 50,
                    Top = y + 50,
                    DrawOutOfSight = true,
                    Scale = .4
                };

                var tiles = tiled.Layers.Select(x => x.Tiles[i]);
                foreach (var tile in tiles)
                {
                    if (tile.FileName.IsNotEmpty())
                    {
                        //img.Image = $"Tiles/{tile.FileName}".AsmImg();
                        var img = new ImageObject($"Tiles/{tile.FileName}".AsmImg());

                        if (tile.FlippedHorizontally && tile.FlippedVertically)
                        {
                            img.Flip = Dungeon.View.Enums.FlipStrategy.Both;
                        }
                        else if (tile.FlippedHorizontally)
                        {
                            img.Flip = Dungeon.View.Enums.FlipStrategy.Horizontally;
                        }
                        else if (tile.FlippedVertically)
                        {
                            img.Flip = Dungeon.View.Enums.FlipStrategy.Vertically;
                        }

                        imgtile.AddTile(img);
                    }
                }

                this.AddChild(imgtile);

                Rows[row].Add(imgtile);
                Columns[x/w].Add(imgtile);

                x += w + coefficient;

                if (y / h == layerHeight)
                {
                    y = 0;
                }

                if (x / w == layerWidth)
                {
                    odd = !odd;
                    x = 0;
                    y += h;
                    y -= 46;
                    if (odd)
                    {
                        x = w / 2;
                    }
                    row++;
                }
            }
        }
    }

    public class ImageTile : EmptySceneControl
    {
        private class BatchTile : EmptySceneControl
        {
            private ImageTile imageTile;
            public BatchTile(ImageTile imageTile)
            {
                this.imageTile = imageTile;
            }

            public override void Focus()
            {
                imageTile.Focus(true);
            }

            public override void Unfocus()
            {
                imageTile.Unfocus(true);
            }
        }

        ImageObject selector;

        BatchTile Batch;

        ImageObject Fog;

        bool IsNotInvestigated
        {
            get => !Fog.Visible;
            set => Fog.Visible = value;
        }

        public ImageTile()
        {
            Batch = new BatchTile(this)
            {
                Width = MapContainer.TileSize,
                Height = MapContainer.TileSize,
                DrawOutOfSight = true,
                IsBatch = true,
                PerPixelCollision = true,
            };
            this.AddChild(Batch);

            this.selector = new ImageObject("GUI/Parts/tileselector.png".AsmImg())
            {
                Visible = false,
                Width = MapContainer.TileSize,
                Height = MapContainer.TileSize,
                CacheAvailable = true
            };
            this.AddChild(selector);


            Fog = new ImageObject("GUI/Parts/fogofwar.png".AsmImg())
            {
                Visible = false
            };
            this.AddChild(Fog);
        }

        private ImageObject tile;

        public void AddTile(ImageObject imageObject)
        {
            tile = imageObject;
            imageObject.Width = MapContainer.TileSize;
            imageObject.Height = MapContainer.TileSize;
            Batch.AddChild(imageObject);
        }

        public void Focus(bool fromDepth)
        {
            if (!Fog.Visible)
                selector.Visible = true;
        }

        public void Unfocus(bool fromDepth)
        {
            if (!Fog.Visible)
                selector.Visible = false;
        }

        public override void Click(PointerArgs args)
        {
            this.Layer.AddObject(new PopupString(tile.Image.AsDrawText(), this.ComputedPosition.Pos,speed:0.5)
            {
                Time=TimeSpan.FromSeconds(0.7),                
            });
        }
    }
}