namespace Rogue.App.DrawClient
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Avalonia.Controls;
    using Avalonia.Media.Imaging;
    using Rogue.Resources;
    using Rogue.Scenes;
    using Rogue.View.Interfaces;
    using SkiaSharp;

    public class SkiaDrawClient : IDrawClient
    {
        private WriteableBitmap ViewportBitmap;
        private Image control;

        SKBitmap drawingBitmap;
        private SKBitmap DrawingBitmap
        {
            get
            {
                if (drawingBitmap == default)
                {
                    drawingBitmap = new SKBitmap(1157, 700, SKColorType.Bgra8888, SKAlphaType.Premul);
                }

                return this.drawingBitmap;
            }

            set
            {
                this.drawingBitmap = value;
            }
        }

        public SkiaDrawClient(WriteableBitmap viewportBitmap, Image image)
        {
            this.ViewportBitmap = viewportBitmap;
            this.control = image;
        }

        private static readonly Dictionary<string, SKBitmap> tilesetsCache = new Dictionary<string, SKBitmap>();

        private static SKBitmap TileSetByName(string tilesetName)
        {
            if (!tilesetsCache.TryGetValue(tilesetName, out var bitmap))
            {
                var stream = ResourceLoader.Load(tilesetName, tilesetName);
                bitmap = SKBitmap.Decode(stream);

                tilesetsCache.Add(tilesetName, bitmap);
            }

            return bitmap;
        }

        public void Draw(IEnumerable<IDrawSession> drawSessions)
        {
            var bitmap = DrawingBitmap;
            var canvas = new SKCanvas(DrawingBitmap);
          
            float fontSize = 20f;
            var font = SKTypeface.FromFamilyName("Lucida Console");

            var wtf = new SKColor[]
            {
                new SKColor(255, 255, 0, 255),
                new SKColor(0, 255, 0, 255),
                new SKColor(0, 0, 255, 255),
                new SKColor(0, 255, 255, 255),
            };

            float YUnit = 20f;
            float XUnit = 11.5625f;

            var blackPaint = new SKPaint { Color = new SKColor(0, 0, 0, 255) };
            //new SKPaint { Color = new SKColor(124, 57, 89, 255), IsStroke=true };

            foreach (var session in drawSessions)
            {
                if (session.Drawables != null)
                {
                    DrawTiles(canvas,session);
                }
                else
                {
                    DrawText(canvas, fontSize, font, ref YUnit, ref XUnit, blackPaint, session);
                }
            }


            canvas.Dispose();
            font.Dispose();

            this.InternalDraw();
        }

        private static void DrawTiles(SKCanvas canvas, IDrawSession session)
        {
            foreach (var drawable in session.Drawables)
            {
                var y = drawable.Region.Y * 24 + 3;
                var x = drawable.Region.X * 24 - 3;

                var tileset = TileSetByName(drawable.Tileset);

                var tileSize = new SKSize
                {
                    Height = (float)drawable.TileSetRegion.Height,
                    Width = (float)drawable.TileSetRegion.Width
                };

                var tilePos = new SKRect
                {
                    Left = drawable.TileSetRegion.X,
                    Top = drawable.TileSetRegion.Y,
                    Size = tileSize
                };

                canvas.DrawBitmap(tileset, tilePos, new SKRect
                {
                    Top = y - 24,
                    Left = x,
                    Size = new SKSize
                    {
                        Height = drawable.Region.Height,
                        Width = drawable.Region.Width
                    }
                });
            }
        }

        private static void DrawText(SKCanvas canvas, float fontSize, SKTypeface font, ref float YUnit, ref float XUnit, SKPaint blackPaint, IDrawSession session)
        {
            ClearRegion(canvas, YUnit, XUnit, blackPaint, session);

            float y = ((session.Region.Y) * YUnit) + 10;
            foreach (var line in session.Content)
            {

                float x = session.Region.X * XUnit;

                foreach (var lne in line.Data)
                {
                    foreach (var range in lne.Data)
                    {
                        var textpaint = new SKPaint
                        {
                            Typeface = font,
                            TextSize = fontSize,
                            IsAntialias = true,
                            Color = new SKColor(range.ForegroundColor.R, range.ForegroundColor.G, range.ForegroundColor.B, range.ForegroundColor.A),
                            Style = SKPaintStyle.Fill
                        };

                        foreach (var @char in range.StringData)
                        {
                            XUnit = 11.5625f;
                            YUnit = 20;
                            canvas.DrawText(@char.ToString(), x, y, textpaint);
                            canvas.DrawText(@char.ToString(), x, y, textpaint);

                            x += XUnit;
                        }
                    }
                }

                y += YUnit;

            }
        }

        private static void ClearRegion(SKCanvas canvas, float YUnit, float XUnit, SKPaint blackPaint, IDrawSession session)
        {
            var rect = new SKRect
            {
                Location = new SKPoint
                {
                    Y = ((session.Region.Y) * YUnit) + 10,
                    X = session.Region.X * XUnit
                },
                Size = new SKSize
                {
                    Height = (session.Region.Height * YUnit) - 10,
                    Width = session.Region.Width * XUnit
                }
            };

            canvas.DrawRect(rect, blackPaint);
        }

        private unsafe void InternalDraw()
        {
            var width = ViewportBitmap.PixelWidth;
            var height = ViewportBitmap.PixelHeight;

            var px = (int)(0 * width);
            var py = (int)(0 * height);

            using (var buf = ViewportBitmap.Lock())
            {
                var w = Math.Min(width - px, DrawingBitmap.Width);
                var h = Math.Min(height - py, DrawingBitmap.Height);

                var ptr = (uint*)buf.Address;


                for (var i = 0; i < w; i++)
                {
                    for (var j = 0; j < h; j++)
                    {
                        var pix = DrawingBitmap.GetPixel(i, j);

                        if (pix.Alpha > 200)
                        {
                            var pixPtr = ptr + (j + py) * width + i + px;
                            *pixPtr = (uint)(pix.Blue | pix.Green << 8 | pix.Red << 16 | byte.MaxValue << 24);
                        }
                    }
                }
            }
            control.InvalidateVisual();
            //this.invalidate();
        }
    }
}
