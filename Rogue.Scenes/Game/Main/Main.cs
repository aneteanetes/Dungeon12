namespace Rogue.Scenes.Game
{
    using Rogue.Control.Keys;
    using Rogue.DataAccess;
    using Rogue.Drawing.Controls;
    using Rogue.Drawing.Data;
    using Rogue.Drawing.GUI;
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.Labirinth;
    using Rogue.Drawing.SceneObjects;
    using Rogue.Drawing.SceneObjects.Main;
    using Rogue.Drawing.SceneObjects.Map;
    using Rogue.Map;
    using Rogue.Map.Objects;
    using Rogue.Scenes.Menus;
    using Rogue.Scenes.Scenes;
    using Rogue.Settings;
    using Rogue.Types;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public class Main : GameScene<Start>
    {
        private readonly Point PlayerPosition = new Point { X = 27, Y = 8 };

        private readonly DrawingSize DrawingSize = new DrawingSize();

        public Main(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => false;

        public override void Init()
        {
            this.AddObject(new ImageControl("Rogue.Resources.Images.d12back.png"));

            this.InitMap();
            this.AddObject(new MapSceneObject(this.Location)
            {
                Left = 0,
                Top = 0
            });

            var portal = new Portal
            {
                Location = new Point(9, 4)
            };
            portal.Region = new Rectangle
            {
                Height = 32,
                Width = 32,
                Pos = portal.Location
            };
            var portalSceneObject = new StandaloneSceneObject(portal,(frameCounter,animMap)=>
            {

                return frameCounter % (180 / animMap.Frames.Count) == 0;
            })
            {
                Left = 9,
                Top = 4,
                Width=1,
                Height=1
            };
            this.Location.Map.Query(portal).Nodes.Add(portal);
            this.AddObject(portalSceneObject);


            this.AddObject(new SkillBar(this.Player)
            {
                Top = 18.45f,
                Left = 9f
            });

            var player = new PlayerSceneObject(this.Player, this.Location)
            {
                Left = 20,
                Top = 11
            };
            this.AddObject(player);
            this.Location.Map.Nodes.Insert(0, this.Player);
        }

        public override void Draw()
        {
            if (this.Location == null)
                this.InitMap();

            if (this.Commands.Count == 0)
                this.FillCommands();
            
            new Image("Rogue.Resources.Images.d12back.png")
            {
                Left = 0.4f,
                Top = 1f,
                Width = 48.2f,
                Height = 29f,
                ImageTileRegion = new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Height = 700,
                    Width = 1057
                }
            }.Run().Publish();

            //Drawing.Draw.Session<GUIBorderDrawSession>()
            //    .Then<MapSceneObject>(x => x.Location = this.Location)
            //    //.Then<CharMapDrawSession>(x => x.Commands = this.Commands.Where(c => c.UI).Select(c => $"[{c.Keys.First()}] - {c.Name}").ToArray())
            //    .Then<CharacterDataDrawSession>(x => x.Player = this.Player)
            //    .Then<MessageDrawSession>(x => x.Message = new DrawText($"{DateTime.Now.ToShortTimeString()}: Вы прибываете в столицу", ConsoleColor.Black))
            //    .Publish();
        }

        private void FillCommands()
        {
            this.Commands.Add(new MoveCommand()
            {
                Location = this.Location,
                PlayerPosition = this.PlayerPosition,
                Player = this.Player
            });

            //this.Commands.Add(new Control.Commands.Command { Key = Key.E, Name = "Действие" });
            //this.Commands.Add(new Control.Commands.Command { Key = Key.F, Name = "Подобрать" });
            //this.Commands.Add(new Control.Commands.Command { Key = Key.C, Name = "Персонаж" });
            //this.Commands.Add(new Control.Commands.Command { Key = Key.I, Name = "Инвентарь" });
            //this.Commands.Add(new Control.Commands.Command { Key = Key.Q, Name = "Атаковать" });
            //this.Commands.Add(new Control.Commands.Command { Key = Key.Z, Name = "Осмотреться" });
            //this.Commands.Add(new Control.Commands.Command { Key = Key.R, Name = "Способности" });
            //this.Commands.Add(new Control.Commands.Command { Key = Key.Escape, Name = "Меню" });
        }

        private void InitMap()
        {
            this.Location = new GameMap
            {
                Biom = ConsoleColor.DarkGray
            };

            var persistMap = Database.Entity<Data.Maps.Map>(e => e.Identity == "Capital").First();

            this.Location.Name = persistMap.Name;

            int x = 0;
            int y = 0;

            foreach (var line in persistMap.Template.Split(Environment.NewLine))
            {
                var listLine = new List<List<Map.MapObject>>();

                x = 0;

                foreach (var @char in line)
                {
                    var mapObj = MapObject.Create(@char.ToString());
                    mapObj.Location = new Point(x, y);
                    mapObj.Region = new Rectangle
                    {
                        Height = 32,
                        Width = 32,
                        Pos = mapObj.Location
                    };

                    if (mapObj.Obstruction)
                    {
                        Location.Map.Query(mapObj)
                            .Nodes
                            .Add(mapObj);

                        this.Location.Objects.Add(mapObj);
                    }

                    listLine.Add(new List<MapObject>() { mapObj });
                    x++;
                }

                y++;

                this.Location.MapOld.Add(listLine);
            }

            //перенести туда где location
            this.Player.Location = new Point(20, 11);
            this.Player.Region = new Rectangle
            {
                Height = 32,
                Width = 32,
                Pos = this.Player.Location
            };           
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers)
        {
            if (keyPressed == Key.Escape)
                this.Switch<Start>();

#if DEBUG
            if (keyPressed == Key.U)
                drawMode = true;

            if(drawMode)
            {
                if(keyPressed== Key.W)
                {
                    this.drawChar = "#";
                }

                if (keyPressed == Key.F)
                {
                    this.drawChar = ".";
                }

                if (keyPressed == Key.E)
                {
                    var export = string.Empty;
                    foreach (var line in this.Location.MapOld)
                    {
                        foreach (var @char in line)
                        {
                            export += @char.First().Icon;
                        }
                        export += Environment.NewLine;
                    }
                    Debugger.Break();
                }
            }
#endif
        }


#if DEBUG
        private bool drawMode = false;
        private string drawChar = ".";
#endif

//        protected override void MousePress(PointerArgs pointerPressedEventArgs)
//        {
//#if DEBUG
//            if (drawMode)
//            {
//                var trulyX = pointerPressedEventArgs.X - 20.125;
//                var trulyY = pointerPressedEventArgs.Y - 27;

//                int x = (int)Math.Round(trulyX / 25, MidpointRounding.ToEven);
//                int y = (int)Math.Round(trulyY / 25, MidpointRounding.ToEven);

//                this.Location.Map[y][x].RemoveAt(0);
//                this.Location.Map[y][x].Insert(0, MapObject.Create(drawChar));

//                this.Draw();
//                this.Redraw();
//            }
//#endif
//        }

//        public override void SceneLoop()
//        {
//            var objs = this.Location.Map
//                .SelectMany(y => y.SelectMany(x => x))
//                .Where(mapObj => mapObj.Animated)
//                .ToArray();

//            foreach (var animatedObj in objs)
//            {
//                Drawing.Draw.Animation<MapAnimationSession>(x =>
//                {
//                    x.MapObject = animatedObj;
//                });
//            }

//            base.SceneLoop();
//        }
    }
}
