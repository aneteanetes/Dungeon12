namespace Dungeon12.Drawing.SceneObjects
{
    using Dungeon.Types;
    using Dungeon.View;
    using Dungeon.View.Interfaces;
    using Dungeon12.Drawing.SceneObjects.Map;
    using System;

    public abstract class AnimatedSceneObject<T> : ClickActionSceneObject<T>
        where T : class, IGameComponent
    {
        public override bool DrawOutOfSight => false;

        public override bool CacheAvailable => false;

        public AnimatedSceneObject( T @object, string tooltip, Rectangle defaultFramePosition, Func<int, Animation, bool> requestNextFrame = null, bool bindView = true) : base( @object, tooltip, bindView)
        {
            this.FramePosition = defaultFramePosition;
            this.RequestNextFrame = requestNextFrame ?? this.DefaultRequestNextFrame;
        }

        private int FrameCounter = 0;

        public bool FreezeForceAnimation { get; set; }

        public override Rectangle ImageRegion
        {
            get
            {
                if (Global.Freezer.World != null && !FreezeForceAnimation)
                    return FramePosition;

                if (Loop || !AnimationPlayed)
                {
                    FrameCounter++;
                }

                if (!drawStop)
                    DrawLoop();
                ChangeAnimationFrame();

                return FramePosition;
            }
        }

        private bool AnimationPlayed = false;

        public override string Image
        {
            get
            {
                if (animationMap?.TilesetAnimation ?? true)
                {
                    return base.Image;
                }

                return this.animationMap.FullFrames[frame];
            }
            set
            {
                if (animationMap?.TilesetAnimation ?? false)
                    return;

                base.Image = value;
            }
        }

        /// <summary>
        /// Устанавливает базовое изображение игнорируя специальный сеттер, что бы сменить источник анимации
        /// </summary>
        /// <param name="img"></param>
        public void SetImageForce(string img) => base.Image = img;

        private int frame = 0;

        private bool animationStop = false;

        protected bool RequestStop()
        {
            FrameCounter = 0;
            this.OnAnimationStop();
            return animationStop = true;
        }

        private bool drawStop = false;
        protected bool RequestDrawStop() => drawStop = true;

        protected virtual void OnAnimationStop() { }

        protected bool RequestResume() => animationStop = false;

        private Animation animationMap;

        protected void SetAnimation(Animation animationMap) => this.animationMap = animationMap;

        protected Rectangle FramePosition;

        protected Func<int, Animation, bool> RequestNextFrame;

        public double FramesPerSecond => this.animationMap.FramesPerSecond == default
                ? this.animationMap.Frames.Count
                : this.animationMap.FramesPerSecond;

        private bool DefaultRequestNextFrame(int frameCounter, Animation animMap)
        {
            var framesPerSec = animMap.FramesPerSecond == default
                ? animMap.Frames.Count
                : animMap.FramesPerSecond;

            return frameCounter % (60 / framesPerSec) == 0;
        }

        protected virtual void ChangeAnimationFrame()
        {
            if (animationMap != null && !animationStop)
            {
                if (animationMap.TilesetAnimation)
                {
                    FramePosition.Pos = animationMap.Frames[frame];
                }

                if (RequestNextFrame(FrameCounter, animationMap))
                {
                    frame++;
                    AnimationLoop();
                }

                var countOfFrames = animationMap.TilesetAnimation
                    ? animationMap.Frames.Count
                    : animationMap.FullFrames.Length;

                if (frame == countOfFrames)
                {
                    frame = 0;
                    FrameCounter = 0;
                    if(!Loop)
                    {
                        AnimationPlayed = true;
                        RequestStop();
                    }
                }
            }
        }

        protected virtual bool Loop { get; set; } = true;
        
        protected virtual void AnimationLoop()
        {
        }

        protected abstract void DrawLoop();
    }
}