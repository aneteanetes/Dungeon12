using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Monogame;
using Dungeon.Resources;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon.Tiled;
using Dungeon12.SceneObjects.GlobalMap;
using Dungeon12.SceneObjects.Map;

namespace Dungeon12.Scenes
{
    internal class GlobalMapScene : GameScene<StartScene>
    {
        public GlobalMapScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => false;

        public override bool IsPreloadedScene => true;

        public override void Initialize()
        {
            var back = this.AddLayer("back");
            back.AddObject(new ImageObject("Backgrounds/Regions/sea.jpg"));

            var portraitsY = 30d;

            foreach (var hero in Global.Game.Party.Heroes)
            {
                var port = back.AddObject(new PortraitHero(hero)
                {
                    Left = 30,
                    Top = portraitsY
                });

                portraitsY += port.Height + 30d;
            }

            back.AddObject(new WorldSceneObject(Global.Game.World));
        }

        protected override IEnumerable<string> LoadResourcesNames()
        {
            var names = new List<string>();

            names.AddRange(Global.Game.Party.Heroes.Select(x => x.Avatar));
            names.Add(ImageObject.MakeImagePath("Backgrounds/Regions/sea.jpg"));

            return names;
        }

        public override void LoadResources()
        {
            LoadWorld();
            base.LoadResources();
        }

        private void LoadWorld()
        {
            var res = LoadResource("Maps/World.tmx");
            var tiled = TiledMap.Load(res);
            Global.Game.World = new Entities.Map.World(tiled);

            foreach (var tileSet in tiled.Tilesets)
            {
                foreach (var tile in tileSet.Tiles)
                {
                    LoadResource(tile.File);
                }
            }
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape)
                Switch<StartScene>();
            base.KeyPress(keyPressed, keyModifiers, hold);
        }
    }
}
