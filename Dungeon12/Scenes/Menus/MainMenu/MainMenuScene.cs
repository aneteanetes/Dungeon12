using Dungeon;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon12.Scenes.Game;
using Dungeon12.Scenes.Menus.MainMenu;
using System;
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
                back.AddObject(new MainMenuButton(item.Item1)
                {
                    Left = x,
                    Top = y,
                    Disabled = item.disabled,
                    OnClick = item.click
                });

                y += 80;
            }
        }

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