namespace Rogue.Scenes.Scenes
{
    using System.Collections.Generic;
    using Rogue.Control.Keys;
    using Rogue.Control.Pointer;
    using Rogue.Drawing.Utils;
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

        public virtual void Destroy()
        {

        }

        protected virtual void Switch<T>() where T : GameScene
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
}