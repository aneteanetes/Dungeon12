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
    using System.Linq;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using Matrix = Microsoft.Xna.Framework.Matrix;
    using Dungeon.Monogame.Effects;

    public partial class XNADrawClient : Game, IDrawClient
    {

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibraryW(string lpszLib);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);


        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int GetNumVideoDisplays();

        public delegate void GetDisplayBounds(int index, out SDL_Rect rect);

        public struct SDL_Rect
        {
            public int x { get; set; }

            public int y { get; set; }

            public int w { get; set; }

            public int h { get; set; }
        }

        private static int MonitorOffsetX;

        private static List<SDL_Rect> MonitorBounds = new List<SDL_Rect>();

        private void SDL_InitMonitors()
        {
            var entrydll = Assembly.GetExecutingAssembly().Location;
            var root = Path.GetDirectoryName(entrydll);
            var sdlPath = Path.Combine(root, @"runtimes\win-x64\native\SDL2.dll");

            if (!File.Exists(sdlPath))
                sdlPath = Path.Combine(root, "SDL2.dll");

            var SDL = LoadLibraryW(sdlPath);

            var SDL_GetNumVideoDisplays = GetProcAddress(SDL, "SDL_GetNumVideoDisplays");
            var SDL_GetNumVideoDisplaysFunc = Marshal.GetDelegateForFunctionPointer<GetNumVideoDisplays>(SDL_GetNumVideoDisplays);

            var dCount = SDL_GetNumVideoDisplaysFunc();

            var SDL_GetDisplayBounds = GetProcAddress(SDL, "SDL_GetDisplayBounds");
            var SDL_GetDisplayBoundsFunc = Marshal.GetDelegateForFunctionPointer<GetDisplayBounds>(SDL_GetDisplayBounds);


            for (int i = 0; i < dCount; i++)
            {
                SDL_Rect r = new SDL_Rect();
                SDL_GetDisplayBoundsFunc(i,out r);
                MonitorBounds.Add(r);
            }
        }

        private bool SetMonitor(int index)
        {
            var bounds = MonitorBounds.ElementAtOrDefault(index);
            if (bounds.w == 0)
                return false;

            MonitorOffsetX = bounds.x;
            this.Window.Position = new Microsoft.Xna.Framework.Point(bounds.x, 0);

            if (bounds.w != DungeonGlobal.Resolution.Width)
            {
                graphics.PreferredBackBufferWidth = bounds.w;
                graphics.PreferredBackBufferHeight = bounds.h;
                graphics.ApplyChanges();
                DungeonGlobal.Resolution = new View.PossibleResolution(bounds.w, bounds.h);
                //ResolutionScale = Matrix.CreateScale(new Vector3((float)originSize.X / (float)bounds.w, (float)originSize.Y / (float)bounds.h, 1));

                Types.Point left = Types.Point.Zero;
                Types.Point right = Types.Point.Zero;

                var size = new Types.Point(bounds.w, bounds.h);

                if (originSize.X > bounds.w)
                {
                    left = size;
                    right = originSize;
                }
                else if (originSize.X < bounds.w)
                {
                    left = originSize;
                    right = size;
                }

                var scaleX = left.Xf / right.Xf;
                var scaleY = left.Yf / right.Yf;

                var scale = new Vector3(scaleX, scaleY, 1);

                ResolutionScale = Matrix.CreateScale(scale);

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

                return true;
            }

            return false;
        }

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
                PreferredDepthStencilFormat= DepthFormat.Depth24Stencil8,
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

            myRenderer= new SpriteBatchRenderer
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
#warning todo
                Console.WriteLine("SDL cant be inited in release mode, TODO: dev build");
            }
        }

        internal XNADrawClientImplementation XNADrawClientImplementation;

        protected override void Initialize()
        {
            ResourceLoader.ResourceResolvers.Add(new EmbeddedResourceResolver(Assembly.GetExecutingAssembly()));
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
            SetMonitor(clientSettings.MonitorIndex);
            var state = GamePad.GetState(0);
                IsMouseVisible = !state.IsConnected;
                DungeonGlobal.GamePadConnected = state.IsConnected;

            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.ApplyChanges();

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

            //GrayscaleShader.Load(this);

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
            //if (!SetMonitor(clientSettings.MonitorIndex))
                SceneManager.Start(isFatal ? "FATAL" : default);
            Network.Start();
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