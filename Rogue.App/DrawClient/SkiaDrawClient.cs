namespace Rogue.App.DrawClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Avalonia.Controls;
    using Avalonia.Media.Imaging;
    using Rogue.Resources;
    using Rogue.View.Interfaces;
    using SkiaSharp;

    public class SkiaDrawClient : IDrawClient
    {
        private static Random Random = new Random();

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

        private SKBitmap _tileset;
        private SKBitmap Tileset
        {
            get
            {
                if (_tileset == null)
                {
                    var stream = ResourceLoader.Load("Rogue.Resources.Images.Tiles.TilesetTransparent.png");
                    _tileset = SKBitmap.Decode(stream);
                }

                return _tileset;
            }
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
                var rect = new SKRect();
                rect.Location = new SKPoint
                {
                    Y = ((session.Region.Y) * YUnit)+10,
                    X = session.Region.X * XUnit
                };
                rect.Size = new SKSize
                {
                    Height = (session.Region.Height * YUnit)-10,
                    Width = session.Region.Width * XUnit
                };

                canvas.DrawRect(rect, blackPaint);

                float y = ((session.Region.Y) * YUnit)+10;
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
                                bool texture = false;

                                XUnit = 20;

                                if (@char == '°')
                                {
                                    texture = true;
                                    var randomX = Random.Next(29, 31);
                                    var randomY = Random.Next(26, 28);

                                    canvas.DrawBitmap(Tileset, new SKRect
                                    {
                                        Left = 24 * randomX,
                                        Top = 24 * (16+8),
                                        Size = new SKSize
                                        {
                                            Height = 24,
                                            Width = 24
                                        }
                                    }, new SKRect
                                    {
                                        Top=y-YUnit,
                                        Left=x,
                                        Size = new SKSize
                                        {
                                            Height = 20,
                                            Width = 20
                                        }
                                    });

                                    
                                }

                                if(@char=='#')
                                {
                                XUnit = 20;
                                    texture = true;
                                    var randomX = Random.Next(29, 31);
                                    var randomY = Random.Next(26, 28);

                                    canvas.DrawBitmap(Tileset, new SKRect
                                    {
                                        Left = 24 * randomX,
                                        Top = 24 * (16 + 8),
                                        Size = new SKSize
                                        {
                                            Height = 24,
                                            Width = 24
                                        }
                                    }, new SKRect
                                    {
                                        Top = y - YUnit,
                                        Left = x,
                                        Size = new SKSize
                                        {
                                            Height = YUnit,
                                            Width = XUnit
                                        }
                                    });

                                    randomX = Random.Next(44, 55);
                                    randomY = Random.Next(1, 5);

                                    canvas.DrawBitmap(Tileset, new SKRect
                                    {
                                        Left = 24 * randomX,
                                        Top = 24 * randomY,
                                        Size = new SKSize
                                        {
                                            Height = 24,
                                            Width = 24
                                        }
                                    }, new SKRect
                                    {
                                        Top = y - YUnit,
                                        Left = x,
                                        Size = new SKSize
                                        {
                                            Height = YUnit,
                                            Width = XUnit
                                        }
                                    });
                                }

                                if (@char == '@')
                                {
                                    XUnit = 20;
                                    texture = true;
                                    var randomX = Random.Next(29, 31);
                                    var randomY = Random.Next(26, 28);

                                    canvas.DrawBitmap(Tileset, new SKRect
                                    {
                                        Left = 24 * randomX,
                                        Top = 24 * (16 + 8),
                                        Size = new SKSize
                                        {
                                            Height = 24,
                                            Width = 24
                                        }
                                    }, new SKRect
                                    {
                                        Top = y - YUnit,
                                        Left = x,
                                        Size = new SKSize
                                        {
                                            Height = YUnit,
                                            Width = XUnit
                                        }
                                    });

                                    randomX = Random.Next(44, 55);
                                    randomY = Random.Next(1, 5);

                                    canvas.DrawBitmap(Tileset, new SKRect
                                    {
                                        Left = 24 * 33,
                                        Top = 24 * 6,
                                        Size = new SKSize
                                        {
                                            Height = 24,
                                            Width = 24
                                        }
                                    }, new SKRect
                                    {
                                        Top = y - YUnit,
                                        Left = x,
                                        Size = new SKSize
                                        {
                                            Height = YUnit,
                                            Width = XUnit
                                        }
                                    });
                                }

                                //6
                                //34

                                if (!texture)
                                {
                                    XUnit = 11.5625f;
                                    canvas.DrawText(@char.ToString(), x, y, textpaint);
                                    canvas.DrawText(@char.ToString(), x, y, textpaint);
                                }

                                x += XUnit;
                                XUnit = 11.5625f;
                            }

                            //x += XUnit * lne.Length;
                        }
                    }

                    y += YUnit;

                }
            }


            canvas.Dispose();
            font.Dispose();

            this.Draw();
        }

        private unsafe void Draw()
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

        private void DrawText(string text)
        {
            //DrawingBitmap.LockPixels();
            var bitmap = DrawingBitmap;
            //var toBitmap =bitmap new SKBitmap(new SKImageInfo(DrawingBitmap.Width, DrawingBitmap.Height, DrawingBitmap.ColorType, DrawingBitmap.AlphaType));// (int)Math.Round(bitmap.Width * resizeFactor), (int)Math.Round(bitmap.Height * resizeFactor), bitmap.ColorType, bitmap.AlphaType);

            var canvas = new SKCanvas(DrawingBitmap);
            // Draw a bitmap rescaled
            //canvas.SetMatrix(SKMatrix.MakeScale(resizeFactor, resizeFactor));
            canvas.DrawBitmap(bitmap, 0, 0);
            //canvas.ResetMatrix();

            var font = SKTypeface.FromFamilyName("Arial");
            var brush = new SKPaint
            {
                Typeface = font,
                TextSize = 64.0f,
                IsAntialias = true,
                Color = new SKColor(255, 255, 255, 255)
            };

            brush.GetFontMetrics(out var fontMetrics);

            //font

            canvas.DrawText(text, 0, fontMetrics.XHeight + fontMetrics.Bottom, brush);

            canvas.Flush();

            var image = SKImage.FromBitmap(DrawingBitmap);
            var data = image.Encode(SKEncodedImageFormat.Jpeg, 100);

            //data.Dispose();
            //image.Dispose();
            //canvas.Dispose();
            //brush.Dispose();
            //font.Dispose();
            //toBitmap.Dispose();
            //bitmap.Dispose();

            //this.DrawingBitmap = toBitmap;

            this.Draw();

            canvas.Dispose();
            brush.Dispose();
            font.Dispose();
        }
    }
}
