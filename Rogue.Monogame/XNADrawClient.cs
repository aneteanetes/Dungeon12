namespace Rogue
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using MonoGame.Extended;
    using MonoGame.Extended.Particles;
    using MonoGame.Extended.Particles.Modifiers;
    using MonoGame.Extended.Particles.Modifiers.Containers;
    using MonoGame.Extended.Particles.Modifiers.Interpolators;
    using MonoGame.Extended.Particles.Profiles;
    using MonoGame.Extended.TextureAtlases;
    using Penumbra;
    using Rogue.Resources;
    using Rogue.Scenes.Manager;
    using Rogue.Scenes.Menus;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using Rect = Rogue.Types.Rectangle;

    public partial class XNADrawClient : Game, IDrawClient
    {
        private PenumbraComponent penumbra;


        Texture2D _blankTexture;

        public SceneManager SceneManager { get; set; }
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        readonly Light SunLight = new Spotlight
        {
            Scale = new Vector2(3700f),
            ShadowType = ShadowType.Illuminated, // Will not lit hulls themselves,
            Rotation = 0.8707998f,
            Position = new Vector2(-2660, -3100),
            ConeDecay = 5f,
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
            penumbra.Lights.Add(light);

            Global.Time.OnMinute += CalculateSunlight;
        }

        protected override void Initialize()
        {
            this.Window.Title = "Dungeon 12";
            Window.AllowUserResizing = true;
            Window.TextInput += OnTextInput;
            // TODO: Add your initialization logic here

            var _particleTexture = new Texture2D(GraphicsDevice, 1, 1);
            _particleTexture.SetData(new[] { Color.White });

            ParticleInit(new TextureRegion2D(_particleTexture));

            base.Initialize();
        }

        private ParticleEffect _particleEffect;

        private void ParticleInit(TextureRegion2D textureRegion)
        {
            _particleEffect = new ParticleEffect(autoTrigger: false)
            {
                Position = new Vector2(610, 160),
                Emitters = new List<ParticleEmitter>
                {
                    new ParticleEmitter(textureRegion, 500, TimeSpan.FromSeconds(2.5),
                    Profile.BoxFill(603,148)
                        /*Profile.Ring(150f, Profile.CircleRadiation.In)*/)
                    {
                        Parameters = new ParticleReleaseParameters
                        {
                            Speed = new Range<float>(0f, 50f),
                            Quantity = 3,
                            Rotation = new Range<float>(-1f, 1f),
                            Scale = new Range<float>(3.0f, 4.0f)
                        },
                        Modifiers =
                        {
                            new AgeModifier
                            {
                                Interpolators =
                                {
                                    new ColorInterpolator
                                    {
                                        StartValue = HslColor.FromRgb(new Color(90,249,252)),
                                        EndValue = HslColor.FromRgb(new Color(20,124,250))
                                    }
                                }
                            },
                            new RotationModifier {RotationRate = -2.1f},
                            new RectangleContainerModifier {Width = 800, Height = 480},
                            new LinearGravityModifier {Direction = -Vector2.UnitY, Strength = 30f}
                        }
                    }
                }
            };
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            SceneManager = new SceneManager
            {
                DrawClient = this
            };
            SceneManager.Change<Start>();

            // TODO: use this.Content to load your game content here
        }

        private bool visibleEmitters = true;

        protected override void Update(GameTime gameTime)
        {
            KeyboardState c = Keyboard.GetState();

            if (c.IsKeyDown(Keys.K))
            {
                penumbra.Visible = false;
                visibleEmitters = false;
            }

            if (c.IsKeyDown(Keys.L))
            {
                penumbra.Visible = true;
                _particleEffect.Trigger();
                visibleEmitters = true;
            }

            if (c.IsKeyDown(Keys.Down))
            {
                SunLight.Position += new Vector2(0, 50);
                _particleEffect.Position += new Vector2(0, 10);
            }
            if (c.IsKeyDown(Keys.Up))
            {
                SunLight.Position -= new Vector2(0, 50);
                _particleEffect.Position -= new Vector2(0, 10);
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

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _particleEffect.Update(deltaTime);

            UpdateLoop();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed/* || Keyboard.GetState().IsKeyDown(Keys.Escape)*/)
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
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
                SunLight.Position = new Vector2(-2660, -3100);
            }
        }
    }
}