using Dungeon;
using Dungeon.Control.Gamepad;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon.Types;
using SidusXII.Enums;
using SidusXII.SceneObjects.Base;
using SidusXII.Scenes.Creation;
using SidusXII.Scenes.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SidusXII.Scenes.MainMenu
{
    public class MainMenuScene : StartScene<RaceScene, MainScene>
    {
        public MainMenuScene(SceneManager sceneManager) : base(sceneManager)
        {
        }


        public override bool AbsolutePositionScene => true;

        public override bool Destroyable => false;

        public override void Initialize()
        {
            var inGame = Args?.ElementAtOrDefault(0) != default;

            var back = CreateLayer("Background");
            back.AddObject(new ImageObject("Common/splash.jpg".AsmImg()));
            //back.AddObject(new NewsSceneObject("GUI/Planes/news.png".AsmImg())
            //{
            //    Left = 1380,
            //    Top = 485
            //});

            back.AddObject(new DarkRectangle()
            {
                Width = 370,
                Height = 504,
                Left = 125,
                Top = 200,
                Opacity = .6
            });

            var data = new (string text, Action click, bool disabled)[]
            {
                (Global.Strings.NewGame,NewGame,false),
                (Global.Strings.Save,SaveGame,!inGame),
                (Global.Strings.Load,LoadGame,true),
                (Global.Strings.Settings,Settings,false),
                (Global.Strings.FastGame,FastGame,false),
                (Global.Strings.ExitGame,Exit,false)
            };

            var y = 200;
            var x = 60;

            foreach (var (text, click, disabled) in data)
            {
                var btn = new Button(text)
                {
                    Left = x,
                    Top = y,
                    Disabled = disabled,
                    OnClick = click
                };
                btns.Add(btn);
                back.AddObject(btn);
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

        private Button FindIdx(bool up)
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
        List<Button> btns = new List<Button>();
        Button currentFocus;

        private void NewGame()
        {
            Global.Game = new SidusXII.Game();
            this.Switch<RaceScene>();
        }

        private void SaveGame()
        {

        }

        private void LoadGame()
        {

        }

        private void Settings()
        {

        }

        private void FastGame()
        {
            Global.Game = new SidusXII.Game();
            this.Switch<MainScene>();
        }

        private void Exit()
        {
            DungeonGlobal.Exit?.Invoke();
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            base.KeyPress(keyPressed, keyModifiers, hold);
        }
    }
}