using Dungeon.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Entites.Journal
{
    public class JournalEntry : NetObject
    {
        public string Display { get; set; }

        public string Group { get; set; }

        //public string RegionGroup { get; set; }

        public bool Hide { get; set; }

        public string Text { get; set; }
    }
}
