using Dungeon.Control;
using Dungeon.Control.Keys;
using Dungeon.Scenes.Manager;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Geranium.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            
        }

        private static void OnTextInput(object sender, TextInputEventArgs e)
        {
            throw new NotImplementedException("text input not implemented yet");
            //SceneManager.Current?.OnText(e.Character.ToString());
        }

        public void OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!Enum.TryParse(typeof(Control.Keys.Key), e.Key.ToString(), true, out var key))
            {
                //sender.As<MainWindow>().ChangeStatus($"{e.Key} not found in Dungeon!");
                key = Control.Keys.Key.None;
            }

            
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

        public void OnKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!Enum.TryParse(typeof(Control.Keys.Key), e.Key.ToString(), true, out var key))
            {
                //sender.As<MainWindow>().ChangeStatus($"{e.Key} not found in Dungeon!");
                key = Control.Keys.Key.None;
            }

            
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
            var offset = new Dot(0,0);

            
        }

        private void OnPointerReleased(MouseButton mouseButton)
        {
            var pos = mousePosition;
            var offset = new Dot(0,0);

        }
    }
}