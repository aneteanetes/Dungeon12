using Rogue.Abilities.Talants.TalantTrees;
using Rogue.Classes;
using Rogue.Drawing.SceneObjects.UI;
using Rogue.Entites.Journal;
using Rogue.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Drawing.SceneObjects.Main.CharacterInfo.Talants
{
    public class JournalTab : TabControlFlex<JournalTabContent, JournalCategory, JournalTab>
    {
        private readonly Character character;

        public JournalTab(SceneObject parent, JournalCategory journalCategory, Character character, bool active = false)
            : base(parent, active, journalCategory, journalCategory.Name,16)
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
