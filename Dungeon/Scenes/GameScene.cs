namespace Dungeon.Scenes
{
    using Dungeon.Logging;
    using Dungeon.Scenes.Manager;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class LoadingScene : GameScene
    {
        public LoadingScene():base(default) { }

        public override bool Destroyable => false;

        public override bool AbsolutePositionScene => true;
    }

    public abstract class GameScene : CommandScene
    {
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

        protected override void Switch<T>(params string[] args)
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
    public abstract class GameScene<TScene> : GameScene
    {
        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            AvailableScenes.Add(typeof(TScene));
        }
    }

    /// <summary>
    /// Genric параметры для навигации по коду
    /// </summary>
    /// <typeparam name="TScene"></typeparam>
    /// <typeparam name="TScene1"></typeparam>
    public abstract class GameScene<TScene, TScene1> : GameScene<TScene>
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
    public abstract class GameScene<TScene, TScene1, TScene2> : GameScene<TScene, TScene2>
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
    public abstract class GameScene<TScene, TScene1, TScene2, TScene3> : GameScene<TScene, TScene2, TScene3>
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
    public abstract class GameScene<TScene, TScene1, TScene2, TScene3, TScene4> : GameScene<TScene, TScene2, TScene3, TScene4>
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
    public abstract class GameScene<TScene, TScene1, TScene2, TScene3, TScene4, TScene5> : GameScene<TScene, TScene2, TScene3, TScene4, TScene5>
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

    public abstract class StartScene : GameScene
    {
        public bool IsFatalException => this.Args?.ElementAtOrDefault(0) == "FATAL";

        public virtual void FatalException() { }

        public StartScene(SceneManager sceneManager) : base(sceneManager) { }
    }

    /// <summary>
    /// Genric параметры для навигации по коду
    /// </summary>
    /// <typeparam name="TScene"></typeparam>
    public abstract class StartScene<TScene> : StartScene
    {
        public StartScene(SceneManager sceneManager) : base(sceneManager)
        {
            AvailableScenes.Add(typeof(TScene));
        }
    }

    /// <summary>
    /// Genric параметры для навигации по коду
    /// </summary>
    /// <typeparam name="TScene"></typeparam>
    /// <typeparam name="TScene1"></typeparam>
    public abstract class StartScene<TScene, TScene1> : StartScene<TScene>
    {
        public StartScene(SceneManager sceneManager) : base(sceneManager)
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
    public abstract class StartScene<TScene, TScene1, TScene2> : StartScene<TScene, TScene2>
    {
        public StartScene(SceneManager sceneManager) : base(sceneManager)
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
    public abstract class StartScene<TScene, TScene1, TScene2, TScene3> : StartScene<TScene, TScene2, TScene3>
    {
        public StartScene(SceneManager sceneManager) : base(sceneManager)
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
    public abstract class StartScene<TScene, TScene1, TScene2, TScene3, TScene4> : StartScene<TScene, TScene2, TScene3, TScene4>
    {
        public StartScene(SceneManager sceneManager) : base(sceneManager)
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
    public abstract class StartScene<TScene, TScene1, TScene2, TScene3, TScene4, TScene5> : StartScene<TScene, TScene2, TScene3, TScene4, TScene5>
    {
        public StartScene(SceneManager sceneManager) : base(sceneManager)
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