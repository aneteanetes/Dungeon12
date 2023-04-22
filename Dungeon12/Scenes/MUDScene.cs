using Dungeon;
using Dungeon.Control.Keys;
using Dungeon.Scenes;
using Dungeon.Scenes.Manager;
using Dungeon12.ECS.Systems;
using Dungeon12.Entities.Map;
using Dungeon12.SceneObjects.Base;
using Dungeon12.SceneObjects.MUD;
using Dungeon12.SceneObjects.MUD.Controls;
using Dungeon12.SceneObjects.MUD.Info;
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
            var main = AddLayer("main");

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

            main.AddSystem(new TooltipDrawTextSystem());
            main.AddSystem(new MouseHintSystem());
            main.AddSystem(new CursorSystem());

            main.AddObject(new Border(400, 30)); // left status bar
            main.AddObject(new Border(400, 30) {  Left=1520 }); // right status bar

            var stepCounter = main.AddObject(new TurnPanel(new Entities.TurnOrder()) {  Left = 400}); // left status bar

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

            Global.Game.Party.Hero1.Chip = $"Chips/w1.png".AsmImg();
            Global.Game.Party.Hero2.Chip = $"Chips/m1.png".AsmImg();
            Global.Game.Party.Hero3.Chip = $"Chips/t1.png".AsmImg();
            Global.Game.Party.Hero4.Chip = $"Chips/p1.png".AsmImg();

            location[2, 4].Object=new Entities.Objects.MapObject()
            {
                Icon=$"Chips/w1.png".AsmImg(),
                Name = Global.Game.Party.Hero1.Name
            };
            location[3, 4].Object=new Entities.Objects.MapObject()
            {
                Icon=$"Chips/m1.png".AsmImg(),
                Name = Global.Game.Party.Hero2.Name
            };
            location[4, 4].Object=new Entities.Objects.MapObject()
            {
                Icon="Chips/t1.png".AsmImg(),
                Name = Global.Game.Party.Hero3.Name
            };
            location[5, 4].Object=new Entities.Objects.MapObject()
            {
                Icon="Chips/p1.png".AsmImg(),
                Name = Global.Game.Party.Hero4.Name
            };
            location[5, 2].Object=new Entities.Objects.MapObject()
            {
                Icon ="Map/Objects/chestc1.tga".AsmImg(),
                Name=$"Сундук {Strings[Global.Game.Party.Fraction.ToString()+"-whom"]}"
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
