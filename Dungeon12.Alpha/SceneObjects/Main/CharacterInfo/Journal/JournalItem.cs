using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon12.Drawing.SceneObjects.Main.CharacterBar;
using Dungeon12.Entites.Journal;
using System;
using System.Collections.Generic;

namespace Dungeon12.SceneObjects.Main.CharacterInfo.Journal
{
    internal class JournalItem : SlideComponent
    {
        public static double SlideDownCount = 0;
        public static int SlideIndex = 0;
        public static List<JournalItem> AllExpandable = new List<JournalItem>();
        public static HashSet<int> Expanded = new HashSet<int>();

        public override double SlideOffsetTop => SlideDownCount;

        public override string CursorOld => _journalEntry==null ? "info" : "question";

        public override bool AbsolutePosition => true;

        public override bool CacheAvailable => false;
        
        public int ItemIndex { get; private set; }
        
        private JournalEntry _journalEntry;
        private Action<ScrollJournalContent> _add;

        public JournalItemModel Model { get; private set; }

        public JournalItem(JournalItemModel model)
        {
            Model = model;
            _add = model.ShowEntryContent;
            _journalEntry = model.JournalEntry;
            ItemIndex = model.ItemIndex;

            this.Image = _journalEntry == null
                ? "ui/journal/group.png".AsmImg()
                : "ui/journal/select.png".AsmImg();

            this.Width = 10;
            this.Height = 1;

            if(_journalEntry==null)
            {
                AllExpandable.Add(this);
            }

            this.AddTextCenter(new DrawText(model.Title).Montserrat());
        }

        public bool Search(string substring)
        {
            var titleContains = Model.Title.Contains(substring, StringComparison.InvariantCultureIgnoreCase);
            if (titleContains)
                return true;

            if (Model.JournalEntry != null)
            {
                return Model.JournalEntry.Text.Contains(substring);
            }

            return false;
        }

        public bool Collapsed { get; set; } = false;

        public override bool Visible
        {
            get
            {
                if (Collapsed)
                    return false;

                if (this.Top < 4)
                    return false;

                if (this.Top > 13)
                    return false;

                return true;
            }
        }

        public bool IsExpanded => Expanded.Contains(Model.ItemIndex);

        public override void Click(PointerArgs args)
        {
            if (_journalEntry != null)
            {
                _add(new ScrollJournalContent(_journalEntry));
            }
            else
            {
                if (!IsExpanded)
                {
                    Expand();
                }
                else
                {
                    Collapse();
                }
            }

            base.Click(args);
        }

        public void Expand()
        {

            Expanded.Add(Model.ItemIndex);
            SlideDownCount += Model.GroupCount * 1.5;
            SlideIndex = this.ItemIndex;

            Model.OnExpand?.Invoke();
        }

        public void Collapse()
        {
            if (IsExpanded)
            {
                SlideDownCount -= Model.GroupCount * 1.5;
                Expanded.Remove(Model.ItemIndex);
            }
            Model.OnCollapse?.Invoke();
        }
    }
}
