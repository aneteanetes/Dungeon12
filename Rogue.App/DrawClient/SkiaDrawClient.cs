namespace Rogue.App.DrawClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
                    drawingBitmap = new SKBitmap(1157, 700, SKColorType.Bgra8888, SKAlphaType.Premul);
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

        private void ExperimentalDraw(IEnumerable<IDrawSession> drawSessions)
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

            float YUnit= 20f;
            float XUnit = 11.5625f;

            var blackPaint = new SKPaint { Color = new SKColor(0, 0, 0, 255) };

            foreach (var session in drawSessions)
            {
                var rect = new SKRect();
                rect.Location = new SKPoint
                {
                    Y = (session.Region.Y) * YUnit,
                    X = session.Region.X * XUnit
                };
                rect.Size = new SKSize
                {
                    Height = (session.Region.Height * YUnit),
                    Width = session.Region.Width * XUnit
                };

                canvas.DrawRect(rect, blackPaint);

                float y = (session.Region.Y+1) * YUnit;
                foreach (var line in session.Content)
                {

                    float x = session.Region.X * XUnit;
                    foreach (var lne in line.Data)
                    {

                        var textpaint = new SKPaint
                        {
                            Typeface = font,
                            TextSize = fontSize,
                            IsAntialias = true,
                            Color = new SKColor(lne.ForegroundColor.R, lne.ForegroundColor.G, lne.ForegroundColor.B, lne.ForegroundColor.A),
                            Style = SKPaintStyle.Fill
                        };

                        canvas.DrawText(lne.StringData, x, y, textpaint);
                        canvas.DrawText(lne.StringData, x, y, textpaint);
                        

                        x += XUnit;
                    }

                    y += YUnit;

                }
            }


            canvas.Dispose();
            font.Dispose();

            this.Draw();
        }

        public void Draw(IEnumerable<IDrawSession> drawSessions)
        {
            this.ExperimentalDraw(drawSessions);
            return;

            var bitmap = DrawingBitmap;

            var canvas = new SKCanvas(DrawingBitmap);

            //canvas.Scale(1.5f);

            var debug = false;

            float fontSize = 20f;
            var font = SKTypeface.FromFamilyName("Lucida Console");
            
            //brush.IsAntialias = true;
            
            float YStep = 17f;
            float XStep = 17f;

            var wtf = new SKColor[]
            {
                new SKColor(255, 255, 0, 255),
                new SKColor(0, 255, 0, 255),
                new SKColor(0, 0, 255, 255),
                new SKColor(0, 255, 255, 255),
            };

            var brush = new SKPaint
            {
                Typeface = font,
                TextSize = fontSize,
                IsAntialias = true,
                Color = //new SKColor(0, 0, 0, 255),//!debug 
                        //? new SKColor(0, 0, 0, 255)
                        /*:*/
                          new SKColor(255, 0, 0, 255),
                Style = SKPaintStyle.Stroke,
                //TextScaleX=1.5f,
                //TextSkewX=1.5f
                //FilterQuality= SKFilterQuality.High
            };

            SKRect rectB = new SKRect();
            brush.MeasureText("@", ref rectB);


            //font
            foreach (var drawSession in drawSessions)
            {
                brush = new SKPaint
                {
                    Typeface = font,
                    TextSize = fontSize,
                    IsAntialias = true,
                    Color = wtf[drawSessions.ToList().IndexOf(drawSession)],
                    Style = SKPaintStyle.Stroke,
                    //TextScaleX=1.5f,
                    //TextSkewX=1.5f
                    //FilterQuality= SKFilterQuality.High
                };

                float y = drawSession.Region.Y * YStep;

                Console.WriteLine($"Y START:{(drawSession.Region.Y) * YStep}");

                var rect = new SKRect(drawSession.Region.X * XStep,
                    (drawSession.Region.Y-1) * YStep, // -1 обоссаный костыль на шрифты
                    (drawSession.Region.X * XStep) + ((drawSession.Region.Width) * XStep),
                    (drawSession.Region.Height) * YStep);

                rect = new SKRect();
                rect.Location = new SKPoint
                {
                    Y = (drawSession.Region.Y) * YStep,
                    X = drawSession.Region.X * XStep
                };
                rect.Size = new SKSize
                {
                    Height = (drawSession.Region.Height * YStep),
                    Width = drawSession.Region.Width * XStep
                };

                canvas.DrawRect(rect, brush);
                canvas.DrawRect(rect, brush);
                canvas.DrawRect(rect, brush);
                canvas.DrawRect(rect, brush);


                //canvas.DrawRect(new SKRect
                //{
                //    Location=new SKPoint
                //    {
                //        Y = (drawSession.Region.Y - 1) * YStep,
                //        X= drawSession.Region.X * XStep
                //    },
                //    Size = new SKSize
                //    {
                //        Height =( drawSession.Region.Height-1) * YStep,
                //        Width = (drawSession.Region.Width*2) * XStep
                //    }
                //}, brush);

                foreach (var line in drawSession.Content)
                {
                    float x = drawSession.Region.X * XStep;
                                        
                    foreach (var lne in line.Data)
                    {
                        var txt = line.Data.Sum(z => z.StringData.Length);

                        var textSegmentBrush = new SKPaint
                        {
                            Typeface = font,
                            TextSize = fontSize,
                            IsAntialias = true,
                            Color = new SKColor(lne.ForegroundColor.R, lne.ForegroundColor.G, lne.ForegroundColor.B, lne.ForegroundColor.A),
                            Style= SKPaintStyle.Fill                            
                            //FilterQuality= SKFilterQuality.High
                        };
                        textSegmentBrush.IsAntialias = true;

                        //foreach (var @char in lne.StringData)
                        //{
                        //    canvas.DrawText(@char.ToString(), x, y, textSegmentBrush);
                        //    canvas.DrawText(@char.ToString(), x, y, textSegmentBrush);
                        //    canvas.DrawText(@char.ToString(), x, y, textSegmentBrush);
                        //    x += XStep;
                        //}

                        //canvas.DrawPath(path, textSegmentBrush);
                        canvas.DrawText(lne.StringData, x, y, textSegmentBrush);
                        canvas.DrawText(lne.StringData, x, y, textSegmentBrush);

                        var measureX = textSegmentBrush.MeasureText(lne.StringData);

                        x += lne.StringData.Length * XStep;
                        //x += XStep;// - (float)(bounds.Width * 0.12);
                    }

                    y += YStep;// *1.2f;

                }

                if (debug)
                    break;

            }

            canvas.Flush();            

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
