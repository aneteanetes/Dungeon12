namespace Rogue.App.DrawClient
{
    using System;
    using System.Collections.Generic;
    using Avalonia.Controls;
    using Avalonia.Media.Imaging;
    using Rogue.View.Interfaces;
    using SkiaSharp;

    public class SkiaDrawClient : IDrawClient
    {
        private WritableBitmap ViewportBitmap;
        private Image control;

        SKBitmap drawingBitmap;
        private SKBitmap DrawingBitmap
        {
            get
            {
                if (drawingBitmap == default)
                {
                    drawingBitmap = new SKBitmap(900, 600, SKColorType.Bgra8888, SKAlphaType.Premul);
                }

                return this.drawingBitmap;
            }

            set
            {
                this.drawingBitmap = value;
            }
        }

        public SkiaDrawClient(WritableBitmap viewportBitmap, Image image)
        {
            this.ViewportBitmap = viewportBitmap;
            this.control = image;
        }

        public void Draw(IEnumerable<IDrawSession> drawSessions)
        {
            var bitmap = DrawingBitmap;

            var canvas = new SKCanvas(DrawingBitmap);
            canvas.DrawBitmap(bitmap, 0, 0);

            var font = SKTypeface.FromFamilyName("Arial");
            var brush = new SKPaint
            {
                Typeface = font,
                TextSize = 16.0f,
                IsAntialias = true,
                Color = new SKColor(255, 255, 255, 255)
            };

            brush.GetFontMetrics(out var fontMetrics);

            //font
            foreach (var drawSession in drawSessions)
            {
                float y = 70+ fontMetrics.XHeight + fontMetrics.Bottom;
                foreach (var line in drawSession.Content)
                {
                    foreach (var lne in line.Data.Split(Environment.NewLine))
                    {
                        canvas.DrawText(lne, -140, y, brush);
                        y += fontMetrics.XHeight + fontMetrics.Bottom;
                    }
                }
            }

            canvas.Flush();

            var image = SKImage.FromBitmap(DrawingBitmap);
            var data = image.Encode(SKEncodedImageFormat.Jpeg, 100);

            this.Draw();

            canvas.Dispose();
            brush.Dispose();
            font.Dispose();
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
