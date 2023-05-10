using System.Collections.Generic;

namespace Dungeon
{
    public class ListR<T> : List<T>
    {
        public new T Add(T item)
        {
            base.Add(item);
            return item;
        }
    }
}
