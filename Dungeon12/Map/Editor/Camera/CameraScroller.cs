namespace Dungeon12.Map.Editor.Camera
{
    using Dungeon;
    using Dungeon.Control;
    using Dungeon.Control.Events;
    using Dungeon.Control.Keys;
    using Dungeon.Drawing;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.Settings;

    public class CameraScroller : DarkRectangle
    {
        private Dungeon.Types.Direction direction;
        private Key key;

        public override bool AbsolutePosition => true;

        public CameraScroller(Dungeon.Types.Direction direction, string text, Key key)
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
