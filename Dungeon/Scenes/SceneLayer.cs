using Dungeon.Control;
using Dungeon.Control.Gamepad;
using Dungeon.Control.Keys;
using Dungeon.Control.Pointer;
using Dungeon.Settings;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon.Scenes
{
    public class SceneLayer : ISceneLayer
    {
        private Scene owner;

        public SceneLayer(Scene parentScene)
        {
            owner = parentScene;
        }

        public string Name { get; set; }

        private readonly List<ISceneObject> SceneObjects = new List<ISceneObject>();

        public ISceneObject[] Objects => SceneObjects.ToArray();

        private readonly List<IEffect> GlobalEffects = new List<IEffect>();

        public IEffect[] SceneGlobalEffects => GlobalEffects.ToArray();

        private List<ISceneControl> sceneObjectControls = new List<ISceneControl>();

        private List<ISceneControl> SceneObjectsControllable => new List<ISceneControl>(sceneObjectControls);

        private List<ISceneControl> sceneObjectsInFocuses = new List<ISceneControl>();

        private List<ISceneControl> SceneObjectsInFocus => new List<ISceneControl>(sceneObjectsInFocuses);

        public double Width { get; set; }

        public double Height { get; set; }

        public double Left { get; set; }

        public double Top { get; set; }

        public bool IsActive
        {
            get => owner.ActiveLayer == this;
            set => owner.ActiveLayer = this;
        }

        public void AddObject(ISceneObject sceneObject)
        {
            if (sceneObject.ControlBinding == null)
            {
                sceneObject.ControlBinding += this.AddControl;
                sceneObject.ControlBinding += this.RemoveControl;
                sceneObject.DestroyBinding += this.RemoveObject;
                sceneObject.ShowInScene += ShowEffectsBinding;
                sceneObject.Destroy += () => this.RemoveObject(sceneObject);
                sceneObject.ShowInScene += this.ShowEffectsBinding;
                sceneObject.Layer = this;
            }
            this.SceneObjects.Add(sceneObject);

            AddControlRecursive(sceneObject);
        }

        public void ShowEffectsBinding(List<ISceneObject> e)
        {
            e.ForEach(effect =>
            {
                if (effect.ShowInScene == null)
                {
                    effect.ShowInScene = ShowEffectsBinding;
                }
                if (effect.ControlBinding == null)
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

        public void AddGlobalEffect(IEffect effect)
        {
            GlobalEffects.Add(effect);
        }

        public void RemoveGlobalEffect(IEffect effect)
        {
            GlobalEffects.Add(effect);
        }

        public void AddControl(ISceneControl sceneObjectControl)
        {
            if (!sceneObjectControls.Contains(sceneObjectControl))
            {
                sceneObjectControl.Layer = this;
                sceneObjectControls.Add(sceneObjectControl);
                sceneObjectControl.Destroy += () => { RemoveControl(sceneObjectControl); };
            }
        }

        public void RemoveControl(ISceneControl sceneObjectControl)
        {
            sceneObjectControls.Remove(sceneObjectControl);
        }

        public void RemoveObject(ISceneObject sceneObject)
        {
            if (sceneObject is ISceneControl sceneObjectControl)
            {
                RemoveControl(sceneObjectControl);
            }

            SceneObjects.Remove(sceneObject);
        }

        private void AddControlRecursive(ISceneObject sceneObject)
        {
            if (sceneObject is ISceneControl sceneObjectControl)
            {
                AddControl(sceneObjectControl);
            }

            foreach (var childSceneObject in sceneObject.Children)
            {
                AddControlRecursive(childSceneObject);
            }
        }

        private IEnumerable<ISceneControl> ControlsByHandle(ControlEventType handleEvent, Key key = Key.None)
        {
            if (owner.Destroyed)
                return Enumerable.Empty<ISceneControl>();

            DungeonGlobal.Freezer.HandleFreezes.TryGetValue(handleEvent, out var freezer);
            if (DungeonGlobal.Freezer.World != null || freezer != null)
            {
                if (freezer == null)
                {
                    freezer = DungeonGlobal.Freezer.World;
                }
                var chain = FreezedChain(SceneObjectsControllable.FirstOrDefault(x => x == freezer));
                return WhereHandles(handleEvent, key, chain);
            }
            else
            {
                return WhereHandles(handleEvent, key, SceneObjectsControllable);
            }
        }

        private IEnumerable<ISceneControl> WhereHandles(ControlEventType handleEvent, Key key, IEnumerable<ISceneControl> elements)
        {
            var handlers = elements
                .Distinct()
                .Where(c => c.Visible)
                .Where(c => c.DrawOutOfSight || owner.sceneManager.DrawClient.InCamera(c))
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

        private IEnumerable<ISceneControl> WhereLayeredHandlers(IEnumerable<ISceneControl> elements, PointerArgs pointerPressedEventArgs, Point offset)
        {
            List<ISceneControl> selected = new List<ISceneControl>();

            var layered = elements.GroupBy(x => x.ZIndex)
                .OrderByDescending(x => x.Key);

            //if(elements.Any(x=>x.ZIndex>0))
            //{
            //    Debugger.Break();
            //}

            IGrouping<int, ISceneControl> upper = null;

            foreach (var layer in layered)
            {
                if (upper == null)
                {
                    upper = layer;
                    foreach (var item in layer)
                    {
                        selected.Add(item);
                    }
                    continue;
                }

                foreach (var item in layer)
                {
                    if (!upper.Any(up => ActualRegion(up, offset).IntersectsWithOrContains(ActualRegion(item, offset))))
                    {
                        selected.Add(item);
                    }
                }

                upper = layer;
            }

            return selected;
        }

        private bool RegionContains(ISceneControl sceneObjControl, PointerArgs pos, Point offset)
        {
            Rectangle newRegion = ActualRegion(sceneObjControl, offset);
            return newRegion.Contains(pos.X, pos.Y);
        }

        private Rectangle ActualRegion(ISceneControl sceneObjControl, Point offset)
        {
            var newRegion = new Rectangle
            {
                X = sceneObjControl.ComputedPosition.X * DrawingSize.CellF,
                Y = sceneObjControl.ComputedPosition.Y * DrawingSize.CellF,
                Height = sceneObjControl.BoundPosition.Height * DrawingSize.CellF,
                Width = sceneObjControl.BoundPosition.Width * DrawingSize.CellF
            };

            if (!owner.AbsolutePositionScene && !sceneObjControl.AbsolutePosition)
            {
                newRegion.X += offset.X;
                newRegion.Y += offset.Y;
            }

            newRegion.X += this.Left;
            newRegion.Y += this.Top;

            return newRegion;
        }

        private IEnumerable<ISceneControl> FreezedChain(ISceneObject freezing)
        {
            if (freezing == null)
            {
                //такая ситуация может быть когда мы зафризили сцену, но компонент удалили
                return Enumerable.Empty<ISceneControl>();
            }

            List<ISceneControl> freezingChain = new List<ISceneControl>();
            freezingChain.Add(freezing as ISceneControl);

            var childControls = freezing.Children.Where(x => SceneObjectsControllable.Contains(x))
                .Cast<ISceneControl>();

            freezingChain.AddRange(childControls);

            foreach (var child in childControls)
            {
                freezingChain.AddRange(FreezedChain(child));
            }

            return freezingChain;
        }

        public void OnText(string text)
        {
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
                        DungeonGlobal.Exception(ex);
                        return;
                    }
                }
            }
        }

        public void OnKeyDown(KeyArgs keyEventArgs)
        {
            var key = keyEventArgs.Key;
            var modifier = keyEventArgs.Modifiers;           

            var keyControls = ControlsByHandle(ControlEventType.Key, keyEventArgs.Key).ToArray();
            foreach (var sceneObjectHandler in keyControls)
            {
                try
                {
                    sceneObjectHandler.KeyDown(key, modifier, keyEventArgs.Hold);
                }
                catch (Exception ex)
                {
                    DungeonGlobal.Exception(ex);
                    return;
                }
            }
        }

        public void OnKeyUp(KeyArgs keyEventArgs)
        {           
            var key = keyEventArgs.Key;
            var modifier = keyEventArgs.Modifiers;

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
                        DungeonGlobal.Exception(ex);
                        return;
                    }
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
                    try
                    {
                        wheelControl.MouseWheel(wheelEnum);
                    }
                    catch (Exception ex)
                    {
                        DungeonGlobal.Exception(ex);
                        return;
                    }
                }
            }
        }

        private void DoClicks(PointerArgs pointerPressedEventArgs, Point offset, IEnumerable<ISceneControl> clickedElements,
            Action<ISceneControl, PointerArgs> whichClick)
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
                        Offset = offset
                    };

                    if (!owner.AbsolutePositionScene && !clickedElement.AbsolutePosition)
                    {
                        args.X += offset.X;
                        args.Y += offset.Y;
                        args.ProcessedOffset = true;
                    }

                    DungeonGlobal.PointerLocation = args;

                    try
                    {
                        whichClick(clickedElement, args);
                    }
                    catch (Exception ex)
                    {
                        DungeonGlobal.Exception(ex);
                        return;
                    }
                }
            }
        }

        public void OnMouseMove(PointerArgs pointerPressedEventArgs, Point offset)
        {
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

            var newFocused = WhereLayeredHandlers(nFocused, pointerPressedEventArgs, offset);

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
                    DungeonGlobal.Exception(ex);
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
                        DungeonGlobal.Exception(ex);
                        return;
                    }
                }

                sceneObjectsInFocuses.AddRange(newFocused);
            }
        }

        public void OnLeftStickMoveOnce(Direction direction, Distance distance)
        {
            var controls = ControlsByHandle(ControlEventType.LeftStickMove);
            for (int i = 0; i < controls.Count(); i++)
            {
                var control = controls.ElementAtOrDefault(i);
                if (control != null)
                {
                    try
                    {
                        control.LeftStickMoveOnce(direction,distance);
                    }
                    catch (Exception ex)
                    {
                        DungeonGlobal.Exception(ex);
                        return;
                    }
                }
            }
        }

        public void OnLeftStickMove(Direction direction, Distance distance)
        {
            var controls = ControlsByHandle(ControlEventType.LeftStickMove);
            for (int i = 0; i < controls.Count(); i++)
            {
                var control = controls.ElementAtOrDefault(i);
                if (control != null)
                {
                    try
                    {
                        control.LeftStickMove(direction, distance);
                    }
                    catch (Exception ex)
                    {
                        DungeonGlobal.Exception(ex);
                        return;
                    }
                }
            }
        }

        public void OnGamePadButtonsPress(GamePadButton[] btns)
        {
            var controls = ControlsByHandle(ControlEventType.GamePadButtonsPress);
            for (int i = 0; i < controls.Count(); i++)
            {
                var control = controls.ElementAtOrDefault(i);
                if (control != null)
                {
                    try
                    {
                        control.GamePadButtonsPress(btns);
                    }
                    catch (Exception ex)
                    {
                        DungeonGlobal.Exception(ex);
                        return;
                    }
                }
            }
        }

        public void OnGamePadButtonsRelease(GamePadButton[] btns)
        {
            var controls = ControlsByHandle(ControlEventType.GamePadButtonsRelease);
            for (int i = 0; i < controls.Count(); i++)
            {
                var control = controls.ElementAtOrDefault(i);
                if (control != null)
                {
                    try
                    {
                        control.GamePadButtonsRelease(btns);
                    }
                    catch (Exception ex)
                    {
                        DungeonGlobal.Exception(ex);
                        return;
                    }
                }
            }
        }

        public bool Destroyed { get; set; }

        public bool AbsoluteLayer { get; set; }

        public void Destroy()
        {
            var sceneObjsForRemove = new List<ISceneObject>(SceneObjects);
            sceneObjsForRemove.ForEach(x => x.Destroy?.Invoke());
            sceneObjsForRemove.Clear();

            sceneObjsForRemove = new List<ISceneObject>(sceneObjectControls);
            sceneObjsForRemove.ForEach(x => x.Destroy?.Invoke());
            sceneObjsForRemove.Clear();
        }

        //private static bool IntersectsPixel(Rectangle hitbox1, Texture2D texture1, Rectangle hitbox2, Texture2D texture2)
        //{
        //    Color[] colorData1 = new Color[texture1.Width * texture1.Height];
        //    texture1.GetData(colorData1);
        //    Color[] colorData2 = new Color[texture2.Width * texture2.Height];
        //    texture2.GetData(colorData2);

        //    int top = Math.Max(hitbox1.Top, hitbox2.Top);
        //    int bottom = Math.Min(hitbox1.Bottom, hitbox2.Bottom);
        //    int right = Math.Max(hitbox1.Right, hitbox2.Right);
        //    int left = Math.Min(hitbox1.Left, hitbox2.Left);

        //    for (y = top; y < bottom; y++)
        //    {
        //        for (x = left; x < right; x++)
        //        {
        //            Color color1 = colorData1[(x - hitbox1.Left) + (y - hitbox1.Top) * hitbox1.Width]
        //            Color color2 = colorData2[(x - hitbox2.Left) + (y - hitbox2.Top) * hitbox2.Width]

        //    if (color1.A != 0 && color2.A != 0)
        //                return true;
        //        }
        //    }
        //    return false;
        //}
    }
}