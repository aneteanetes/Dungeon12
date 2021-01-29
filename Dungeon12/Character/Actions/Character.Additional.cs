using Dungeon12.Entites.Journal;
using Dungeon12.Entities.Quests;
using System.Collections.Generic;

namespace Dungeon12.Classes
{
    public abstract partial class Character
    {
        public Journal Journal { get; set; } = new Journal();

        public List<IQuest> ActiveQuests { get; set; } = new List<IQuest>();
    }
}
