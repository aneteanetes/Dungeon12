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
            var mapSceneObect = new GameMapSceneObject(this.Gamemap, this.Player)
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
                }
            };
            this.AddObject(mapSceneObect);
            mapSceneObect.Init();

            this.AddObject(new SkillBar(this.Player, this.Gamemap, e => e.ForEach(effect=> {
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

            this.Player.Die += () =>
            {
                this.Switch<End>();
            };

            var player = new PlayerSceneObject(this.Player, this.Gamemap)
            {
                Left = 20,
                Top = 11
            };
            player.OnStop = () =>
            {
                MapObjectCanAffectCamera(this.Player, Types.Direction.Idle, false);
            };
            this.AddObject(player);
            this.Gamemap.Map.Add(this.Player);
        }

        private void FillCommands()
        {
            this.Commands.Add(new MoveCommand()
            {
                Location = this.Gamemap,
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
            this.Gamemap = new GameMap
            {
                Biom = ConsoleColor.DarkGray
            };
            this.Gamemap.OnMoving += (MapObject obj, Types.Direction dir, bool availabe) =>
            {
                MapObjectCanAffectCamera(obj, dir, availabe);
            };

            this.Gamemap.Load("Capital");

            //перенести туда где location
            this.Player.Location = new Point(20, 11);
            this.Player.Region = new Rectangle
            {
                Height = 32,
                Width = 32,
                Pos = this.Player.Location
            };
        }

        private static void MapObjectCanAffectCamera(MapObject obj, Types.Direction dir, bool availabe)
        {
            if (obj.CameraAffect)
            {
                var drawClient = SceneManager.StaticDrawClient;
                //drawClient.SetCameraSpeed(obj.MovementSpeed*2);
                var pos = obj.Position;

                if (dir == Types.Direction.Idle)
                {
                    drawClient.MoveCamera(Types.Direction.Right, true);
                    drawClient.MoveCamera(Types.Direction.Left, true);
                    drawClient.MoveCamera(Types.Direction.Down, true);
                    drawClient.MoveCamera(Types.Direction.Up, true);
                }

                if (dir == Types.Direction.Right && pos.X > 1280 * 0.33 * 2)
                {
                    drawClient.MoveCamera(Types.Direction.Right);
                }
                if (dir == Types.Direction.Left && pos.X < 1280 * 0.33)
                {
                    drawClient.MoveCamera(Types.Direction.Left);
                }
                if (dir == Types.Direction.Down && pos.Y > 720 * 0.33 * 2)
                {
                    drawClient.MoveCamera(Types.Direction.Down);
                }
                if (dir == Types.Direction.Up && pos.Y < 720 * 0.33)
                {
                    drawClient.MoveCamera(Types.Direction.Up);
                }
            }
        }

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers)
        {
            if (keyPressed==Key.B)
            {
                this.Player.Character.HitPoints -= 1;
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