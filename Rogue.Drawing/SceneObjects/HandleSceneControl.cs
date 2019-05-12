namespace Rogue.Drawing.SceneObjects
{
    using Rogue.Control.Events;
    using Rogue.Control.Keys;
    using Rogue.Control.Pointer;
    using Rogue.View.Interfaces;
    using System;

    public abstract class HandleSceneControl : SceneObject, ISceneObjectControl
    {
        private readonly ControlEventType[] allHandling = new ControlEventType[]
        {
             ControlEventType.Click,
             ControlEventType.Focus,
             ControlEventType.Key             
        };

        protected virtual ControlEventType[] Handles { get; } = null;

        protected virtual Key[] KeyHandles { get; } = null;

        private Lazy<ControlEventType[]> ControlHandlers => new Lazy<ControlEventType[]>(() => Handles);

        private Lazy<Key[]> KeyHandlers => new Lazy<Key[]>(() => KeyHandles);

        public ControlEventType[] CanHandle => ControlHandlers.Value ?? allHandling;

        public Key[] KeysHandle => KeyHandlers.Value ?? new Key[0];

        public virtual void Click(PointerArgs args) { }

        public virtual void GlobalClick(PointerArgs args) { }

        public virtual void Focus() { }

        public virtual void Unfocus() { }

        public virtual void KeyDown(Key key, KeyModifiers modifier) { }

        public virtual void KeyUp(Key key, KeyModifiers modifier) { }
    }
}