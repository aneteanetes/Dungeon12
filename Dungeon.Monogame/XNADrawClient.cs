namespace Dungeon.Monogame
{
    using Dungeon.Network;
    using Dungeon.Scenes.Manager;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using Dungeon12;
    using Dungeon12.Scenes.Menus;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;
    using Penumbra;
    using ProjectMercury.Renderers;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Resources;
    using System.Runtime.InteropServices;

    public partial class XNADrawClient : Game, IDrawClient
    {
        private PenumbraComponent penumbra;

        Renderer myRenderer;

        ResourceContentManager _resources;

        public bool isFatal;

        public SceneManager SceneManager { get; set; }
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        readonly Light SunLight = new PointLight
        {
            Scale = new Vector2(3700f),
            ShadowType = ShadowType.Illuminated, // Will not lit hulls themselves,
            Rotation = 0.8707998f,
            Position = new Vector2(-2660, -500),
            //ConeDecay = 5f,
            Radius = 10000,            
        };

        Light light = new PointLight()
        {
            Scale = new Vector2(300),
            ShadowType = ShadowType.Occluded,
            Radius = 100
        };

        Matrix screenScale = Matrix.Identity;

        private bool blockControls = false;
        private float audioVolume = 0;

        public XNADrawClient()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen =
#if Android
                true,
#endif
#if Core
                false,
#endif
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720,
                SynchronizeWithVerticalRetrace = true,
            };

            audioVolume = (float)DungeonGlobal.AudioOptions.Volume;
            MediaPlayer.Volume = audioVolume;

            this.Activated += (_, __) =>
            {
                blockControls = false;
                MediaPlayer.Volume = audioVolume;
            };

            this.Deactivated += (_, __) =>
            {
                blockControls = true;
                audioVolume = MediaPlayer.Volume;
                MediaPlayer.Volume = 0;
            };

            Window.AllowUserResizing = false;
            //Window.IsBorderless = true;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // fixing framerate
            this.IsFixedTimeStep = true;//false;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d); //60);

            var manager = new ResourceManager("Dungeon.Monogame.Resources", typeof(XNADrawClient).Assembly);
            var data = manager.GetResourceSet(CultureInfo.InvariantCulture, true, true).Cast<DictionaryEntry>()
                                    .ToDictionary(r => r.Key.ToString(),
                                                  r => r.Value.ToString());
            foreach (var item in data)
            {
                Console.WriteLine(item.Key);
                Console.WriteLine(item.Value);
                try
                {
                    Console.WriteLine(manager.GetObject(item.Key));
                }
                catch
                {
                    Console.WriteLine(manager.GetObject(item.Key));
                    //throw;
                }
            }

            _resources = new ResourceContentManager(this.Services,manager);

            penumbra = new PenumbraComponent(this, _resources);
            Components.Add(penumbra);

            penumbra.Lights.Add(SunLight);

            Dungeon12.Global.AudioPlayer = this;
            Dungeon12.Global.Time.OnTimeSet += WhenTimeSetted;
            Dungeon12.Global.Time.OnMinute += CalculateSunlight;

            myRenderer= new SpriteBatchRenderer
            {
                GraphicsDeviceService = graphics
            };

            Dungeon12.Global.TransportVariable = GraphicsDevice;
        }

        protected override void Initialize()
        {
            this.Window.Title = "Dungeon 12";
#if Android
            GraphicsDevice.PresentationParameters.IsFullScreen = true;
            var r = GraphicsDevice.Viewport.Bounds;
            var bw = GraphicsDevice.PresentationParameters.BackBufferWidth;
            var bh = GraphicsDevice.PresentationParameters.BackBufferHeight;
            screenScale = Matrix.Identity * Matrix.CreateScale(bw / 1280, bh / 720, 0f);
#endif
#if Core
            Window.TextInput += OnTextInput;
            // TODO: Add your initialization logic here
#endif
            base.Initialize();
        }

        private Effect GlobalImageFilter;

        protected override void LoadContent()
        {
            GraphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            spriteBatch = new SpriteBatch(GraphicsDevice);


            IntPtr winHandle = Window.Handle;            

            SceneManager = new SceneManager
            {
                DrawClient = this
            };

            Global.Camera = this;


            Global.SceneManager = this.SceneManager;

            GlobalImageFilter = _resources.Load<Effect>("ExtractLight");

            SceneManager.Start(isFatal ? "FATAL" : default);
            Network.Start();
            // TODO: use this.Content to load your game content here
        }

        private bool loaded = false;


        private Microsoft.Xna.Framework.GameTime gameTime;

        private bool drawCicled = false;

        protected override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.сallback != default && !skipCallback && drawCicled)
            {
                this.сallback.Call();
                this.сallback = default;
            }

            this.gameTime = gameTime;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed/* || Keyboard.GetState().IsKeyDown(Keys.Escape)*/)
                Exit();

            // TODO: Add your update logic here
            DebugUpdate();
            UpdateLoop();

            drawCicled = false;
            skipCallback = false;

            base.Update(gameTime);
        }

        private void DebugUpdate()
        {
            if (!loaded)
            {
                Dungeon12.Global.TransportVariable = GraphicsDevice;
                myRenderer.LoadContent(Content);
                loaded = true;
            }

            KeyboardState c = Keyboard.GetState();

            if (c.IsKeyDown(Keys.B))
            {
                penumbra.Visible = false;
            }

            if (c.IsKeyDown(Keys.Y))
            {
                penumbra.Visible = true;
            }

            if (c.IsKeyDown(Keys.Down))
            {
                SunLight.Position += new Vector2(0, 50);
                //_particleEffect.Position += new Vector2(0, 10);
            }
            if (c.IsKeyDown(Keys.Up))
            {
                SunLight.Position -= new Vector2(0, 50);
                //_particleEffect.Position -= new Vector2(0, 10);
            }


            if (c.IsKeyDown(Keys.Add))
            {
                SunLight.Scale += new Vector2(50);
                //light.Radius += 10;
            }

            if (c.IsKeyDown(Keys.Subtract))
            {
                SunLight.Scale -= new Vector2(50);
                //light.Radius -= 10;
            }

            if (c.IsKeyDown(Keys.M))
            {
                SunLight.Rotation -= 0.1f;
            }
            if (c.IsKeyDown(Keys.N))
            {
                SunLight.Rotation += 0.1f;
            }

            if(c.IsKeyDown(Keys.OemPlus))
            {
                MediaPlayer.Volume += .01f;
            }
            if (c.IsKeyDown(Keys.OemMinus))
            {
                MediaPlayer.Volume -= .01f;
            }

            if(c.IsKeyDown(Keys.U))
            {
                Dungeon12.Global.Time.Pause();
            }
            if (c.IsKeyDown(Keys.Y))
            {
                Dungeon12.Global.Time.Resume();
            }

            if (c.IsKeyDown(Keys.Left))
            {
                SunLight.Position -= new Vector2(50, 0);
                //_particleEffect.Position -= new Vector2(50, 0);
            }
            if (c.IsKeyDown(Keys.Right))
            {
                SunLight.Position += new Vector2(50, 0);
                //_particleEffect.Position += new Vector2(50, 0);
            }


            // get the latest mouse state
            MouseState ms = Mouse.GetState();
            //// Check if mouse left button was presed
            //if (ms.MiddleButton == ButtonState.Pressed)
            //{
            //    ParticleInit()
            //    _particleEffect.Trigger(new Vector2(626, 121));
            //    var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //    _particleEffect.Update(deltaTime);
            //}
        }

        private IScene scene;

        private Callback сallback;

        private bool skipCallback = false;
        public Callback SetScene(IScene scene)
        {
            this.scene = scene;
            сallback = new Callback(()=>
            {
                scene.Destroy();
            });

            if (drawCicled)
            {
                skipCallback = true;
            }

            return сallback;
        }

        private const float Seconds = 1320;
        private float RotationUnit = (1.5708f - 0.8707998f) / Seconds * 2;
        private float BaseIlluminationUnit = (12350 - 3700) / Seconds;
        private float IllumnationUnit
        {
            get
            {
                float illum = BaseIlluminationUnit;

                if (Dungeon12.Global.Time.Hours >= 6 && Dungeon12.Global.Time.Hours < 18)
                {
                    illum = BaseIlluminationUnit * 2;
                }

                if (Dungeon12.Global.Time.Hours >= 18)
                {

                    illum = BaseIlluminationUnit * 4;
                }

                return illum;
            }
        }

        private readonly float PositionUnit = 6600 / Seconds;

        private void CalculateSunlight()
        {
            var time = Global.Time;
            AddSunLight(1, time);
        }

        private void WhenTimeSetted(Dungeon.Time was, Dungeon.Time now)
        {
            var wasTime = new DateTime(1, 1, 1, was.Hours, was.Minutes, 0);
            var nowTime = new DateTime(1, 1, 1, now.Hours, now.Minutes, 0);

            var minutes = (wasTime - nowTime).TotalMinutes;
            if (minutes > 0)
            {
                //throw new Exception("Двигаем время назад, да?");

                wasTime = new DateTime(1, 1, 1, was.Hours, was.Minutes, 0);
                nowTime = new DateTime(1, 1, 2, now.Hours, now.Minutes, 0);

                minutes = (wasTime - nowTime).TotalMinutes;
            }

            AddSunLight((int)Math.Abs(minutes), was);
        }

        private void AddSunLight(int minutes, Time was)
        {
            for (int i = 0; i < minutes; i++)
            {
                was.AddMinute();
                if (was.Hours >= 4 && was.Hours < 22)
                {
                    this.SunLight.Rotation += RotationUnit;
                    this.SunLight.Position += new Vector2(PositionUnit, 0);

                    if (was.Hours >= 13)
                    {
                        this.SunLight.Scale -= new Vector2(IllumnationUnit);
                    }
                    else
                    {
                        this.SunLight.Scale += new Vector2(IllumnationUnit);
                    }
                }

                if (was.Hours >= 22)
                {
                    SunLight.Rotation = 0.8707998f;
                    SunLight.Scale = new Vector2(3700f);
                    SunLight.Position = new Vector2(-2660, -500);
                }
            }
        }
    }
}