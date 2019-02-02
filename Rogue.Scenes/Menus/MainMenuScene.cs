namespace Rogue.Scenes.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Rogue.Control.Events;
    using Rogue.Control.Keys;
    using Rogue.Drawing.Controls;
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.Utils;
    using Rogue.Races.Perks;
    using Rogue.Scenes.Controls;
    using Rogue.Scenes.Menus.Creation;
    using Rogue.Scenes.Scenes;
    using Rogue.Types;
    using Rogue.View.Interfaces;

    public class MainMenuScene : GameScene<PlayerNameScene, Game.MainScene>
    {
        public MainMenuScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override bool Destroyable => true;
        
        public override void Init()
        {
            this.AddObject(new ImageControl("Rogue.Resources.Images.d12back.png"));
            this.AddObject(new ImageControl("Rogue.Resources.Images.d12textM.png")
            {
                Top = 2f,
                Left = 10f
            });

            this.AddObject(new ButtonControl(new DrawText("Новая игра", ConsoleColor.White) { Size = 28 })
            {
                ActiveColor = new DrawColor(ConsoleColor.Red),
                InactiveColor = new DrawColor(ConsoleColor.White),
                Left = 15.5f,
                Top = 8,
                Width = 7,
                Height = 2,
                OnClick = () => { this.Switch<PlayerNameScene>(); }
            });
        }

        public override void Draw()
        {
            //if (pd == null)
            //{
            //    pd = new PlayerDemo();
            //    this.AddObject(pd);
            //}

            //this.AddObject(new Image("Rogue.Resources.Images.d12back.png")
            //{
            //    Top = 0f,
            //    Left = 0f,
            //    Width = 40f,
            //    Height = 22.5f,
            //    ImageTileRegion = new Rectangle
            //    {
            //        X = 0,
            //        Y = 0,
            //        Height = 720,
            //        Width = 1280
            //    }
            //});

            //this.AddObject(new Image("Rogue.Resources.Images.d12textM.png")
            //{
            //    Top = 2f,
            //    Left = 10f,
            //    Width = 20f,
            //    Height = 4f,
            //    ImageTileRegion = new Rectangle
            //    {
            //        X = 0,
            //        Y = 0,
            //        Height = 148,
            //        Width = 603
            //    }
            //});

            //this.AddObject(new Button()
            //{
            //    ActiveColor = new DrawColor(ConsoleColor.Red),
            //    InactiveColor = new DrawColor(ConsoleColor.White),
            //    Left = 15.5f,
            //    Top = 8,
            //    Width = 7,
            //    Height = 2,
            //    Label = new DrawText("Новая игра", ConsoleColor.White) { Size = 28 },
            //    OnClick = () => { this.Switch<PlayerNameScene>(); }
            //});

            //var win = new Window
            //{
            //    Direction= Drawing.Controls.Direction.Vertical,
            //    Left = 16f,
            //    Top = 4,
            //    Width = 15,
            //    Height = 20
            //};

            //win.Append(new Title
            //{
            //    Left = 3.6f,
            //    Top = -1f,
            //    Width = 8,
            //    Height = 2.4f,
            //    Label = new DrawText("Dungeon 12", ConsoleColor.Black) { Size = 30 }
            //});

            //win.Append(new Text
            //{
            //    Left = 9f,
            //    Top = 1,
            //    DrawText = new DrawText("remastered", ConsoleColor.Red) { Size = 15 }
            //});

            //win.Append(new Image("Rogue.Resources.Images.d12logo.png")
            //{
            //    Left = 6.1f,
            //    Top = 1.9f,
            //    Width = 3f,
            //    Height = 3f,
            //    ImageTileRegion = new Rectangle
            //    {
            //        X = 0,
            //        Y = 0,
            //        Height = 300,
            //        Width = 300
            //    }
            //});

            //win.append(new button
            //{
            //    activecolor = new drawcolor(consolecolor.red),
            //    inactivecolor = new drawcolor(consolecolor.darkred),
            //    left = 4.1f,
            //    top = 6,
            //    width = 7,
            //    height = 2,
            //    label = new drawtext("новая игра", consolecolor.darkred) { size = 30 },
            //    onclick = () => { this.switch<playernamescene>(); }
            //});

            //var fastgamelabel = new DrawText("Быстрая игра ", ConsoleColor.DarkRed) { Size = 30 };
            //fastgamelabel.ReplaceAt(0, new DrawText("Б", ConsoleColor.DarkRed) { Size = 30, LetterSpacing = 20 });

            //win.Append(new Button
            //{
            //    ActiveColor = new DrawColor(ConsoleColor.Red),
            //    InactiveColor = new DrawColor(ConsoleColor.DarkRed),
            //    Left = 4.1f,
            //    Top = 9,
            //    Width = 7,
            //    Height = 2,
            //    Label = fastgamelabel,
            //    OnClick = () =>
            //    {
            //        this.Player = Classes.All().Skip(1).First();
            //        this.Player.Name = "Adventurer";
            //        this.Player.Race = Race.Elf;
            //        this.Player.Add<RacePerk>();

            //        this.Switch<Game.MainScene>();
            //    }
            //});

            //var creators = new DrawText("Создатели ", ConsoleColor.DarkRed) { Size = 30 };
            //creators.ReplaceAt(0, new DrawText("С", ConsoleColor.DarkRed) { Size = 30, LetterSpacing = 20 });

            //win.Append(new Button
            //{
            //    ActiveColor = new DrawColor(ConsoleColor.Red),
            //    InactiveColor = new DrawColor(ConsoleColor.DarkRed),
            //    Left = 4.1f,
            //    Top = 12,
            //    Width = 7,
            //    Height = 2,
            //    Label = creators,
            //    OnClick = () => { /*MenuEngine.CreditsWindow.Draw(); MenuEngine.MainMenu.Draw();*/ }
            //});

            //var exit = new DrawText("Выход  ", ConsoleColor.DarkRed) { Size = 30 };
            //exit.ReplaceAt(0, new DrawText("В", ConsoleColor.DarkRed) { Size = 30, LetterSpacing = 20 });

            //win.Append(new Button
            //{
            //    ActiveColor = new DrawColor(ConsoleColor.Red),
            //    InactiveColor = new DrawColor(ConsoleColor.DarkRed),
            //    Left = 4.1f,
            //    Top = 15,
            //    Width = 7,
            //    Height = 2,
            //    Label = exit,
            //    OnClick =()=> { Environment.Exit(0); }
            //});

            //Drawing.Draw.RunSession(win);
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