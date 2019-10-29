using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Entites.Journal
{
    public class JournalCategory
    {
        public string Name { get; set; }

        public List<JournalEntry> Content { get; set; }
    }
}
