using System;

namespace Dungeon.Types
{
    public class DrawCallback : Callback
    {
        public DrawCallback(Action dispose) : base(dispose)
        {
        }
    }
}