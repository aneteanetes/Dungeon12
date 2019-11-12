using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon
{
    public static class QueueExtensions
    {
        public static Queue<T> AsQueue<T>(this IEnumerable<T> @enum) => new Queue<T>(@enum);
    }
}
