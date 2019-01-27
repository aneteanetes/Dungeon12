using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using Avalonia.VisualTree;
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
            this.CanResize = false;
            //this.HasSystemDecorations = false;
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
            var drawClient = new SkiaDrawClient(this.ViewportBitmap, this.control);
            SceneManager = new SceneManager() { DrawClient = drawClient };            
            SceneManager.Change<MainMenuScene>();
        }

        private void InitImage()
        {
            Image viewport = this.Content as Image;
            control = viewport;
            viewport.PointerPressed += Viewport_PointerPressed;
            viewport.PointerMoved += Viewport_PointerMoved;
            var bitmap = new WriteableBitmap(new PixelSize(1157, 700), new Vector(72, 72), PixelFormat.Bgra8888);
            ViewportBitmap = bitmap;
            viewport.Source = bitmap;

            this.invalidate = () =>
                 Dispatcher.UIThread.InvokeAsync(() => viewport.InvalidateVisual()).Wait();

            //this.ResetBitmap();
            this.LoadImage();
        }

        private int delay;
        private void Viewport_PointerMoved(object sender, PointerEventArgs e)
        {
            var pos = e.GetPosition(control);

            //if (delay == 15)
            //{
            //    delay = 0;
                SceneManager.Current.OnMouseMove(new Control.Pointer.PointerArgs
                {
                    ClickCount = 0,
                    MouseButton = Control.Pointer.MouseButton.None,
                    X = pos.X,
                    Y = pos.Y
                });
            //}
            //else
            //{
            //    delay++;
            //}
        }

        private void Viewport_PointerPressed(object sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            var pos = e.GetPosition(control);

            SceneManager.Current.OnMousePress(new Control.Pointer.PointerArgs
            {
                ClickCount = e.ClickCount,
                MouseButton = (Control.Pointer.MouseButton)e.MouseButton,
                X = pos.X,
                Y = pos.Y
            });
        }

        private unsafe void ResetBitmap()
        {
            using (var buf = ViewportBitmap.Lock())
            {
                var ptr = (uint*)buf.Address;

                var w = ViewportBitmap.PixelSize.Width;
                var h = ViewportBitmap.PixelSize.Height;

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
        private WriteableBitmap ViewportBitmap;

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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            SceneManager.Current.OnKeyPress(new Control.Keys.KeyArgs
            {
                Key = (Control.Keys.Key)e.Key,
                Modifiers = (Control.Keys.KeyModifiers)e.Modifiers
            });

            base.OnKeyDown(e);
        }

        private void LoadImage()
        {
            var stream = ResourceLoader.Load("Rogue.Resources.Images.Splash.sceneHD.png");
            var bitmap = SKBitmap.Decode(stream);

            var dstInfo = new SKImageInfo(1157, 700);
            DrawingBitmap = bitmap;// = bitmap.Resize(dstInfo, SKBitmapResizeMethod.Hamming);
            //mybitmap.Resize(dstInfo, SKBitmapResizeMethod.Hamming);

            Draw();

        }
        private unsafe void Draw()
        {
            var width = ViewportBitmap.PixelSize.Width;
            var height = ViewportBitmap.PixelSize.Height;

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