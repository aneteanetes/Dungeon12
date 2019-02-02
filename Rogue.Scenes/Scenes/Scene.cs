namespace Rogue.Scenes.Scenes
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Timers;
    using Rogue.Control.Events;
    using Rogue.Control.Keys;
    using Rogue.Control.Pointer;
    using Rogue.Drawing.Utils;
    using Rogue.Settings;
    using Rogue.View.Interfaces;

    public abstract class Scene : IScene
    {
        private readonly SceneManager sceneManager;
        public abstract bool Destroyable { get; }

        public Scene(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        #region scene object collection

        public ISceneObject[] Objects => SceneObjects.ToArray();

        private List<ISceneObject> SceneObjects = new List<ISceneObject>();

        private List<ISceneObjectControl> SceneObjectsControllable = new List<ISceneObjectControl>();

        private List<ISceneObjectControl> SceneObjectsInFocus = new List<ISceneObjectControl>();

        protected void AddObject(ISceneObject sceneObject)
        {
            if (sceneObject is ISceneObjectControl sceneObjectControl)
            {
                SceneObjectsControllable.Add(sceneObjectControl);
            }

            SceneObjects.Add(sceneObject);
        }

        protected void RemoveObject(ISceneObject sceneObject)
        {
            if (sceneObject is ISceneObjectControl sceneObjectControl)
            {
                SceneObjectsControllable.Remove(sceneObjectControl);
            }

            SceneObjects.Remove(sceneObject);
        }

        #endregion

        #region scene contollable
        
        public void OnKeyDown(KeyArgs keyEventArgs)
        {
            var key = keyEventArgs.Key;
            var modifier = keyEventArgs.Modifiers;

            for (int i = 0; i < SceneObjectsControllable.Count; i++)
            {
                SceneObjectsControllable[i].KeyDown(key, modifier);
            }

            KeyPress(key,modifier);
        }

        public void OnKeyUp(KeyArgs keyEventArgs)
        {
            var key = keyEventArgs.Key;
            var modifier = keyEventArgs.Modifiers;

            for (int i = 0; i < SceneObjectsControllable.Count; i++)
            {
                SceneObjectsControllable[i].KeyUp(keyEventArgs.Key, keyEventArgs.Modifiers);
            }

            KeyPress(key, modifier);
        }

        public void OnMousePress(PointerArgs pointerPressedEventArgs)
        {
            var clickedElements = SceneObjectsControllable.Where(so => RegionContains(so, pointerPressedEventArgs));
            foreach (var clickedElement in clickedElements)
            {
                clickedElement.Click();
            }
        }

        public void OnMouseMove(PointerArgs pointerPressedEventArgs)
        {
            if (sceneManager.Current != this)
                return;
            
            var newFocused = SceneObjectsControllable.Where(handler => RegionContains(handler, pointerPressedEventArgs))
                .Where(x => !SceneObjectsInFocus.Contains(x));

            var newNotFocused = SceneObjectsControllable.Where(handler => !RegionContains(handler, pointerPressedEventArgs))
                .Where(x => SceneObjectsInFocus.Contains(x));

            if (newNotFocused.Count() > 0)
            {
                foreach (var item in newNotFocused)
                {
                    item.Unfocus();
                    SceneObjectsInFocus.Remove(item);
                }
            }

            if (newFocused.Count() > 0)
            {
                foreach (var control in newFocused)
                {
                    control.Focus();
                }

                SceneObjectsInFocus = newFocused.ToList();
            }
        }

        private bool RegionContains(ISceneObjectControl sceneObjControl, PointerArgs pos)
        {
            var newRegion = new RectangleF
            {
                X = sceneObjControl.Position.X * DrawingSize.CellF,
                Y = sceneObjControl.Position.Y * DrawingSize.CellF,
                Height = sceneObjControl.Position.Height * DrawingSize.CellF,
                Width = sceneObjControl.Position.Width * DrawingSize.CellF
            };

            return newRegion.Contains((float)pos.X, (float)pos.Y);
        }

        protected virtual void KeyPress(Key keyPressed, KeyModifiers keyModifiers) { }

        protected virtual void KeyUp(Key keyPressed, KeyModifiers keyModifiers) { }

        #endregion
                        
        public abstract void Draw();

        public virtual void Init() { }

        public void Activate()
        {
            this.sceneManager.DrawClient.SetScene(this);
        }

        #region protected utils
        
        protected virtual void Switch<T>() where T : GameScene
        {
            this.sceneManager.Change<T>();
        }

        #endregion

    }
}