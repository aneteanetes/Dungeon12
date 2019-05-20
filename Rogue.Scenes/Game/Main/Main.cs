namespace Rogue.Scenes.Game
{
    using Rogue.Control.Keys;
    using Rogue.Drawing.Labirinth;
    using Rogue.Drawing.SceneObjects;
    using Rogue.Drawing.SceneObjects.Main;
    using Rogue.Drawing.SceneObjects.Map;
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.Map;
    using Rogue.Scenes.Manager;
    using Rogue.Scenes.Menus;
    using Rogue.Settings;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public class Main : GameScene<Start, Main,End>
    {
        private readonly Point PlayerPosition = new Point { X = 27, Y = 8 };

        private readonly DrawingSize DrawingSize = new DrawingSize();

        //public override bool CameraAffect => true;
        public override bool AbsolutePositionScene => false;

        public Main(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => false;

        public override void Init()
        {
            this.AddObject(new ImageControl("Rogue.Resources.Images.d12back.png"));


            this.InitMap();


            var player = new PlayerSceneObject(this.PlayerAvatar, this.Gamemap, (obj) => this.RemoveObject(obj), (obj) => this.AddObject(obj))
            {
                Left = 20,
                Top = 11
            };
            player.OnStop = () =>
            {
                MapObjectCanAffectCamera(this.PlayerAvatar, Types.Direction.Idle, false);
            };


            List<ISceneObject> temp = new List<ISceneObject>();

            var mapSceneObect = new GameMapSceneObject(this.Gamemap, player)
            {
                Left = 0,
                Top = 0,
                OnReload = (olditems, newitems) =>
                {
                    foreach (var old in olditems)
                    {
                        this.RemoveObject(old);
                    }
                    foreach (var @new in newitems)
                    {
                        @new.Destroy += () => this.RemoveObject(@new);
                        @new.ShowEffects += e => e.ForEach(effect =>
                        {
                            effect.Destroy += () =>
                            {
                                this.RemoveObject(effect);
                            };
                            this.AddObject(effect);
                        });
                        this.AddObject(@new);
                    }
                },
                drawpath = (listObj) =>
                {
                    temp.ForEach(o => this.RemoveObject(o));
                    temp = listObj;
                    listObj.ForEach(o => this.AddObject(o));
                }
            };
            this.AddObject(mapSceneObect);
            mapSceneObect.Init();

            this.AddObject(new SkillBar(this.PlayerAvatar, this.Gamemap, e => e.ForEach(effect =>
            {
                effect.Destroy += () =>
                {
                    this.RemoveObject(effect);
                };
                this.AddObject(effect);
            }))
            {
                Top = 18.45f,
                Left = 9f
            });

            this.PlayerAvatar.Die += () =>
            {
                this.Switch<End>();
            };

            this.AddObject(player);
            this.Gamemap.Map.Add(this.PlayerAvatar);


            this.AddObject(new PlayerUI(this.PlayerAvatar.Character));
        }

        private void FillCommands()
        {
            this.Commands.Add(new MoveCommand()
            {
                Location = this.Gamemap,
                PlayerPosition = this.PlayerPosition,
                Player = this.PlayerAvatar
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
            this.Gamemap = new GameMap
            {
                Biom = ConsoleColor.DarkGray
            };
            this.Gamemap.OnMoving += (MapObject obj, Types.Direction dir, bool availabe) =>
            {
                MapObjectCanAffectCamera(obj, dir, availabe);
            };

            //this.Gamemap.Load("Capital");
            this.Gamemap.LoadRegion("FaithIsland");
            this.AddObject(new ImageControl("Rogue.Resources.Images.Regions.FaithIsland.png"));

            //перенести туда где location
            this.PlayerAvatar.Location = new Point(20, 11);
            this.PlayerAvatar.Region = new Rectangle
            {
                Height = 32,
                Width = 32,
                Pos = this.PlayerAvatar.Location
            };
        }

        private static void MapObjectCanAffectCamera(MapObject obj, Types.Direction dir, bool availabe)
        {
            if (obj.CameraAffect)
            {
                var drawClient = SceneManager.StaticDrawClient;

                if (obj.SceenPosition == null)
                {
                    obj.SceenPosition = new Point(0, 0);
                }

                var pos = obj.SceenPosition;

                if (dir == Types.Direction.Idle)
                {
                    drawClient.MoveCamera(Types.Direction.Right, true);
                    drawClient.MoveCamera(Types.Direction.Left, true);
                    drawClient.MoveCamera(Types.Direction.Down, true);
                    drawClient.MoveCamera(Types.Direction.Up, true);
                }

                if (dir == Types.Direction.Right && pos.X > 10)
                {
                    obj.SceenPosition.X -= 2;
                    drawClient.MoveCamera(Types.Direction.Right);
                }
                if (dir == Types.Direction.Left && pos.X < -7.5)
                {
                    obj.SceenPosition.X += 2;
                    drawClient.MoveCamera(Types.Direction.Left);
                }
                if (dir == Types.Direction.Down && pos.Y > 3)
                {
                    obj.SceenPosition.Y -= 2;
                    drawClient.MoveCamera(Types.Direction.Down);
                }
                if (dir == Types.Direction.Up && pos.Y < -6)
                {
                    obj.SceenPosition.Y += 2;
                    drawClient.MoveCamera(Types.Direction.Up);
                }
            }
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {

            if(keyPressed== Key.L)
            {
                this.PlayerAvatar.Character.Level++;
            }

            if (keyPressed==Key.B)
            {
                this.PlayerAvatar.Character.HitPoints -= 1;
            }

            if (keyPressed == Key.Home)
            {
                var drawClient = SceneManager.StaticDrawClient;
                drawClient.ResetCamera();
            }

            if (keyPressed == Key.I)
            {
                this.Gamemap.Level += 1;
            }

            if (keyPressed == Key.Escape)
                this.Switch<Start>();

#if DEBUG
            if (keyPressed == Key.U)
                drawMode = true;

            if (drawMode)
            {
                if (keyPressed == Key.W)
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
                    foreach (var line in this.Gamemap.MapOld)
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