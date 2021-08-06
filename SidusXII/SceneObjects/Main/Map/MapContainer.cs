using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.Tiled;
using System.Collections.Generic;
using System.Linq;

namespace SidusXII.SceneObjects.Main.Map
{
    public class MapContainer : EmptySceneControl
    {
        public const double TileSize = 205;

        public override bool CacheAvailable => false;

        public MapContainer()
        {
            Image = "GUI/Planes/maphd.png".AsmImg();
            Width = 1234;
            Height = 710;

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

            //this.Scale = .4;

            var w = 167;///3;
            var h = 192;///3;

            var tiled = TiledMap.Load("Maps/faithisland.tmx".AsmRes());

            var examplelayer = tiled.Layers.FirstOrDefault();

            var tileCount = examplelayer.Tiles.Count;

            var layerWidth = examplelayer.width;
            var layerHeight = examplelayer.height;

            var coefficient = 0;

            var x = 0;
            var y = 0;

            bool odd = false;

            for (int i = 0; i < tileCount; i++)
            {
                var img = new ImageTile()
                {
                    Width = TileSize,
                    Height = TileSize,
                    Left = x ,
                    Top = y,
                    DrawOutOfSight = true
                };

                var tiles = tiled.Layers.Select(x => x.Tiles[i]);
                foreach (var tile in tiles)
                {
                    if (tile.FileName.IsNotEmpty())
                    {
                        //img.Image = $"Tiles/{tile.FileName}".AsmImg();
                        var imgtile = new ImageObject($"Tiles/{tile.FileName}".AsmImg());

                        if (tile.FlippedHorizontally && tile.FlippedVertically)
                        {
                            imgtile.Flip = Dungeon.View.Enums.FlipStrategy.Both;
                        }
                        else if (tile.FlippedHorizontally)
                        {
                            imgtile.Flip = Dungeon.View.Enums.FlipStrategy.Horizontally;
                        }
                        else if (tile.FlippedVertically)
                        {
                            imgtile.Flip = Dungeon.View.Enums.FlipStrategy.Vertically;
                        }

                        img.AddTile(imgtile);
                    }
                }

                this.AddChild(img);

                break;

                x += w+ coefficient;

                if (y / h == layerHeight)
                    y = 0;

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
        }

        public void AddTile(ImageObject imageObject)
        {
            imageObject.Width = MapContainer.TileSize;
            imageObject.Height = MapContainer.TileSize;
            Batch.AddChild(imageObject);
        }

        public void Focus(bool fromDepth)
        {
            selector.Visible = true;
        }

        public void Unfocus(bool fromDepth)
        {
            selector.Visible = false;
        }
    }
}