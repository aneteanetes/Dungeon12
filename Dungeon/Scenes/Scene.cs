namespace Dungeon.Scenes
{
    using Dungeon.Control;
    using Dungeon.Control.Keys;
    using Dungeon.Control.Pointer;
    using Dungeon.Resources;
    using Dungeon.Scenes.Manager;
    using Dungeon.Settings;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
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

        private List<ISceneObjectControl> sceneObjectControls = new List<ISceneObjectControl>();        
        private List<ISceneObjectControl> SceneObjectsControllable=> new List<ISceneObjectControl>(sceneObjectControls);
        
        private List<ISceneObjectControl> sceneObjectsInFocuses = new List<ISceneObjectControl>();
        private List<ISceneObjectControl> SceneObjectsInFocus=> new List<ISceneObjectControl>(sceneObjectsInFocuses);
        
        protected void AddObject(ISceneObject sceneObject)
        {
            if (sceneObject.ControlBinding == null)
            {
                sceneObject.ControlBinding += this.RemoveControl;
                sceneObject.DestroyBinding += this.RemoveObject;
                sceneObject.ShowInScene += ShowEffectsBinding;
            }

            AddControlRecursive(sceneObject);

            SceneObjects.Add(sceneObject);
        }

        public void ShowEffectsBinding(List<ISceneObject> e)
        {
            e.ForEach(effect =>
            {
                if (effect.ShowInScene == null)
                {
                    effect.ShowInScene = ShowEffectsBinding;
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
            if(!sceneObjectControls.Contains(sceneObjectControl))
            {
                sceneObjectControls.Add(sceneObjectControl);
                sceneObjectControl.Destroy += () => { RemoveControl(sceneObjectControl); };
            }
        }

        public void RemoveControl(ISceneObjectControl sceneObjectControl)
        {
            sceneObjectControls.Remove(sceneObjectControl);
        }

        public void RemoveObject(ISceneObject sceneObject)
        {
            if (sceneObject is ISceneObjectControl sceneObjectControl)
            {
                RemoveControl(sceneObjectControl);
            }

            SceneObjects.Remove(sceneObject);
        }

        private void AddControlRecursive(ISceneObject sceneObject)
        {
            if (sceneObject is ISceneObjectControl sceneObjectControl)
            {
                AddControl(sceneObjectControl);
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
            if (destroyed)
                return;

            var textControls = ControlsByHandle(ControlEventType.Text);

            for (int i = 0; i < textControls.Count(); i++)
            {
                var textControl = textControls.ElementAtOrDefault(i);
                if (textControl != null)
                {
                    try
                    {
                        textControl.TextInput(text);
                    }
                    catch (Exception ex)
                    {
                        Global.Exception(ex);
                        return;
                    }
                }
            }
        }

        public void OnKeyDown(KeyArgs keyEventArgs)
        {
            var key = keyEventArgs.Key;
            var modifier = keyEventArgs.Modifiers;
            
            if (Global.Freezer.World==null && !Global.BlockSceneControls)
                try
                {
                    KeyPress(key, modifier, keyEventArgs.Hold);
                }
                catch (Exception ex)
                {
                    Global.Exception(ex);
                    return;
                }

            var keyControls = ControlsByHandle(ControlEventType.Key, keyEventArgs.Key).ToArray();
            foreach (var sceneObjectHandler in keyControls)
            {
                try
                {
                    sceneObjectHandler.KeyDown(key, modifier, keyEventArgs.Hold);
                }
                catch (Exception ex)
                {
                    Global.Exception(ex);
                    return;
                }
            }
        }

        public void OnKeyUp(KeyArgs keyEventArgs)
        {
            if (destroyed)
                return;

            var key = keyEventArgs.Key;
            var modifier = keyEventArgs.Modifiers;
            
            if (Global.Freezer.World== null && !Global.BlockSceneControls)
                try
                {
                    KeyUp(key, modifier);
                }
                catch (Exception ex)
                {
                    Global.Exception(ex);
                    return;
                }

            var keyControls = ControlsByHandle(ControlEventType.Key, keyEventArgs.Key);
            for (int i = 0; i < keyControls.Count(); i++)
            {
                var keyControl = keyControls.ElementAtOrDefault(i);
                if (keyControl != null)
                {
                    try
                    {
                        keyControl.KeyUp(key, modifier);
                    }
                    catch (Exception ex)
                    {
                        Global.Exception(ex);
                        return;
                    }
                }
            }
        }

        public void OnMousePress(PointerArgs pointerPressedEventArgs, Point offset)
        {
            if (destroyed)
                return;

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
            if (destroyed)
                return;

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
            if (destroyed)
                return;

            var wheelControls = ControlsByHandle(ControlEventType.MouseWheel);

            for (int i = 0; i < wheelControls.Count(); i++)
            {
                var wheelControl = wheelControls.ElementAtOrDefault(i);
                if (wheelControl != null)
                {
                    try
                    {
                        wheelControl.MouseWheel(wheelEnum);
                    }
                    catch (Exception ex)
                    {
                        Global.Exception(ex);
                        return;
                    }
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

                    try
                    {
                        whichClick(clickedElement, args);
                    }
                    catch (Exception ex)
                    {
                        Global.Exception(ex);
                        return;
                    }
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

            var nFocused = controls.Where(handler => RegionContains(handler, pointerPressedEventArgs, offset));

            var newFocused = WhereLayeredHandlers(nFocused, pointerPressedEventArgs,offset);

            var inFocus = SceneObjectsInFocus;

            newFocused = newFocused
                .Where(x => !inFocus.Contains(x));

            var newLostFocused = inFocus.Where(x => !RegionContains(x, pointerPressedEventArgs, offset));

            foreach (var item in newLostFocused)
            {
                try
                {
                    item.Unfocus();
                }
                catch (Exception ex)
                {
                    Global.Exception(ex);
                    return;
                }
                sceneObjectsInFocuses.Remove(item);
                //SceneObjectsInFocus.Remove(item);
            }

            if (newFocused.Count() > 0)
            {
                foreach (var control in newFocused)
                {
                    try
                    {
                        control.Focus();
                    }
                    catch (Exception ex)
                    {
                        Global.Exception(ex);
                        return;
                    }
                }

                sceneObjectsInFocuses.AddRange(newFocused);
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
            if (destroyed)
                return Enumerable.Empty<ISceneObjectControl>();

            Global.Freezer.HandleFreezes.TryGetValue(handleEvent, out var freezer);
            if (Global.Freezer.World != null || freezer != null)
            {
                if (freezer == null)
                {
                    freezer = Global.Freezer.World;
                }
                var chain = FreezedChain(SceneObjectsControllable.FirstOrDefault(x => x == freezer));
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
                .Where(c=> c.DrawOutOfSight || Global.DrawClient.InCamera(c))
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
            if(freezing==null)
            {
                //такая ситуация может быть когда мы зафризили сцену, но компонент удалили
                return Enumerable.Empty<ISceneObjectControl>();
            }

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

        public List<Dungeon.Resources.Resource> Resources = new List<Resources.Resource>();

        public virtual void Activate()
        {
            this.sceneManager.DrawClient.SetScene(this);
        }

        #region protected utils
        
        protected virtual void Switch<T>(params string[] args) where T : GameScene
        {
            this.sceneManager.Change<T>(args);
        }

        protected void Switch(string sceneClassName)
        {

            
        }

        private bool destroyed = false;
        public void Destroy()
        {
            var sceneObjsForRemove = new List<ISceneObject>(SceneObjects);
            sceneObjsForRemove.ForEach(x => x.Destroy?.Invoke());
            sceneObjsForRemove.Clear();

            sceneObjsForRemove = new List<ISceneObject>(sceneObjectControls);
            sceneObjsForRemove.ForEach(x => x.Destroy?.Invoke());
            sceneObjsForRemove.Clear();

            if (!ResourceLoader.NotDisposingResources)
            {
                Resources.ForEach(r => r.Dispose?.Invoke());
                Resources.Clear();
                GC.Collect();
            }
            destroyed = true;
        }

        #endregion

    }
}