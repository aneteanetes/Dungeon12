using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Rogue.App.DrawClient;
using Rogue.App.Visual;
using Rogue.Resources;
using Rogue.Scenes;
using Rogue.Scenes.Manager;
using Rogue.Scenes.Menus;
using Rogue.View.Interfaces;
using SkiaSharp;

namespace Rogue.App
{
    public class MainView : Window
    {
        public SceneManager SceneManager { get; set; }

        private IDrawClient drawClient = null;

        public MainView()
        {
            this.Background = Brush.Parse("black");
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
            drawClient = AppVisual.AppVisualDrawClient;// new SkiaDrawClient(this.ViewportBitmap, this.control);
            SceneManager = new SceneManager
            {
                DrawClient = drawClient
            };
            SceneManager.Change<Start>();
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            var pos = e.GetPosition(this);

            var currentScene = SceneManager.Current;

            if (currentScene.CameraAffect)
            {
                drawClient.MoveCamera(Types.Direction.Right, !(pos.X >= 1180));
                drawClient.MoveCamera(Types.Direction.Left, !(pos.X <= 100));
                drawClient.MoveCamera(Types.Direction.Down, !(pos.Y >= 620));
                drawClient.MoveCamera(Types.Direction.Up, !(pos.Y <= 100));
            }

             MouseButton mb = MouseButton.None;

            switch (e.InputModifiers)
            {
                case InputModifiers.LeftMouseButton:
                    mb = MouseButton.Left;
                    break;
                case InputModifiers.RightMouseButton:
                    mb = MouseButton.Right;
                    break;
                case InputModifiers.MiddleMouseButton:
                    mb = MouseButton.Middle;
                    break;
                default:
                    break;
            }

            currentScene.OnMouseMove(new Control.Pointer.PointerArgs
            {
                ClickCount = 0,
                MouseButton = (Control.Pointer.MouseButton)mb,
                X = pos.X,
                Y = pos.Y
            }, new Types.Point(drawClient.CameraOffsetX, drawClient.CameraOffsetY));
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            var pos = e.GetPosition(this);
            var offset = new Types.Point(drawClient.CameraOffsetX, drawClient.CameraOffsetY);

            SceneManager.Current.OnMousePress(new Control.Pointer.PointerArgs
            {
                ClickCount = e.ClickCount,
                MouseButton = (Control.Pointer.MouseButton)e.MouseButton,
                X = pos.X,
                Y = pos.Y,
                Offset= offset
            }, offset);
        }

        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            var isTop = e.Delta.Y == 1;

            SceneManager.Current.OnMouseWheel(isTop ? Control.Pointer.MouseWheelEnum.Up : Control.Pointer.MouseWheelEnum.Down);
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            var pos = e.GetPosition(this);

            SceneManager.Current.OnMouseRelease(new Control.Pointer.PointerArgs
            {
                MouseButton = (Control.Pointer.MouseButton)e.MouseButton,
                X = pos.X,
                Y = pos.Y
            }, new Types.Point(drawClient.CameraOffsetX, drawClient.CameraOffsetY));
        }

        private HashSet<Key> keysHold = new HashSet<Key>();

        protected override void OnKeyDown(KeyEventArgs e)
        {
            var hold = keysHold.Contains(e.Key);

            keysHold.Add(e.Key);
            SceneManager.Current.OnKeyDown(new Control.Keys.KeyArgs
            {
                Key = (Control.Keys.Key)e.Key,
                Modifiers = (Control.Keys.KeyModifiers)e.Modifiers,
                Hold= hold
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

        protected override void OnTextInput(TextInputEventArgs e)
        {
            SceneManager.Current.OnText(e.Text);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            keysHold.Remove(e.Key);
            SceneManager.Current.OnKeyUp(new Control.Keys.KeyArgs
            {
                Key = (Control.Keys.Key)e.Key,
                Modifiers = (Control.Keys.KeyModifiers)e.Modifiers
            });
        }
    }
}