using System.Collections.Generic;

namespace Dungeon12.Entities.Quests
{
    public class Quest
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<Goal> Goals { get; set; } = new List<Goal>();
    }
}
