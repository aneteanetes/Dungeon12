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
    using System.Diagnostics;
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
            if (sceneObject.ControlBinding == null)
            {
                sceneObject.ControlBinding += this.RemoveControl;
                sceneObject.DestroyBinding += this.RemoveObject;
                sceneObject.ShowEffects += ShowEffectsBinding;
            }

            AddControlRecursive(sceneObject);

            SceneObjects.Add(sceneObject);
        }

        public void ShowEffectsBinding(List<ISceneObject> e)
        {
            e.ForEach(effect =>
            {
                if (effect.ShowEffects == null)
                {
                    effect.ShowEffects = ShowEffectsBinding;
                }
                if (effect.ControlBinding==null)
                {
                    effect.ControlBinding = this.AddControl;
                }

                effect.Destroy += () =>
                {
                    this.RemoveObject(effect);
                };
                this.AddObject(effect);
            });
        }

        public void AddControl(ISceneObjectControl sceneObjectControl)
        {
            sceneObjectControl.Destroy +=()=> { RemoveControl(sceneObjectControl); };
            SceneObjectsControllable.Add(sceneObjectControl);
        }

        public void RemoveControl(ISceneObjectControl sceneObjectControl)
        {
            SceneObjectsControllable.Remove(sceneObjectControl);
        }

        public void RemoveObject(ISceneObject sceneObject)
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
            
            if (Global.FreezeWorld==null && !Global.BlockSceneControls)
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
            
            if (Global.FreezeWorld== null && !Global.BlockSceneControls)
                KeyUp(key, modifier);

            var keyControls = ControlsByHandle(ControlEventType.Key, keyEventArgs.Key);
            for (int i = 0; i < keyControls.Count(); i++)
            {
                var keyControl = keyControls.ElementAtOrDefault(i);
                if (keyControl != null)
                {
                    keyControl.KeyUp(key, modifier);
                }
            }
        }

        public void OnMousePress(PointerArgs pointerPressedEventArgs, Point offset)
        {
            var keyControls = ControlsByHandle(ControlEventType.Click);
            var globalKeyHandlers = ControlsByHandle(ControlEventType.GlobalClick);
            
            var clickedElements = keyControls.Where(so => RegionContains(so, pointerPressedEventArgs, offset));
            clickedElements = WhereLayeredHandlers(clickedElements, pointerPressedEventArgs, offset);

            DoClicks(pointerPressedEventArgs, offset, clickedElements, (c, a) => c.Click(a));

            /// глобальный клик позже потому что он в никуда, да и отмекнять его придётся чаще
            if (globalKeyHandlers.Count() != 0)
            {
                DoClicks(pointerPressedEventArgs, offset, globalKeyHandlers, (c, a) => c.GlobalClick(a));
            }
        }

        public void OnMouseRelease(PointerArgs pointerPressedEventArgs, Point offset)
        {
            var keyControls = ControlsByHandle(ControlEventType.ClickRelease);
            var globalKeyHandlers = ControlsByHandle(ControlEventType.GlobalClickRelease);

            var clickedElements = keyControls.Where(so => RegionContains(so, pointerPressedEventArgs, offset));
            clickedElements = WhereLayeredHandlers(clickedElements, pointerPressedEventArgs, offset);
            DoClicks(pointerPressedEventArgs, offset, clickedElements, (c, a) => c.ClickRelease(a));

            /// глобальный клик позже потому что он в никуда, да и отмекнять его придётся чаще
            if (globalKeyHandlers.Count() != 0)
            {
                DoClicks(pointerPressedEventArgs, offset, globalKeyHandlers, (c, a) => c.GlobalClickRelease(a));
            }
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
                        args.ProcessedOffset = true;
                    }

                    Global.PointerLocation = args;

                    whichClick(clickedElement, args);
                }
            }
        }

        public void OnMouseMove(PointerArgs pointerPressedEventArgs, Point offset)
        {
            if (SceneManager.Current != this)
                return;

            var globalMouseMoves = ControlsByHandle(ControlEventType.GlobalMouseMove);
            DoClicks(pointerPressedEventArgs, offset, globalMouseMoves, (c, a) => c.GlobalMouseMove(a));

            OnMouseMoveOnFocus(pointerPressedEventArgs, offset);

            var moveControls = ControlsByHandle(ControlEventType.MouseMove)
                .Where(so => RegionContains(so, pointerPressedEventArgs, offset));

            var layered = WhereLayeredHandlers(moveControls, pointerPressedEventArgs, offset);

            DoClicks(pointerPressedEventArgs, offset, moveControls, (c, a) => c.MouseMove(a));
        }

        private void OnMouseMoveOnFocus(PointerArgs pointerPressedEventArgs, Point offset)
        {
            var controls = ControlsByHandle(ControlEventType.Focus);

            var newFocused = controls.Where(handler => RegionContains(handler, pointerPressedEventArgs, offset));

            newFocused = WhereLayeredHandlers(newFocused,pointerPressedEventArgs,offset);

            newFocused= newFocused
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
            Rectangle newRegion = ActualRegion(sceneObjControl, offset);
            return newRegion.Contains(pos.X, pos.Y);
        }

        private Rectangle ActualRegion(ISceneObjectControl sceneObjControl, Point offset)
        {
            var newRegion = new Rectangle
            {
                X = sceneObjControl.ComputedPosition.X * DrawingSize.CellF,
                Y = sceneObjControl.ComputedPosition.Y * DrawingSize.CellF,
                Height = sceneObjControl.Position.Height * DrawingSize.CellF,
                Width = sceneObjControl.Position.Width * DrawingSize.CellF
            };

            if (!this.AbsolutePositionScene && !sceneObjControl.AbsolutePosition)
            {
                newRegion.X += offset.X;
                newRegion.Y += offset.Y;
            }

            return newRegion;
        }

        protected virtual void KeyPress(Key keyPressed, KeyModifiers keyModifiers, bool hold) { }

        protected virtual void KeyUp(Key keyPressed, KeyModifiers keyModifiers) { }

        private IEnumerable<ISceneObjectControl> ControlsByHandle(ControlEventType handleEvent, Key key = Key.None)
        {
            if (Global.FreezeWorld!=null)
            {
                var chain = FreezedChain(SceneObjectsControllable.FirstOrDefault(x => x == Global.FreezeWorld));
                return WhereHandles(handleEvent, key, chain);
            }
            else
            {
                return WhereHandles(handleEvent, key, SceneObjectsControllable);
            }
        }

        private static IEnumerable<ISceneObjectControl> WhereHandles(ControlEventType handleEvent, Key key, IEnumerable<ISceneObjectControl> elements)
        {
            var handlers = elements
                .Distinct()
                .Where(c => c.Visible)
                .Where(c=> Global.DrawClient.InCamera(c))
                .Where(x =>
                {
                    bool handle = x.CanHandle.Contains(handleEvent);

                    if (handleEvent == ControlEventType.Key)
                    {
                        handle = x.AllKeysHandle || x.KeysHandle.Contains(key);
                    }

                    return handle;
                });

            return handlers;
        }

        private IEnumerable<ISceneObjectControl> WhereLayeredHandlers(IEnumerable<ISceneObjectControl> elements, PointerArgs pointerPressedEventArgs, Point offset)
        {
            List<ISceneObjectControl> selected = new List<ISceneObjectControl>();

            var layered = elements.GroupBy(x => x.ZIndex)
                .OrderByDescending(x => x.Key);

            //if(elements.Any(x=>x.ZIndex>0))
            //{
            //    Debugger.Break();
            //}

            IGrouping<int, ISceneObjectControl> upper = null;

            foreach (var layer in layered)
            {
                if (upper == null)
                {
                    upper = layer;
                    selected.AddRange(layer);
                    continue;
                }

                foreach (var item in layer)
                {
                    if (!upper.Any(up => ActualRegion(up,offset).IntersectsWith(ActualRegion(item,offset))))
                    {
                        selected.Add(item);
                    }
                }

                upper = layer;
            }

            return selected;
        }

        private IEnumerable<ISceneObjectControl> FreezedChain(ISceneObject freezing)
        {
            List<ISceneObjectControl> freezingChain = new List<ISceneObjectControl>();
            freezingChain.Add(freezing as ISceneObjectControl);

            var childControls = freezing.Children.Where(x => SceneObjectsControllable.Contains(x))
                .Cast<ISceneObjectControl>();

            freezingChain.AddRange(childControls);

            foreach (var child in childControls)
            {
                freezingChain.AddRange(FreezedChain(child));
            }

            return freezingChain;
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