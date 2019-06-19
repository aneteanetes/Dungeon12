namespace Rogue.Scenes.Game
{
    using Rogue.Control.Keys;
    using Rogue.Drawing;
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.Labirinth;
    using Rogue.Drawing.SceneObjects;
    using Rogue.Drawing.SceneObjects.Main;
    using Rogue.Drawing.SceneObjects.Map;
    using Rogue.Drawing.SceneObjects.UI;
    using Rogue.Map;
    using Rogue.Map.Objects;
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
        private readonly Point PlayerPosition = new Point { X = 42, Y = 45 };

        private readonly DrawingSize DrawingSize = new DrawingSize();

        //public override bool CameraAffect => true;
        public override bool AbsolutePositionScene => false;

        public Main(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => false;

        public override void Init()
        {
            this.InitMap();

            var player = new PlayerSceneObject(this.PlayerAvatar, this.Gamemap, x=>this.RemoveObject(x))
            {
                Left = PlayerPosition.X,
                Top = PlayerPosition.Y
            };
            player.OnStop = (dir) =>
            {
                MapObjectCanAffectCamera(this.PlayerAvatar, dir, false);
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
                        @new.ShowEffects = ShowEffectsBinding;
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
            mapSceneObect.ShowEffects = this.ShowEffectsBinding;
            this.AddObject(mapSceneObect);
            mapSceneObect.Init();

            this.AddObject(new SkillBar(player, this.Gamemap, ShowEffectsBinding,x=>this.RemoveObject(x),x=>
            {
                this.AddControl(x);
                x.Destroy += () => this.RemoveControl(x);
            })
            {
                Top = 18.45f,
                Left = 9f
            });

            this.AddObject(new PlayerBar(Gamemap, player, ShowEffectsBinding)
            {
                Top = 18.45 + 0.5 + 2,
                Left = 9 + 2.9 
            });

            this.PlayerAvatar.Die += () =>
            {
                this.Switch<End>();
            };

            this.AddObject(player);
            this.Gamemap.Map.Add(this.PlayerAvatar);


            this.AddObject(new PlayerUI(this.PlayerAvatar.Character));            

#if DEBUG
            this.AddObject(new Position(this.PlayerAvatar));
#endif
        }

        //private void ShowEffectsBinding(List<ISceneObject> e)
        //{
        //    e.ForEach(effect =>
        //    {
        //        if (effect.ShowEffects == null)
        //        {
        //            effect.ShowEffects = ShowEffectsBinding;
        //        }
        //        if(effect is HandleSceneControl effectControl)
        //        {
        //            effectControl.ControlBinding = this.AddControl;
        //        }

        //        effect.Destroy += () =>
        //        {
        //            this.RemoveObject(effect);
        //        };
        //        this.AddObject(effect);
        //    });
        //}

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
            this.Gamemap = new GameMap()
            {
                Biom = ConsoleColor.DarkGray
            };
            this.Gamemap.OnMoving += (MapObject obj, Types.Direction dir, bool availabe) =>
            {
                MapObjectCanAffectCamera(obj, dir, availabe);
            };

            this.Gamemap.LoadRegion("FaithIsland");
            this.AddObject(new ImageControl("Rogue.Resources.Images.Regions.FaithIsland.png"));


            //width = 40
            //height = 22.5

            var playerPos = PlayerPosition;

            //перенести туда где location
            this.PlayerAvatar.Location = playerPos;

            double xOffset = 0;
            double yOffset = 0;

            if (playerPos.X > 29)
            {
                xOffset -= playerPos.X - 20;
            }
            if (playerPos.X < 11)
            {
                xOffset += playerPos.X - 20;
            }
            if (playerPos.Y > 16.25)
            {
                yOffset -= playerPos.Y - 11.25;
            }
            if (playerPos.Y < 6.25)
            {
                yOffset += playerPos.Y - 11.25;
            }

            SceneManager.StaticDrawClient.SetCamera(xOffset*32, yOffset*32);


            var drawClient = SceneManager.StaticDrawClient;
            //drawClient.off

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

                if (!availabe)
                {
                    drawClient.MoveCamera(dir, true);
                    return;
                }

                var spd = obj.MovementSpeed * 64;
                drawClient.SetCameraSpeed(spd);

                if (obj.SceenPosition == null)
                {
                    obj.SceenPosition = new Point(0, 0);
                }
                var pos = obj.SceenPosition;

                switch (dir)
                {
                    case Direction.Up when (pos.Y > -5):
                        obj.SceenPosition.Y -= obj.MovementSpeed;
                        break;
                    case Direction.Down when pos.Y < 5:
                        obj.SceenPosition.Y += obj.MovementSpeed;
                        break;
                    case Direction.Left when pos.X > -9:
                        obj.SceenPosition.X -= obj.MovementSpeed;
                        break;
                    case Direction.Right when pos.X < 9:
                        obj.SceenPosition.X += obj.MovementSpeed;
                        break;
                    default:
                        break;
                }

                if (dir == Direction.Right && pos.X > 9)
                {
                    drawClient.MoveCamera(Direction.Right);
                }
                if (dir == Direction.Left && pos.X < -9)
                {
                    drawClient.MoveCamera(Direction.Left);
                }
                if (dir == Direction.Down && pos.Y > 5)
                {
                    drawClient.MoveCamera(Direction.Down);
                }
                if (dir == Direction.Up && pos.Y < -5)
                {
                    drawClient.MoveCamera(Direction.Up);
                }
            }
        }
        
        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if (keyPressed == Key.Escape)
                this.Switch<Start>();
        }

        private class Position : TextControl
        {
            public override bool AbsolutePosition => true;

            private Avatar playerAvatar;

            public Position(Avatar playerAvatar) : base(null) => this.playerAvatar = playerAvatar;

            public override bool CacheAvailable => false;

            public override IDrawText Text => new DrawText($"X:{playerAvatar.Location.X} Y:{playerAvatar.Location.Y}").Montserrat();

            public override double Left => 40 / 2 - this.MeasureText(Text).X/32 / 2;
            
            public override double Height => 1;

            public override double Width => this.MeasureText(Text).X/32;
        }
    }
}