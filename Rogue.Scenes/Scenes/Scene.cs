namespace Rogue.Scenes.Scenes
{
    using Rogue.Control.Events;
    using Rogue.Control.Keys;
    using Rogue.Control.Pointer;
    using Rogue.Settings;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

        public virtual bool AbsolutePositionScene => true;

        private List<ISceneObject> SceneObjects = new List<ISceneObject>();

        private List<ISceneObjectControl> SceneObjectsControllable = new List<ISceneObjectControl>();

        private List<ISceneObjectControl> SceneObjectsInFocus = new List<ISceneObjectControl>();

        protected void AddObject(ISceneObject sceneObject)
        {
            AddControlRecursive(sceneObject);

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

        private void AddControlRecursive(ISceneObject sceneObject)
        {
            if (sceneObject is ISceneObjectControl sceneObjectControl)
            {
                SceneObjectsControllable.Add(sceneObjectControl);
            }

            foreach (var childSceneObject in sceneObject.Children)
            {
                AddControlRecursive(childSceneObject);
            }
        }

        #endregion

        #region scene contollable

        public void OnKeyDown(KeyArgs keyEventArgs)
        {
            var key = keyEventArgs.Key;
            var modifier = keyEventArgs.Modifiers;

            var keyControls = ControlsByHandle(ControlEventType.Key, keyEventArgs.Key).ToArray();
            foreach (var sceneObjectHandler in keyControls)
            {
                sceneObjectHandler.KeyDown(key, modifier);
            }

            KeyPress(key,modifier);
        }

        public void OnKeyUp(KeyArgs keyEventArgs)
        {
            var key = keyEventArgs.Key;
            var modifier = keyEventArgs.Modifiers;

            var keyControls = ControlsByHandle(ControlEventType.Key, keyEventArgs.Key);
            foreach (var sceneObjectHandler in keyControls)
            {
                sceneObjectHandler.KeyUp(key, modifier);
            }

            KeyPress(key, modifier);
        }

        public void OnMousePress(PointerArgs pointerPressedEventArgs, Point offset)
        {
            var keyControls = ControlsByHandle(ControlEventType.Click);
            var globalKeyHandlers = ControlsByHandle(ControlEventType.GlobalClick);

            if (globalKeyHandlers.Count() != 0)
            {
                DoClicks(pointerPressedEventArgs, offset, globalKeyHandlers, (c, a) => c.GlobalClick(a));
            }
            
            var clickedElements = keyControls.Where(so => RegionContains(so, pointerPressedEventArgs, offset));
            DoClicks(pointerPressedEventArgs, offset, clickedElements, (c, a) => c.Click(a));
        }

        private void DoClicks(PointerArgs pointerPressedEventArgs, Point offset, IEnumerable<ISceneObjectControl> clickedElements,
            Action<ISceneObjectControl,PointerArgs> whichClick)
        {
            for (int i = 0; i < clickedElements.Count(); i++)
            {
                var clickedElement = clickedElements.ElementAtOrDefault(i);
                if (clickedElement != null)
                {
                    var args = new PointerArgs
                    {
                        ClickCount = pointerPressedEventArgs.ClickCount,
                        MouseButton = pointerPressedEventArgs.MouseButton,
                        X = pointerPressedEventArgs.X,
                        Y = pointerPressedEventArgs.Y
                    };

                    if (!this.AbsolutePositionScene && !clickedElement.AbsolutePosition)
                    {
                        args.X += offset.X;
                        args.Y += offset.Y;
                    }

                    whichClick(clickedElement, args);
                }
            }
        }

        public void OnMouseMove(PointerArgs pointerPressedEventArgs, Point offset)
        {
            if (sceneManager.Current != this)
                return;
            
            var keyControls = ControlsByHandle(ControlEventType.Focus);

            var newFocused = keyControls.Where(handler => RegionContains(handler, pointerPressedEventArgs, offset))
                .Where(x => !SceneObjectsInFocus.Contains(x));

            var newNotFocused = keyControls.Where(handler => !RegionContains(handler, pointerPressedEventArgs,offset))
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

        private bool RegionContains(ISceneObjectControl sceneObjControl, PointerArgs pos, Point offset)
        {
            var newRegion = new Rectangle
            {
                X = sceneObjControl.ComputedPosition.X * DrawingSize.CellF,
                Y = sceneObjControl.ComputedPosition.Y * DrawingSize.CellF,
                Height = sceneObjControl.Position.Height * DrawingSize.CellF,
                Width = sceneObjControl.Position.Width * DrawingSize.CellF
            };

            if(!this.AbsolutePositionScene && !sceneObjControl.AbsolutePosition)
            {
                newRegion.X += offset.X;
                newRegion.Y += offset.Y;
            }

            return newRegion.Contains(pos.X, pos.Y);
        }

        protected virtual void KeyPress(Key keyPressed, KeyModifiers keyModifiers) { }

        protected virtual void KeyUp(Key keyPressed, KeyModifiers keyModifiers) { }

        private IEnumerable<ISceneObjectControl> ControlsByHandle(ControlEventType handleEvent, Key key = Key.None)
            => SceneObjectsControllable.Where(x =>
            {
                bool handle = x.CanHandle.Contains(handleEvent);

                if (handleEvent == ControlEventType.Key)
                {
                    handle = x.KeysHandle.Contains(key);
                }

                return handle;
            });

        #endregion

        [Obsolete("Старый метод, как только подсистема перепишется - выпили его к хуям пожалуйста")]
        public virtual void Draw() { }

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