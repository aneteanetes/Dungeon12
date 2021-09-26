using Dungeon;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using SidusXII.Layers.Main;
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

        public override bool Destroyable => true;

        public override void Initialize()
        {
            var map = this.CreateLayer<MapLayer>("map");
            map.Top = 33;
            map.AddObject(new MapSceneObject(Global.Game.Location) {/* Top = 33, */});
            map.AddObject(new ImageObject("GUI/Planes/maphd_border.png".AsmImg()) { Width = 1600, Height = 710 });


            var ui = this.CreateLayer("ui");
            ui.AddObject(new StatusBar());
            //ui.AddObject(new ScreenImageBox() { Left = 1232, Top = 33 });
            //ui.AddObject(new ObjectListBox() { Left = 1232, Top = 390 });
            //ui.AddObject(new PerksView() { Left = 1233, Top = 690 });

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