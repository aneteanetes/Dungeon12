using Dungeon.Scenes.Manager;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Dungeon.Monogame
{
    public partial class GameClient
    {
        public SceneManager SceneManager { get; set; }

        private IScene _scene { get; set; }
        public IScene Scene
        {
            get => _scene;
            set
            {
                foreach (var sceneLayer in SceneLayers)
                {
                    sceneLayer.Value.Dispose();
                }
                SceneLayers.Clear();

                SceneLayers = new Dictionary<ISceneLayer, RenderTarget2D>();
                //if (value.Is<Scenes.Sys_Clear_Screen>())
                //{
                //    GraphicsDevice.Clear(Color.Black);
                //}
                _scene = value;
            }
        }

        public Callback SetScene(IScene scene)
        {
            this.Scene = scene;
            сallback = new Callback(() =>
            {
                scene.Destroy();
            });

            if (drawCicled)
            {
                skipCallback = true;
            }

            return сallback;
        }
    }
}
