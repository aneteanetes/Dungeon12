using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using Rogue.App.DrawClient;
using Rogue.Resources;
using Rogue.Scenes;
using Rogue.Scenes.Menus;
using SkiaSharp;

namespace Rogue.App
{
    public class MainView : Window
    {
        public SceneManager SceneManager { get; set; }

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

        public void RunGame()
        {
            SceneManager = new SceneManager()
            {
                DrawClient = new SkiaDrawClient(this.ViewportBitmap, this.control)
            };

            SceneManager.Change<MainMenuScene>();
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
            //RunGame();
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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            SceneManager.Current.KeyPress(new Scenes.Controls.Keys.KeyArgs
            {
                Key = (Scenes.Controls.Keys.Key)e.Key,
                Modifiers = (Scenes.Controls.Keys.KeyModifiers)e.Modifiers
            });

            base.OnKeyDown(e);
        }

        private void LoadImage()
        {
            var stream = ResourceLoader.Load("Rogue.Resources.Images.Splash.start.png");
            var bitmap = SKBitmap.Decode(stream);

            var dstInfo = new SKImageInfo(900, 600);
            DrawingBitmap = bitmap;// = bitmap.Resize(dstInfo, SKBitmapResizeMethod.Hamming);
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
            RunGame();
        }
    }
}