using Dungeon.Monogame;
using Dungeon.Network;
using Dungeon.Resources;
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
namespace VegaPQ
{

    public class VegaMG : XNADrawClient
    {
        Matrix screenScale = Matrix.Identity;

        public VegaMG()
        {
            this.Window.ClientSizeChanged += HeyCallMeWhenTheWindowChangesSize;
            
            this.Activated += (_, __) =>
            {
                //blockControls = false;
            };

            this.Deactivated += (_, __) =>
            {
                //blockControls = true;
            };

            Window.AllowUserResizing = false;
            //Window.IsBorderless = true;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // fixing framerate
            this.IsFixedTimeStep = true;//false;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d); //60);


            Dungeon12.Global.AudioPlayer = this;
        }

        protected override string GameTitle => "Vega PQ";

        protected override void GraphicsDeviceManagerInitialization()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = false,
#if Android
                true,
#endif
#if Core
                false,
#endif
                PreferredBackBufferWidth = designWidth,
                PreferredBackBufferHeight = designHeight,
                SynchronizeWithVerticalRetrace = true,
            };
        }

        private int designWidth = 540;//960;//1920;
        private int designHeight = 960;//540;//1080;

        public void HeyCallMeWhenTheWindowChangesSize(object sender, EventArgs e)
        {
            /* we are here when the windows being resized */
            var r = GraphicsDevice.Viewport.Bounds;
            screenScale = Matrix.Identity * Matrix.CreateScale(r.Width / designWidth, r.Height / designHeight, 0f);
        }
    }
}
