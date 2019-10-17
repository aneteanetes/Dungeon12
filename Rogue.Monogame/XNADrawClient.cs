namespace Rogue
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;
    using Penumbra;
    using ProjectMercury;
    using ProjectMercury.Renderers;
    using Rogue.Resources;
    using Rogue.Scenes.Manager;
    using Rogue.Scenes.Menus;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;

    public partial class XNADrawClient : Game, IDrawClient
    {
        private PenumbraComponent penumbra;
        
        Texture2D _blankTexture;
        Renderer myRenderer;

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

        public XNADrawClient()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = false,
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720,
                SynchronizeWithVerticalRetrace = true,
            };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // fixing framerate
            this.IsFixedTimeStep = true;//false;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d); //60);

            penumbra = new PenumbraComponent(this);
            Components.Add(penumbra);

            penumbra.Lights.Add(SunLight);

            Global.AudioPlayer = this;
            Global.Time.OnMinute += CalculateSunlight;

            myRenderer= new SpriteBatchRenderer
            {
                GraphicsDeviceService = graphics
            };

            Global.TransportVariable = GraphicsDevice;
        }

        protected override void Initialize()
        {
            this.Window.Title = "Dungeon 12";
            Window.AllowUserResizing = true;
#if Core
            Window.TextInput += OnTextInput;
            // TODO: Add your initialization logic here
#endif
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            GraphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            spriteBatch = new SpriteBatch(GraphicsDevice);

            SceneManager = new SceneManager
            {
                DrawClient = this
            };
            SceneManager.Change<Start>();
            // TODO: use this.Content to load your game content here
        }

        private bool visibleEmitters = true;


        private bool loaded = false;


        private GameTime gameTime;
        
        protected override void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed/* || Keyboard.GetState().IsKeyDown(Keys.Escape)*/)
                Exit();

            // TODO: Add your update logic here
            DebugUpdate();
            UpdateLoop();

            base.Update(gameTime);
        }

        private void DebugUpdate()
        {
            if (!loaded)
            {
                Global.TransportVariable = GraphicsDevice;
                myRenderer.LoadContent(Content);
                loaded = true;
            }

            KeyboardState c = Keyboard.GetState();

            if (c.IsKeyDown(Keys.K))
            {
                penumbra.Visible = false;
                visibleEmitters = false;
            }

            if (c.IsKeyDown(Keys.L))
            {
                penumbra.Visible = true;
                //_particleEffect.Trigger();
                visibleEmitters = true;
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
                Global.Time.Pause();
            }
            if (c.IsKeyDown(Keys.Y))
            {
                Global.Time.Resume();
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

        public void SetScene(IScene scene) => this.scene = scene;


        private const float Seconds = 1320;
        private float RotationUnit = (1.5708f - 0.8707998f) / Seconds * 2;
        private float BaseIlluminationUnit = (12350 - 3700) / Seconds;
        private float IllumnationUnit
        {
            get
            {
                float illum = BaseIlluminationUnit;

                if (Global.Time.Hours >= 6 && Global.Time.Hours < 18)
                {
                    illum = BaseIlluminationUnit * 2;
                }

                if (Global.Time.Hours >= 18)
                {

                    illum = BaseIlluminationUnit * 4;
                }

                return illum;
            }
        }

        private float PositionUnit = 6600 / Seconds;

        private void CalculateSunlight()
        {
            var time = Global.Time;

            if (time.Hours >= 4 && time.Hours < 22)
            {
                this.SunLight.Rotation += RotationUnit;
                this.SunLight.Position += new Vector2(PositionUnit, 0);

                if (time.Hours >= 13)
                {
                    this.SunLight.Scale -= new Vector2(IllumnationUnit);
                }
                else
                {
                    this.SunLight.Scale += new Vector2(IllumnationUnit);
                }
            }

            if (time.Hours >= 22)
            {
                SunLight.Rotation = 0.8707998f;
                SunLight.Scale = new Vector2(3700f);
                SunLight.Position = new Vector2(-2660, -500);
            }
        }
    }
}