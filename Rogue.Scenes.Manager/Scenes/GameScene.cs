namespace Rogue.Scenes
{
    using Rogue.Logging;
    using Rogue.Map;
    using Rogue.Scenes.Manager;
    using System;
    using System.Collections.Generic;

    public abstract class GameScene : CommandScene
    {
        protected readonly List<Type> AvailableScenes = new List<Type>();

        public Rogue.Map.Objects.Avatar PlayerAvatar;

        public virtual bool CameraAffect => false;

        public GameMap Gamemap;

        public Logger Log;

        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
        }

        protected override void Switch<T>()
        {
            if (!AvailableScenes.Contains(typeof(T)))
                throw new Exception($"Scene of type '{typeof(T)}' can't be switched from '{this.GetType()}' scene!");

            base.Switch<T>();
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
}