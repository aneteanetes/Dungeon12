using Dungeon.Classes;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Drawing.SceneObjects.UI;
using Dungeon.Events;
using Dungeon12.Entites.Journal;
using System;
namespace Dungeon12.Drawing.SceneObjects.Main.CharacterInfo.Talants
{
    public class JournalTab : TabControl<JournalTabContent, JournalCategory, JournalTab>
    {
        private readonly Character character;

        public JournalTab(SceneObject parent, JournalCategory journalCategory, Character character, bool active = false)
            : base(parent, active, journalCategory,titleImg: journalCategory.Icon, tooltip:journalCategory.Name)
        {
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
            return new JournalTabContent(journalCategory,this.character, left);
        }
    }
}
