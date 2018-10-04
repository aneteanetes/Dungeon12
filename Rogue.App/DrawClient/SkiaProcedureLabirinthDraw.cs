namespace Rogue.App.DrawClient
{
    using System;
    using System.Collections.Generic;
    using Rogue.Resources;
    using Rogue.View.Interfaces;
    using SkiaSharp;

    public class SkiaProcedureLabirinthDraw
    {
        private Random random;
        private IDrawSession labirinthDrawSession;
        private SKCanvas canvas;

        public SkiaProcedureLabirinthDraw(IDrawSession labirinthDrawSession, SKCanvas canvas, Random random)
        {
            this.labirinthDrawSession = labirinthDrawSession;
            this.canvas = canvas;
            this.random = random;

            this.DrawMethodMapping = new Dictionary<char, Action<float, float>>()
            {
                {'°',this.DrawFloor },
                {'#',this.DrawWall },
                {'@',this.DrawWall }
            };
        }
        
        private SKBitmap _tileset;
        private SKBitmap Tileset
        {
            get
            {
                if (_tileset == null)
                {
                    var stream = ResourceLoader.Load("Rogue.Resources.Images.Tiles.dblue.png");
                    _tileset = SKBitmap.Decode(stream);
                }

                return _tileset;
            }
        }

        private SKBitmap _itemTileset;
        private SKBitmap ItemTileset
        {
            get
            {
                if (_itemTileset == null)
                {
                    var stream = ResourceLoader.Load("Rogue.Resources.Images.Tiles.items.png");
                    _itemTileset = SKBitmap.Decode(stream);
                }

                return _itemTileset;
            }
        }

        public void Draw()
        {
            var y = labirinthDrawSession.Region.Y * 24 + 3;

            foreach (var line in labirinthDrawSession.Content)
            {
                var x = labirinthDrawSession.Region.X * 11.5625f - 3;

                foreach (var lne in line.Data)
                {
                    foreach (var @char in lne.StringData)
                    {
                        DrawMethodMapping[@char](x, y);
                        x += 24;
                    }
                }

                y += 24;
            }
        }

        private readonly Dictionary<char, Action<float, float>> DrawMethodMapping;

        private void DrawFloor(float x, float y)
        {
            var randomX = random.Next(0, 8);

            canvas.DrawBitmap(Tileset, new SKRect
            {
                Left = 24 * randomX,
                Top = 24 * 0,
                Size = new SKSize
                {
                    Height = 24,
                    Width = 24
                }
            }, new SKRect
            {
                Top = y - 24,
                Left = x,
                Size = new SKSize
                {
                    Height = 24,
                    Width = 24
                }
            });
        }

        private void DrawWall(float x, float y)
        {
            //здесь будет логика по выбору текстуры

            canvas.DrawBitmap(Tileset, new SKRect
            {
                Left = 24 * 0,
                Top = 24 * 1,
                Size = new SKSize
                {
                    Height = 24,
                    Width = 24
                }
            }, new SKRect
            {
                Top = y - 24,
                Left = x,
                Size = new SKSize
                {
                    Height = 24,
                    Width = 24
                }
            });
        }
    }
}