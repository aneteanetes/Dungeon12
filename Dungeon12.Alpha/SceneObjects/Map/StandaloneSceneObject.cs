namespace Dungeon12.Drawing.SceneObjects.Map
{
    using Dungeon.Control;
    using Dungeon.Control.Pointer;
    using Dungeon.Types;
    using Dungeon.View;
    using Dungeon12.Map;
    using System;

    public class StandaloneSceneObject : AnimatedSceneObject<MapObject>
    {
        public StandaloneSceneObject(PlayerSceneObject playerSceneObject, string img, Animation animationMap,string tooltip, Func<int, Animation, bool> requestNextFrame = null, Rectangle defaultFramePosition = null)
            : base(null,tooltip, defaultFramePosition ?? new Dungeon.Types.Rectangle
            {
                X = 0,
                Y = 0,
                Height = 32,
                Width = 32,
            }, requestNextFrame)
        {
            this.Image = img;
            this.SetAnimation(animationMap);
        }

        protected override void DrawLoop()
        {
        }

        public override void Click(PointerArgs args)
        {
        }

        protected override void Action(MouseButton mouseButton)
        {
        }

        protected override void StopAction()
        {
        }
    }
}
