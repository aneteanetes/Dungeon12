using Dungeon;
using Dungeon12.CardGame.Engine;
using Dungeon12.Entites.Journal;
using Dungeon12.Entities.Quests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Classes
{
    public abstract partial class Character
    {
        public virtual int InitialHP => 100;

        public Journal Journal { get; set; } = new Journal();

        public List<IQuest> ActiveQuests { get; set; } = new List<IQuest>();
    }
}
