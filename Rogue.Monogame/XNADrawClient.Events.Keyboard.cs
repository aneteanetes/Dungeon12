namespace Rogue
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Rogue.Control.Keys;
    using Rogue.Control.Pointer;
    using Rogue.Resources;
    using Rogue.Scenes.Manager;
    using Rogue.Scenes.Menus;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using Rect = Rogue.Types.Rectangle;

    public partial class XNADrawClient
    {
        private KeyboardState keyboardState;
        private Keys[] pressed;
        private HashSet<Keys> keysState = new HashSet<Keys>();
        private HashSet<Keys> keysHolds = new HashSet<Keys>();

        private void UpdateKeyboardEvents()
        {
            keyboardState = Keyboard.GetState();

            var pressed = keyboardState.GetPressedKeys();

            //detect that keys in keyboard different, so, one of keys was pressed or released
            if (!keysState.SetEquals(pressed))
            {
                var currentHashset = new HashSet<Keys>(keysState);
                var pressedHashset = new HashSet<Keys>(pressed);

                keysState.ExceptWith(pressedHashset);
                foreach (var key in keysState)
                {
                    OnKeyUp(key);
                }

                pressedHashset.ExceptWith(currentHashset);
                foreach (var key in pressedHashset)
                {
                    OnKeyDown(key);
                }

                keysState = new HashSet<Keys>(pressed);
            }

            //detect that keyboard took new array of keys (it means, keyboard interacting)
            if (this.pressed != pressed)
            {
                var pressedHashset = new HashSet<Keys>(pressed);
                pressedHashset.IntersectWith(keysState);
                keysHolds = pressedHashset;
            }

            this.pressed = pressed;
        }

        private void OnTextInput(object sender, TextInputEventArgs e)
        {
            SceneManager.Current.OnText(e.Character.ToString());
        }

        private void OnKeyDown(Keys key)
        {
            var hold = keysHolds.Contains(key);

            SceneManager.Current.OnKeyDown(new Control.Keys.KeyArgs
            {
                Key = (Key)key,
                Modifiers = GetModifier(),
                Hold = hold
            });
        }

        private void OnKeyUp(Keys key)
        {
            keysHolds.Remove(key);

            SceneManager.Current.OnKeyDown(new Control.Keys.KeyArgs
            {
                Key = (Key)key,
                Modifiers = GetModifier()
            });
        }

        private KeyModifiers GetModifier()
        {
            if(keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt))
            {
                return KeyModifiers.Alt;
            }

            if (keyboardState.IsKeyDown(Keys.LeftControl) || keyboardState.IsKeyDown(Keys.RightControl))
            {
                return KeyModifiers.Control;
            }

            if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
            {
                return KeyModifiers.Shift;
            }

            if (keyboardState.IsKeyDown(Keys.LeftWindows) || keyboardState.IsKeyDown(Keys.RightWindows))
            {
                return KeyModifiers.Windows;
            }

            return KeyModifiers.None;
        }
    }
}