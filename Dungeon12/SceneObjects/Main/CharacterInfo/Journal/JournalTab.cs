using Dungeon.Classes;
using Dungeon.Drawing.SceneObjects.UI;
using Dungeon.Events;
using Dungeon.SceneObjects;
using Dungeon12.Drawing.SceneObjects.Main.CharacterInfo.Journal;
using Dungeon12.Entites.Journal;
using System;
namespace Dungeon12.Drawing.SceneObjects.Main.CharacterInfo.Talants
{
    public class JournalTab : TabControl<JournalTabContent, JournalCategory, JournalTab>
    {
        private readonly Character character;
        private readonly JournalList journalList;

        public JournalTab(SceneObject parent, JournalCategory journalCategory, JournalList journalList, Character character, bool active = false)
            : base(parent, active, journalCategory,titleImg: journalCategory.Icon, tooltip:journalCategory.Name)
        {
            this.journalList = journalList;
            this.character = character;
        }
        
        protected override JournalTab Self => this;

        protected override Func<JournalCategory, double, JournalTabContent> CreateContent => OpenJournalTab;

        protected override void CallOnEvent(dynamic obj)
        {
            OnEvent(obj);
        }

        public void OnEvent(ClassChangeEvent @event)
        {

        }

        private JournalTabContent OpenJournalTab(JournalCategory journalCategory, double left)
        {
            return new JournalTabContent(journalCategory, journalList, left);
        }
    }
}
