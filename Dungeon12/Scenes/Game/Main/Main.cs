namespace Dungeon12.Scenes.Game
{
    using Dungeon;
    using Dungeon.Control.Keys;
    using Dungeon.Drawing;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.SceneObjects;
    using Dungeon.Scenes;
    using Dungeon.Scenes.Manager;
    using Dungeon.Settings;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using Dungeon12.CardGame.Scene;
    using Dungeon12.Drawing.Labirinth;
    using Dungeon12.Drawing.SceneObjects.Main;
    using Dungeon12.Map;
    using Dungeon12.Map.Objects;
    using Dungeon12.SceneObjects;
    using Dungeon12.SceneObjects.UI;
    using Dungeon12.Scenes.Menus;
    using System;
    using System.Collections.Generic;

    public class Main : GameScene<Start, Main,End, CardGameScene>
    {
        private readonly DrawingSize DrawingSize = new DrawingSize();

        //public override bool CameraAffect => true;
        public override bool AbsolutePositionScene => false;

        public Main(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => false;

        private GameMap Gamemap => Global.GameState.Map;
        private Avatar PlayerAvatar => Global.GameState.PlayerAvatar;

        public override void Init()
        {
            var player = new Player(Global.GameState.PlayerAvatar, x => this.RemoveObject(x))
            {
                Left = this.PlayerAvatar?.Location?.X ??0 ,
                Top = this.PlayerAvatar?.Location?.Y?? 0
            };
            player.OnStop = (dir) =>
            {
                MapObjectCanAffectCamera(this.PlayerAvatar, dir, false);
            };

            Global.GameState.Player = player;

            this.InitMap();

            Global.Time.Set(Dungeon.Time.GameStart);
            Global.Time.Start();

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
                        @new.ShowInScene = ShowEffectsBinding;
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
            mapSceneObect.ShowInScene = this.ShowEffectsBinding;
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
            this.Gamemap.MapObject.Add(this.PlayerAvatar);


            this.AddObject(new Dungeon12PlayerUI(this.PlayerAvatar.Character));

            // размораживаем рисование пресонажа
            Global.GameState.Player.FreezeDrawLoop = false;

#if DEBUG
            this.AddObject(new Position(this.PlayerAvatar));
#endif
        }

        private void InitMap()
        {
            if (!this.Gamemap.Loaded)
            {
                try
                {
                    this.Gamemap.InitRegion("FaithIsland");
                }
                catch (Exception ex)
                {
                    throw;
                    //Global.Exception(ex, () => { this.Switch<Start>(); });                    
                    return;
                }
            }

            if (this.Gamemap.OnMoving == default)
            {
                this.Gamemap.OnMoving += (MapObject obj, Dungeon.Types.Direction dir, bool availabe) =>
                {
                    MapObjectCanAffectCamera(obj, dir, availabe);
                };
            }

            var persistRegion = this.Gamemap.LoadedRegionData;
            var tileBack = persistRegion.TileBack;
            if (tileBack != default)
            {
                var back = new ImageControl(tileBack);
                var offset = persistRegion.TileBackOffset;
                if (offset != default)
                {
                    back.Left = offset.X;
                    back.Top = offset.Y;
                }
                this.AddObject(back);
            }

            this.AddObject(new ImageControl(persistRegion.Tile));

            //перенести туда где location
            if (this.PlayerAvatar.Location == default)
            {
                var playerPos = new Point { X = 42, Y = 45 }; ;

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

                SceneManager.StaticDrawClient.SetCamera(xOffset * 32, yOffset * 32);

                Global.GameState.Player.Left = this.PlayerAvatar.Location.X;
                Global.GameState.Player.Top = this.PlayerAvatar.Location.Y;
            }


            var drawClient = SceneManager.StaticDrawClient;
            //drawClient.off

            this.PlayerAvatar.Region = new Rectangle
            {
                Height = 32,
                Width = 32,
                Pos = this.PlayerAvatar.Location
            };

            this.Gamemap.Loaded = false;
        }

        private static void MapObjectCanAffectCamera(MapObject obj, Dungeon.Types.Direction dir, bool availabe)
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
                this.Switch<Start>("main");
        }

        private class Position : TextControl
        {
            public override bool AbsolutePosition => true;

            private Avatar playerAvatar;

            public Position(Avatar playerAvatar) : base(null) => this.playerAvatar = playerAvatar;

            public override bool CacheAvailable => false;

            public override IDrawText Text => new DrawText($"X:{playerAvatar.Location.X} Y:{playerAvatar.Location.Y}").Montserrat();

            public override double Left => 30;
            
            public override double Height => 1;

            public override double Width => this.MeasureText(Text).X/32;
        }
    }
}