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
                _scene = value;
            }
        }

        public void ValidateAndChangeScene()
        {
            if (_nextScene != null)
            {
                _scene?.Destroy();
                Scene = _nextScene;
                _nextScene = null;
            }
        }

        private IScene _nextScene;

        public void ChangeScene(IScene scene)
        {
            _nextScene = scene;
        }
    }
}
