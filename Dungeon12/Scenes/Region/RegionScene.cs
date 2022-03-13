using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon12.ECS.Systems;
using Dungeon12.SceneObjects.HeroPanelObjs;
using Dungeon12.SceneObjects.RegionScreen;
using Dungeon12.SceneObjects.RegionSkillPanel;

namespace Dungeon12.Scenes
{
    public class RegionScene : GameScene<StartScene, RegionScene>
    {
        public RegionScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void Initialize()
        {
            var back = CreateLayer("background");
            back.AddObject(new ImageObject("Regions/background.png"));
            back.AddObject(new AreaImage());
            back.AddObject(new ImageObject("UI/layout/mainframe_a.png"));
            back.AddObject(new ImageObject("UI/layout/mainframe_c.png")
            {
                Top = 864
            });

            var ui = CreateLayer("ui");

            ui.AddSystem(new TooltipSystem());
            ui.AddSystem(new MouseHintSystem());
            ui.AddSystem(new CursorSystem());

            ui.AddObject(new ButtonPanel());

            ui.AddObject(new HeroPanel(Global.Game.Party.Hero1)
            {
                Left = 427,
                Top = 887
            });

            ui.AddObject(new HeroPanel(Global.Game.Party.Hero2)
            {
                Left = 712,
                Top = 887
            });

            ui.AddObject(new HeroPanel(Global.Game.Party.Hero3)
            {
                Left = 997,
                Top = 887
            });

            ui.AddObject(new HeroPanel(Global.Game.Party.Hero4)
            {
                Left = 1282,
                Top = 887
            });

            ui.AddObject(new FoodPanel(Global.Game.Party)
            {
                Left = 1530,
                Top = 45
            });

            ui.AddObject(new MapRegionTitle(Global.Game.MapRegion));
            ui.AddObject(new MapRegionPoints(Global.Game.MapRegion));
            ui.AddObject(new InfluencePanel(Global.Game.MapRegion)
            {
                Left = 1685,
                Top = 130
            });

            ui.AddObject(new TextWindow(Global.Game.Log));

            ui.AddObject(new QuestBar(Global.Game.QuestBook));
        }

        public override void Loaded()
        {
            Global.Game.Log.Push("Вы прибываете на Остров Веры");
        }

        private void CharacterSkills() { }

        private void ItemsAndInventory() { }

        private void Quests() { }

        private void EscapeMenu() { }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape)
            {
                this.Switch<StartScene>();
            }

            base.KeyPress(keyPressed, keyModifiers, hold);
        }
    }
}
