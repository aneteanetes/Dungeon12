namespace Rogue.Scenes
{
    using System;
    using System.Collections.Generic;
    using Rogue.Scenes.Scenes;
    using Rogue.View.Interfaces;
    using Rogue.View.Publish;

    public class SceneManager
    {
        public IDrawClient DrawClient { get; set; }

        private static readonly Dictionary<Type, Scene> SceneCache = new Dictionary<Type, Scene>();

        public Scene Current = null;

        public void Change<TScene>() where TScene : Scene
        {
            if (Current?.Destroyable ?? false)
                Current.Destroy();

            var sceneType = typeof(TScene);

            if (!SceneCache.TryGetValue(sceneType, out var nextScene))
            {
                nextScene = sceneType.New<TScene>(this);
                SceneCache.Add(typeof(TScene), nextScene);
            }

            nextScene.BeforeActivate();
            this.Current = nextScene;

            PublishManager.Set(Current);
            this.Current.Draw();
            this.Current.Activate();
        }
    }
}