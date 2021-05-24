using Dungeon.Control.Keys;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using SidusXII.SceneObjects.Main;
using SidusXII.SceneObjects.Main.Map;
using SidusXII.Scenes.MainMenu;

namespace SidusXII.Scenes.Game
{
    public class MainScene : GameScene<MainMenuScene>
    {
        public MainScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => false;

        public override void Initialize()
        {
            var ui = this.CreateLayer("ui");
            ui.AddObject(new StatusBar());
            ui.AddObject(new MapContainer() { Top = 33, });
            ui.AddObject(new PerksView() { Top = 531, Left = 1233 });
            ui.AddObject(new ObjectInspector() { Left = 1232, Top = 33 });

            ui.AddObject(new CharBar() { Top = 743, Left = 0 });
            ui.AddObject(new CharBar() { Top = 743, Left = 411 });
            ui.AddObject(new CharBar() { Top = 743, Left = 822 });
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape)
            {
                this.Switch<MainMenuScene>();
            }
        }
    }
}