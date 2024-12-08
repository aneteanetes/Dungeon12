using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Monogame;
using Dungeon.Resources;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon.Tiled;
using Dungeon12.SceneObjects.Playing;
using Dungeon12.SceneObjects.GlobalMap;
using Dungeon12.SceneObjects.Map;

namespace Dungeon12.Scenes
{
    internal class GlobalMapScene : GameScene<MenuScene>
    {
        public GlobalMapScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override bool IsPreloadedScene => true;

        public override void Initialize()
        {
            var back = this.AddLayer("back");
            back.AddObject(new ImageObject("Backgrounds/Regions/sea.jpg"));

            var world = back.AddObject(new WorldSceneObject(Global.Game.World));
            world.SetCoords(48, 35);

            var portraitsY = 30d;

            foreach (var hero in Global.Game.Party.Heroes)
            {
                var port = back.AddObject(new PortraitHero(hero)
                {
                    Left = 30,
                    Top = portraitsY,
                    AbsolutePosition=true
                });

                portraitsY += port.Height + 30d;
            }

            back.AddObject(new StatusBar(Global.Game));
            back.AddObject(new TextWindow(Global.Game));
            back.AddObject(new Information(Global.Game));
            back.AddObject(new MapMoveBar(Global.Game));
        }

        protected override IEnumerable<string> LoadResourcesNames()
        {
            var names = new List<string>();

            names.AddRange(Global.Game.Party.Heroes.Select(x => x.Avatar));
            names.Add(ImageObject.MakeImagePath("Backgrounds/Regions/sea.jpg"));

            return names;
        }

        public override void Load()
        {
            LoadWorld();
            base.Load();
        }

        private void LoadWorld()
        {
            var res = LoadResource("Maps/World.tmx");
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
