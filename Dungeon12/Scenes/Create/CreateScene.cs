using Dungeon;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon12.ECS.Systems;
using Dungeon12.SceneObjects.Create;
using Dungeon12.SceneObjects.UserInterface.Common;

namespace Dungeon12.Scenes.Create
{
    public class CreateScene : GameScene<StartScene>
    {
        public CreateScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void Initialize()
        {
            InitGame();

            var backlayer = this.CreateLayer("back");
            backlayer.AddObject(new ImageObject("Scenes/create.png")
            {
                Width = Global.Resolution.Width,
                Height = Global.Resolution.Height
            });

            var layer = this.CreateLayer("main");
            layer.AbsoluteLayer = true;
            layer.AddSystem(new TooltipSystem());
            layer.AddSystem(new TooltipCustomSystem());

            var title = new Title();
            layer.AddObjectCenter(title, vertical: false);
            title.Top = 25;

            layer.AddObject(new ArrowBtn()
            {
                Left = 984,
                Top = 988,
                OnClick = () => { }
            });

            layer.AddObject(new ArrowBtn(false)
            {
                Left = 890,
                Top = 988,
                OnClick = () => this.Switch<StartScene>()
            });

            var h1 = layer.AddObject(new Charplate(Global.Game.Party.Hero1)
            {
                Left = 125,
                Top = 190,
            });

            var h2 = layer.AddObject(new Charplate(Global.Game.Party.Hero2)
            {
                Left = h1.Left + h1.Width + 40,
                Top = 190,
            });

            var h3 = layer.AddObject(new Charplate(Global.Game.Party.Hero3)
            {
                Left = h2.Left + h2.Width + 40,
                Top = 190,
            });

            var h4 = layer.AddObject(new Charplate(Global.Game.Party.Hero4)
            {
                Left = h3.Left + h3.Width + 40,
                Top = 190,
            });
        }

        private static void InitGame()
        {
            Global.Game = new Game()
            {
                Party = new Entities.Party()
                {
                    Hero1 = new Entities.Hero(),
                    Hero2 = new Entities.Hero(),
                    Hero3 = new Entities.Hero(),
                    Hero4 = new Entities.Hero()
                },
                Calendar = new Entities.Calendar()
            };
        }
    }
}
