namespace Rogue.Scenes
{
    using Rogue.Control.Events;
    using Rogue.Control.Keys;
    using Rogue.Control.Pointer;
    using Rogue.Scenes.Manager;
    using Rogue.Settings;
    using Rogue.Types;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

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

        protected void AddControl(ISceneObjectControl sceneObjectControl)
        {
            SceneObjectsControllable.Add(sceneObjectControl);
        }

        protected void RemoveControl(ISceneObjectControl sceneObjectControl)
        {
            SceneObjectsControllable.Remove(sceneObjectControl);
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

        public void OnText(string text)
        {
            var textControls = ControlsByHandle(ControlEventType.Text);

            for (int i = 0; i < textControls.Count(); i++)
            {
                var textControl = textControls.ElementAtOrDefault(i);
                if (textControl != null)
                {
                    textControl.TextInput(text);
                }
            }
        }

        public void OnKeyDown(KeyArgs keyEventArgs)
        {
            var key = keyEventArgs.Key;
            var modifier = keyEventArgs.Modifiers;


            if (Global.FreezeWorld == null || Global.FreezeWorld == this)
                KeyPress(key, modifier, keyEventArgs.Hold);

            var keyControls = ControlsByHandle(ControlEventType.Key, keyEventArgs.Key).ToArray();
            foreach (var sceneObjectHandler in keyControls)
            {
                sceneObjectHandler.KeyDown(key, modifier, keyEventArgs.Hold);
            }
        }

        public void OnKeyUp(KeyArgs keyEventArgs)
        {
            var key = keyEventArgs.Key;
            var modifier = keyEventArgs.Modifiers;
            
            if (Global.FreezeWorld == null || Global.FreezeWorld == this)
                KeyUp(key, modifier);

            var keyControls = ControlsByHandle(ControlEventType.Key, keyEventArgs.Key);
            foreach (var sceneObjectHandler in keyControls)
            {
                sceneObjectHandler.KeyUp(key, modifier);
            }
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

        public void OnMouseRelease(PointerArgs pointerPressedEventArgs, Point offset)
        {
            var keyControls = ControlsByHandle(ControlEventType.ClickRelease);
            var globalKeyHandlers = ControlsByHandle(ControlEventType.GlobalClickRelease);

            if (globalKeyHandlers.Count() != 0)
            {
                DoClicks(pointerPressedEventArgs, offset, globalKeyHandlers, (c, a) => c.GlobalClickRelease(a));
            }

            var clickedElements = keyControls.Where(so => RegionContains(so, pointerPressedEventArgs, offset));
            DoClicks(pointerPressedEventArgs, offset, clickedElements, (c, a) => c.ClickRelease(a));
        }

        public void OnMouseWheel(MouseWheelEnum wheelEnum)
        {
            var wheelControls = ControlsByHandle(ControlEventType.MouseWheel);

            for (int i = 0; i < wheelControls.Count(); i++)
            {
                var wheelControl = wheelControls.ElementAtOrDefault(i);
                if (wheelControl != null)
                {
                    wheelControl.MouseWheel(wheelEnum);
                }
            }
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
                        Y = pointerPressedEventArgs.Y,
                        Offset=offset
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

            OnMouseMoveOnFocus(pointerPressedEventArgs, offset);

            var moveControls = ControlsByHandle(ControlEventType.MouseMove)
                .Where(so => RegionContains(so, pointerPressedEventArgs, offset));

            DoClicks(pointerPressedEventArgs, offset, moveControls, (c, a) => c.MouseMove(a));
        }

        private void OnMouseMoveOnFocus(PointerArgs pointerPressedEventArgs, Point offset)
        {
            var controls = ControlsByHandle(ControlEventType.Focus);

            var newFocused = controls.Where(handler => RegionContains(handler, pointerPressedEventArgs, offset))
                .Where(x => !SceneObjectsInFocus.Contains(x))
                .ToArray();

            var newLostFocused = SceneObjectsInFocus.Where(x => !RegionContains(x, pointerPressedEventArgs, offset))
                .ToArray();

            foreach (var item in newLostFocused)
            {
                item.Unfocus();
                SceneObjectsInFocus.Remove(item);
            }

            if (newFocused.Count() > 0)
            {
                foreach (var control in newFocused)
                {
                    control.Focus();
                }

                SceneObjectsInFocus.AddRange(newFocused);
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

        protected virtual void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold) { }

        protected virtual void KeyUp(Key keyPressed, KeyModifiers keyModifiers) { }

        private IEnumerable<ISceneObjectControl> ControlsByHandle(ControlEventType handleEvent, Key key = Key.None)
        {
            if (Global.FreezeWorld != null)
            {
                return SceneObjectsControllable.Where(x => x == Global.FreezeWorld);
            }
            else
            {
                return SceneObjectsControllable.Where(x =>
                {
                    bool handle = x.CanHandle.Contains(handleEvent);

                    if (handleEvent == ControlEventType.Key)
                    {
                        handle = x.AllKeysHandle || x.KeysHandle.Contains(key);
                    }

                    return handle;
                });
            }
        }

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

        protected void Switch(string sceneClassName)
        {

            
        }

        #endregion

    }
}