﻿using Dungeon.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Entities.Alive.Events
{
    public class AliveKillEvent : IEvent
    {
        public Alive Victim { get; set; }

        public Alive Killer { get; set; }
    }
}
