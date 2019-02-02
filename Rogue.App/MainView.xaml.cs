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
using Rogue.App.Visual;
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
            this.HasSystemDecorations = false;
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            this.RunGame();
        }

        public void RunGame()
        {
            var drawClient = AppVisual.AppVisualDrawClient;// new SkiaDrawClient(this.ViewportBitmap, this.control);
            SceneManager = new SceneManager
            {
                DrawClient = drawClient
            };
            SceneManager.Change<Start>();
        }
        
        protected override void OnPointerMoved(PointerEventArgs e)
        {
            var pos = e.GetPosition(this);

            SceneManager.Current.OnMouseMove(new Control.Pointer.PointerArgs
            {
                ClickCount = 0,
                MouseButton = Control.Pointer.MouseButton.None,
                X = pos.X,
                Y = pos.Y
            });
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            var pos = e.GetPosition(this);

            SceneManager.Current.OnMousePress(new Control.Pointer.PointerArgs
            {
                ClickCount = e.ClickCount,
                MouseButton = (Control.Pointer.MouseButton)e.MouseButton,
                X = pos.X,
                Y = pos.Y
            });
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            SceneManager.Current.OnKeyDown(new Control.Keys.KeyArgs
            {
                Key = (Control.Keys.Key)e.Key,
                Modifiers = (Control.Keys.KeyModifiers)e.Modifiers
            });

            if (e.Key== Key.R)
            {
                if(e.Modifiers== InputModifiers.Control)
                {
                    AppVisual.frameInfo = !AppVisual.frameInfo;
                }
            }
            
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            SceneManager.Current.OnKeyUp(new Control.Keys.KeyArgs
            {
                Key = (Control.Keys.Key)e.Key,
                Modifiers = (Control.Keys.KeyModifiers)e.Modifiers
            });
        }
    }
}