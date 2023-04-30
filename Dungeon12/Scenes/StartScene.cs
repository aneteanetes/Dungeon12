using Dungeon;
using Dungeon.Control.Gamepad;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon.Types;
using Dungeon12.SceneObjects.UserInterface.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.Scenes
{
    internal class StartScene : StartScene<TCGScene, CreateScene>
    {
        public StartScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void Initialize()
        {
            Global.AudioPlayer.Music("Main.ogg".AsmMusicRes());
            Global.GameClient.SetCursor("Cursors/common.png".AsmImg());

            var layerBack = this.CreateLayer("back");
            layerBack.AddObject(new ImageObject("back.png".AsmImg())
            {
                Width = Global.Resolution.Width,
                Height = Global.Resolution.Height
            });
            //layerBack.AddObjectCenter(new ImageObject("d12textM.png".AsmImg()), vertical: false);

            var ui = this.CreateLayer("ui");
            InitButtons(ui);

            //var snow = this.CreateLayer("snow");
            //snow.AddObject(new BackgroundSnow());
        }

        private void InitButtons(SceneLayer ui)
        {
            var data = new (string text, Action click, bool disabled)[]
            {
                (Strings["NewGame"],NewGame,false),
                (Strings["Save"],SaveGame,!InGame),
                (Strings["Load"],LoadGame,true),
                (Strings["Settings"],Settings,true),
                (Strings["Credits"],TCG,true),
                (Strings["ExitGame"],Exit,false)
            };

            var y = 450;
            var x = 60;

            foreach (var (text, click, disabled) in data)
            {
                var btn = new ClassicButton(text)
                {
                    Left = x,
                    Top = y,
                    Disabled = disabled,
                    OnClick = click
                };
                btns.Add(btn);
                ui.AddObject(btn);
                y += 80;
            }

            if (DungeonGlobal.GamePadConnected)
            {
                btns.FirstOrDefault().Focus();
                currentFocus = btns.FirstOrDefault();
            }
        }

        protected override void StickMoveOnce(Direction direction, GamePadStick stick)
        {
            if (stick == GamePadStick.RightStick)
                return;

            var currentIdx = button_idx;
            bool up = false;

            if (direction == Direction.Up || direction == Direction.UpLeft || direction == Direction.UpRight)
            {
                up = true;
                button_idx -= 1;
            }


            if (direction == Direction.Down || direction == Direction.DownLeft || direction == Direction.DownRight)
            {
                button_idx += 1;
            }

            if (button_idx < 0)
                button_idx = 0;

            if (button_idx > btns.Count - 1)
                button_idx = btns.Count - 1;

            if (currentIdx == button_idx)
                return;

            currentFocus.Unfocus();
            currentFocus = FindIdx(up);
            currentFocus.Focus();

            base.StickMoveOnce(direction, stick);
        }

        private ClassicButton FindIdx(bool up)
        {
            currentFocus = btns.ElementAtOrDefault(button_idx);
            if (currentFocus.Disabled)
            {
                if (up)
                    button_idx--;
                else
                    button_idx++;

                if (button_idx > btns.Count - 1)
                    button_idx = btns.Count - 1;

                return FindIdx(up);
            }
            return currentFocus;
        }

        protected override void GamePadButtonPress(GamePadButton[] btns)
        {
            if (btns.Contains(GamePadButton.A))
            {
                currentFocus.Click(default);
            }

            base.GamePadButtonPress(btns);
        }

        private int button_idx = 0;
        List<ClassicButton> btns = new();
        ClassicButton currentFocus;

        private void NewGame()
        {
            this.Switch<CreateScene>();
        }

        private void SaveGame()
        {
        }

        private void LoadGame()
        {
            Global.AudioPlayer.Music("test3");
        }

        private void Settings()
        {
        }

        private void TCG()
        {
            this.Switch<TCGScene>();
        }

        private void Exit()
        {
            DungeonGlobal.Exit?.Invoke();
        }
    }
}