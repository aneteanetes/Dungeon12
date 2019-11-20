using Dungeon.Entities;
using Dungeon.Network;
using Dungeon12.Database.Journal;
using Dungeon12.Entities.Quests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entites.Journal
{
    public class JournalEntry : DataEntity<JournalEntry, JournalData>
    {
        public string Display { get; set; }

        public string Group { get; set; }

        public bool Hide { get; set; }

        public int Order { get; set; }

        public string Text { get; set; }

        public IQuest Quest { get; set; }

        protected override void Init(JournalData dataClass)
        {
            this.Display = dataClass.Display;
            this.Group = dataClass.Group;
            this.Text = dataClass.Text;
            this.Order = dataClass.Order;
        }
    }
}