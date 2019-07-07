namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Control.Events;
    using Rogue.Control.Keys;
    using Rogue.Control.Pointer;

    public abstract class ClickActionSceneObject<T> : TooltipedSceneObject
        where T : Physics.PhysicalObject
    {
        protected readonly PlayerSceneObject playerSceneObject;
        protected readonly T @object;

        public ClickActionSceneObject(PlayerSceneObject playerSceneObject, T @object, string tooltip) : base(tooltip, null)
        {
            this.@object = @object;

            if (this.GetType() != typeof(PlayerSceneObject))
            {
                this.playerSceneObject = playerSceneObject;

                this.playerSceneObject.OnMove += CheckStopAction;
            }
        }

        protected virtual int GrowSize { get; set; } = 3;

        protected override ControlEventType[] Handles => new ControlEventType[]
        {
             ControlEventType.Focus,
             ControlEventType.Click,
            ControlEventType.Key
        };

        private bool acting = false;

        public override void Click(PointerArgs args)
        {
            if (CheckActionAvailable(args.MouseButton))
            {
                acting = true;
                Action(args.MouseButton);
            }
        }

        private void CheckStopAction()
        {
            if (!acting)
                return;

            if (!CheckActionAvailable(MouseButton.None))
            {
                StopAction();
                acting = false;
            }
        }

        protected virtual bool CheckActionAvailable(MouseButton mouseButton)
        {
            var range = @object.Grow(3);
            return playerSceneObject.Avatar.IntersectsWith(range);
        }

        protected abstract void Action(MouseButton mouseButton);

        protected abstract void StopAction();

        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key == Key.LeftAlt && !hold)
            {
                this.ShowTooltip();
            }
        }

        public override void KeyUp(Key key, KeyModifiers modifier)
        {
            if (key == Key.LeftAlt)
            {
                this.HideTooltip();
            }
        }
    }
}