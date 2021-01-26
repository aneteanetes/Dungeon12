using Dungeon;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon12.Scenes.Game;
using Dungeon12.Scenes.Menus.MainMenu;

namespace Dungeon12.Scenes.Menus
{
    public class MainMenuScene : StartScene<MainMenuScene, GameplayScene>
    {
        public MainMenuScene(SceneManager sceneManager) : base(sceneManager)
        {
        }


        public override bool AbsolutePositionScene => true;

        public override bool Destroyable => true;

        public override void Initialize()
        {
            var back = this.CreateLayer("Background");
            back.AddObject(new ImageControl("Splash/MainMenu/menu.jfif".AsmImgRes()));
            back.AddObject(new ImageControl("Splash/d12.png".AsmImg())
            {
                Left = 332,
                Top = 16
            });
            back.AddObject(new NewsSceneObject("GUI/Planes/news.png".AsmImg())
            {
                Left = 92,
                Top = 441
            });
            back.AddObject(new MainMenuButton("Новая игра")
            {
                Left = 800,
                Top = 550,
                OnClick = NewGame
            });
            back.AddObject(new MainMenuButton("Загрузить")
            {
                Left = 800,
                Top = 700,
                OnClick = Load
            });
            back.AddObject(new MainMenuButton("Выйти")
            {
                Left = 800,
                Top = 850,
                OnClick = Exit
            });
        }

        private void NewGame()
        {
            this.Switch<GameplayScene>();
        }

        private void Load()
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