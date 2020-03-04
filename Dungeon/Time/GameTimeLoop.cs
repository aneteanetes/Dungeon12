using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon
{
    public struct GameTimeLoop
    {
        public GameTimeLoop(TimeSpan total, TimeSpan elapsed, bool isFreezing)
        {
            TotalGameTime = total;
            ElapsedGameTime = elapsed;
            IsRunningSlowly = isFreezing;
        }

        public TimeSpan TotalGameTime { get; }

        public TimeSpan ElapsedGameTime { get; }

        public bool IsRunningSlowly { get; set; }
    }
}