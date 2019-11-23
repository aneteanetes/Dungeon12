using Dungeon.Classes;
using Dungeon.Drawing.SceneObjects.UI;
using Dungeon.Events;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.Drawing.SceneObjects.Main.CharacterInfo.Journal;
using Dungeon12.Entites.Journal;
using Dungeon12.Entities.Quests;
using System;
namespace Dungeon12.Drawing.SceneObjects.Main.CharacterInfo.Talants
{
    public class JournalTab : TabControl<JournalTabContent, JournalCategory, JournalTab>
    {
        private readonly Character character;
        private readonly JournalList journalList;
        private readonly JournalWindow _journalWindow;

        public JournalTab(ISceneObject parent, JournalCategory journalCategory, JournalList journalList, Character character, bool active, JournalWindow journalWindow)
            : base(parent, active, journalCategory,titleImg: journalCategory.Icon, tooltip:journalCategory.Name)
        {
            this.journalList = journalList;
            this.character = character;
            this._journalWindow = journalWindow;
        }
        
        protected override JournalTab Self => this;

        protected override Func<JournalCategory, double, JournalTabContent> CreateContent => OpenJournalTab;
        
        private JournalTabContent OpenJournalTab(JournalCategory journalCategory, double left)
        {
            return new JournalTabContent(journalCategory,left, _journalWindow,_quest);
        }

        private IQuest _quest;

        public void Open(IQuest quest)
        {
            _quest = quest;
            base.Open();
        }
    }
}
