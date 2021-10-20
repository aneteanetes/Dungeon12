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
            //map.AddObject(new ImageObject("GUI/Planes/maphd_border.png".AsmImg()) { Width = 1600, Height = 710 });


            //var ui = this.CreateLayer("ui");
            //ui.AddObject(new StatusBar());
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