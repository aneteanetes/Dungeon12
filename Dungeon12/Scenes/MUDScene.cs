using Dungeon;
using Dungeon.Control.Keys;
using Dungeon.SceneObjects.Grouping;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon12.ECS.Systems;
using Dungeon12.Entities.Map;
using Dungeon12.Entities.Objects.OnMap;
using Dungeon12.SceneObjects.Base;
using Dungeon12.SceneObjects.MUD;
using Dungeon12.SceneObjects.MUD.Controls;
using Dungeon12.SceneObjects.MUD.Info;
using Dungeon12.SceneObjects.MUD.Turning;
using Dungeon12.SceneObjects.MUD.ViewRegion;

namespace Dungeon12.Scenes
{
    internal class MUDScene : GameScene<StartScene>
    {
        public MUDScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void Initialize()
        {
            DungeonGlobal.ScreenshotSaved+=path =>
            {
                Global.Game.Log.Push($"Скриншот сохранён: {path}");
            };

            var main = AddLayer("main");

            var ui = this.AddLayer("ui");

            this.ActiveLayer=main;

            this.AddSystem(new TooltipSystem(ui));
            this.AddSystem(new TooltipCustomSystem(ui));
            this.AddSystem(new MouseHintSystem(ui));
            this.AddSystem(new CursorSystem());

            var region = new Region()
            {
                MapId="sea",
                Title=@"""Волна Света"""
            };

            var location = new Location()
            {
                Name=@"Каюты",
                Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                BackgroundImage="sea.jpg",
                ObjectImage="ship_cockpit.png",
                Region=region
            };

            main.AddObject(new DateTimePanel(Global.Game.Calendar));// left status bar
            main.AddObject(new ResourcePanel(Global.Game.Party) {  Left=1520 }); // right status bar

            var stepCounter = main.AddObject(new TurnPanel(Global.Game.Turns) {  Left = 400}); // left status bar

            main.AddObject(new LocationPreviewImg(location)
            {
                Top = 30
            });

            _=main.AddObject(new MUDMapView(region) // ?
            {
                Top = 430
            });

            main.AddObject(new HeroesPanel(Global.Game.Party) // heroes
            {
                Top = 830
            });

            main.AddObject(new ChatPanel(Global.Game.Log)
            {
                Left = 400,
                Top = 630
            });

            InitField(location);
            main.AddObject(new FieldLocationPanel(location) // center
            {
                Top = stepCounter.Height,
                Left = 400
            });

            main.AddObject(new InfoPanel(Global.Game) // info panel
            {
                Top = 30,
                Left = 1520
            });

            main.AddObject(new ButtonPanel // btns
            {
                Top = 830,
                Left = 1520
            });

            Global.Game.Turns.NewRound();

            //Global.Game.Log.Push("Вы просыпаетесь после шторма в каютах корабля 'Волна света'...");
        }

        private void InitField(Location location)
        {
            var icons = new string[]
            {
                "hk_new-blank_005",
                "hk_new-blank_006"
            };

            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (y % 2 != 0 && x==7)
                        continue;

                    location.Polygons.Add(new Polygon()
                    {
                        Icon = icons[RandomGlobal.Next(0, 1)],
                        X=x,
                        Y=y,
                    });
                }
            }

            location.Init();

            var party = Global.Game.Party;
            Global.Game.Party.Hero1.Chip = $"Chips/w1.png".AsmImg();
            Global.Game.Party.Hero2.Chip = $"Chips/m1.png".AsmImg();
            Global.Game.Party.Hero3.Chip = $"Chips/t1.png".AsmImg();
            Global.Game.Party.Hero4.Chip = $"Chips/p1.png".AsmImg();


            var heromap1 = new HeroMapObject(party.Hero1)
            {
                IsSelected=true,
            };
            var heromap2 = new HeroMapObject(party.Hero2);
            var heromap3 = new HeroMapObject(party.Hero3);
            var heromap4 = new HeroMapObject(party.Hero4);

            var heroObjects = ObjectGroupBuilder<HeroMapObject>.Build(x => x.Selected, heromap1, heromap2, heromap3, heromap4);

            location[2, 4].Object=heromap1;
            location[3, 4].Object=heromap2;
            location[4, 4].Object=heromap3;
            location[5, 4].Object=heromap4;

            location[5, 2].Object=new Entities.Objects.MapObject()
            {
                GameObject=new Entities.Objects.GameObject()
                {
                    Chip="Map/Objects/chestc1.tga".AsmImg(),
                    Name=$"Сундук {Strings[Global.Game.Party.Fraction.ToString()+"-whom"]}"
                }
            };
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape)
                Switch<StartScene>();
            base.KeyPress(keyPressed, keyModifiers, hold);
        }
    }
}
