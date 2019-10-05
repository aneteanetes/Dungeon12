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

        public virtual string Cursor { get; set; } = null;

        protected virtual ControlEventType[] Handles { get; } = null;

        protected virtual Key[] KeyHandles { get; set; } = null;

        private Lazy<ControlEventType[]> ControlHandlers => new Lazy<ControlEventType[]>(() => Handles);

        private Lazy<Key[]> KeyHandlers => new Lazy<Key[]>(() => KeyHandles);

        public ControlEventType[] CanHandle => ControlHandlers.Value ?? allHandling;

        public Key[] KeysHandle => KeyHandlers.Value ?? new Key[0];

        public virtual bool AllKeysHandle => false;

        public virtual void Click(PointerArgs args) { }

        public virtual void GlobalClick(PointerArgs args) { }

        protected T AddControlCenter<T>(T control, bool horizontal = true, bool vertical = true)
            where T : SceneObject, ISceneObjectControl
        {
            var measure = MeasureImage(control.Image);
            measure.X = measure.X * 32;
            measure.Y = measure.Y * 32;

            var width = this.Width * 32;
            var height = this.Height * 32;

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
            this.AddChild(control);

            return control;
        }

        public virtual void Focus()
        {
            if (this.Cursor != null)
            {
                Global.DrawClient.SetCursor(("Cursors." + this.Cursor + ".png").PathImage());
            }
        }

        public virtual void Unfocus()
        {
            if (this.Cursor != null)
            {
                Global.DrawClient.SetCursor("Cursors.common.png".PathImage());
            }
        }

        public virtual void KeyDown(Key key, KeyModifiers modifier, bool hold) { }

        public virtual void KeyUp(Key key, KeyModifiers modifier) { }

        public virtual void ClickRelease(PointerArgs args) { }

        public virtual void GlobalClickRelease(PointerArgs args) { }

        public virtual void MouseWheel(MouseWheelEnum mouseWheelEnum) { }

        public virtual void MouseMove(PointerArgs args) { }

        public virtual void GlobalMouseMove(PointerArgs args) { }

        public virtual void TextInput(string text) { }

        protected virtual void AddChild(ISceneObjectControl sceneObject)
        {
            ControlBinding?.Invoke(sceneObject);
            base.AddChild(sceneObject);
        }
    }
}