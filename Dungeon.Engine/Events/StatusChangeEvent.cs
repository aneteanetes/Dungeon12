using Dungeon.Engine.Projects;
using Dungeon.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Engine.Events
{
    public class StatusChangeEvent : IEvent
    {
        public string Status { get; set; }

        public StatusChangeEvent(string status) => Status = status;
    }
}
