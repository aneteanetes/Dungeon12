using Dungeon.Control.Keys;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon12.ECS.Systems;
using Dungeon12.SceneObjects.Base;
using Dungeon12.SceneObjects.MUD;

namespace Dungeon12.Scenes
{
    internal class MUDScene : GameScene<StartScene>
    {
        public MUDScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => false;

        public override void Initialize()
        {
            var main = AddLayer("main");

            main.AddSystem(new TooltipDrawTextSystem());
            main.AddSystem(new MouseHintSystem());
            main.AddSystem(new CursorSystem());

            main.AddObject(new Border(1920, 30));
            main.AddObject(new Border(400, 400)
            {
                Top = 30
            });
            main.AddObject(new Border(400, 400)
            {
                Top = 430
            });

            main.AddObject(new HeroesPanel(Global.Game.Party)
            {
                Top = 830
            });

            main.AddObject(new Border(1120, 800)
            {
                Top = 30,
                Left = 400
            });
            main.AddObject(new Border(400, 800)
            {
                Top = 30,
                Left = 1520
            });
            main.AddObject(new Border(400, 250)
            {
                Top = 830,
                Left = 1520
            });
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape)
                Switch<StartScene>();
            base.KeyPress(keyPressed, keyModifiers, hold);
        }
    }
}
