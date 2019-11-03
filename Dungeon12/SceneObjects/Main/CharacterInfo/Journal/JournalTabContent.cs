using Dungeon.Classes;
using Dungeon.Drawing.SceneObjects;
using Dungeon12.Entites.Journal;
using System.Collections.Generic;
using System.Linq;
using Dungeon;
using Dungeon.Drawing.Impl;
using Dungeon12.Drawing.SceneObjects.Main.CharacterBar;
using System;
using Dungeon.Control.Pointer;
using Dungeon.View.Interfaces;
using Dungeon.Control.Keys;
using Dungeon12.Drawing.SceneObjects.Main.CharacterInfo.Journal;
using Dungeon.Control.Events;
using Dungeon12.SceneObjects.Main.CharacterInfo.Journal;
using Dungeon.SceneObjects;
using Dungeon.SceneObjects.Mixins;
using Dungeon.SceneObjects.Base;

namespace Dungeon12.Drawing.SceneObjects.Main.CharacterInfo.Talants
{
    public class JournalTabContent : HandleSceneControl, IScrollableMixin
    {
        public override bool CacheAvailable => false;

        public override bool AbsolutePosition => true;

        private static readonly HashSet<string> ExpandedGroups = new HashSet<string>();

        private HandleSceneControl currentOpenedScroll;

        private JournalCategory _jcategory;

        public JournalTabContent(JournalCategory jcategory, JournalList journalList, double left)
        {
            this.Mix_ScrollableMixin(index => FillContent(left, index), new Dungeon.Types.Point()
            {
                X = 11 - left,
                Y = 2
            }, 13);

            _jcategory = jcategory;
            JournalGroup.SlideIndex = 0;
            JournalGroup.SlideDownCount = 0;
            JournalGroup.groups.Clear();

            Global.Events.Subscribe<JournalWindowOnKeyProcessedEvent>(e =>
            {
                currentOpenedScroll = null;
            }, false);

            journalList.CanDestroyParentBinding = () => currentOpenedScroll == null;

            this.Width = 12;

            this.Height = 15;

            var groups = _jcategory.Content?.GroupBy(c => c.Group) ?? Enumerable.Empty<IGrouping<string, JournalEntry>>();
            JournalGrouped = groups.ToList();

            FillContent(left);
        }

        private int ScrollShift
        {
            get
            {
                var commonHeight = JournalGroups.Sum(g => {
                    return 1.5 + (g.IsExpanded ? g.JournalEntries.Sum(e => 1.5) : 0);
                }) + 1.5;
                return (int)Math.Round(commonHeight - 13.5, MidpointRounding.AwayFromZero);
            }
        }

        List<IGrouping<string, JournalEntry>> JournalGrouped = new List<IGrouping<string, JournalEntry>>();
        List<JournalGroup> JournalGroups = new List<JournalGroup>();

