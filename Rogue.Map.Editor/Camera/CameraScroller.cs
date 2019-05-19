namespace Rogue.Map.Editor.Camera
{
    using Rogue.Control.Events;
    using Rogue.Control.Keys;
    using Rogue.Control.Pointer;
    using Rogue.Drawing.Impl;
    using Rogue.Drawing.SceneObjects.Base;
    using Rogue.Settings;

    public class CameraScroller : DarkRectangle
    {
        private Types.Direction direction;
        private Key key;

        public override bool AbsolutePosition => true;

        public CameraScroller(Types.Direction direction, string text, Key key)
        {
            this.key = key;
            this.direction = direction;
            this.Height = 1;
            this.Width = 1;
            this.AddTextCenter(new DrawText(text));
        }

        protected override ControlEventType[] Handles => new ControlEventType[]
        {
            ControlEventType.Click,
            ControlEventType.ClickRelease,
             ControlEventType.GlobalClickRelease,
             ControlEventType.Key
        };

        protected override Key[] KeyHandles => new Key[] { key };

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold) => Click(null);

        public override void KeyUp(Key key, KeyModifiers modifier) => ClickRelease(null);

        public override void Click(PointerArgs args)
        {
            Global.DrawClient.MoveCamera(this.direction);
        }

        public override void ClickRelease(PointerArgs args)
        {
            Global.DrawClient.MoveCamera(this.direction, true);
        }

        public override void GlobalClickRelease(PointerArgs args) => ClickRelease(args);
    }
}
