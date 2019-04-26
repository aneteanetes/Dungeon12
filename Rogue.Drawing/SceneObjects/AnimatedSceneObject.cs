namespace Rogue.Drawing.SceneObjects
{
    using Rogue.Entites.Animations;
    using Rogue.Types;
    using System;

    public abstract class AnimatedSceneObject : HandleSceneControl
    {
        public override bool CacheAvailable => false;

        public AnimatedSceneObject(Rectangle defaultFramePosition, Func<int, AnimationMap, bool> requestNextFrame = null)
        {
            this.FramePosition = defaultFramePosition;
            this.RequestNextFrame = requestNextFrame ?? this.DefaultRequestNextFrame;
        }

        private int FrameCounter = 0;

        public override Rectangle ImageRegion
        {
            get
            {
                FrameCounter++;

                DrawLoop();
                ChangeAnimationFrame();

                return FramePosition;
            }
        }

        private int frame = 0;

        private bool animationStop = false;

        protected bool RequestStop()
        {
            FrameCounter = 0;
            return animationStop = true;
        }

        protected bool RequestResume() => animationStop = false;

        private AnimationMap animationMap;

        protected void SetAnimation(AnimationMap animationMap) => this.animationMap = animationMap;

        protected Rectangle FramePosition;

        protected Func<int, AnimationMap, bool> RequestNextFrame;

        private bool DefaultRequestNextFrame(int frameCounter, AnimationMap animMap)
        {
            return frameCounter % (60 / animMap.Frames.Count) == 0;
        }

        protected virtual void ChangeAnimationFrame()
        {
            if (animationMap != null && !animationStop)
            {
                FramePosition.Pos = animationMap.Frames[frame];

                if (RequestNextFrame(FrameCounter, animationMap))
                {
                    frame++;
                }

                if (frame == animationMap.Frames.Count)
                {
                    frame = 0;
                    FrameCounter = 0;
                }
            }
        }

        protected abstract void DrawLoop();
    }
}