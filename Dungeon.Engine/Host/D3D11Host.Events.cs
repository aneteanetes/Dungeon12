using Dungeon.Control;
using Dungeon.Control.Keys;
using Dungeon.Scenes.Manager;
using Dungeon.View.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Dungeon.Engine.Host
{
    public partial class D3D11Host
    {
        private bool blockControls = false;

        private void InitEvents()
        {
        }

        private void UpdateLoop(Microsoft.Xna.Framework.GameTime gameTime)
        {
            var gameTimeLoop = new GameTimeLoop(gameTime.TotalGameTime, gameTime.ElapsedGameTime, gameTime.IsRunningSlowly);

            if (!blockControls)
            {
                UpdateMouseEvents();
            }

            if (scene != default)
            {
                for (int i = 0; i < scene.Objects.Length; i++)
                {
                    var obj = scene.Objects[i];
                    if (DungeonGlobal.ComponentUpdateCompatibility)
                    {
                        if (obj.Updatable && InCamera(obj))
                            UpdateComponent(obj);
                    }
                    else
                    {
                        UpdateComponent(obj, gameTimeLoop);
                    }
                }
            }
        }

        private void UpdateComponent(ISceneObject sceneObject, GameTimeLoop gameTimeLoop)
        {
            sceneObject.Update(gameTimeLoop);

            for (int i = 0; i < sceneObject.Children.Count; i++)
            {
                var child = sceneObject.Children.ElementAtOrDefault(i);
                if (child != null)
                {
                    UpdateComponent(child, gameTimeLoop);
                }
            }
        }

        private void UpdateComponent(ISceneObject sceneObject)
        {
            //if(frameEnd)
            //{
            sceneObject.Update();
            //}

            for (int i = 0; i < sceneObject.Children.Count; i++)
            {
                var child = sceneObject.Children.ElementAtOrDefault(i);
                if (child != null)
                {
                    if (child.Updatable && InCamera(child))
                        UpdateComponent(child);
                }
            }
        }

        private static void OnTextInput(object sender, TextInputEventArgs e)
        {
            throw new NotImplementedException("text input not implemented yet");
            //SceneManager.Current?.OnText(e.Character.ToString());
        }

        public static void OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!Enum.TryParse(typeof(Control.Keys.Key), e.Key.ToString(), true, out var key))
            {
                sender.As<MainWindow>().ChangeStatus($"{e.Key} not found in Dungeon!");
                key = Control.Keys.Key.None;
            }

            SceneManager.Current?.OnKeyDown(new Dungeon.Control.Keys.KeyArgs
            {
                Key = key.As<Control.Keys.Key>(),
                Modifiers = GetModifier()
                //Hold = 
            });
        }

        private static KeyModifiers GetModifier()
        {
            if (Keyboard.IsKeyDown(System.Windows.Input.Key.LeftAlt) || Keyboard.IsKeyDown(System.Windows.Input.Key.RightAlt))
            {
                return KeyModifiers.Alt;
            }

            if (Keyboard.IsKeyDown(System.Windows.Input.Key.LeftCtrl) || Keyboard.IsKeyDown(System.Windows.Input.Key.RightCtrl))
            {
                return KeyModifiers.Control;
            }

            if (Keyboard.IsKeyDown(System.Windows.Input.Key.LeftShift) || Keyboard.IsKeyDown(System.Windows.Input.Key.RightShift))
            {
                return KeyModifiers.Shift;
            }

            if (Keyboard.IsKeyDown(System.Windows.Input.Key.LWin) || Keyboard.IsKeyDown(System.Windows.Input.Key.RWin))
            {
                return KeyModifiers.Windows;
            }

            return KeyModifiers.None;
        }

        public static void OnKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!Enum.TryParse(typeof(Control.Keys.Key), e.Key.ToString(), true, out var key))
            {
                sender.As<MainWindow>().ChangeStatus($"{e.Key} not found in Dungeon!");
                key = Control.Keys.Key.None;
            }

            SceneManager.Current?.OnKeyUp(new Dungeon.Control.Keys.KeyArgs
            {
                Key = key.As<Control.Keys.Key>(),
                Modifiers = GetModifier()
            });
        }


        private System.Windows.Point mousePosition;
        private int scrollWeelValue;
        private MouseState mouseState;

        private void UpdateMouseEvents()
        {
            var p = Mouse.GetPosition(this);

            if (p.X < 0 || p.Y < 0)
                return;

            mouseState = new MouseState(p.X, p.Y, 0, Mouse.LeftButton, Mouse.MiddleButton, Mouse.RightButton);

            if (mouseState.Position != mousePosition)
            {
                mousePosition = mouseState.Position;
                OnPointerMoved();
            }

            MouseClicksPipe
                (CheckMouseClick, MouseButton.Right, mouseState.RightButton)
                (CheckMouseClick, MouseButton.Left, mouseState.LeftButton)
                (CheckMouseClick, MouseButton.Middle, mouseState.MiddleButton);

            if (mouseState.ScrollWheelValue != scrollWeelValue)
            {
                OnPointerWheelChanged(mouseState.ScrollWheelValue > scrollWeelValue);
                scrollWeelValue = mouseState.ScrollWheelValue;
            }
        }

        private void OnPointerWheelChanged(bool isTop)
        {
            SceneManager.Current?.OnMouseWheel(isTop ? Dungeon.Control.Pointer.MouseWheelEnum.Up : Dungeon.Control.Pointer.MouseWheelEnum.Down);
        }

        private delegate ConditionalDelegate ConditionalDelegate(Func<MouseButton, MouseButtonState, bool> func, MouseButton arg, MouseButtonState arg2);

        private ConditionalDelegate MouseClicksPipe(Func<MouseButton, MouseButtonState, bool> action, MouseButton arg, MouseButtonState arg2)
        {
            if (action?.Invoke(arg, arg2) ?? false)
            {
                return MouseClicksPipeEmpty;
            }
            else
            {
                return MouseClicksPipe;
            }
        }

        private ConditionalDelegate MouseClicksPipeEmpty(Func<MouseButton, MouseButtonState, bool> action, MouseButton arg, MouseButtonState arg2)
        {
            return MouseClicksPipeEmpty;
        }

        private bool CheckMouseClick(MouseButton mouseButton, MouseButtonState buttonState)
        {
            var changes = buttonPressings[mouseButton] != buttonState;

            if (!changes)
                return false;

            if (buttonPressings[mouseButton] != buttonState)
            {
                if (buttonPressings[mouseButton] == MouseButtonState.Released)
                {
                    buttonPressings[mouseButton] = MouseButtonState.Pressed;
                    OnPointerPressed(mouseButton);
                }
                else
                {
                    buttonPressings[mouseButton] = MouseButtonState.Released;
                    OnPointerReleased(mouseButton);
                }
            }

            return true;
        }

        private void OnPointerMoved()
        {
            var currentScene = SceneManager.Current;
            if (currentScene != default)
            {

                MouseButton mb = MouseButton.XButton1;

                if (mouseState.LeftButton == MouseButtonState.Pressed)
                    mb = MouseButton.Left;

                if (mouseState.RightButton == MouseButtonState.Pressed)
                    mb = MouseButton.Right;

                if (mouseState.MiddleButton == MouseButtonState.Pressed)
                    mb = MouseButton.Middle;

                //this.light.Position = new Microsoft.Xna.Framework.Vector2(mousePosition.X, mousePosition.Y);

                currentScene.OnMouseMove(new PointerArgs
                {
                    ClickCount = 0,
                    MouseButton = (Dungeon.Control.Pointer.MouseButton)Enum.Parse(typeof(Control.Pointer.MouseButton), mb== MouseButton.XButton1 ? "None" : mb.ToString()),
                    X = mousePosition.X,
                    Y = mousePosition.Y
                }, new Dungeon.Types.Point(CameraOffsetX, CameraOffsetY));
            }
        }

        private readonly Dictionary<MouseButton, MouseButtonState> buttonPressings = new Dictionary<MouseButton, MouseButtonState>()
        {
            { MouseButton.Left, MouseButtonState.Released },
            { MouseButton.Right, MouseButtonState.Released },
            { MouseButton.Middle, MouseButtonState.Released },
        };

        private void OnPointerPressed(MouseButton mouseButton)
        {
            var pos = mousePosition;
            var offset = new Dungeon.Types.Point(CameraOffsetX, CameraOffsetY);

            SceneManager.Current?.OnMousePress(new PointerArgs
            {
                ClickCount = 1,
                MouseButton = (Dungeon.Control.Pointer.MouseButton)Enum.Parse(typeof(Control.Pointer.MouseButton), mouseButton.ToString()),
                X = pos.X,
                Y = pos.Y,
                Offset = offset
            }, offset);
        }

        private void OnPointerReleased(MouseButton mouseButton)
        {
            var pos = mousePosition;
            var offset = new Types.Point(CameraOffsetX, CameraOffsetY);

            SceneManager.Current?.OnMouseRelease(new PointerArgs
            {
                ClickCount = 1,
                MouseButton = (Dungeon.Control.Pointer.MouseButton)Enum.Parse(typeof(Control.Pointer.MouseButton), mouseButton.ToString()),
                X = pos.X,
                Y = pos.Y,
                Offset = offset
            }, offset);
        }
    }
}