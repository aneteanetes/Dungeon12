using System;
using Avalonia;
using SkiaSharp.Views.Desktop;

namespace Rogue.App
{
    public class Program //: Window
    {

        static void Main(string[] args)
        {
            BuildAvaloniaApp().Start<MainView>();
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<Application>()
                .UsePlatformDetect();

        //static void Main21()
        //{

        //    Console.ReadLine();
        //    //WindowCreateInfo windowCI = new WindowCreateInfo()
        //    //{
        //    //    X = 450,
        //    //    Y = 150,
        //    //    WindowWidth = 900,
        //    //    WindowHeight = 700,
        //    //    WindowTitle = "Veldrid Tutorial"
        //    //};
        //    //Sdl2Window window = VeldridStartup.CreateWindow(ref windowCI);
        //    //window.

        //    //_graphicsDevice = VeldridStartup.CreateGraphicsDevice(window);

        //    //while (window.Exists)
        //    //{
        //    //    window.PumpEvents();
        //    //}
        //}

        //static void Main(string[] args)
        //{
        //    WindowCreateInfo windowCI = new WindowCreateInfo()
        //    {
        //        X = 100,
        //        Y = 100,
        //        WindowWidth = 960,
        //        WindowHeight = 540,
        //        WindowTitle = "Veldrid Tutorial"
        //    };
        //    Sdl2Window window = VeldridStartup.CreateWindow(ref windowCI);

        //    //System.Windows.De
        //    //var wpf = new SkiaSharp.Views.WPF.SKElement();
        //    //wpf.PaintSurface += Wpf_PaintSurface;
        //    //var widget = new SkiaSharp.Views.Gtk.SKWidget();
        //    //var ctrl = new SkiaSharp.Views.Desktop.SKGLControl(OpenTK.Graphics.GraphicsMode.Default, 500, 500, OpenTK.Graphics.GraphicsContextFlags.Default);


        //    //ctrl.PaintSurface += Ctrl_PaintSurface;
        //    var info = new SKImageInfo(100, 100);
        //    using (var surface = SKSurface.Create(info))
        //    {
        //        SKCanvas canvas = surface.Canvas;

        //        canvas.DrawColor(SKColors.White);

        //        // set up drawing tools
        //        using (var paint = new SKPaint())
        //        {
        //            paint.TextSize = 64.0f;
        //            paint.IsAntialias = true;
        //            paint.Color = new SKColor(0x42, 0x81, 0xA4);
        //            paint.IsStroke = false;

        //            // draw the text
        //            canvas.DrawText("Skia", 0.0f, 64.0f, paint);
        //        }
        //    }
        //    Console.WriteLine("Hello World!");
        //    Console.ReadLine();
        //}

        private static void Wpf_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void Ctrl_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
