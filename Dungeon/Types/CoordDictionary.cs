using System;
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

        public TValue this[double xd, double yd]
        {
            get
            {
                int x = (int)xd;
                int y = (int)yd;

                if (ContainsKey(x, y))
                    return instance[$"{x},{y}"];

                return default;
            }
            set { instance[$"{xd},{yd}"] = value; }
        }

        public void Add(int x, int y, TValue value) => instance.Add(Key(x, y), value);

        public bool ContainsKey(int x, int y) => instance.ContainsKey(Key(x, y));

        private string Key(int x, int y) => $"{x},{y}";

        public void ForEach(Action<TValue> action)
        {
            foreach (var item in instance)
            {
                action(item.Value);
            }
        }

        public void ForEach(Action<string,TValue> action)
        {
            foreach (var item in instance)
            {
                action(item.Key, item.Value);
            }
        }
    }
}