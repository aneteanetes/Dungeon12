﻿namespace Dungeon.SceneObjects
{
    using Dungeon.Control;
    using Dungeon.Control.Events;
    using Dungeon.Control.Keys;
    using Dungeon.Control.Pointer;
    using Dungeon.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class HandleSceneControl : SceneObject, ISceneObjectControl
    {
        public HandleSceneControl()
        {
            //dynamic binding
            new string[] {
                nameof(KeyDown),
                nameof(KeyUp),
                nameof(Focus),
                nameof(Unfocus),
                nameof(Click),
                nameof(ClickRelease),
                nameof(MouseMove),
                nameof(GlobalMouseMove),
                nameof(MouseWheel),
                nameof(TextInput),
                nameof(GlobalClick),
                nameof(GlobalClickRelease),
            }.ForEach(s => dynamicEvents.Add(s, null));
        }

        public void AddDynamicEvent(string eventName, Delegate method)
        {
            if (dynamicEvents.ContainsKey(eventName))
            {
                dynamicEvents[eventName] = Delegate.Combine(dynamicEvents[eventName], method);
            }
        }

        public void RemoveDynamicEvent(string eventName, Delegate method)
        {
            if (dynamicEvents.ContainsKey(eventName))
            {
                dynamicEvents[eventName] = Delegate.Remove(dynamicEvents[eventName], method);
            }
        }

        private Dictionary<string, Delegate> dynamicEvents = new Dictionary<string, Delegate>();

        private readonly ControlEventType[] allHandling = new ControlEventType[]
        {
             ControlEventType.Click,
             ControlEventType.Focus,
             ControlEventType.Key
        };

        public virtual string Cursor { get; set; } = null;

        protected virtual ControlEventType[] Handles { get; } = null;

        protected virtual Key[] KeyHandles { get; set; } = null;

        private Lazy<ControlEventType[]> ControlHandlers => new Lazy<ControlEventType[]>(() => Handles);

        private HashSet<ControlEventType> dynamicHandles = new HashSet<ControlEventType>();

        public void AddHandle(ControlEventType controlEventType)
        {
            dynamicHandles.Add(controlEventType);
        }

        public void RemoveHandle(ControlEventType controlEventType)
        {
            dynamicHandles.Remove(controlEventType);
        }

        private Lazy<Key[]> KeyHandlers => new Lazy<Key[]>(() => KeyHandles);

        public ControlEventType[] CanHandle => (ControlHandlers.Value ?? allHandling).Concat(dynamicHandles).ToArray();

        public Key[] KeysHandle => KeyHandlers.Value ?? new Key[0];

        public virtual bool AllKeysHandle => false;

        public virtual void Click(PointerArgs args) => dynamicEvents[nameof(Click)]?.DynamicInvoke(args);

        public virtual void GlobalClick(PointerArgs args) => dynamicEvents[nameof(GlobalClick)]?.DynamicInvoke(args);

        protected T AddControlCenter<T>(T control, bool horizontal = true, bool vertical = true)
            where T : SceneObject, ISceneObjectControl
        {
            var measure = MeasureImage(control.Image);
            measure.X = measure.X * 32;
            measure.Y = measure.Y * 32;

            var width = Width * 32;
            var height = Height * 32;

            if (horizontal)
            {
                var left = width / 2 - measure.X / 2;
                control.Left = left / 32;
            }

            if (vertical)
            {
                var top = height / 2 - measure.Y / 2;
                control.Top = top / 32;
            }
            AddChild(control);

            return control;
        }

        protected bool InFocus { get; private set; }

        public virtual void Focus()
        {
            InFocus = true;
            if (Cursor != null)
            {
                Global.DrawClient.SetCursor(("Cursors." + Cursor + ".png").PathImage());
            }
            dynamicEvents[nameof(Focus)]?.DynamicInvoke();
        }

        public virtual void Unfocus()
        {
            InFocus = false;
            if (Cursor != null)
            {
                Global.DrawClient.SetCursor("Cursors.common.png".PathImage());
            }
            dynamicEvents[nameof(Unfocus)]?.DynamicInvoke();
        }

        public virtual void KeyDown(Key key, KeyModifiers modifier, bool hold) => dynamicEvents[nameof(KeyDown)]?.DynamicInvoke(key,modifier,hold);

        public virtual void KeyUp(Key key, KeyModifiers modifier) => dynamicEvents[nameof(KeyUp)]?.DynamicInvoke(key, modifier);

        public virtual void ClickRelease(PointerArgs args) => dynamicEvents[nameof(ClickRelease)]?.DynamicInvoke(args);

        public virtual void GlobalClickRelease(PointerArgs args) => dynamicEvents[nameof(GlobalClickRelease)]?.DynamicInvoke(args);
        
        public virtual void MouseWheel(MouseWheelEnum mouseWheelEnum) => dynamicEvents[nameof(MouseWheel)]?.DynamicInvoke(mouseWheelEnum);

        public virtual void MouseMove(PointerArgs args) => dynamicEvents[nameof(MouseMove)]?.DynamicInvoke(args);

        public virtual void GlobalMouseMove(PointerArgs args) => dynamicEvents[nameof(GlobalMouseMove)]?.DynamicInvoke(args);

        public virtual void TextInput(string text) => dynamicEvents[nameof(TextInput)]?.DynamicInvoke(text);

        protected virtual void AddChild(ISceneObjectControl sceneObject)
        {
            ControlBinding?.Invoke(sceneObject);
            base.AddChild(sceneObject);
        }

        /// <summary>
        /// Добавляет компонент как миксин
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mixin"></param>
        public override void AddMixin<T>(T mixin)
        {
            mixin.InitAsMixin(this);
            Mixins.Add(mixin);
            if (mixin is ISceneObjectControl mixinControl)
            {
                this.AddChild(mixinControl);
            }
        }
    }
}