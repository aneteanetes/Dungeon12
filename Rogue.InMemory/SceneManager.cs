namespace Rogue.InMemory
{
    using System;
    using System.Collections.Generic;
    using Rogue.InMemory.Scenes;

    public class SceneManager
    {
        private static readonly Dictionary<Type, Scene> SceneCache = new Dictionary<Type, Scene>();

        public Scene Current = null;

        public void Change<TScene>() where TScene : Scene
        {
            if (Current.Destroyable)
                Current.Destroy();

            var sceneType = typeof(TScene);

            if (!SceneCache.TryGetValue(sceneType, out var nextScene))
            {
                nextScene = sceneType.New<TScene>(this);
                SceneCache.Add(typeof(TScene), nextScene);
            }

            nextScene.BeforeActivate();
            this.Current = nextScene;
        }
    }
}