using Dungeon;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Resources;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon12.ECS.Systems;
using Dungeon12.Entities.Abilities;
using Dungeon12.Entities.Enums;
using Dungeon12.Entities.Map;
using Dungeon12.SceneObjects.Create;

namespace Dungeon12.Scenes.Create
{
    public class CreateScene : GameScene<StartScene, RegionScene>
    {
        public CreateScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void Initialize()
        {
            InitGame();
            Global.AudioPlayer.Music("test4.ogg".AsmMusicRes());

            var backlayer = this.CreateLayer("back");
            backlayer.AddObject(new ImageObject("Scenes/create.png")
            {
                Width = Global.Resolution.Width,
                Height = Global.Resolution.Height
            });

            var layer = this.CreateLayer("main");
            layer.AbsoluteLayer = true;
            layer.AddSystem(new TooltipDrawTextSystem());
            layer.AddSystem(new TooltipCustomSystem());
            layer.AddSystem(new MouseHintSystem());

            var title = new CreateTitle();
            layer.AddObjectCenter(title, vertical: false);
            title.Top = 25;

            layer.AddObject(new ArrowBtn()
            {
                Left = 984,
                Top = 988,
                OnClick = () => Next()
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
                    Hero1 = new Entities.Hero()
                    {
                        Class = Entities.Enums.Archetype.Warrior
                    },
                    Hero2 = new Entities.Hero()
                    {
                        Class = Entities.Enums.Archetype.Mage
                    },
                    Hero3 = new Entities.Hero()
                    {
                        Class = Entities.Enums.Archetype.Thief
                    },
                    Hero4 = new Entities.Hero()
                    {
                        Class = Entities.Enums.Archetype.Priest
                    },
                },
                Calendar = new Entities.Calendar(),
                Log = new Entities.GameLog(),
                QuestBook = new Entities.Quests.QuestBook()
            };
        }

        private void Next()
        {
            foreach (var hero in Global.Game.Party.Heroes)
            {
                if(hero.Name.IsEmpty())
                {
                    hero.Name = Global.Strings[$"{hero.Class}{hero.Sex}"];
                }

                hero.Abilities = new System.Collections.Generic.List<Ability>(Ability.ByClass(hero.Class));
                hero.BindSkills();

                Global.Game.Party.Food.Init();

                //foreach (var startFood in Global.Game.Party.Food.Components)
                //{
                //    startFood.Value = 5;
                //    startFood.Image = "Icons/Food/apple.png".AsmImg();
                //    startFood.Name = Global.Strings.Apples;
                //}
                var startFood = Global.Game.Party.Food.Components[0];
                startFood.Value = 5;
                startFood.Image = "Icons/Food/apple.png".AsmImg();
                startFood.Name = Global.Strings["Apples"];

                Global.Game.MapRegion = ResourceLoader.LoadJson<MapRegion>("Regions/FaithIsland.json".AsmRes());
                Global.Game.MapRegion.BuildGraph();
            }


            this.Switch<RegionScene>();
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed== Key.Escape)
                this.Switch<StartScene>();
            base.KeyPress(keyPressed, keyModifiers, hold);
        }
    }
}
