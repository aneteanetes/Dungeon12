using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Nabunassar.SceneObjects.HUD;
using Nabunassar.Scenes.Creating.Character;
using Nabunassar.Scenes.Start;

namespace Nabunassar.Scenes.Creating.Heroes
{
    internal class CreateHeroScene : GameScene<NabLoadingScreen, CreateScene>
    {
        public CreateHeroScene(SceneManager sceneManager) : base(sceneManager)
        {

        }

        public override bool Destroyable => true;

        public override void Initialize()
        {
            var layer = AddLayer("main");
            var hero = Global.Game.Party[Global.Game.Creation.CharacterCreationPosition];

            layer.AddObject(new HeroCreatingBackground(hero));

            var title = new TextPanelFade(Global.Strings["CreateHero"], 45, .8);
            layer.AddObjectCenter(title, vertical: false);
            title.Top = 25;

            var panel = new HeroCreatePanel(hero, Global.Game.Creation.CharacterCreationPosition);
            panel.Top += 65;
            layer.AddObjectCenter(panel);

            var raceSelector = new RaceSelector(hero);
            raceSelector.Top = 300;
            raceSelector.Left = 50;
            layer.AddObject(raceSelector);


            layer.AddCancelNextBtns<CreateScene>();
        }

        public override void Load()
        {
            this.Resources.LoadFolder("Backgrounds/Races".AsmImg());
        }
    }
}
