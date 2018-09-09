using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using Rogue.Resources;
using SkiaSharp;

namespace Rogue.App
{
    public class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private Action invalidate;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            this.InitImage();
        }

        private void InitImage()
        {
            Image viewport = this.Content as Image;
            control = viewport;
            viewport.PointerPressed += Viewport_PointerPressed;
            var bitmap = new WritableBitmap(900, 600, PixelFormat.Bgra8888);
            ViewportBitmap = bitmap;
            viewport.Source = bitmap;

            this.invalidate = () =>
                 Dispatcher.UIThread.InvokeAsync(() => viewport.InvalidateVisual()).Wait();

            //this.ResetBitmap();
            this.LoadImage();
        }

        private void Viewport_PointerPressed(object sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            DrawText("Aвыаddddddddddddddddddddddddddddddddddddddddddddd");
        }

        private unsafe void ResetBitmap()
        {
            using (var buf = ViewportBitmap.Lock())
            {
                var ptr = (uint*)buf.Address;

                var w = ViewportBitmap.PixelWidth;
                var h = ViewportBitmap.PixelHeight;

                // Clear.
                for (var i = 0; i < w * (h - 1); i++)
                {
                    *(ptr + i) = 0;
                }

                // Draw bottom line.
                for (var i = w * (h - 1); i < w * h; i++)
                {
                    *(ptr + i) = uint.MaxValue;
                }
            }
        }

        private Image control;
        private WritableBitmap ViewportBitmap;

        SKBitmap drawingBitmap;
        private SKBitmap DrawingBitmap
        {
            get
            {
                if(drawingBitmap==default)
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

        private void LoadImage()
        {
            var stream = ResourceLoader.Load("Rogue.Resources.Images.Splash.splash.jpg");
            var bitmap = SKBitmap.Decode(stream);

            var dstInfo = new SKImageInfo(900, 600);
            DrawingBitmap = bitmap = bitmap.Resize(dstInfo, SKBitmapResizeMethod.Hamming);
            //mybitmap.Resize(dstInfo, SKBitmapResizeMethod.Hamming);

            Draw();
           
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
            
            canvas.DrawText(text, 0, fontMetrics.XHeight+ fontMetrics.Bottom, brush);

            canvas.Flush();

            var image = SKImage.FromBitmap(DrawingBitmap);
            var data = image.Encode(SKEncodedImageFormat.Jpeg, 100);

            using (var stream = new FileStream("output.jpg", FileMode.Create, FileAccess.Write))
                data.SaveTo(stream);

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