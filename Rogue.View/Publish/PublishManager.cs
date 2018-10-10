namespace Rogue.View.Publish
{
    using System.Collections.Generic;
    using Rogue.View.Interfaces;

    public static class PublishManager
    {
        private static readonly List<IPublisher> publishers = new List<IPublisher>();

        private static IPublisher Current;

        public static void Set(IPublisher publisher)
        {
            if(!publishers.Contains(publisher))
            {
                publishers.Add(publisher);
            }

            Current = publisher;
        }

        public static void Publish(List<IDrawSession> drawSessions)
        {
            Current.Publish(drawSessions);
        }

        public static void Animation(IAnimationSession animationSession)
        {
            Current.Animation(animationSession);
        }

        public static void Block(bool block)
        {
            Current.BlockControls(block);
        }
    }
}