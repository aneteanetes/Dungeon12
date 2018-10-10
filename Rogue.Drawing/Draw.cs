namespace Rogue.Drawing
{
    using System;
    using System.Collections.Generic;
    using Rogue.View.Interfaces;
    using Rogue.View.Publish;

    public class Draw
    {
        List<IDrawSession> DrawSessionBatch = new List<IDrawSession>();

        public Draw Then<T>(Action<T> init = null) where T : class, IDrawSession, new()
        {
            this.DrawSessionBatch.Add(RunnedSession<T>(init));
            return this;
        }

        public static void RunSession<T>(Action<T> init = null) where T : class, IDrawSession, new()
        {
            RunnedSession(init).Publish();
        }

        public static Draw Session<T>(Action<T> init = null) where T : class, IDrawSession, new()
        {
            var draw = new Draw();

            draw.DrawSessionBatch.Add(RunnedSession<T>(init));

            return draw;
        }

        public static void Animation<T>(Action<T> init = null) where T : class, IAnimationSession, new()
        {
            var animation = new T();

            init?.Invoke(animation);

            animation
                .Run()
                .Publish();
        }

        private static IDrawSession RunnedSession<T>(Action<T> init = null) where T : class, IDrawSession, new()
        {
            var session = new T();

            init?.Invoke(session);

            return session.Run();
        }

        public void Publish()
        {
            PublishManager.Publish(DrawSessionBatch);
        }
    }
}