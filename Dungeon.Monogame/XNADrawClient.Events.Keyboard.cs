namespace Dungeon.Monogame
{
    using Dungeon.Control.Keys;
    using Dungeon.Scenes.Manager;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using System.Collections.Generic;
    using System.IO;

    public partial class XNADrawClient
    {
        private KeyboardState keyboardState;
        private Keys[] pressed;
        private HashSet<Keys> keysState = new HashSet<Keys>();
        private static HashSet<Keys> keysHolds = new HashSet<Keys>();

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

            foreach (var keyHold in keysHolds)
            {
                OnKeyDown(keyHold);
            }

            this.pressed = pressed;
        }
#if Core
        private void OnTextInput(object sender, TextInputEventArgs e)
        {
            SceneManager.Current?.OnText(e.Character.ToString());
        }
#endif

        private void OnKeyDown(Keys key)
        {
            if (key == Keys.P)
                makingscreenshot = true;

            var hold = keysHolds.Contains(key);

            SceneManager.Current?.OnKeyDown(new Dungeon.Control.Keys.KeyArgs
            {
                Key = (Key)key,
                Modifiers = GetModifier(),
                Hold = hold
            });
        }


        Vector2 shadowMaskPosition = Vector2.Zero;

        private bool makingscreenshot = false;

        private void OnKeyUp(Keys key)
        {
            this.StopMoveCamera();

            keysHolds.Remove(key);

            SceneManager.Current?.OnKeyUp(new Dungeon.Control.Keys.KeyArgs
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