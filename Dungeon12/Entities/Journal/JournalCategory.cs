using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entites.Journal
{
    public class JournalCategory
    {
        public string Name { get; set; }

        public string Icon { get; set; }

        public List<JournalEntry> Content { get; set; }
    }
}
