using Dungeon.Scenes.Manager;
using System;

namespace Dungeon.Scenes
{
    public abstract class LoadingScreen : GameScene
    {
        private bool _loadCompletedCalled = false;
        Action _loadCompleted;

        public override bool Destroyable => true;

        public LoadingScreen(SceneManager sceneManager, Action onLoadCompleted) : base(sceneManager)
        {
            _loadCompleted = onLoadCompleted;
        }

        public override void Initialize() { }

        public void LoadComplete()
        {
            if (!_loadCompletedCalled)
            {
                _loadCompletedCalled = true;
                _loadCompleted?.Invoke();
            }
        }
    }
}
