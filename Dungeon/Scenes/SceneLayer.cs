using Dungeon.Control;
using Dungeon.Control.Gamepad;
using Dungeon.Control.Keys;
using Dungeon.Control.Pointer;
using Dungeon.Drawing.SceneObjects;
using Dungeon.ECS;
using Dungeon.Settings;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace Dungeon.Scenes
{
    [DebuggerDisplay("{Name}")]
    public class SceneLayer : ISceneLayer
    {
        protected Scene Owner;

        protected Scene Parent => Owner;

        public IScene Scene => Owner;

        public SceneLayer(Scene parentScene)
        {
            Owner = parentScene;
        }

        public string Name { get; set; }

        private readonly List<ISceneObject> SceneObjects = new List<ISceneObject>();

        public ISceneObject[] Objects => SceneObjects.ToArray();

        private readonly List<IEffect> GlobalEffects = new();

        private IEffect[] sceneGlobalEffects = new IEffect[0];

        public IEffect[] SceneGlobalEffects
        {
            get
            {
                if (sceneGlobalEffects.Length != GlobalEffects.Count)
                    return sceneGlobalEffects = GlobalEffects.ToArray();

                return sceneGlobalEffects;
            
            }
        }

        private List<ISceneControl> sceneObjectControls = new List<ISceneControl>();

        private List<ISceneControl> SceneObjectsControllable => new List<ISceneControl>(sceneObjectControls);

        private List<ISceneControl> sceneObjectsInFocuses = new List<ISceneControl>();

        private List<ISceneControl> SceneObjectsInFocus => new List<ISceneControl>(sceneObjectsInFocuses);

        public virtual double Width { get; set; }

        public virtual double Height { get; set; }

        public double Left { get; set; }

        public double Top { get; set; }

        public bool IsActive
        {
            get => Owner.ActiveLayer == this;
            set => Owner.ActiveLayer = this;
        }

        public TSceneObject AddObject<TSceneObject>(TSceneObject sceneObject)
            where TSceneObject : ISceneObject
        {
            sceneObject.HighLevelComponent = true;
            if (sceneObject.ControlBinding == null)
            {
                sceneObject.ControlBinding += this.AddControl;
                sceneObject.ControlBinding += this.RemoveControl;
                sceneObject.DestroyBinding += this.RemoveObject;
                sceneObject.ShowInScene += ShowEffectsBinding;
                sceneObject.Destroy += () => this.RemoveObject(sceneObject);
                sceneObject.ShowInScene += this.ShowEffectsBinding;
                sceneObject.Layer = this;
                sceneObject.Init();
            }
            this.SceneObjects.Add(sceneObject);

            AddControlRecursive(sceneObject);

            return sceneObject;
        }

        public void AddObjectCenter<TSceneObject>(TSceneObject sceneObject, bool horizontal = true, bool vertical = true)
            where TSceneObject : ISceneObject
        {
            if (sceneObject is ImageObject imageObject)
            {

                var measure = MeasureImage(imageObject.Image);

                if (horizontal)
                {
                    var left = Width / 2 - measure.X / 2;
                    imageObject.Left = left;
                }

                if (vertical)
                {
                    var top = Height / 2 - measure.Y / 2;
                    imageObject.Top = top;
                }
            }
            else
            {
                if (horizontal)
                {
                    var left = Width / 2d - sceneObject.Width / 2d;
                    sceneObject.Left = left;
                }

                if (vertical)
                {
                    var top = Height / 2d - sceneObject.Height / 2d;
                    sceneObject.Top = top;
                }
            }

            AddObject(sceneObject);
        }

        protected Point MeasureImage(string img)
        {
            var m = DungeonGlobal.DrawClient.MeasureImage(img);

            return new Point(m.X, m.Y);
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

        /// <summary>
        /// Только добавляет в коллекцию контролов, НЕ ДОБАВЛЯЕТ НА СЦЕНУ
        /// </summary>
        /// <param name="sceneObjectControl"></param>
        public void AddControl(ISceneControl sceneObjectControl)
        {
            if (!sceneObjectControls.Contains(sceneObjectControl))
            {
                sceneObjectControl.Layer = this;
                sceneObjectControl.Init();
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
            if (Owner.Destroyed)
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
                .Where(c => c.DrawOutOfSight || (c.HighLevelComponent && Owner.sceneManager.DrawClient.InCamera(c)))
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
            Rectangle actualRegion = ActualRegion(sceneObjControl, offset);
            var hitboxContains = actualRegion.Contains(pos.X, pos.Y);

            if (hitboxContains && sceneObjControl.PerPixelCollision)
            {
                var point = new Point(pos.X - actualRegion.X, pos.Y - actualRegion.Y);

                if (sceneObjControl.Texture == null)
                    return false;

                bool value = sceneObjControl.Texture.Contains(point, actualRegion.Size);
                return value;
            }

            return hitboxContains;
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
            var scaledSize = Scaled(newRegion.Width, newRegion.Height);
            newRegion.Height = scaledSize.Y;
            newRegion.Width = scaledSize.X;

            var scaledPos = Scaled(newRegion.X, newRegion.Y);
            newRegion.X = scaledPos.X;
            newRegion.Y = scaledPos.Y;

            if (!Owner.AbsolutePositionScene && !sceneObjControl.AbsolutePosition)
            {
                newRegion.X += offset.X;
                newRegion.Y += offset.Y;
            }

            var thisScaledPos = Scaled(this.Left, this.Top);
            newRegion.X += thisScaledPos.X;
            newRegion.Y += thisScaledPos.Y;

            return newRegion;
        }

        private Vector2 Scaled(double x, double y) => Vector2.Transform(new Vector2((float)x, (float)y), DungeonGlobal.ResolutionScaleMatrix);

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

        public virtual void OnText(string text)
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

        public virtual void OnKeyDown(KeyArgs keyEventArgs)
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

        public virtual void OnKeyUp(KeyArgs keyEventArgs)
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

        public virtual void OnMousePress(PointerArgs pointerPressedEventArgs, Point offset)
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

        public virtual void OnMouseRelease(PointerArgs pointerPressedEventArgs, Point offset)
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

        public virtual void OnMouseWheel(MouseWheelEnum wheelEnum)
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
            DungeonGlobal.PointerLocation = pointerPressedEventArgs;

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

                    if (!Owner.AbsolutePositionScene && !clickedElement.AbsolutePosition)
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

        public virtual void OnMouseMove(PointerArgs pointerPressedEventArgs, Point offset)
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
            var controls = ControlsByHandle(ControlEventType.Focus).ToArray();

            var nFocused = controls.Where(handler => RegionContains(handler, pointerPressedEventArgs, offset)).ToArray();

            var newFocused = WhereLayeredHandlers(nFocused, pointerPressedEventArgs, offset);

            var inFocus = SceneObjectsInFocus;

            newFocused = newFocused
                .Where(x => !inFocus.Contains(x));

            var newLostFocused = inFocus.Where(x => !RegionContains(x, pointerPressedEventArgs, offset));



            foreach (var item in newLostFocused)
            {
                try
                {
                    foreach (var system in Systems)
                    {
                        if (system.IsApplicable(item))
                        {
                            system.ProcessUnfocus(item);
                        }
                    }

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
                        foreach (var system in Systems)
                        {
                            if (system.IsApplicable(control))
                            {
                                system.ProcessFocus(control);
                            }
                        }
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

        private List<ISystem> Systems = new List<ISystem>();

        public virtual void OnStickMoveOnce(Direction direction, GamePadStick stick)
        {
            var controls = ControlsByHandle(ControlEventType.GamePadStickMoves);
            for (int i = 0; i < controls.Count(); i++)
            {
                var control = controls.ElementAtOrDefault(i);
                if (control != null)
                {
                    try
                    {
                        control.StickMoveOnce(direction, stick);
                    }
                    catch (Exception ex)
                    {
                        DungeonGlobal.Exception(ex);
                        return;
                    }
                }
            }
        }

        public virtual void OnStickMove(Direction direction, GamePadStick stick)
        {
            var controls = ControlsByHandle(ControlEventType.GamePadStickMoves);
            for (int i = 0; i < controls.Count(); i++)
            {
                var control = controls.ElementAtOrDefault(i);
                if (control != null)
                {
                    try
                    {
                        control.StickMove(direction, stick);
                    }
                    catch (Exception ex)
                    {
                        DungeonGlobal.Exception(ex);
                        return;
                    }
                }
            }
        }

        public virtual void OnGamePadButtonsPress(GamePadButton[] btns)
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

        public virtual void OnGamePadButtonsRelease(GamePadButton[] btns)
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

        public void AddSystem(ISystem system)
        {
            if (!Systems.Contains(system))
            {
                system.SceneLayer = this;
                Systems.Add(system);
            }
        }

        public void RemoveSystem(ISystem system)
        {
            Systems.Remove(system);
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