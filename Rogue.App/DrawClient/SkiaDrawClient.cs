namespace Rogue.App.DrawClient
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Avalonia.Controls;
    using Avalonia.Media.Imaging;
    using Avalonia.Threading;
    using MoreLinq;
    using Rogue.Resources;
    using Rogue.Scenes;
    using Rogue.View.Interfaces;
    using SkiaSharp;

    public class SkiaDrawClient : IDrawClient
    {
        private static float YUnit = 20f;
        private static float XUnit = 11.5625f;
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
            if (drawSessions.Count() == 0)
                return;

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

            var blackPaint = new SKPaint { Color = new SKColor(0, 0, 0, 255) };
            //new SKPaint { Color = new SKColor(124, 57, 89, 255), IsStroke=true };


            foreach (var session in drawSessions)
            {
                if (session.Drawables != null)
                {
                    DrawTiles(canvas, session.Drawables);
                }
                else
                {
                    DrawText(canvas, fontSize, font, ref YUnit, ref XUnit, blackPaint, session);
                }
            }


            canvas.Dispose();
            font.Dispose();

            this.InternalDraw(GetBounds(drawSessions));

        }

        private static void DrawTiles(SKCanvas canvas, IEnumerable<IDrawable> drawables)
        {
            foreach (var drawable in drawables)
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
        
        private unsafe void InternalDraw(SKRect bitmapReplaceRegion)
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


                for (var i = (int)bitmapReplaceRegion.Left; i < (int)bitmapReplaceRegion.Left + bitmapReplaceRegion.Size.Width; i++)
                {
                    for (var j = (int)bitmapReplaceRegion.Top; j < (int)bitmapReplaceRegion.Top+ bitmapReplaceRegion.Size.Height; j++)
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
        }

        public void Animate(IAnimationSession animationSession)
        {
            //void invalidate() { Dispatcher.UIThread.InvokeAsync(() => control.InvalidateVisual()).Wait(); }

            Task.Run(() =>
            {
                foreach (var frame in animationSession.Frames)
                {
                    var bitmap = DrawingBitmap;
                    var canvas = new SKCanvas(DrawingBitmap);
                    DrawTiles(canvas, frame);
                    canvas.Dispose();

                    InternalDraw(GetBounds(frame));

                    Thread.Sleep(50);
                    //invalidate();
                }

                animationSession.End();
            });
        }

        private static SKRect GetBounds(IEnumerable<IDrawable> drawables)
        {
            var leftUpdate = drawables.Min(session =>
            {
                return session.Region.X;
            });

            var topUpdate = drawables.Min(session =>
            {
                return session.Region.Y;
            });


            var maxX = drawables.MaxBy(session =>
            {
                return session.Region.X;
            }).First();

            var widthUpdate = maxX.Region.X * 24 + maxX.Region.Width;

            var maxY = drawables.MaxBy(session =>
            {
                    return session.Region.Y;
            }).First();

            var heightUpdate = (maxY.Region.Y * 24 + maxY.Region.Height) - (topUpdate * 24 - 24);

            //height - абсолютная величина СКОЛЬКО надо нарисовать
            //какой-то уебанский косяк, чес слово
            return new SKRect
            {
                Top = topUpdate * 24 -24+3,
                Left = leftUpdate * 24,
                Size = new SKSize
                {
                    Height = heightUpdate-24+3,
                    Width = widthUpdate
                }
            };
        }

        private static SKRect GetBounds(IEnumerable<IDrawSession> drawSessions)
        {
            var leftUpdate = drawSessions.Min(session =>
            {
                if (session.Drawables != null)
                {
                    return session.Drawables.Min(x => x.Region.X);
                }
                else
                {
                    return session.Region.X;
                }
            });

            var topUpdate = drawSessions.Min(session =>
            {
                if (session.Drawables != null)
                {
                    return session.Drawables.Min(x => x.Region.Y);
                }
                else
                {
                    return session.Region.Y;
                }
            });


            var maxX = drawSessions.MaxBy(session =>
            {
                if (session.Drawables != null)
                {
                    return session.Drawables.Max(x => x.Region.X);
                }
                else
                {
                    return session.Region.X + session.Region.Width;
                }
            }).First();

            var widthUpdate = 0f;
            if (maxX.Drawables != null)
            {
                var maxYDrawable = maxX.Drawables.MaxBy(x => x.Region.Y).FirstOrDefault();
                widthUpdate = maxYDrawable.Region.Y;
            }
            else
            {
                widthUpdate = maxX.Region.X + maxX.Region.Width;
            }

            var maxY = drawSessions.MaxBy(session =>
            {
                if (session.Drawables != null)
                {
                    return session.Drawables.Max(x => x.Region.Y);
                }
                else
                {
                    return session.Region.Y + session.Region.Height;
                }
            }).First();

            var heightUpdate = 0f;
            if (maxY.Drawables != null)
            {
                var maxYDrawable = maxY.Drawables.MaxBy(x => x.Region.Y).FirstOrDefault();
                heightUpdate = maxYDrawable.Region.Y;
            }
            else
            {
                heightUpdate = maxY.Region.Y + maxY.Region.Height;
            }

            return new SKRect
            {
                Top = topUpdate * YUnit,
                Left = leftUpdate * XUnit + XUnit,
                Size = new SKSize
                {
                    Height = (heightUpdate * YUnit) - (topUpdate * YUnit),
                    Width = widthUpdate * XUnit
                }
            };
        }
    }
}
