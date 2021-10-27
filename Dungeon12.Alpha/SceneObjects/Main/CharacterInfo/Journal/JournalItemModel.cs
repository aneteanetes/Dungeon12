using Dungeon12.Entites.Journal;
using System;

namespace Dungeon12.SceneObjects.Main.CharacterInfo.Journal
{
    internal class JournalItemModel
    {
        public string Title { get; set; }

        public int ItemIndex { get; set; }

        public string Group { get; set; }

        public JournalEntry JournalEntry { get; set; }

        public Action<ScrollJournalContent> ShowEntryContent { get; set; }

        public Action OnExpand { get; set; }

        public Action OnCollapse { get; set; }

        public int GroupCount { get; set; }
    }
}