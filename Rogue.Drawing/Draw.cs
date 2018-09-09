using System;
using System.Collections.Generic;
using System.Text;
using Rogue.View.Interfaces;

namespace Rogue.Drawing
{
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

        private static IDrawSession RunnedSession<T>(Action<T> init = null) where T : class, IDrawSession, new()
        {
            var session = new T();

            if (init != null)
                init(session);

            return session.Run();
        }

        public void Publish()
        {

        }
    }
}