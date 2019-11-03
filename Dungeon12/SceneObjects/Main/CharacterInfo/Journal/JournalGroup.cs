using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.SceneObjects;
using Dungeon.SceneObjects.Base;
using Dungeon12.Drawing.SceneObjects.Main.CharacterBar;
using Dungeon12.Entites.Journal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.SceneObjects.Main.CharacterInfo.Journal
{
    internal class JournalGroup : SlideComponent
    {
        public static double SlideDownCount = 0;
        public static int SlideIndex = 0;

        public static Dictionary<int, List<JournalGroup>> groups = new Dictionary<int, List<JournalGroup>>();

        public override double SlideOffsetTop => SlideDownCount;

        public override string Cursor => "info";

        public override bool AbsolutePosition => true;

        public override bool CacheAvailable => false;

        public List<JournalEntryClickable> JournalEntries { get; } = new List<JournalEntryClickable>();

        public int GroupIndex { get; private set; }

        private int _group;

        public string GroupName { get; }

        private Scrollbar _scrollbar;

        public JournalGroup(string name, int index, int group, IEnumerable<JournalEntry> entries, Action<HandleSceneControl> add, Scrollbar scrollbar)
        {
            _scrollbar = scrollbar;
            GroupName = name;
            _group = group;
            if (!groups.ContainsKey(group))
            {
                groups.Add(group, new List<JournalGroup>());
            }

            groups[group].Add(this);

            GroupIndex = index;
            this.Image = "ui/journal/group.png".AsmImgRes();
            this.Width = 10;
            this.Height = 1;

            this.AddTextCenter(new DrawText(name).Montserrat());

            double top = 1.5;

            foreach (var entry in entries)
            {
                //var jec = new JournalEntryClickable(entry, add


                ///*, jentry =>
                //// {
                ////     if (!this.IsExpanded)
                ////         return false;

                ////     if (((jentry.Top + this.Top) - scrollIndexProvide()) > 13)
                ////         return false;

                ////    //jentry.Top+this.Top scrollIndexProvide()

                ////    return true;
                /// }, scrollIndexProvide*/
                
                //    )
                //{
                //    Top = top,
                //    Visible = false
                //};
                //JournalEntries.Add(jec);
                //this.AddChild(jec);
                //top += 1.5;
            }
        }

        public override bool Visible
        {
            get
            {
                if (this.Top < 4)
                    return false;

                if (this.Top > 13)
                    return false;

                return true;
            }
        }

        public bool IsExpanded { get; private set; } = false;

        public Action OnExpand = () => { };
        public Action AfterExpandAnother = () => { };
        public Action OnCollapse = () => { };
        public Action AfterCollapseAnother = () => { };

        public override void Click(PointerArgs args)
        {
            if (!IsExpanded)
            {
                Expand();
            }
            else
            {
                Collapse();
            }

            base.Click(args);
        }

        public void Expand()
        {
            groups[_group].ForEach(j =>
            {
                if (j != this)
                {
                    j.Collapse();
                }
            });

            JournalEntries.ForEach(je => je.Visible = true);

            IsExpanded = true;
            SlideDownCount += this.JournalEntries.Count * 1.5;
            SlideIndex = this.GroupIndex;
            OnExpand?.Invoke();

            groups[_group].ForEach(j =>
            {
                if (j != this)
                {
                    j.AfterExpandAnother?.Invoke();
                }
            });
        }

        public void Collapse()
        {
            if (IsExpanded)
            {
                SlideDownCount -= this.JournalEntries.Count * 1.5;
            }
            IsExpanded = false;
            JournalEntries.ForEach(je => je.Visible = false);
            OnCollapse?.Invoke();

            groups[_group].ForEach(j =>
            {
                if (j != this)
                {
                    j.AfterCollapseAnother?.Invoke();
                }
            });
        }
    }
}
