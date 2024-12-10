using Dungeon.Scenes.Manager;
using System;

namespace Dungeon.Scenes
{
    public abstract class LoadingScreen : GameScene
    {
        private bool _loadCompletedCalled = false;

        public override bool Destroyable => true;

        protected abstract bool IsNeedWaitUntilLoad { get; }

        private Action _onLoaded;

        public LoadingScreen(SceneManager sceneManager, Action onLoaded) : base(sceneManager)
        {
            _onLoaded = onLoaded;
        }

        public override void Initialize() { }

        public bool BackgroudSceneIsLoaded { get; set; }

        public override void Update(GameTimeLoop gameTimeLoop)
        {
            if (BackgroudSceneIsLoaded)
            {
                if (IsNeedWaitUntilLoad)
                {
                    if (_loadCompletedCalled)
                    {
                        _onLoaded?.Invoke();
                    }
                }
                else
                {
                    _onLoaded?.Invoke();
                }
            }
            base.Update(gameTimeLoop);
        }

        public void LoadComplete() => _loadCompletedCalled = true;
    }
}