        private void FillContent(double left, int scrollIndex=0)
        {
            this.RemoveChild<JournalGroup>();
            JournalGroups = new List<JournalGroup>();

            double top = 1.5;
            int i = 0;
            foreach (var group in JournalGrouped)
            {
                var index = JournalGrouped.IndexOf(group);
                var jgroup = new JournalGroup(group.Key, index, i, group, so =>
                {
                    so.Top -= 1.5;
                    so.Left -= left;
                    so.Left += 12;
                    if (currentOpenedScroll != default)
                    {
                        currentOpenedScroll.Destroy();
                    }
                    currentOpenedScroll = so;
                    this.AddChild(so);
                });
                jgroup.SlideNeed = SlideNeed(jgroup);
                jgroup.Left -= left;
                jgroup.Left += 1;
                jgroup.Top += 2.5 + top;

                JournalGroups.Add(jgroup);
                this.AddChild(jgroup);

                jgroup.OnExpand = () =>
                {
                    var scrollindex =this.GetMixinValue<int>(ScrollableMixin.ScrollBarIndex);
                    ExpandedGroups.Add(group.Key);
                    Mixin<Scrollbar>().Down.Active = ScrollShift > 0;
                    Mixin<Scrollbar>().Up.Active = scrollindex > 0;

                    if(ScrollShift < 0)
                    {
                        this.SetMixinValue(ScrollableMixin.ScrollBarMaxDown, 0);
                        this.SetMixinValue<int>(ScrollableMixin.ScrollBarIndex, 0);
                    }
                    else
                    {
                        this.SetMixinValue(ScrollableMixin.ScrollBarMaxDown, ScrollShift);
                    }
                };
                jgroup.OnCollapse = () =>
                {
                    var scrollindex = this.GetMixinValue<int>(ScrollableMixin.ScrollBarIndex);
                    ExpandedGroups.Remove(group.Key);
                    Mixin<Scrollbar>().Down.Active = ScrollShift > 0;
                    Mixin<Scrollbar>().Up.Active = scrollindex > 0;
                    if (ScrollShift > 0)
                    {
                        this.SetMixinValue(ScrollableMixin.ScrollBarMaxDown, 0);
                        this.SetMixinValue<int>(ScrollableMixin.ScrollBarIndex, 0);
                    }
                    else
                    {
                        this.SetMixinValue(ScrollableMixin.ScrollBarMaxDown, ScrollShift);
                    }
                };
                jgroup.AfterExpandAnother = () =>
                {
                    jgroup.Visible = !(jgroup.Top + ScrollShift > 13);
                };
                jgroup.AfterCollapseAnother = jgroup.AfterExpandAnother;

                top += 1.5;
            }

            var shift = ScrollShift;
            foreach (var group in JournalGrouped)
            {
                var jg = JournalGroups.FirstOrDefault(j => j.GroupName == group.Key);
                if (ExpandedGroups.Contains(group.Key))
                {
                    jg?.Expand();
                }

                if(jg.Top+shift>13)
                {
                    jg.Visible = false;
                }
            }
        }

        protected override Key[] KeyHandles => new Key[]
        {
            Key.Escape
        };
        
        public override void KeyDown(Key key, KeyModifiers modifier, bool hold)
        {
            if (key == Key.Escape)
            {
                if (currentOpenedScroll != default)
                {
                    currentOpenedScroll.Destroy();
                }
                else
                {
                    base.KeyDown(key, modifier, hold);
                }
            }
        }

        private Func<bool> SlideNeed(JournalGroup jgroup)
        {
            return () =>
            {
                if (jgroup.GroupIndex <= JournalGroup.SlideIndex)
                {
                    return false;
                }
                return JournalGroup.SlideDownCount != 0;
            };
        }

        private class JournalGroup : SlideComponent
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

            public JournalGroup(string name,int index,int group, IEnumerable<JournalEntry> entries, Action<HandleSceneControl> add)
            {
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
                    var jec = new JournalEntryClickable(entry, add)
                    {
                        Top = top,
                        Visible = false
                    };
                    JournalEntries.Add(jec);
                    this.AddChild(jec);
                    top += 1.5;
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

        private class JournalEntryClickable : HandleSceneControl
        {
            public override string Cursor => "question";

            public override bool AbsolutePosition => true;
            public override bool CacheAvailable => false;

            JournalEntry _journalEntry;

            private TextControl text;

            Action<HandleSceneControl> _add;

            public JournalEntryClickable(JournalEntry journalEntry, Action<HandleSceneControl> add)
            {
                _add = add;
                _journalEntry = journalEntry;
                this.Image = "ui/journal/select.png".AsmImgRes();
                this.Width = 10;
                this.Height = 1;
                text = this.AddTextCenter(new DrawText(journalEntry.Display).Montserrat());
            }

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

        private class ScrollJournalContent : HandleSceneControl
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

                var allText = new DrawText(journalEntry.Text, new DrawColor(ConsoleColor.Black)).Montserrat();
                var text = new TextControl(allText);
                text.Top += plusTop;
                text.Left = 0.5;
                this.AddChild(text);
            }
        }
    }
}