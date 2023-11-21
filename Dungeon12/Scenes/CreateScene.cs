﻿using Dungeon;
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
using Dungeon12.SceneObjects.UserInterface.Common;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace Dungeon12.Scenes
{
    internal class CreateScene : GameScene<MenuScene, RegionScene, MUDScene, GlobalMapScene>
    {
        public CreateScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;

        public override void Initialize()
        {
            InitGame();
            DungeonGlobal.AudioPlayer.Music("CreateParty.ogg".AsmMusicRes());

            var backlayer = CreateLayer("back");
            backlayer.AddObject(new ImageObject("Scenes/create.png")
            {
                Width = DungeonGlobal.Resolution.Width,
                Height = DungeonGlobal.Resolution.Height
            });

            var layer = CreateLayer("main");
            layer.AbsoluteLayer = true;

            var ui = CreateLayer("ui");
            this.AddSystem(new TooltipSystem(ui));
            this.AddSystem(new TooltipCustomSystem(ui));
            this.AddSystem(new MouseHintSystem(ui));

            this.ActiveLayer=layer;

            var title = new CreateTitle();
            layer.AddObjectCenter(title, vertical: false);
            title.Top = 25;

            //layer.AddObject(new ArrowBtn()
            //{
            //    Left = 984,
            //    Top = 988,
            //    OnClick = () => Next()
            //});

            //layer.AddObject(new ArrowBtn(false)
            //{
            //    Left = 890,
            //    Top = 988,
            //    OnClick = () => Switch<StartScene>()
            //});

            var cancelBtn = layer.AddObject(new ClassicButton(Global.Strings["Cancel"])
            {
                Left = 700,
                Top = 988,
                OnClick = () => Switch<MenuScene>()
            });

            var nextBtn = layer.AddObject(new ClassicButton(Global.Strings["StartGame"])
            {
                Left = 1020,
                Top = 988,
                OnClick = () => Next()
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
                    Gold=500,
                    Fame=new Entities.Fame(Fraction.Vanguard),
                    Hero1 = new Entities.Hero()
                    {
                        Archetype = Archetype.Warrior,
                        Class = Classes.Warrior
                    },
                    Hero2 = new Entities.Hero()
                    {
                        Archetype = Archetype.Mage,
                        Class = Classes.Elementalist
                    },
                    Hero3 = new Entities.Hero()
                    {
                        Archetype = Archetype.Thief,
                        Class = Classes.Rogue
                    },
                    Hero4 = new Entities.Hero()
                    {
                        Archetype = Archetype.Priest,
                        Class = Classes.Priest
                    },
                },
                Calendar = new Entities.Calendar(),
                Log = new Entities.Journal.GameLog(),
                QuestBook = new Entities.Quests.QuestBook()
            };

            Global.Game.Party.Init();
        }

        private void Next()
        {
            foreach (var hero in Global.Game.Party.Heroes)
            {
                if (hero.Name.IsEmpty())
                {
                    hero.Name = Global.Strings[$"{hero.Archetype}{hero.Sex}"];
                }

                hero.Abilities = new List<Ability>(Ability.ByClass(hero.Archetype));
                hero.BindSkills();

                Global.Game.Party.Food.Init();

                //foreach (var startFood in Global.Game.Party.Food.Components)
                //{
                //    startFood.Value = 5;
                //    startFood.Image = "Icons/Food/apple.png".AsmImg();
                //    startFood.Title = Global.Strings.Apples;
                //}
                var startFood = Global.Game.Party.Food.Components[0];
                startFood.Value = 5;
                startFood.Image = "Icons/Food/apple.png".AsmImg();
                startFood.Name = Global.Strings["Apples"];

                Global.Game.MapRegion = ResourceLoader.LoadJson<MapRegion>("Regions/FaithIsland.json".AsmRes());
                Global.Game.MapRegion.BuildGraph();
            }

            var turns = Global.Game.Turns=new Entities.Turning.Turns(Global.Game);
            turns.Init();


            Switch<GlobalMapScene>();
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape)
                Switch<MenuScene>();
            base.KeyPress(keyPressed, keyModifiers, hold);
        }
    }
}
