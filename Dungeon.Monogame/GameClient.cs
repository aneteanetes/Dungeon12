using Dungeon.Monogame.Resolvers;
using Dungeon.Monogame.Settings;
using Dungeon.Resources;
using Dungeon.Scenes.Manager;
using Dungeon.View.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ProjectMercury.Renderers;
using System;
using System.Reflection;
using System.Reflection.Metadata;

namespace Dungeon.Monogame
{
    public partial class GameClient : Game, IGameClient
    {
        public static GameClient Instance;

        public GameClient(GameSettings settings)
        {
            InactiveSleepTime=settings.DropFpsOnUnfocus;
            Instance =this;
            this._settings = settings;
            GraphicsDeviceManagerInitialization(settings);

            contentResolver = new EmbeddedContentResolver();

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
            Window.Position = new Microsoft.Xna.Framework.Point(500, 500);
            //Window.IsBorderless = true;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // fixing framerate
            this.IsFixedTimeStep = true;//false;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d); //60);


            DungeonGlobal.AudioPlayer = this;
            DungeonGlobal.Time.OnTimeSet += WhenTimeSetted;
            DungeonGlobal.Time.OnMinute += CalculateSunlight;

            ParticleRenderer = new SpriteBatchRenderer
            {
                GraphicsDeviceService = graphics
            };

            DungeonGlobal.TransportVariable = GraphicsDevice;

            try
            {
                SDL_InitMonitors();
            }
            catch
            {
#warning SDL cant be inited in release mode?
                Console.WriteLine("SDL cant be inited in release mode, TODO: dev build");
            }
        }

        protected virtual void GraphicsDeviceManagerInitialization(GameSettings settings)
        {
            ResourceLoader.Settings.StretchResources = settings.ResouceStretching;

            originSize = new Types.Dot(settings.OriginWidthPixel, settings.OriginHeightPixel);
            var size = new Types.Dot(settings.WidthPixel, settings.HeightPixel);

            graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = settings.IsFullScreen,
                PreferredBackBufferWidth = settings.WidthPixel,
                PreferredBackBufferHeight = settings.HeightPixel,
                SynchronizeWithVerticalRetrace = settings.VerticalSync,
                PreferredDepthStencilFormat= DepthFormat.Depth24Stencil8,
            };

            ResolutionScale = Matrix.Identity;

            bool scaling = false;
            Types.Dot left = Types.Dot.Zero;
            Types.Dot right = Types.Dot.Zero;

            if (originSize.X > settings.WidthPixel)
            {
                scaling = true;
                left = size;
                right = originSize;
            }
            else if (originSize.X < settings.WidthPixel)
            {
                scaling = true;
                left = originSize;
                right = size;
            }

            if (scaling)
            {
                var scaleX = left.Xf / right.Xf;
                var scaleY = left.Yf / right.Yf;

                var scale = new Vector3(scaleX, scaleY, 1);

                ResolutionScale = Matrix.CreateScale(scale);
            }

            DungeonGlobal.ResolutionScaleMatrix =
                new System.Numerics.Matrix4x4(
                    ResolutionScale.M11,
                    ResolutionScale.M12,
                    ResolutionScale.M13,
                    ResolutionScale.M14,
                    ResolutionScale.M21,
                    ResolutionScale.M22,
                    ResolutionScale.M23,
                    ResolutionScale.M24,
                    ResolutionScale.M31,
                    ResolutionScale.M32,
                    ResolutionScale.M33,
                    ResolutionScale.M34,
                    ResolutionScale.M41,
                    ResolutionScale.M42,
                    ResolutionScale.M43,
                    ResolutionScale.M44);

            DungeonGlobal.ChangeResolution += r =>
            {
                graphics.PreferredBackBufferWidth = r.Width;
                graphics.PreferredBackBufferHeight = r.Height;
                graphics.ApplyChanges();
                DungeonGlobal.Resolution = r;
                ResolutionScale = Matrix.CreateScale(new Vector3((float)originSize.X / (float)r.Width, (float)originSize.Y / (float)r.Height, 1));
                DrawClient.ChangeResolution(ResolutionScale);
                SceneManager.Start(isFatal ? "FATAL" : default);
            };

            this.Window.IsBorderless = settings.Borderless;
            if (settings.IsWindowedFullScreen)
            {
                graphics.HardwareModeSwitch = false;
                this.Window.IsBorderless = true;
            }

            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        protected override void Initialize()
        {
            ResourceLoader.ResourceResolvers.Add(new EmbeddedResourceResolver(Assembly.GetExecutingAssembly()));
            this.Window.Title = DungeonGlobal.GameTitle;

            Window.TextInput += OnTextInput;

            SetMonitor(_settings.MonitorIndex);

            var state = GamePad.GetState(0);
                IsMouseVisible = !state.IsConnected;
                DungeonGlobal.GamePadConnected = state.IsConnected;

            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            GraphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;

            DefaultSpriteBatch = new SpriteBatchKnowed(GraphicsDevice);
            LayerSpriteBatch = new SpriteBatchKnowed(GraphicsDevice);
            ImageLoader = new ImageLoader(GraphicsDevice);

            LoadPenumbra();
            Load3D();

            DrawClient = new DrawClient(GraphicsDevice, Content, ImageLoader, ParticleRenderer, penumbra)
            {
                SpriteBatchManager=new SpriteBatchManager(GraphicsDevice, Content)
            };

            DungeonGlobal.Camera = this;
            DungeonGlobal.SceneManager = SceneManager =  new SceneManager(this);
            SceneManager.Start(isFatal ? "FATAL" : default);
        }


        protected override void Update(GameTime gameTime)
        {
            if (this.сallback != default && !skipCallback && drawCicled)
            {
                this.сallback.Call();
                this.сallback = default;
            }

            this.gameTime = gameTime;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed/* || Keyboard.GetState().IsKeyDown(Keys.Escape)*/)
                Exit();

            if (_settings.IsDebug)
                DebugUpdate();

            UpdateLoop(gameTime);
            UpdateLayersExistance(gameTime);

            drawCicled = false;
            skipCallback = false;


            gameTimePrev = gameTime.TotalGameTime;

            base.Update(gameTime);
        }

        private void DebugUpdate()
        {
            if (!loaded)
            {
                DungeonGlobal.TransportVariable = GraphicsDevice;
                ParticleRenderer.LoadContent(Content);
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

            //if (c.IsKeyDown(Keys.Down))
            //{
            //    SunLight.Position += new Vector2(0, 50);
            //    //_particleEffect.Position += new Vector2(0, 10);
            //}
            //if (c.IsKeyDown(Keys.Up))
            //{
            //    SunLight.Position -= new Vector2(0, 50);
            //    //_particleEffect.Position -= new Vector2(0, 10);
            //}


            //if (c.IsKeyDown(Keys.Add))
            //{
            //    SunLight.Scale += new Vector2(50);
            //    //light.Radius += 10;
            //}

            //if (c.IsKeyDown(Keys.Subtract))
            //{
            //    SunLight.Scale -= new Vector2(50);
            //    //light.Radius -= 10;
            //}

            //if (c.IsKeyDown(Keys.M))
            //{
            //    SunLight.Rotation -= 0.1f;
            //}
            //if (c.IsKeyDown(Keys.N))
            //{
            //    SunLight.Rotation += 0.1f;
            //}

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
                DungeonGlobal.Time.Pause();
            }
            if (c.IsKeyDown(Keys.Y))
            {
                DungeonGlobal.Time.Resume();
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
    }
}