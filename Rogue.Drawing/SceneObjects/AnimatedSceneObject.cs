namespace Rogue.Drawing.SceneObjects
{
    using Rogue.Entites.Animations;
    using Rogue.Types;

    public abstract class AnimatedSceneObject : HandleSceneControl
    {
        public AnimatedSceneObject(Rectangle defaultFramePosition)
        {
            this.FramePosition = defaultFramePosition;
        }

        private int FrameCounter = 0;

        public override Rectangle ImageRegion
        {
            get
            {
                FrameCounter++;
                System.Console.WriteLine(FrameCounter);

                DrawLoop();
                ChangeAnimationFrame();

                return FramePosition;
            }
        }

        private  int frame = 0;
        
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

        protected virtual void ChangeAnimationFrame()
        {
            if (animationMap != null && !animationStop)
            {
                FramePosition.Pos = animationMap.Frames[frame];

                if (FrameCounter % (60 / animationMap.Frames.Count) == 0)
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