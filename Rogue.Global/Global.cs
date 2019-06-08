namespace Rogue
{
    using Rogue.View.Interfaces;
    using System.Collections.Generic;
    using System.Linq;

    public static class Global
    {
        public static IDrawClient DrawClient;

        public static object FreezeWorld = null;
        
        private static List<object> freezeControls = new List<object>();

        public static void AddFreezeControl(object obj)
        {
            freezeControls.Add(obj);
        }

        public static void RemoveFreezeControl(object obj)
        {
            freezeControls.Remove(obj);
        }

        public static object[] Freezed
        {
            get
            {
                var arr = new object[0];

                if (FreezeWorld != null)
                {
                    arr = new object[] { FreezeWorld };
                }
                else if (freezeControls.Count > 0)
                {
                    arr = arr.Concat(freezeControls).ToArray();
                }

                return arr;
            }
        }
    }
}