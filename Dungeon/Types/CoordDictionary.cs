using System.Collections.Generic;

namespace Dungeon.Types
{
    public class CoordDictionary<TValue>
    {
        private Dictionary<string, TValue> instance = new Dictionary<string, TValue>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public TValue this[int x, int y]
        {
            get
            {
                if (ContainsKey(x, y))
                    return instance[$"{x},{y}"];

                return default;
            }
            set { instance[$"{x},{y}"] = value; }
        }

        public void Add(int x, int y, TValue value) => instance.Add(Key(x, y), value);

        public bool ContainsKey(int x, int y) => instance.ContainsKey(Key(x, y));

        private string Key(int x, int y) => $"{x},{y}";
    }
}