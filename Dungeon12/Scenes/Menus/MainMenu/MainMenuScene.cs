using Dungeon;
using Dungeon.Control.Gamepad;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon.Types;
using Dungeon12.Scenes.Game;
using Dungeon12.Scenes.Menus.MainMenu;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.Scenes.Menus
{
    public class MainMenuScene : StartScene<MainMenuScene, GameplayScene>
    {
        public MainMenuScene(SceneManager sceneManager) : base(sceneManager)
        {
        }


        public override bool AbsolutePositionScene => true;

        public override bool Destroyable => false;

        public override void Initialize()
        {
            Global.DrawClient.SetCursor("Cursors.common.png".PathImage());

            var inGame = Args.ElementAtOrDefault(0) != default;

            var back = this.CreateLayer("Background");
            back.AddObject(new ImageControl("Splash/MainMenu/menu.jfif".AsmImgRes()));
            back.AddObject(new ImageControl("Splash/d12.png".AsmImg())
            {
                Left = 332,
                Top = 16
            });
            back.AddObject(new NewsSceneObject("GUI/Planes/news.png".AsmImg())
            {
                Left = 1380,
                Top = 485
            });

            back.AddObject(new DarkRectangle()
            {
                Width = 370,
                Height = 504,
                Left=125,
                Top=430,
                Opacity = .6
            });

            var data = new (string text, Action click, bool disabled)[]
            {
                (Global.Strings.NewGame,NewGame,false),
                (Global.Strings.Save,SaveGame,!inGame),
                (Global.Strings.Load,LoadGame,true),
                (Global.Strings.Settings,Settings,false),
                (Global.Strings.Credits,Credits,false),
                (Global.Strings.ExitGame,Exit,false)
            };

            var y = 455;
            var x = 130;

            foreach (var item in data)
            {
                var btn =new MainMenuButton(item.Item1)
                {
                    Left = x,
                    Top = y,
                    Disabled = item.disabled,
                    OnClick = item.click
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

        protected override void LeftStickMoveOnce(Direction direction, Distance distance)
        {
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

            base.LeftStickMoveOnce(direction, distance);
        }

        private MainMenuButton FindIdx(bool up)
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

        private int button_idx=0;
        List<MainMenuButton> btns = new List<MainMenuButton>();
        MainMenuButton currentFocus;

        private void NewGame()
        {
            this.Switch<GameplayScene>();
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

        private void Credits()
        {

        }

        private void Exit()
        {
            Global.Exit?.Invoke();
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            base.KeyPress(keyPressed, keyModifiers, hold);
        }
    }
}