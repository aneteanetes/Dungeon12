namespace Rogue.Scenes.Scenes
{
    using System;
    using System.Collections.Generic;
    using Rogue.Scenes.Controls.Keys;
    using Rogue.Scenes.Controls.Pointer;
    using Rogue.View.Interfaces;

    public abstract class Scene : IPublisher
    {
        private readonly SceneManager sceneManager;

        public Scene(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        public abstract bool Destroyable { get; }

        public virtual void BeforeActivate() { }


        public virtual void KeyPress(KeyArgs keyEventArgs) { }

        public virtual void MousePress(PointerArgs pointerPressedEventArgs) { }


        public abstract void Draw();

        public virtual void Destroy() { }

        protected virtual void Switch<T>() where T : Scene
        {
            this.sceneManager.Change<T>();
        }

        private List<IDrawSession> drawSessions = new List<IDrawSession>();
        public void Activate()
        {
            this.sceneManager.DrawClient.Draw(drawSessions);
            drawSessions = new List<IDrawSession>();
        }

        protected void Redraw()
        {
            this.Activate();
        }

        public void Publish(List<IDrawSession> drawSessions)
        {
            this.drawSessions.AddRange(drawSessions);
        }
    }

    /// <summary>
    /// generics - switches
    /// </summary>
    /// <typeparam name="TScene"></typeparam>
    public abstract class Scene<TScene> : Scene
    {
        protected readonly List<Type> AvailableScenes = new List<Type>();

        public Scene(SceneManager sceneManager) : base(sceneManager)
        {
            AvailableScenes.Add(typeof(TScene));
        }

        protected override void Switch<T>()
        {
            if (!AvailableScenes.Contains(typeof(T)))
                throw new Exception($"Scene of type '{typeof(T)}' can't be switched from '{this.GetType()}' scene!");

            base.Switch<T>();
        }
    }

    /// <summary>
    /// generics - switches
    /// </summary>
    /// <typeparam name="TScene"></typeparam>
    /// <typeparam name="TScene1"></typeparam>
    public abstract class Scene<TScene, TScene1> : Scene<TScene>
    {
        public Scene(SceneManager sceneManager) : base(sceneManager)
        {
            this.AvailableScenes.Add(typeof(TScene1));
        }
    }

    /// <summary>
    /// generics - switches
    /// </summary>
    /// <typeparam name="TScene"></typeparam>
    /// <typeparam name="TScene1"></typeparam>
    /// <typeparam name="TScene2"></typeparam>
    public abstract class Scene<TScene, TScene1, TScene2> : Scene<TScene, TScene2>
    {
        public Scene(SceneManager sceneManager) : base(sceneManager)
        {
            this.AvailableScenes.Add(typeof(TScene2));
        }
    }

    /// <summary>
    /// generics - switches
    /// </summary>
    /// <typeparam name="TScene"></typeparam>
    /// <typeparam name="TScene1"></typeparam>
    /// <typeparam name="TScene2"></typeparam>
    /// <typeparam name="TScene3"></typeparam>
    public abstract class Scene<TScene, TScene1, TScene2, TScene3> : Scene<TScene, TScene2, TScene3>
    {
        public Scene(SceneManager sceneManager) : base(sceneManager)
        {
            this.AvailableScenes.Add(typeof(TScene3));
        }
    }

    /// <summary>
    /// generics - switches
    /// </summary>
    /// <typeparam name="TScene"></typeparam>
    /// <typeparam name="TScene1"></typeparam>
    /// <typeparam name="TScene2"></typeparam>
    /// <typeparam name="TScene3"></typeparam>
    /// <typeparam name="TScene4"></typeparam>
    public abstract class Scene<TScene, TScene1, TScene2, TScene3, TScene4> : Scene<TScene, TScene2, TScene3, TScene4>
    {
        public Scene(SceneManager sceneManager) : base(sceneManager)
        {
            this.AvailableScenes.Add(typeof(TScene4));
        }
    }

    /// <summary>
    /// generics - switches
    /// </summary>
    /// <typeparam name="TScene"></typeparam>
    /// <typeparam name="TScene1"></typeparam>
    /// <typeparam name="TScene2"></typeparam>
    /// <typeparam name="TScene3"></typeparam>
    /// <typeparam name="TScene4"></typeparam>
    /// <typeparam name="TScene5"></typeparam>
    public abstract class Scene<TScene, TScene1, TScene2, TScene3, TScene4, TScene5> : Scene<TScene, TScene2, TScene3, TScene4, TScene5>
    {
        public Scene(SceneManager sceneManager) : base(sceneManager)
        {
            this.AvailableScenes.Add(typeof(TScene5));
        }
    }
}