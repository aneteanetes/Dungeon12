using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon.Tiled;
using Nabunassar.SceneObjects.GlobalMap;
using Nabunassar.SceneObjects.Map;
using Nabunassar.SceneObjects.Playing;
using Nabunassar.Scenes.Start;

namespace Nabunassar.Scenes
{
    internal class GlobalMapScene : GameScene<NabLoadingScreen, MenuScene>
    {
        public GlobalMapScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void Initialize()
        {
            var back = this.AddLayer("back");
            back.AddObject(new ImageObject("Backgrounds/Regions/sea.jpg"));

            var world = back.AddObject(new WorldSceneObject(Global.Game.World));
            world.SetCoords(48, 35);

            var portraitsY = 30d;

            //foreach (var hero in Global.Game.Party.Heroes)
            //{
            //    var port = back.AddObject(new PortraitHero(hero)
            //    {
            //        Left = 30,
            //        Top = portraitsY,
            //        AbsolutePosition=true
            //    });

            //    portraitsY += port.Height + 30d;
            //}

            back.AddObject(new StatusBar(Global.Game));
            back.AddObject(new TextWindow(Global.Game));
            back.AddObject(new Information(Global.Game));
            back.AddObject(new MapMoveBar(Global.Game));
        }

        public override void Load()
        {
            LoadWorld();
            base.Load();
        }

        private void LoadWorld()
        {
            var res = Resources.Load("Maps/World.tmx");
            var tiled = TiledMap.Load(res);
            Global.Game.World = new Entities.Map.World(tiled);
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape)
                Switch<MenuScene>();
            base.KeyPress(keyPressed, keyModifiers, hold);
        }
    }
}
