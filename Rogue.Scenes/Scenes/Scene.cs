namespace Rogue.Scenes.Scenes
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Rogue.Control.Events;
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

            this.BlockControls(false);
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
        
        private List<IControlEventHandler> CanHandle = new List<IControlEventHandler>();
        private List<IControlEventHandler> InFocus = new List<IControlEventHandler>();
        public void OnMouseMove(PointerArgs pointerPressedEventArgs)
        {
            if (sceneManager.Current != this)
                return;

            if (blockedControls)
                return;

            var newFocused = CanHandle.Where(handler => RegionContains(handler, pointerPressedEventArgs))
                .Where(x => !InFocus.Contains(x));

            var newNotFocused = CanHandle.Where(handler => !RegionContains(handler, pointerPressedEventArgs))
                .Where(x => InFocus.Contains(x));

            if (newNotFocused.Count() > 0)
            {
                foreach (var item in newNotFocused)
                {
                    item.Handle(ControlEventType.Unfocus);
                    InFocus.Remove(item);
                }

                this.Activate();
            }

            if (newFocused.Count() > 0)
            {
                foreach (var control in newFocused)
                {
                    control.Handle(ControlEventType.Focus);
                }

                InFocus = newFocused.ToList();

                this.Activate();
            }
        }

        private bool RegionContains(IControlEventHandler drawable, PointerArgs pos)
        {
            var newRegion = new RectangleF
            {
                X = drawable.Location.X * 24 - 3,
                Y = drawable.Location.Y * 24 + 3 -24,
                Height = drawable.Location.Height * 24,
                Width = drawable.Location.Width * 24
            };

            return newRegion.Contains((float)pos.X, (float)pos.Y);
        }

        protected virtual void KeyPress(KeyArgs keyEventArgs) { }

        protected virtual void MousePress(PointerArgs pointerPressedEventArgs)
        {
            var clickedElements = CanHandle.Where(handler => RegionContains(handler, pointerPressedEventArgs));
            foreach (var clickedElement in clickedElements)
            {
                clickedElement.Handle(ControlEventType.Click);
            }
        }

        public abstract void Draw();

        public virtual void Destroy()
        {
            this.BlockControls(true);
            this.CanHandle = null;
        }

        protected virtual void Switch<T>() where T : GameScene
        {
            this.sceneManager.Change<T>();
        }

        private List<IDrawSession> drawSessions = new List<IDrawSession>();
        public void Activate()
        {
            this.BuildHandlerMap();
            this.sceneManager.DrawClient.Draw(drawSessions);
            drawSessions = new List<IDrawSession>();
        }

        public void BuildHandlerMap()
        {
            foreach (var eventHandler in drawSessions.Where(x=>x.IsControlable))
            {
                if (!CanHandle.Contains(eventHandler))
                {
                    CanHandle.Add(eventHandler);
                }
            }
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