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
        
        public void OnKeyPress(KeyArgs keyEventArgs)
        {
            if (!blockedControls)
                KeyPress(keyEventArgs);
        }

        public void OnMousePress(PointerArgs pointerPressedEventArgs)
        {
            if (!blockedControls)
                MousePress(pointerPressedEventArgs);
        }

        protected virtual void KeyPress(KeyArgs keyEventArgs) { }

        protected virtual void MousePress(PointerArgs pointerPressedEventArgs) { }

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

        public void Animation(IAnimationSession animation)
        {
            this.sceneManager.DrawClient.Animate(animation);
        }

        private bool blockedControls = false;

        public void BlockControls(bool block) => blockedControls = block;
    }    
}