using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.SceneObjects;
using Dungeon12.Entites.Journal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.SceneObjects.Main.CharacterInfo.Journal
{
    internal class JournalEntryClickable : HandleSceneControl
    {
        public override string Cursor => "question";

        public override bool AbsolutePosition => true;
        public override bool CacheAvailable => false;

        JournalEntry _journalEntry;

        private TextControl text;

        Action<HandleSceneControl> _add;

        private Func<JournalEntryClickable, bool> _visible;
        private Func<int> _scrollIndexProvide;

        public JournalEntryClickable(JournalEntry journalEntry, Action<HandleSceneControl> add, Func<JournalEntryClickable, bool> visible, Func<int> scrollIndexProvide)
        {
            _scrollIndexProvide = scrollIndexProvide;
            _visible = visible;
            _add = add;
            _journalEntry = journalEntry;
            this.Image = "ui/journal/select.png".AsmImgRes();
            this.Width = 10;
            this.Height = 1;
            text = this.AddTextCenter(new DrawText(journalEntry.Display).Montserrat());
        }

        public override bool Visible => _visible(this);

        public override void Focus()
        {
            text.Text.ForegroundColor = new DrawColor(ConsoleColor.Yellow);
            base.Focus();
        }

        public override void Unfocus()
        {
            text.Text.ForegroundColor = new DrawColor(ConsoleColor.White);
            base.Unfocus();
        }

        public override double Opacity => 0.5;

        public override void Click(PointerArgs args)
        {
            _add(new ScrollJournalContent(_journalEntry));
            base.Click(args);
        }
    }
}