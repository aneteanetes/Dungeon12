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
    using Dungeon12;
    using Dungeon12.CardGame.Scene;
    using Dungeon12.Drawing.Labirinth;
    using Dungeon12.Drawing.SceneObjects;
    using Dungeon12.Drawing.SceneObjects.Main;
    using Dungeon12.Map;
    using Dungeon12.Map.Objects;
    using Dungeon12.SceneObjects;
    using Dungeon12.SceneObjects.UI;
    using Dungeon12.Scenes.Menus;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MainScene : GameScene<Start, MainScene, End, CardGameScene, EndGame>
    {
        private readonly DrawingSize DrawingSize = new DrawingSize();

        //public override bool CameraAffect => true;
        public override bool AbsolutePositionScene => false;

        private ControlOverlay controlOverlay = new ControlOverlay()
        {
            Visible=false
        };

        public MainScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => false;

        private GameMap Gamemap => Global.GameState.Map;

        private Avatar PlayerAvatar => Global.GameState.PlayerAvatar;
        
        public override void Initialize()
        {
            controlOverlay.Visible = Args?.ElementAtOrDefault(0) != default;

            this.AddObject(variableViewer);

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

            this.AddObject(player);
            this.Gamemap.MapObject.Add(this.PlayerAvatar);


            this.AddObject(new Dungeon12PlayerUI(this.PlayerAvatar.Character));

            // размораживаем рисование пресонажа
            Global.GameState.Player.FreezeDrawLoop = false;

#if DEBUG
            //this.AddObject(new Position(this.PlayerAvatar));
#endif

            this.AddObject(controlOverlay);
        }

        private void InitMap()
        {
            if (this.Gamemap == default)
            {
                Global.GameState.Map = new GameMap();
            }

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
            else
            {
                var back = new ImageControl("Regions/_back.png".AsmImg());
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

                DungeonGlobal.DrawClient.SetCamera(xOffset * 32, yOffset * 32);

                Global.GameState.Player.Left = this.PlayerAvatar.Location.X;
                Global.GameState.Player.Top = this.PlayerAvatar.Location.Y;
            }


            var drawClient = DungeonGlobal.DrawClient;
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
                var drawClient = DungeonGlobal.DrawClient;

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

                MoveCameraAndScreenPosByDirection(obj, dir, pos);

                if(dir== Direction.UpLeft)
                {
                    MoveCameraAndScreenPosByDirection(obj, Direction.Up, pos);
                    MoveCameraAndScreenPosByDirection(obj, Direction.Left, pos);
                }
                if (dir == Direction.UpRight)
                {
                    MoveCameraAndScreenPosByDirection(obj, Direction.Up, pos);
                    MoveCameraAndScreenPosByDirection(obj, Direction.Right, pos);
                }
                if (dir == Direction.DownLeft)
                {
                    MoveCameraAndScreenPosByDirection(obj, Direction.Down, pos);
                    MoveCameraAndScreenPosByDirection(obj, Direction.Left, pos);
                }
                if (dir == Direction.DownRight)
                {
                    MoveCameraAndScreenPosByDirection(obj, Direction.Down, pos);
                    MoveCameraAndScreenPosByDirection(obj, Direction.Right, pos);
                }
            }
        }

        private static void MoveCameraAndScreenPosByDirection(MapObject obj, Direction dir, Point pos)
        {
            var drawClient = DungeonGlobal.DrawClient;
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

        protected override void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold)
        {
            if(keyPressed== Key.Tab)
            {
                controlOverlay.Visible = !controlOverlay.Visible;
            }

            if (keyPressed == Key.Escape)
                this.Switch<Start>("main");

            if(keyPressed== Key.N && keyModifiers== KeyModifiers.Control)
            {
                //variableViewer.Visible = !variableViewer.Visible;
            }

            if (keyPressed == Key.K && keyModifiers == KeyModifiers.Control)
            {
                //Global.GameState.Character.Exp(70);
            }
        }

        private CharacterVariableViewer variableViewer = new CharacterVariableViewer();

        private class CharacterVariableViewer : DarkRectangle
        {
            public override bool AbsolutePosition => true;

            public override bool CacheAvailable => false;

            public CharacterVariableViewer()
            {
                this.Width = 7;
                this.Height = 15;

                this.Left = 32;
                this.Top = 3;

                this.Text = " ".AsDrawText().Montserrat().InSize(12);

                this.Visible = false;
            }

            public override void Update()
            {
                var vars = Global.GameState.Character?.Variables.Select(x => $"{x.First}: {x.Second}");
                if (vars != default)
                {
                    this.Text.SetText(string.Join(Environment.NewLine, vars));
                }
            }
        }

        public override void Destroy()
        {
            base.Destroy();
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

    public class ControlOverlay : DarkRectangle
    {
        public override bool AbsolutePosition => true;

        public ControlOverlay()
        {
            this.Width = 40;
            this.Height = 22.5;
            
            var t = this.AddTextCenter($"[TAB] - Закрыть/Открыть эту подсказку.{Environment.NewLine}Все остальные подсказки и ответы на вопросы вы можете найти{Environment.NewLine}в журнале [L] на вкладке 'Детали' в категории 'Справка'.".AsDrawText().InSize(14).Montserrat(), true, false);
            t.Top = 2.5;

            AddWASD();
            AddTab();
            AddShift();
            AddAlt();
            AddTorch();
            AddJOURNAL();
            AddCharInf();
            AddSkills();
            AddAbils();
            //AddJournal();
            AddCharInfo();
        }

        private void AddAbils()
        {
            var с = this.AddTextCenter("[Клик] (левый)".AsDrawText().Montserrat(), false, false);
            с.Left = 25;
            с.Top = 5;
            AddLine(new Point(25, 6), new Point(25, 9));

            var q = this.AddTextCenter("[Q]".AsDrawText().Montserrat(), false, false);
            q.Left = 28;
            q.Top = 6;
            AddLine(new Point(28, 7), new Point(28, 9));

            var e = this.AddTextCenter("[E]".AsDrawText().Montserrat(), false, false);
            e.Left = 31;
            e.Top = 6;
            AddLine(new Point(31, 7), new Point(31, 9));

            var o = this.AddTextCenter("[Клик] (правый)".AsDrawText().Montserrat(), false, false);
            o.Left = 34;
            o.Top = 5;
            AddLine(new Point(34, 6), new Point(34, 9));


            AddLine(new Point(25, 9), new Point(34, 9));

            var t = this.AddTextCenter("Использование способностей".AsDrawText().Montserrat(), false, false);
            t.Left = 25;
            t.Top = 10;
            var t1 = this.AddTextCenter("(Вне города)".AsDrawText().Montserrat(), false, false);
            t1.Left = 25;
            t1.Top = 11;
        }

        private void AddJournal()
        {
            AddLine(new Point(31, 18), new Point(31, 22.5));
        }

        private void AddSkills()
        {
            var t = this.AddTextCenter("[V] Описание способностей".AsDrawText().Montserrat(), false, false);
            t.Left = 20.5;
            t.Top = 19;
            AddLine(new Point(20.5, 20), new Point(20.5, 21.5));
            AddLine(new Point(20.5, 20), new Point(26, 20));
        }

        private void AddCharInf()
        {
            var t = this.AddTextCenter("[С] Персонаж".AsDrawText().Montserrat(), false, false);
            t.Left = 15;
            t.Top = 14;
            AddLine(new Point(17.5, 14.5), new Point(17.5, 21));
        }

        private void AddCharInfo()
        {
            //AddLine(new Point(16, 14.5), new Point(16, 21));
        }

        private void AddJOURNAL()
        {
            var t = this.AddTextCenter("[L] Журнал".AsDrawText().Montserrat(), false, false);
            t.Left = 11;
            t.Top = 16;
            AddLine(new Point(11, 17), new Point(16.5, 17));
            AddLine(new Point(16.5, 17), new Point(16.5, 21.5));
        }

        private void AddTorch()
        {
            var t = this.AddTextCenter("[F] - Факел (с 18 до 7)".AsDrawText().Montserrat(), false, false);
            t.Left = 8;
            t.Top = 19;

            AddLine(new Point(8, 20), new Point(15.5, 20));
            AddLine(new Point(15.5, 20), new Point(15.5, 21.5));
        }

        private void AddAlt()
        {
            var t = this.AddTextCenter("[Alt] - Предметы".AsDrawText().Montserrat(), false, false);
            t.Left = 5;
            t.Top = 20;

            AddLine(new Point(5, 20), new Point(5, 22.5));
        }

        private void AddShift()
        {
            var t = this.AddTextCenter("[Shift] - Дома".AsDrawText().Montserrat(), false, false);
            t.Left = 2;
            t.Top = 17;
            AddLine(new Point(2, 18), new Point(2, 22.5));
        }

        private void AddTab()
        {
            var t = this.AddTextCenter("[Tab] - открыть/закрыть этот экран".AsDrawText().Montserrat(), false, false);
            t.Left = 4;
            t.Top = 10;

            AddLine(new Point(0, 10), new Point(3.5, 10));
        }

        private void AddWASD()
        {
            var t = this.AddTextCenter("Передвижение".AsDrawText().Montserrat(), false, false);
            t.Left = 3;
            t.Top = 2;

            var w = this.AddTextCenter("[W]".AsDrawText().Montserrat(), false, false);
            w.Left = 4;
            w.Top = 2.5;
            var a = this.AddTextCenter("[A]".AsDrawText().Montserrat(), false, false);
            a.Left = .5;
            a.Top = 5;
            var d = this.AddTextCenter("[D]".AsDrawText().Montserrat(), false, false);
            d.Left = 6.5;
            d.Top = 5;
            var s = this.AddTextCenter("[S]".AsDrawText().Montserrat(), false, false);
            s.Left = 4;
            s.Top = 7.5;
            AddLine(new Point(4, 3), new Point(4, 7));
            AddLine(new Point(2, 5), new Point(6, 5));
        }

        private void AddLine(Point from, Point to)
        {
            this.AddChild(new LineSceneControl(new LineSceneModel()
            {
                From = from,
                To = to,
                Color = ConsoleColor.White
            })
            {
                Depth = 2
            });
        }
    }
}