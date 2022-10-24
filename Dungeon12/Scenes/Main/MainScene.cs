using Dungeon;
using Dungeon.Control.Keys;
using Dungeon.Monogame.Effects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon.Tiled;
using Dungeon12.SceneObjects.World;

namespace Dungeon12.Scenes.Main
{
    internal class MainScene : GameScene<StartScene, MainScene>
    {
        public MainScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void Initialize()
        {
            var map = this.AddLayer("map");
            map.AddGlobalEffect(new Light2D());
            var tiled = TiledMap.Load($"Maps/test2.tmx".AsmRes());
            var world = new WorldSceneObject(tiled);
            map.AddObjectCenter(world);
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed== Key.Escape)
                this.Switch<StartScene>();

            base.KeyPress(keyPressed, keyModifiers, hold);
        }
    }
}