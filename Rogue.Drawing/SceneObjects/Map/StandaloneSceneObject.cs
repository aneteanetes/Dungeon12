namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Control.Pointer;
    using Rogue.Entites.Animations;
    using Rogue.Map;
    using Rogue.Types;
    using System;

    public class StandaloneSceneObject : AnimatedSceneObject<MapObject>
    {
        public StandaloneSceneObject(PlayerSceneObject playerSceneObject, string img, AnimationMap animationMap,string tooltip, Func<int, AnimationMap, bool> requestNextFrame = null, Rectangle defaultFramePosition = null)
            : base(playerSceneObject,null,tooltip, defaultFramePosition ?? new Types.Rectangle
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
