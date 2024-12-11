﻿using System.Collections.Generic;

namespace Nabunassar.Entities.Quests
{
    internal class Quest
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<Goal> Goals { get; set; } = new List<Goal>();
    }
}
