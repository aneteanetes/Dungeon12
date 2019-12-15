using System;

namespace Dungeon.Types
{
    public class Callback : IDisposable
    {
        Action<Callback> _then;
        Action _dispose;

        public Callback(Action dispose) => _dispose = dispose;

        public void Call() => _then?.Invoke(this);

        public void Dispose() => _dispose?.Invoke();

        public void Then(Action<Callback> action) => _then = action;
    }
}