using Dungeon.Control.Gamepad;
using Dungeon.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dungeon.Monogame
{
    public partial class GameClient
    {
        public void UpdateGamepadEvents()
        {
            var state = GamePad.GetState(0);
            IsMouseVisible = !state.IsConnected;
            DungeonGlobal.GamePadConnected = state.IsConnected;

            if (!state.IsConnected)
                return;

            UpdateSticks(state);

            var nowBtns = state.Buttons;
            OnGamePadButtons(nowBtns);
            wasBtns = nowBtns;
        }

        GamePadButtons wasBtns;

        public void OnGamePadButtons(GamePadButtons nowBtns)
        {
            List<(ButtonState state, GamePadButton btn)> states = new List<(ButtonState, GamePadButton)>();
            if (nowBtns.A != wasBtns.A)
            {
                states.Add((nowBtns.A, GamePadButton.A));
            }
            if (nowBtns.B != wasBtns.B)
            {
                states.Add((nowBtns.B, GamePadButton.B));
            }
            if (nowBtns.X != wasBtns.X)
            {
                states.Add((nowBtns.X, GamePadButton.X));
            }
            if (nowBtns.Y != wasBtns.Y)
            {
                states.Add((nowBtns.Y, GamePadButton.Y));
            }
            if (nowBtns.Back != wasBtns.Back)
            {
                states.Add((nowBtns.Back, GamePadButton.Back));
            }
            if (nowBtns.BigButton != wasBtns.BigButton)
            {
                states.Add((nowBtns.BigButton, GamePadButton.BigButton));
            }
            if (nowBtns.LeftShoulder != wasBtns.LeftShoulder)
            {
                states.Add((nowBtns.LeftShoulder, GamePadButton.LeftShoulder));
            }
            if (nowBtns.RightShoulder != wasBtns.RightShoulder)
            {
                states.Add((nowBtns.RightShoulder, GamePadButton.RightShoulder));
            }
            if (nowBtns.LeftStick != wasBtns.LeftStick)
            {
                states.Add((nowBtns.LeftStick, GamePadButton.LeftStick));
            }
            if (nowBtns.RightStick != wasBtns.RightStick)
            {
                states.Add((nowBtns.RightStick, GamePadButton.RightStick));
            }
            if (nowBtns.Start != wasBtns.Start)
            {
                states.Add((nowBtns.Start, GamePadButton.Start));
            }
            var pressed = states.Where(x => x.state == ButtonState.Pressed).Select(x => x.btn).ToArray();
            var released = states.Where(x => x.state == ButtonState.Released).Select(x=>x.btn).ToArray();

            SceneManager.Current?.OnGamePadButtons(pressed,true);
            SceneManager.Current?.OnGamePadButtons(released, false);
        }

        private void UpdateSticks(GamePadState gamePadState)
        {
            UpdateLeft(gamePadState);
            UpdateRight(gamePadState);
        }

        private void UpdateLeft(GamePadState gamePadState)
        {
            var left = gamePadState.ThumbSticks.Left;
            if (left != Vector2.Zero)
            {
                var dir = DetectDirection(leftStickWas.X, left.X, leftStickWas.Y, left.Y);

                if (startDirLeft == Direction.Idle)
                {
                    startDirLeft = dir;
                }

                if (dir == startDirLeft)
                {
                    leftStickWas = left;
                }
                OnStickMove(left, GamePadStick.LeftStick);
            }
            else if (leftStickWas != Vector2.Zero && left == Vector2.Zero)
            {
                OnLeftStickMoveOnce(left, GamePadStick.LeftStick);
            }
        }

        private void UpdateRight(GamePadState gamePadState)
        {
            var right = gamePadState.ThumbSticks.Right;
            if (right != Vector2.Zero)
            {
                var dir = DetectDirection(rightStickWas.X, right.X, rightStickWas.Y, right.Y);

                if (startDirRight == Direction.Idle)
                {
                    startDirRight = dir;
                }

                if (dir == startDirRight)
                {
                    rightStickWas = right;
                }
                OnStickMove(right, GamePadStick.RightStick);
            }
            else if (rightStickWas != Vector2.Zero && right == Vector2.Zero)
            {
                OnRightStickMoveOnce(right, GamePadStick.RightStick);
            }
        }

        #region left stick

        Direction startDirLeft = Direction.Idle;
        Direction startDirRight = Direction.Idle;
        Vector2 leftStickWas = new Vector2(0, 0);
        Vector2 rightStickWas = new Vector2(0, 0);

        public void OnLeftStickMoveOnce(Vector2 leftStickNow, GamePadStick stick)
        {
            var dir = DetectDirection(leftStickNow.X, leftStickWas.X, leftStickNow.Y, leftStickWas.Y);
            SceneManager.Current?.OnStickMoveOnce(dir, stick);
            startDirLeft = Direction.Idle;
            leftStickWas = default;
        }

        public void OnRightStickMoveOnce(Vector2 rightStickNow, GamePadStick stick)
        {
            var dir = DetectDirection(rightStickNow.X, rightStickWas.X, rightStickNow.Y, rightStickWas.Y);
            SceneManager.Current?.OnStickMoveOnce(dir, stick);
            startDirRight = Direction.Idle;
            rightStickWas = default;
        }

        Vector2 leftStickWasContinue = new Vector2(0, 0);
        public void OnStickMove(Vector2 leftStickNow, GamePadStick stick)
        {
            var dir = DetectDirection(leftStickWasContinue.X, leftStickNow.X, leftStickWasContinue.Y, leftStickNow.Y);
            SceneManager.Current?.OnStickMove(dir, stick);
            leftStickWasContinue = default;
        }

        #endregion

        protected Direction DetectDirection(float x1, float x2, float y1, float y2)
        {
            Direction dirX = Direction.Idle;
            Direction dirY = Direction.Idle;

            if (x1 < x2)
            {
                dirX = Direction.Left;
            }
            if (x1 > x2)
            {
                dirX = Direction.Right;
            }

            if (y1 > y2)
            {
                dirY = Direction.Down;
            }

            if (y1 < y2)
            {
                dirY = Direction.Up;
            }

            return (Direction)((int)dirX + (int)dirY);
        }
    }
}