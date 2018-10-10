namespace Rogue.Scenes
{
    using System;
    using System.Collections.Generic;
    using Rogue.Drawing;
    using Rogue.Drawing.Utils;
    using Rogue.Scenes.Scenes;
    using Rogue.View.Interfaces;
    using Rogue.View.Publish;

    public class SceneManager
    {
        public IDrawClient DrawClient { get; set; }

        public static bool AnimationBlockingInput = false;

        private static readonly Dictionary<Type, GameScene> SceneCache = new Dictionary<Type, GameScene>();

        public GameScene Current = null;

        public void Change<TScene>() where TScene : GameScene
        {
            var sceneType = typeof(TScene);

            if (!SceneCache.TryGetValue(sceneType, out var nextScene))
            {
                nextScene = sceneType.New<TScene>(this);
                SceneCache.Add(typeof(TScene), nextScene);
            }

            if (Current?.Destroyable ?? false)
            {
                Current.Destroy();
                SceneCache.Remove(Current.GetType());
            }

            this.Populate(this.Current, nextScene);
            nextScene.BeforeActivate();
            this.Current = nextScene;

            PublishManager.Set(Current);
            Draw.RunSession<ClearSession>();

            this.Current.Draw();
            this.Current.Activate();
        }

        private void Populate(GameScene previous, GameScene next)
        {
            if (previous == null)
                return;

            next.Player = previous.Player;
            next.Log = previous.Log;
            next.Location = previous.Location;
        }
    }
}