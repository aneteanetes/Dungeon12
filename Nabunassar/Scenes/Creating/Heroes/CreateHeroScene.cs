using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon.View.Interfaces;
using Nabunassar.Entities.Characters;
using Nabunassar.SceneObjects.HUD;
using Nabunassar.Scenes.Creating.Character;
using Nabunassar.Scenes.Creating.Character.Stats;
using Nabunassar.Scenes.Start;

namespace Nabunassar.Scenes.Creating.Heroes
{
    internal class CreateHeroScene : GameScene<NabLoadingScreen, CreateScene>
    {
        private CreatePart _activePart;
        public CreatePart ActivePart
        {
            get => _activePart;
            set {
                _activePart= value;
                _activePart.IsActivated = true;
            }
        }

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


            Global.Game.Creation.Hint = Global.Strings["guide"]["race"];// hero.Race.ToString()];

            var raceSelector = new RaceSelector(hero);
            layer.AddObject(raceSelector);
            ActivePart = raceSelector;


            var cubeoffcet = 50;
            var cubetop = 25;

            var racebtn = new CreatePartCube("Icons/Flat/race.png", Global.Strings["Race"], Global.Strings["guide"]["race"], raceSelector, this)
            {
                Left = cubeoffcet,
                Top = cubetop
            };
            racebtn.SetColor(Global.CommonColorLight);
            layer.AddObject(racebtn);

            var fractionSelector = new FractionSelector(hero);
            fractionSelector.Visible = false;
            layer.AddObject(fractionSelector);

            var fractbtn = new CreatePartCube("Icons/Flat/fraction.png", Global.Strings["Fraction"], Global.Strings["guide"]["fraction"], fractionSelector, this)
            {
                Left = title.Left-cubeoffcet-racebtn.Width,
                Top = cubetop
            };
            layer.AddObject(fractbtn);

            var classSelector = new ClassSelector(hero);
            classSelector.Visible = false;
            layer.AddObject(classSelector);


            var classbtn = new CreatePartCube("Icons/Flat/class.png", Global.Strings["Archetype"], Global.Strings["guide"]["Archetype"], classSelector, this)
            {
                Left = 275,
                Top = cubetop
            };
            layer.AddObject(classbtn);

            var aftertitlespacepart = (Global.Resolution.Width - (title.Left + cubeoffcet * 2) / 2);

            var statEditor = new StatsEditor(hero);
            statEditor.Visible = false;
            layer.AddObject(statEditor);

            var statsbtn = new CreatePartCube("Icons/Flat/stats.png", Global.Strings["Stats"], Global.Strings["guide"]["stats"], statEditor, this)
            {
                Left = 1300,
                Top = cubetop
            };
            layer.AddObject(statsbtn);

            var nameEditor = new NameSelector(hero);
            nameEditor.Visible = false;
            layer.AddObject(nameEditor);

            var namebtn = new CreatePartCube("Icons/Flat/name.png", Global.Strings["Name"], Global.Strings["guide"]["name"], nameEditor, this)
            {
                Left = 1535,
                Top = cubetop
            };
            layer.AddObject(namebtn);

            var abilSelector = new AbilitySelector(hero);
            abilSelector.Visible = false;
            layer.AddObject(abilSelector);

            var abilbtn = new CreatePartCube("Icons/Flat/abilities.png", Global.Strings["Abilities"], Global.Strings["guide"]["CreateAbilities"], abilSelector, this)
            {
                Left = 1750,
                Top = cubetop
            };
            layer.AddObject(abilbtn);

            racebtn.Next = classbtn;
            racebtn.Visible = true;

            classbtn.Next = fractbtn;
            fractbtn.Next = statsbtn;
            statsbtn.Next = namebtn;
            namebtn.Next = abilbtn;

            var hints = new CreateDescWindow();
            hints.Top = 300;
            hints.Left = Global.Resolution.Width - 50 - hints.Width;
            layer.AddObject(hints);

            var ctrlbtns = layer.AddCancelNextBtns<CreateScene>();
            ctrlbtns.next.OnClick = Next;
        }

        private void Next()
        {
            if (ActivePart.Cube.Next?.Visible ?? false == true)
            {
                ActivePart.Cube.Next.Click(null);
            }
        }

        public override void Load()
        {
        }
    }
}
