using Dungeon;
using Dungeon.Drawing;
using Dungeon.SceneObjects;
using Dungeon12.Entites.Journal;
using System;

namespace Dungeon12.SceneObjects.Main.CharacterInfo.Journal
{
    internal class ScrollJournalContent : EmptyHandleSceneControl
    {
        public override bool AbsolutePosition => true;

        public override bool CacheAvailable => false;

        public ScrollJournalContent(JournalEntry journalEntry)
        {
            this.Width = 12;
            this.Height = 17;
            this.Image = "ui/scrollback(17x12).png".AsmImgRes();

            var titleText = new DrawText(journalEntry.Display, new DrawColor(ConsoleColor.Black)) { Size = 24 }.Triforce();
            var title = this.AddTextCenter(titleText, vertical: false);
            title.Top += .5;

            var plusTop = (MeasureText(titleText).Y / 32) + 1;

            var allText = new DrawText(journalEntry.Text, new DrawColor(ConsoleColor.Black), true).Montserrat();
            var text = new TextControl(allText);
            text.Width = 11;
            text.Top += plusTop;
            text.Left = 0.5;
            this.AddChild(text);
        }
    }
}
