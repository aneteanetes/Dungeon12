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
            back.AddObject(new AreaImage());
            back.AddObject(new ImageObject("UI/layout/mainframe2.png"));
            back.AddObject(new ImageObject("UI/layout/bot.png")
            {
                Left = 1400,
                Top = 518
            });
            // wip
            back.AddObject(new ImageObject("UI/layout/map.png")
            {
                Left = 1400
            });
            back.AddObject(new ImageObject("Locations/FaithIsland/ship.png")
            {
                Left = 1408,
                Top=11
            });


            back.AddObject(new ImageObject("UI/layout/locationsquares.png")
            {
                Left = 1437,
                Top = 575
            });

            var ui = CreateLayer("ui");

            ui.AddSystem(new TooltipSystem());
            ui.AddSystem(new MouseHintSystem());
            ui.AddSystem(new CursorSystem());

            ui.AddObject(new ControlButton("UI/layout/skillsbtn.png", "[С] Персонажи", CharacterSkills)
            {
                Left=40,
                Top= 895
            });

            ui.AddObject(new ControlButton("UI/layout/itemsbtn.png", "[I] Экипировка", ItemsAndInventory)
            {
                Left=40,
                Top= 980
            });

            ui.AddObject(new ControlButton("UI/layout/questbtn.png", "[Q] Журнал", Quests)
            {
                Left=1290,
                Top=895
            });

            ui.AddObject(new ControlButton("UI/layout/escbtn.png", "[Esc] Меню", EscapeMenu)
            {
                Left = 1290,
                Top = 980
            });

            ui.AddObject(new HeroPanel(Global.Game.Party.Hero1)
            {
                Left=155,
                Top=890
            });

            ui.AddObject(new HeroPanel(Global.Game.Party.Hero2)
            {
                Left = 440,
                Top = 890
            });

            ui.AddObject(new HeroPanel(Global.Game.Party.Hero3)
            {
                Left = 725,
                Top = 890
            });

            ui.AddObject(new HeroPanel(Global.Game.Party.Hero4)
            {
                Left = 1010,
                Top = 890
            });

            ui.AddObject(new SkillPanel(Global.Game.Party));

            ui.AddObject(new FoodPanel(Global.Game.Party)
            {
                Left = 1020,
                Top = 45
            });

            ui.AddObject(new MapRegionTitle(Global.Game.MapRegion));
            ui.AddObject(new MapRegionPoints(Global.Game.MapRegion));
            ui.AddObject(new InfluencePanel(Global.Game.MapRegion)
            {
                Left=1175,
                Top=130
            });
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
