using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Types
{
    public class SafeQueue<T>
    {
        private Queue<T> _queue = new Queue<T>();

        public SafeQueue()
        {

        }

        public SafeQueue(IEnumerable<T> @enum)
        {
            this._queue = new Queue<T>(@enum);
        }

        public T Dequeue()
        {
            _queue.TryDequeue(out var item);
            return item;
        }

        public void Enqueue(T item)
        {
            _queue.Enqueue(item);
        }
    }
}
