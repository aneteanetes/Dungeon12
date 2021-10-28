namespace Dungeon.Monogame
{
    using Dungeon.Monogame.Effects.Fogofwar;
    using Dungeon.Network;
    using Dungeon.Resources;
    using Dungeon.Scenes.Manager;
    using Dungeon.Settings;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;
    using Newtonsoft.Json;
    using Penumbra;
    using ProjectMercury.Renderers;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Matrix = Microsoft.Xna.Framework.Matrix;

    public partial class XNADrawClient : Game, IDrawClient
    {
        private PenumbraComponent penumbra;

        Renderer myRenderer;

        public bool isFatal;

        public SceneManager SceneManager { get; set; }
        internal GraphicsDeviceManager graphics;
        internal SpriteBatchKnowed spriteBatch;
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


        Matrix ResolutionScale;
        Types.Point originSize;

        protected virtual void GraphicsDeviceManagerInitialization(MonogameClientSettings settings)
        {
            ResourceLoader.Settings.StretchResources = settings.ResouceStretching;

            originSize = new Types.Point(settings.OriginWidthPixel, settings.OriginHeightPixel);
            var size = new Types.Point(settings.WidthPixel, settings.HeightPixel);

            graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = settings.IsFullScreen,
                PreferredBackBufferWidth = settings.WidthPixel,
                PreferredBackBufferHeight = settings.HeightPixel,
                SynchronizeWithVerticalRetrace = settings.VerticalSync,
                PreferredDepthStencilFormat= DepthFormat.Depth24Stencil8
            };

            ResolutionScale = Matrix.Identity;

            bool scaling = false;
            Types.Point left = Types.Point.Zero;
            Types.Point right = Types.Point.Zero;

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
                SceneManager.Start();
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

        public ContentResolver contentResolver;

        private MonogameClientSettings clientSettings;

        public XNADrawClient(MonogameClientSettings settings)
        {
            clientSettings = settings;
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
            //Window.IsBorderless = true;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // fixing framerate
            this.IsFixedTimeStep = true;//false;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d); //60);


            DungeonGlobal.AudioPlayer = this;
            DungeonGlobal.Time.OnTimeSet += WhenTimeSetted;
            DungeonGlobal.Time.OnMinute += CalculateSunlight;

            myRenderer= new SpriteBatchRenderer
            {
                GraphicsDeviceService = graphics
            };

            DungeonGlobal.TransportVariable = GraphicsDevice;
        }

        VertexPositionTexture[] floorVerts;
        // new code:
        BasicEffect effect;


        internal XNADrawClientImplementation XNADrawClientImplementation;

        protected override void Initialize()
        {
            this.Window.Title = DungeonGlobal.GameTitle;
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

            var state = GamePad.GetState(0);
                IsMouseVisible = !state.IsConnected;
                DungeonGlobal.GamePadConnected = state.IsConnected;

            base.Initialize();
        }

        protected virtual string GameTitle => "Dungeon 12";

        private Effect GlobalImageFilter;

        RenderTarget2D backBuffer;

        protected override void LoadContent()
        {
            GraphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;

            backBuffer = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            spriteBatch = new SpriteBatchKnowed(GraphicsDevice);

            SceneManager = new SceneManager
            {
                DrawClient = this
            };

            DungeonGlobal.Camera = this;


            var penumbraShaders = new Dictionary<string, Effect>();            
            var penumbraShaderPaths = new (string path, string key)[]
            {
                ( $"Dungeon.Monogame.Resources.Shaders.PenumbraHull.xnb" ,"PenumbraHull"),
                ( $"Dungeon.Monogame.Resources.Shaders.PenumbraLight.xnb" ,"PenumbraLight"),
                ( $"Dungeon.Monogame.Resources.Shaders.PenumbraShadow.xnb" ,"PenumbraShadow"),
                ( $"Dungeon.Monogame.Resources.Shaders.PenumbraTexture.xnb" ,"PenumbraTexture")
            };


            var asm = Assembly.GetExecutingAssembly();

            foreach (var (path, key) in penumbraShaderPaths)
            {
                using (Stream stream = asm.GetManifestResourceStream(path))
                {
                    if(stream.CanSeek)
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                    }
                    penumbraShaders.Add(key, Content.Load<Effect>(path, stream));
                }
            }

            penumbra = new PenumbraComponent(this, penumbraShaders);
            penumbra.Initialize();
            if (clientSettings.Add2DLighting)
            {
                if (clientSettings.Add2DLighting != default)
                {
                    penumbra.AmbientColor = clientSettings.AmbientColor2DLight;
                }
                Components.Add(penumbra);
            }

            //penumbra.Lights.Add(SunLight);

            DungeonGlobal.SceneManager = this.SceneManager;

            //var pathShader = "Dungeon.Monogame.Resources.Shaders.ExtractLight.xnb";
            //using (Stream stream = asm.GetManifestResourceStream(pathShader))
            //{
            //    if (stream.CanSeek)
            //    {
            //        stream.Seek(0, SeekOrigin.Begin);
            //    }

            //    GlobalImageFilter = Content.Load<Effect>(pathShader, stream);
            //}

            XNADrawClientImplementation = new XNADrawClientImplementation(GraphicsDevice, clientSettings.Add2DLighting ? penumbra : null, spriteBatch, clientSettings.CellSize, GlobalImageFilter, Content, this, myRenderer);

            Load3D();
            ShadowMask = Texture2D.FromFile(GraphicsDevice, @"C:\Users\anete\source\repos\Dungeon12\Dungeon12\Resources\Images\Effects\fow.png");
            //Texture2D.FromFile(GraphicsDevice, @"C:\Users\anete\Source\Repos\Dungeon12\Dungeon.Monogame\Resources\mask.png");
            background = Texture2D.FromFile(GraphicsDevice, @"C:\Users\anete\source\repos\Dungeon12\Dungeon12\Resources\Images\d12back.png");
            bitmap = new RenderTarget2D(GraphicsDevice, 1600, 900, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24Stencil8);
            
            overlay = new RenderTarget2D(GraphicsDevice, 1600, 900, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.None);

            var pp = GraphicsDevice.PresentationParameters;
            bitmap2 = new RenderTarget2D(GraphicsDevice, 1600, 900, false, pp.BackBufferFormat, pp.DepthStencilFormat, pp.MultiSampleCount, pp.RenderTargetUsage);

            SceneManager.Start(isFatal ? "FATAL" : default);
            Network.Start();
        }

        private Texture2D ShadowMask;
        private Texture2D background;
        private RenderTarget2D bitmap;
        private RenderTarget2D overlay;
        private RenderTarget2D bitmap2;

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
            UpdateLoop(gameTime);
            UpdateLayers(gameTime);

            drawCicled = false;
            skipCallback = false;

            base.Update(gameTime);
        }

        private void DebugUpdate()
        {
            if (!loaded)
            {
                DungeonGlobal.TransportVariable = GraphicsDevice;
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

        private IScene _scene { get; set; }
        public IScene scene
        {
            get => _scene;
            set
            {
                SceneLayers = new Dictionary<ISceneLayer, RenderTarget2D>();
                //if (value.Is<Scenes.Sys_Clear_Screen>())
                //{
                //    GraphicsDevice.Clear(Color.Black);
                //}
                _scene = value;
            }
        }

        private Callback сallback;

        private bool skipCallback = false;
        public Callback SetScene(IScene scene)
        {
            XNADrawClientImplementation.scene = scene;
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

                if (DungeonGlobal.Time.Hours >= 6 && DungeonGlobal.Time.Hours < 18)
                {
                    illum = BaseIlluminationUnit * 2;
                }

                if (DungeonGlobal.Time.Hours >= 18)
                {

                    illum = BaseIlluminationUnit * 4;
                }

                return illum;
            }
        }

        private readonly float PositionUnit = 6600 / Seconds;

        private void CalculateSunlight()
        {
            var time = DungeonGlobal.Time;
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
        
        public IEffect GetEffect(string name)
        {
            if (name == "FogOfWar")
                return new FogOfWarEffect();

            return null;
        }
    }
}