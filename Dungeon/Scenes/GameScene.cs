namespace Dungeon.Scenes
{
    using Dungeon.Localization;
    using Dungeon.Logging;
    using Dungeon.Scenes.Manager;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class GameScene : CommandScene
    {
        public Type LoadingScreenType { get; protected set; }

        public object Freezer;

        public bool InGame { get; set; }

        protected readonly List<Type> AvailableScenes = new List<Type>();

        public string[] Args { get; set; }

        public virtual bool CameraAffect => false;

        public Logger Log;

        public virtual void ClearState()
        {
            Args = default;
        }

        public virtual bool Loadable => false;

        public virtual object[] LoadArguments => default;

        public virtual bool LoadingScreen => false;

        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        public override void Switch<T>(params string[] args)
        {
            if (!AvailableScenes.Contains(typeof(T)))
                throw new Exception($"Scene of type '{typeof(T)}' can't be switched from '{this.GetType()}' scene!");

            base.Switch<T>(args);
        }
    }

    /// <summary>
    /// Genric параметры для навигации по коду
    /// </summary>
    /// <typeparam name="TScene"></typeparam>
    public abstract class GameScene<TLoadingScene,TScene> : GameScene
        where TLoadingScene : LoadingScreen
        where TScene : GameScene
    {
        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            LoadingScreenType = typeof(TLoadingScene);
            AvailableScenes.Add(typeof(TScene));
        }
    }

    /// <summary>
    /// Genric параметры для навигации по коду
    /// </summary>
    /// <typeparam name="TScene"></typeparam>
    /// <typeparam name="TScene1"></typeparam>
    public abstract class GameScene<TLoadingScene,TScene, TScene1> : GameScene<TLoadingScene,TScene>
        where TLoadingScene : LoadingScreen
        where TScene : GameScene
        where TScene1 : GameScene
    {
        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            this.AvailableScenes.Add(typeof(TScene));
            this.AvailableScenes.Add(typeof(TScene1));
        }
    }

    /// <summary>
    /// Genric параметры для навигации по коду
    /// </summary>
    /// <typeparam name="TScene"></typeparam>
    /// <typeparam name="TScene1"></typeparam>
    /// <typeparam name="TScene2"></typeparam>
    public abstract class GameScene<TLoadingScene, TScene, TScene1, TScene2> : GameScene<TLoadingScene, TScene, TScene1>
        where TLoadingScene : LoadingScreen
        where TScene : GameScene
        where TScene1 : GameScene
        where TScene2 : GameScene
    {
        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            this.AvailableScenes.Add(typeof(TScene));
            this.AvailableScenes.Add(typeof(TScene1));
            this.AvailableScenes.Add(typeof(TScene2));
        }
    }

    /// <summary>
    /// Genric параметры для навигации по коду
    /// </summary>
    /// <typeparam name="TScene"></typeparam>
    /// <typeparam name="TScene1"></typeparam>
    /// <typeparam name="TScene2"></typeparam>
    /// <typeparam name="TScene3"></typeparam>
    public abstract class GameScene<TLoadingScene, TScene, TScene1, TScene2, TScene3> : GameScene<TLoadingScene, TScene, TScene1, TScene2>
        where TLoadingScene : LoadingScreen
        where TScene : GameScene
        where TScene1 : GameScene
        where TScene2 : GameScene
        where TScene3 : GameScene
    {
        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            this.AvailableScenes.Add(typeof(TScene));
            this.AvailableScenes.Add(typeof(TScene1));
            this.AvailableScenes.Add(typeof(TScene2));
            this.AvailableScenes.Add(typeof(TScene3));
        }
    }

    /// <summary>
    /// Genric параметры для навигации по коду
    /// </summary>
    /// <typeparam name="TScene"></typeparam>
    /// <typeparam name="TScene1"></typeparam>
    /// <typeparam name="TScene2"></typeparam>
    /// <typeparam name="TScene3"></typeparam>
    /// <typeparam name="TScene4"></typeparam>
    public abstract class GameScene<TLoadingScene, TScene, TScene1, TScene2, TScene3, TScene4> : GameScene<TLoadingScene, TScene, TScene1, TScene2, TScene3>
        where TLoadingScene : LoadingScreen
        where TScene : GameScene
        where TScene1 : GameScene
        where TScene2 : GameScene
        where TScene3 : GameScene
        where TScene4 : GameScene
    {
        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            this.AvailableScenes.Add(typeof(TScene));
            this.AvailableScenes.Add(typeof(TScene1));
            this.AvailableScenes.Add(typeof(TScene2));
            this.AvailableScenes.Add(typeof(TScene3));
            this.AvailableScenes.Add(typeof(TScene4));
        }
    }

    /// <summary>
    /// Genric параметры для навигации по коду
    /// </summary>
    /// <typeparam name="TScene"></typeparam>
    /// <typeparam name="TScene1"></typeparam>
    /// <typeparam name="TScene2"></typeparam>
    /// <typeparam name="TScene3"></typeparam>
    /// <typeparam name="TScene4"></typeparam>
    /// <typeparam name="TScene5"></typeparam>
    public abstract class GameScene<TLoadingScene, TScene, TScene1, TScene2, TScene3, TScene4, TScene5> : GameScene<TLoadingScene, TScene, TScene1, TScene2, TScene3, TScene4>
        where TLoadingScene : LoadingScreen
        where TScene : GameScene
        where TScene1 : GameScene
        where TScene2 : GameScene
        where TScene3 : GameScene
        where TScene4 : GameScene
        where TScene5 : GameScene
    {
        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            this.AvailableScenes.Add(typeof(TScene));
            this.AvailableScenes.Add(typeof(TScene1));
            this.AvailableScenes.Add(typeof(TScene2));
            this.AvailableScenes.Add(typeof(TScene3));
            this.AvailableScenes.Add(typeof(TScene4));
            this.AvailableScenes.Add(typeof(TScene5));
        }
    }
}