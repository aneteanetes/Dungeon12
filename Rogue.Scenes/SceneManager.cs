namespace Rogue.Scenes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Rogue.Drawing;
    using Rogue.Drawing.Utils;
    using Rogue.Scenes.Scenes;
    using Rogue.View.Interfaces;
    using Rogue.View.Publish;

    public class SceneManager
    {
        public IDrawClient DrawClient { get; set; }
        
        private static readonly Dictionary<Type, GameScene> SceneCache = new Dictionary<Type, GameScene>();

        public GameScene Current { get; private set; }

        private static object currentVisual;

        public static Action<object> DestroyVisual { get; set; }

        public static Action<object> CreateVisual { get; set; }

        public static Action OnVisualDestroy { get; set; }

        public static Action<object> keyDown;
        public static void KeyDown<T>(T arg)
        {
            keyDown(arg);
        }

        public void Switch<TVisual>()
        {
            var sceneType = typeof(TVisual).BaseType.GetGenericArguments().First();

            if (!SceneCache.TryGetValue(sceneType, out var nextScene))
            {
                nextScene = (GameScene)sceneType.New(this);
                SceneCache.Add(sceneType, nextScene);
            }
            else
            {
                nextScene.ResumeScene();
            }

            this.Populate(Current, nextScene);

            if (Current?.Destroyable ?? false)
            {
                Current.Destroy();
                SceneCache.Remove(Current.GetType());
            }
            else
            {
                Current?.FreezeScene();
            }

            if (currentVisual != null)
            {
                DestroyVisual?.Invoke(currentVisual);
                OnVisualDestroy?.Invoke();
                OnVisualDestroy = null;
            }

            nextScene.BeforeActivate();
            Current = nextScene;

            PublishManager.Set(Current);
            Draw.RunSession<ClearSession>();

            Current.Render();

            currentVisual = typeof(TVisual).New<TVisual>(Current);
            CreateVisual(currentVisual);
        }

        public void Change<TScene>() where TScene : GameScene
        {
            var sceneType = typeof(TScene);

            if (!SceneCache.TryGetValue(sceneType, out var nextScene))
            {
                nextScene = sceneType.New<TScene>(this);
                SceneCache.Add(typeof(TScene), nextScene);
            }
            else
            {
                nextScene.ResumeScene();
            }

            this.Populate(Current, nextScene);

            if (Current?.Destroyable ?? false)
            {
                Current.Destroy();
                SceneCache.Remove(Current.GetType());
            }
            else
            {
                Current?.FreezeScene();
            }

            nextScene.BeforeActivate();
            Current = nextScene;

            PublishManager.Set(Current);
            Draw.RunSession<ClearSession>();

            Current.Render();
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