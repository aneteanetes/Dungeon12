using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Nabunassar.ECS.Systems;
using Nabunassar.Extensions.Resources;
using Nabunassar.SceneObjects.Create;
using Nabunassar.SceneObjects.HUD;
using Nabunassar.SceneObjects.UserInterface.Common;
using Nabunassar.Scenes.Creating.Character;
using Nabunassar.Scenes.Start;
using static System.Net.Mime.MediaTypeNames;

namespace Nabunassar.Scenes.Creating
{
    internal class CreateScene : GameScene<NabLoadingScreen, MenuScene, RegionScene, MUDScene, GlobalMapScene>
    {
        public CreateScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void Initialize()
        {
            AudioPlayer.Music("CreateParty.ogg".AsmMusicRes());

            var backlayer = CreateLayer("back");
            backlayer.AddObject(new ImageObject("Scenes/create.png")
            {
                Width = DungeonGlobal.Resolution.Width,
                Height = DungeonGlobal.Resolution.Height
            });

            var layer = CreateLayer("main");
            layer.AbsoluteLayer = true;

            var title = new TextPanel(Global.Strings["CreateParty"],45);
            layer.AddObjectCenter(title, vertical: false);
            title.Top = 25;

            var cancelBtn = new ClassicButton(Strings["Cancel"])
            {
                Left = DungeonGlobal.Resolution.CenterH(250) - 250 / 2 - 25,
                Top = DungeonGlobal.Resolution.Height - 100,
                Disabled = false,
                OnClick = CancelDialogue
            };

            var completeBtn = new ClassicButton(Strings["Complete"])
            {
                Left = DungeonGlobal.Resolution.CenterH(250) + 250 / 2 + 25,
                Top = DungeonGlobal.Resolution.Height - 100,
                Disabled = true,
                OnClick = StartGame
            };

            layer.AddObject(cancelBtn);
            layer.AddObject(completeBtn);

            double plus = 50;
            double left = plus;

            var hero1 = layer.AddObject(new HeroCreatePanel(null)
            {
                Left = left,
                Top = 215
            });
            left += hero1.Width+ plus;

            var hero2 = layer.AddObject(new HeroCreatePanel(null)
            {
                Left = left,
                Top = 215
            });
            left += hero2.Width+ plus;

            var hero3 = layer.AddObject(new HeroCreatePanel(null)
            {
                Left = left,
                Top = 215
            });
            left += hero3.Width + plus;

            var hero4 = layer.AddObject(new HeroCreatePanel(null)
            {
                Left = left,
                Top = 215
            });
        }

        private void CancelDialogue()
        {
            var box = new DialogueBox(Strings["Yes"], Strings["Cancel"], Strings["AreYouSureAbort"]);
            box.OnLeft = () => Switch<MenuScene>();
            box.OnRight = box.Destroy;

            DungeonGlobal.Freezer.Freeze(box);

            ActiveLayer.AddObjectCenter(box);
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
            this.LoadBorders();
        }
    }
}
