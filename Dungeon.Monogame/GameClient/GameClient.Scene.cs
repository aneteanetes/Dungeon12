using Dungeon.Scenes.Manager;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        TaskCompletionSource changeTask = null;

        public void ValidateAndChangeScene()
        {
            if (_nextScene != null)
            {
                _scene?.Destroy();
                Scene = _nextScene;
                _nextScene = null;
                changeTask.SetResult();
            }
        }

        private IScene _nextScene;

        public Task ChangeScene(IScene scene)
        {
            changeTask =  new TaskCompletionSource();
            _nextScene = scene;
            return changeTask.Task;
        }
    }
}
