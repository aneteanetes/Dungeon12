using Dungeon.Drawing.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Nabunassar.Extensions.Resources;
using Nabunassar.SceneObjects.HUD;
using Nabunassar.SceneObjects.UserInterface.Common;
using Nabunassar.Scenes.Creating.Character;
using Nabunassar.Scenes.Creating.Heroes;
using Nabunassar.Scenes.Start;

namespace Nabunassar.Scenes.Creating
{
    internal class CreateScene : GameScene<NabLoadingScreen, MenuScene, RegionScene, CreateHeroScene, GlobalMapScene>
    {
        public CreateScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void Initialize()
        {
            Global.Game = new Game.GameState();
            AudioPlayer.Music("CreateParty.ogg".AsmMusicRes());

            var backlayer = CreateLayer("back");
            backlayer.AddObject(new ImageObject("Scenes/create.png")
            {
                Width = DungeonGlobal.Resolution.Width,
                Height = DungeonGlobal.Resolution.Height
            });

            var layer = CreateLayer("main");
            layer.AbsoluteLayer = true;

            var title = new TextPanelFade(Global.Strings["CreateParty"],45,.8);
            layer.AddObjectCenter(title, vertical: false);
            title.Top = 25;

            var csbtns = layer.AddCancelNextBtns<MenuScene>();
            csbtns.next.OnClick = StartGame;

            double plus = 50;
            double left = plus;

            var hero1 = layer.AddObject(new HeroCreatePanel(null,0)
            {
                Left = left,
                Top = 215
            });
            left += hero1.Width+ plus;

            var hero2 = layer.AddObject(new HeroCreatePanel(null,1)
            {
                Left = left,
                Top = 215
            });
            left += hero2.Width+ plus;

            var hero3 = layer.AddObject(new HeroCreatePanel(null,2)
            {
                Left = left,
                Top = 215
            });
            left += hero3.Width + plus;

            var hero4 = layer.AddObject(new HeroCreatePanel(null,3)
            {
                Left = left,
                Top = 215
            });
        }

        private void StartGame()
        {
            this.Switch<GlobalMapScene>();
        }

        public override void Load()
        {
            Resources.Load("UI/start/title.png".AsmImg());
            Resources.Load("CreateParty.ogg".AsmMusicRes());
            Resources.Load("Scenes/create.png".AsmImg());
            Resources.Load("UI/btn_a.png".AsmImg());
            Resources.Load("UI/bordermin/bord1.png".AsmImg());
            Resources.LoadFolder("UI/panelmin".AsmImg());
            this.LoadBorders();
        }

        public override void Update(GameTimeLoop gameTimeLoop)
        {
            base.Update(gameTimeLoop);
        }
    }
}
