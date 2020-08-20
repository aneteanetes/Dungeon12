using Dungeon.Engine.Projects;
using Dungeon.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Engine.Events
{
    public class FreezeAllEvent : IEvent
    {
        public double Seconds { get; set; }

        public FreezeAllEvent(double seconds) => Seconds = seconds;
    }
}
