using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon12.ECS.Systems;
using Dungeon12.Extensions.Resources;
using Dungeon12.SceneObjects.Create;
using Dungeon12.SceneObjects.HUD;
using Dungeon12.SceneObjects.UserInterface.Common;
using Dungeon12.Scenes.Start;
using static System.Net.Mime.MediaTypeNames;

namespace Dungeon12.Scenes
{
    internal class CreateScene : GameScene<NabLoadingScreen, MenuScene, RegionScene, MUDScene, GlobalMapScene>
    {
        public CreateScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void Initialize()
        {
            DungeonGlobal.AudioPlayer.Music("CreateParty.ogg".AsmMusicRes());

            var backlayer = CreateLayer("back");
            backlayer.AddObject(new ImageObject("Scenes/create.png")
            {
                Width = DungeonGlobal.Resolution.Width,
                Height = DungeonGlobal.Resolution.Height
            });

            var layer = CreateLayer("main");
            layer.AbsoluteLayer = true;

            var title = new CreateTitle();
            layer.AddObjectCenter(title, vertical: false);
            title.Top = 25;

            var cancelBtn = new ClassicButton(Strings["Cancel"])
            {
                Left = Global.Resolution.CenterH(250) - 250/2 - 25,
                Top = Global.Resolution.Height-100,
                Disabled = false,
                OnClick = CancelDialogue
            };

            var completeBtn = new ClassicButton(Strings["Complete"])
            {
                Left = Global.Resolution.CenterH(250) + 250 / 2 + 25,
                Top = Global.Resolution.Height - 100,
                Disabled = false,
                OnClick = CancelDialogue
            };

            layer.AddObject(cancelBtn);
            layer.AddObject(completeBtn);
        }

        private void CancelDialogue()
        {
            var box = new DialogueBox(Strings["Yes"], Strings["Cancel"], Strings["AreYouSureAbort"]);
            box.OnLeft = () => this.Switch<MenuScene>();
            box.OnRight = box.Destroy;

            Global.Freezer.Freeze(box);

            this.ActiveLayer.AddObjectCenter(box);
        }

        public override void Load()
        {
            this.Resources.Load("UI/start/title.png".AsmImg());
            this.Resources.Load("CreateParty.ogg".AsmMusicRes());
            this.Resources.Load("Scenes/create.png".AsmImg());
            this.Resources.Load("UI/btn_a.png".AsmImg());
            this.LoadBorders();
        }
    }
}
