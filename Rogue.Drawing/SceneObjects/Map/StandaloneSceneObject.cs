namespace Rogue.Drawing.SceneObjects.Map
{
    using Rogue.Control.Pointer;
    using Rogue.Entites.Animations;
    using Rogue.Map;
    using Rogue.Types;
    using System;

    public class StandaloneSceneObject : AnimatedSceneObject
    {
        public StandaloneSceneObject(string img, AnimationMap animationMap,string tooltip, Func<int, AnimationMap, bool> requestNextFrame = null, Rectangle defaultFramePosition = null)
            : base(tooltip, defaultFramePosition ?? new Types.Rectangle
            {
                X = 0,
                Y = 0,
                Height = 32,
                Width = 32,
            }, null, requestNextFrame)
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
    }
}
