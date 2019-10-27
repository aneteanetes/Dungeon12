namespace Rogue.Scenes.Menus
{
    using Rogue.Drawing;
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.SceneObjects;
    using Rogue.Map.Editor;
    using Rogue.Races.Perks;
    using Rogue.Scenes.Manager;
    using Rogue.Scenes.Menus.Creation;
    using System;

    public class Start : GameScene<SoloDuoScene, Game.Main, EditorScene>
    {
        public override bool AbsolutePositionScene => true;

        public Start(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;
        
        public override void Init()
        {
            Global.DrawClient.SetCursor("Cursors.common.png".PathImage());


            Global.AudioPlayer.Music("maintheme", new Audio.AudioOptions()
            {
                Repeat = true,
                Volume = 0.3
            });
            
            this.AddObject(new Background(true)
            {
                AbsolutePosition = true,
            });
            this.AddObject(new ImageControl("Rogue.Resources.Images.d12textM.png")
            {
                Top = 2f,
                Left = 10f,
                AbsolutePosition = true,
            });

            this.AddObject(new MetallButtonControl("Новая игра")
            {
                Left = 15.5f,
                Top = 8,
                OnClick = () => { this.Switch<SoloDuoScene>(); },
                AbsolutePosition=true,
            });


            this.AddObject(new MetallButtonControl("Загрузить")
            {
                Left = 15.5f,
                Top = 11,
                AbsolutePosition = true,
                OnClick = () =>
                {
                    this.PlayerAvatar = new Map.Objects.Avatar(new Rogue.Classes.Noone.Noone()
                    {
                        Origin = Entites.Alive.Enums.Origins.Farmer
                    });
                    this.PlayerAvatar.Character.Name = "Ваш персонаж";
                    this.PlayerAvatar.Character.Race = Race.Elf;
                    this.PlayerAvatar.Character.Add<RacePerk>();

                    Global.AudioPlayer.Music("town", new Audio.AudioOptions()
                    {
                        Repeat = true,
                        Volume = 0.3
                    });

                    this.Switch<Game.Main>();
                }
            });

            this.AddObject(new SmallMetallButtonControl(new DrawText("#") { Size = 40 }.Montserrat())
            {
                Left = 24,
                Top = 11,
                AbsolutePosition = true,
                OnClick = () =>
                {
                    this.Switch<EditorScene>();
                }
            });

            this.AddObject(new MetallButtonControl("Создатели")
            {
                Left = 15.5f,
                Top = 14,
                AbsolutePosition = true,
                OnClick = () =>
                {

                }
            });

            this.AddObject(new MetallButtonControl("Выход")
            {
                Left = 15.5f,
                Top = 17,
                AbsolutePosition = true,
                OnClick = () =>
                {
                    Environment.Exit(0);
                }
            });
        }
    }


    //public class PlayerDemo : ISceneObject
    //{
    //    public bool IsControlable => false;
    //    public Rectangle Position { get; } = new Rectangle
    //    {
    //        Height = 1,
    //        Width = 1,
    //        X = 1,
    //        Y = 1
    //    };

    //    public string TileSet => this.Texture.Tileset;

    //    private DrawablePlayerDemo bloodmage = null;

    //    public IDrawable Texture
    //    {
    //        get
    //        {
    //            if (bloodmage == null)
    //            {
    //                bloodmage = new DrawablePlayerDemo();
    //            }

    //            return bloodmage;
    //        }
    //    }

    //    public IDrawText Text => null;

    //    public IDrawablePath Path => null;

    //    public ICollection<ISceneObject> Children => new List<ISceneObject>();

    //    public Rectangle Location => throw new NotImplementedException();

    //    public void Handle(ControlEventType @event)
    //    {
    //        //throw new NotImplementedException();
    //    }
    //}

    //public class DrawablePlayerDemo : IDrawable
    //{
    //    public string Icon => "";

    //    public string Name => "nevermind";

    //    public string Tileset => "Rogue.Classes.BloodMage.Images.Dolls.Character.png";

    //    public Rectangle TileSetRegion => Apply();

    //    public bool Container => false;

    //    public IDrawColor BackgroundColor { get; set; }
    //    public IDrawColor ForegroundColor { get; set; }

    //    public Rectangle Region { get; set; }


    //    private int frame = 0;
    //    private readonly Dictionary<int, Point> frames = new Dictionary<int, Point>()
    //    {
    //        {0,     new Point(64, 64) },
    //        {1,     new Point(0, 64)},
    //        {2,     new Point(32, 64)},
    //    };

    //    private Rectangle pos = new Rectangle
    //    {
    //        X = 32,
    //        Y = 0,
    //        Height = 32,
    //        Width = 32
    //    };

    //    private List<bool> playing = new List<bool>();

    //    public void StopPlay()
    //    {
    //        playing.Clear();
    //    }

    //    public bool play
    //    {
    //        get
    //        {
    //            return playing.LastOrDefault();
    //        }
    //        set
    //        {
    //            if (value)
    //                playing.Add(true);
    //            else
    //                playing.Remove(playing.LastOrDefault());
    //        }
    //    }

    //    private double FrameDraw => 24;//60 / frames.Count;
    //    private double FrameCounter = 0;

    //    public Rectangle Apply()
    //    {
    //        FrameCounter++;

    //        if (play && FrameCounter == Math.Round(frameTimeIn_60))
    //        {
    //            MainMenuScene.pd.Position.X += distanceOneFrame;

    //            FrameCounter = 0;
    //            pos.Pos = frames[frame];

    //            frame++;
    //            framesDrawed++;

    //            if (frame == frames.Count)
    //            {
    //                frame = 0;
    //            }

    //            if (framesDrawed > frameCount)
    //            {
    //                framesDrawed = 0;
    //                play = false;
    //            }
    //        }

    //        if (FrameCounter > 60) FrameCounter = 0;

    //        return pos;
    //    }

    //    //реальные константы

    //    public int framesDrawed = 0;

    //    public float distanceOneFrame = 0.1f;
    //    public double frameCount => frames.Count * speed;

    //    public double frameTimeIn_60 => 60 / frameCount;

    //    public double speed = 1;

    //}
}

//new Point(64,64),
//                new Point(0,64),
//                new Point(32,64)