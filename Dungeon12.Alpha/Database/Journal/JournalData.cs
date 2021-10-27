using Dungeon.Data;

namespace Dungeon12.Database.Journal
{
    public class JournalData : Persist
    {
        public string Display { get; set; }

        public string Group { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }
    }
}