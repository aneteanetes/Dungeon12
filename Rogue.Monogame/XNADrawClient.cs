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
    using Penumbra;
    using Rogue.Resources;
    using Rogue.Scenes.Manager;
    using Rogue.Scenes.Menus;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using Rect = Rogue.Types.Rectangle;

    public partial class XNADrawClient : Game, IDrawClient
    {
        float ambient = 1f;
        Color ambientColor = Color.White;

        private PenumbraComponent penumbra;


        public SceneManager SceneManager { get; set; }
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Light light = new PointLight
        {
            Scale = new Vector2(1000f), // Range of the light source (how far the light will travel)
            ShadowType = ShadowType.Solid // Will not lit hulls themselves
        };

        public XNADrawClient()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = false,
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720,
                SynchronizeWithVerticalRetrace=true,
            };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // fixing framerate
            this.IsFixedTimeStep = true;//false;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d); //60);

            penumbra = new PenumbraComponent(this);
            Components.Add(penumbra);

            penumbra.Lights.Add(light);
        }

        protected override void Initialize()
        {
            this.Window.Title = "Dungeon 12";
            Window.AllowUserResizing = true;
            Window.TextInput += OnTextInput;
            // TODO: Add your initialization logic here

            base.Initialize();
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

        protected override void Update(GameTime gameTime)
        {
            KeyboardState c = Keyboard.GetState();
            if (c.IsKeyDown(Keys.Space))
            {
                ambient -= 0.01f;
                if (ambient <= 0)
                    ambient = 1f;
            }

            UpdateLoop();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed/* || Keyboard.GetState().IsKeyDown(Keys.Escape)*/)
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        private IScene scene;

        public void SetScene(IScene scene) => this.scene = scene;
    }
}