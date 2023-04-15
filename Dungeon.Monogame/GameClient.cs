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
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;

namespace Dungeon.Monogame
{
    public partial class GameClient : Game, IGameClient
    {
        public static GameClient Instance;

        public GameClient(MonogameSettings settings)
        {
            try
            {
                SDL_InitMonitors();
            }
            catch (Exception ex)
            {
                Console.WriteLine("SDL cant be inited in release mode, TODO: dev build");
            }

            InactiveSleepTime =settings.DropFpsOnUnfocus;
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
        }

        private Vector3 Scale = default;

        protected virtual void GraphicsDeviceManagerInitialization(MonogameSettings settings)
        {
            ResourceLoader.Settings.StretchResources = settings.ResouceStretching;

            var monitor = MonitorBounds.ElementAtOrDefault(settings.MonitorIndex);
            if (monitor.w == 0)
                monitor = MonitorBounds.ElementAtOrDefault(0);

            if (settings.WidthHeightAutomated && monitor.w != originSize.X && (settings.WindowMode== WindowMode.FullScreenSoftware || settings.WindowMode== WindowMode.WindowedScaled))
            {
                settings.WidthPixel = monitor.w;
                settings.HeightPixel = monitor.h;
            }

            originSize = new Types.Dot(settings.OriginWidthPixel, settings.OriginHeightPixel);
            var size = new Types.Dot(settings.WidthPixel, settings.HeightPixel);

            graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = settings.WindowMode== WindowMode.FullScreenSoftware || settings.WindowMode== WindowMode.FullScreenHardware,
                PreferredBackBufferWidth = settings.WidthPixel,
                PreferredBackBufferHeight = settings.HeightPixel,
                SynchronizeWithVerticalRetrace = settings.VerticalSync,
                PreferredDepthStencilFormat= DepthFormat.Depth24Stencil8,
            };

            ResolutionMatrix = Matrix.Identity;

            bool scaling = false;
            Types.Dot left = Types.Dot.Zero;
            Types.Dot right = Types.Dot.Zero;


            if (originSize.X > settings.WidthPixel)
            {
                scaling = true;
                left = originSize;
                right = size;
            }
            else if (originSize.X < settings.WidthPixel)
            {
                scaling = true;
                left = size;
                right = originSize;
            }

            if (scaling)
            {
                var scaleX = left.Xf / right.Xf;
                var scaleY = left.Yf / right.Yf;

                var scale = Scale = new Vector3(scaleX, scaleY, 1);

                ResolutionMatrix = Matrix.CreateScale(scale);
            }
            else Scale = new Vector3(1, 1, 1);

            DungeonGlobal.ResolutionScaleMatrix =
                new System.Numerics.Matrix4x4(
                    ResolutionMatrix.M11,
                    ResolutionMatrix.M12,
                    ResolutionMatrix.M13,
                    ResolutionMatrix.M14,
                    ResolutionMatrix.M21,
                    ResolutionMatrix.M22,
                    ResolutionMatrix.M23,
                    ResolutionMatrix.M24,
                    ResolutionMatrix.M31,
                    ResolutionMatrix.M32,
                    ResolutionMatrix.M33,
                    ResolutionMatrix.M34,
                    ResolutionMatrix.M41,
                    ResolutionMatrix.M42,
                    ResolutionMatrix.M43,
                    ResolutionMatrix.M44);

            DungeonGlobal.ChangeResolution += r =>
            {
                graphics.PreferredBackBufferWidth = r.Width;
                graphics.PreferredBackBufferHeight = r.Height;
                graphics.ApplyChanges();
                DungeonGlobal.Resolution = r;
                var scale  = Matrix.CreateScale(new Vector3((float)originSize.X / (float)r.Width, (float)originSize.Y / (float)r.Height, 1));
                DrawClient.ChangeResolution(scale);
                SceneManager.Start(isFatal ? "FATAL" : default);
            };

            if (settings.WindowMode== WindowMode.FullScreenSoftware)
            {
                graphics.HardwareModeSwitch = false;
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
            DrawClient.ChangeResolution(ResolutionMatrix);

            DungeonGlobal.Camera = this;
            DungeonGlobal.SceneManager = SceneManager =  new SceneManager(this);
            SceneManager.Start(isFatal ? "FATAL" : default);

            LoadFontSystem();
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