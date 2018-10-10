namespace Rogue.Drawing.Animations
{
    using System.Collections.Generic;
    using Rogue.View.Interfaces;
    using Rogue.View.Publish;

    public abstract class AnimationSession : IAnimationSession
    {
        public bool BlockingAnimation { get; set; }

        private readonly List<IEnumerable<IDrawable>> _Frames = new List<IEnumerable<IDrawable>>();
        public IEnumerable<IEnumerable<IDrawable>> Frames => _Frames;

        protected void AddFrame(IEnumerable<IDrawable> frame) => _Frames.Add(frame);

        public void Publish()
        {
            if (this.BlockingAnimation)
            {
                PublishManager.Block(true);
            }

            PublishManager.Animation(this);
        }

        public abstract IAnimationSession Run();

        public void End()
        {
            if (this.BlockingAnimation)
            {
                PublishManager.Block(false);
            }
        }
    }
}